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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Shell;
namespace ORMSolutions.ORMArchitect.Core.ObjectModel
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
		/// <summary>
		/// Return true if the reading is a generated default reading
		/// </summary>
		bool IsDefault { get;}
	}
	#endregion // IReading interface
	#region Reading text utility delegates
	/// <summary>
	/// A callback function for use with the <see cref="Reading.ReplaceFields(string,ReadingTextFieldReplace)"/> method. Provides
	/// a replacement string for the provided <paramref name="index"/>.
	/// </summary>
	/// <param name="index">The zero-based index of the replacement field. The index is relative to the <see cref="ReadingOrder.RoleCollection"/>
	/// for the <see cref="ReadingOrder"/> associated with the <see cref="Reading"/> that owns this text. The index is not guaranteed to be in range.</param>
	/// <returns>A string to replace the placeholder for the given index. Return <see langword="null"/> to leave the replacement field.</returns>
	public delegate string ReadingTextFieldReplace(int index);
	/// <summary>
	/// A callback function for use with the <see cref="Reading.ReplaceFields(string,ReadingTextFieldReplaceWithMatch)"/> method. Provides
	/// a replacement string for the provided <paramref name="index"/>.
	/// </summary>
	/// <param name="index">The zero-based index of the replacement field. The index is relative to the <see cref="ReadingOrder.RoleCollection"/>
	/// for the <see cref="ReadingOrder"/> associated with the <see cref="Reading"/> that owns this text. The index is not guaranteed to be in range.</param>
	/// <param name="match">The <see cref="Match"/> information for this replacement. Contains information about the replacement location in the provided reading text.</param>
	/// <returns>A string to replace the placeholder for the given index. Return <see langword="null"/> to leave the replacement field.</returns>
	public delegate string ReadingTextFieldReplaceWithMatch(int index, Match match);
	/// <summary>
	/// A callback function for use with the <see cref="Reading.VisitFields"/> method. Called once for
	/// each integer replacement field.
	/// </summary>
	/// <param name="index">The zero-based index of the replacement field. The index is relative to the <see cref="ReadingOrder.RoleCollection"/>
	/// for the <see cref="ReadingOrder"/> associated with the <see cref="Reading"/> that owns this text. The index is not guaranteed to be in range, and
	/// the same index may be returned multiple times.</param>
	/// <returns><see langword="true"/> to continue, or <see langword="false"/> to stop visiting.</returns>
	public delegate bool ReadingTextFieldVisit(int index);
	#endregion // Reading text utility delegates
	#region Reading class
	public partial class Reading : IModelErrorOwner, IHasIndirectModelErrorOwner, IReading, IXmlSerializable
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
				null != (model = factType.ResolvedModel))
			{
				RuleManager ruleManager = Store.RuleManager;
				Type ruleType = typeof(ReadingPropertiesChangedRuleClass);
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
						ReadingRequiresUserModificationError newError = new ReadingRequiresUserModificationError(Partition);
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
				FactType factType = readingOrder.FactType;
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
						tooFewError = new TooFewReadingRolesError(factType.Partition);
						tooFewError.Reading = this;
						tooFewError.Model = factType.ResolvedModel;
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
					if (factType.UnaryRole == null)
					{
						if (null == (tooManyError = TooManyRolesError))
						{
							tooManyError = new TooManyReadingRolesError(factType.Partition);
							tooManyError.Reading = this;
							tooManyError.Model = factType.ResolvedModel;
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

		#region ReadingPropertiesChangedRule
		/// <summary>
		/// ChangeRule: typeof(Reading)
		/// Handles the resetting the current primary reading when a new one is selected.
		/// Also rejects trying to change the current primary reading's IsPrimary to false.
		/// Validates that the reading text has the necessary number of placeholders.
		/// </summary>
		private static void ReadingPropertiesChangedRule(ElementPropertyChangedEventArgs e)
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
				FrameworkDomainModel.DelayValidateElement(this, DelayValidateRoleCountError);
			}
		}

		#endregion // ReadingPropertiesChangedRule
		#region ReadingOrderHasRoleDeletedRule
		/// <summary>
		/// DeleteRule: typeof(ReadingOrderHasRole)
		/// Validate readings when a role is removed from a reading order
		/// </summary>
		private static void ReadingOrderHasRoleDeletedRule(ElementDeletedEventArgs e)
		{
			ReadingOrderHasRole link = e.ModelElement as ReadingOrderHasRole;
			ReadingOrder ord = link.ReadingOrder;
			if (!ord.IsDeleted)
			{
				LinkedElementCollection<Reading> readings = ord.ReadingCollection;
				foreach (Reading read in readings)
				{
					FrameworkDomainModel.DelayValidateElement(read, DelayValidateRoleCountError);
				}
			}
		}
		#endregion // ReadingOrderHasRoleDeletedRule
		#region ReadingSignatureChangedRule
		/// <summary>
		/// ChangeRule: typeof(FactType)
		/// </summary>
		private static void ReadingSignatureChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == FactType.NameChangedDomainPropertyId)
			{
				ModelElement element = e.ModelElement;
				// Check whether duplicate names are currently allowed. If so, then
				// make sure the names are also allowed in the delay validator so
				// that we don't get unwanted errors being thrown.
				FrameworkDomainModel.DelayValidateElement(
					element,
					element.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.ContainsKey(ORMModel.BlockDuplicateReadingSignaturesKey) ?
						(ElementValidation)DelayUpdateReadingSignaturesBlockDuplicates :
						DelayUpdateReadingSignatures);
			}
		}
		private static void DelayUpdateReadingSignatures(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				Dictionary<object, object> contextInfo = element.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
				object duplicateSignaturesKey = ORMModel.BlockDuplicateReadingSignaturesKey;
				bool addDuplicateSignaturesKey = false;
				try
				{
					if (contextInfo.ContainsKey(duplicateSignaturesKey))
					{
						contextInfo.Remove(duplicateSignaturesKey);
						addDuplicateSignaturesKey = true;
					}
					UpdateReadingSignatures((FactType)element);
				}
				finally
				{
					if (addDuplicateSignaturesKey)
					{
						contextInfo[duplicateSignaturesKey] = null;
					}
				}
			}
		}
		[DelayValidateReplaces("DelayUpdateReadingSignatures")]
		private static void DelayUpdateReadingSignaturesBlockDuplicates(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				Dictionary<object, object> contextInfo = element.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
				object duplicateNamesKey = ORMModel.BlockDuplicateReadingSignaturesKey;
				bool removeDuplicateNamesKey = false;
				try
				{
					if (!contextInfo.ContainsKey(duplicateNamesKey))
					{
						contextInfo[duplicateNamesKey] = null;
						removeDuplicateNamesKey = true;
					}
					UpdateReadingSignatures((FactType)element);
				}
				finally
				{
					if (removeDuplicateNamesKey)
					{
						contextInfo.Remove(duplicateNamesKey);
					}
				}
			}
		}
		private static Dictionary<string, object> mySignatureRenderingOptions;
		private static IDictionary<string, object> SignatureRenderingOptions
		{
			get
			{
				Dictionary<string, object> options;
				if (null == (options = mySignatureRenderingOptions))
				{
					options = new Dictionary<string, object>();
					options[CoreVerbalizationOption.ObjectTypeNameDisplay] = ObjectTypeNameVerbalizationStyle.SeparateCombinedNames;
					options[CoreVerbalizationOption.RemoveObjectTypeNameCharactersOnSeparate] = ".:_";
					System.Threading.Interlocked.CompareExchange<Dictionary<string, object>>(ref mySignatureRenderingOptions, options, null);
					options = mySignatureRenderingOptions;
				}
				return options;
			}
		}
		/// <summary>
		/// Update all reading signatures for readings owned by a given fact type.
		/// </summary>
		public static void UpdateReadingSignatures(FactType factType)
		{
			foreach (ReadingOrder order in factType.ReadingOrderCollection)
			{
				LinkedElementCollection<RoleBase> roles = order.RoleCollection;
				int roleCount = roles.Count;
				foreach (Reading reading in order.ReadingCollection)
				{
					reading.Signature = Reading.ReplaceFields(
						VerbalizationHyphenBinder.DehyphenateReadingText(reading.Text),
						delegate(int replaceIndex)
						{
							if (replaceIndex < roleCount)
							{
								ObjectType rolePlayer = roles[replaceIndex].Role.RolePlayer;
								return rolePlayer != null ? VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, SignatureRenderingOptions) : "";
							}
							return "";
						});
				}
			}
		}
		/// <summary>
		/// Create a reading signature from a reading text format string and a list
		/// of role players.
		/// </summary>
		/// <param name="readingText">Reading format text.</param>
		/// <param name="rolePlayers">A list of role players, with one role player per replacement field.</param>
		/// <param name="reverseReading">Set if <paramref name="readingText"/> represents a reverse reading</param>
		/// <returns>An expanded reading signature.</returns>
		public static string GenerateReadingSignature(string readingText, IList<ObjectType> rolePlayers, bool reverseReading)
		{
			int rolePlayerCount = rolePlayers.Count;
			return Reading.ReplaceFields(
				VerbalizationHyphenBinder.DehyphenateReadingText(readingText),
				delegate(int replaceIndex)
				{
					ObjectType rolePlayer;
					return (replaceIndex < rolePlayerCount && null != (rolePlayer = rolePlayers[reverseReading ? (rolePlayerCount - replaceIndex - 1) : replaceIndex])) ?
						VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, SignatureRenderingOptions) :
						"";
				});
		}
		#endregion // ReadingSignatureChangedRule
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
				DuplicateReadingSignatureError duplicateSignature;
				if (null != (duplicateSignature = DuplicateSignatureError))
				{
					yield return new ModelErrorUsage(duplicateSignature, ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary);
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
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateRoleCountError);
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
		protected static bool IsEditable
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
		/// <summary>
		/// Implements <see cref="IReading.IsDefault"/>. Returns <see langword="true"/>
		/// for default generated readings on implied fact types
		/// </summary>
		protected bool IsDefault
		{
			get
			{
				ReadingOrder order;
				FactType factType;
				if (null != (order = this.ReadingOrder) &&
					null != (factType = order.FactType) &&
					null != factType.ImpliedByObjectification)
				{
					RoleBase testRole = order.RoleCollection[0];
					return Text == ((testRole is RoleProxy || testRole is ObjectifiedUnaryRole) ? ResourceStrings.ImpliedFactTypePredicateReading : ResourceStrings.ImpliedFactTypePredicateInverseReading);
				}
				return false;
			}
		}
		bool IReading.IsDefault
		{
			get
			{
				return IsDefault;
			}
		}
		#endregion // IReading Implementation
		#region Reading text utility fields and methods
		/// <summary>
		/// Named field for the <see cref="Match"/> passed to the <see cref="ReadingTextFieldReplaceWithMatch"/> delegate.
		/// Represents the entire replacement field, including the curly braces.
		/// </summary>
		public const string ReplaceFieldsMatchFieldGroupName = "Field";
		/// <summary>
		/// Named field for the <see cref="Match"/> passed to the <see cref="ReadingTextFieldReplaceWithMatch"/> delegate.
		/// Represents the index portion of the  replacement field, not including the curly braces.
		/// </summary>
		public const string ReplaceFieldsMatchIndexGroupName = "Index";
		/// <summary>
		/// Named field for the <see cref="Match"/> used with the <see cref="FieldAndPredicatePartRegex"/>.
		/// </summary>
		private const string ReplaceFieldsMatchPredicatePartGroupName = "PredicatePart";
		private static Regex myFieldRegex;
		private static Regex myFieldAndPredicatePartRegex;

		/// <summary>
		/// Visits the fields in the reading to verify the text can be correctly modified.
		/// </summary>
		/// <param name="readingText">The reading text.<see cref="Reading"/></param>
		/// <param name="visitCallback">The visit callback.</param>
		/// <returns><see langword="true"/> to continue, or <see langword="false"/> to stop visiting.</returns>
		public static bool VisitFields(string readingText, ReadingTextFieldVisit visitCallback)
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
		/// <param name="replaceCallback">The replace callback. See <see cref="ReadingTextFieldReplace"/>.</param>
		/// <returns>Reading text with modified replacement fields.</returns>
		public static string ReplaceFields(string readingText, ReadingTextFieldReplace replaceCallback)
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
		/// <param name="readingText">The reading text. See <see cref="ReadingTextFieldReplaceWithMatch"/>.</param>
		/// <param name="replaceCallback">The replace callback.</param>
		/// <returns>Reading text with modified replacement fields.</returns>
		public static string ReplaceFields(string readingText, ReadingTextFieldReplaceWithMatch replaceCallback)
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
		/// Replaces the fields in the reading text.
		/// </summary>
		/// <param name="readingText">The reading text.</param>
		/// <param name="formatProvider">A <see cref="IFormatProvider"/>, or null to use the current culture</param>
		/// <param name="predicatePartDecorator">A format string applied to predicate text between fields.</param>
		/// <param name="replaceCallback">The replace callback. See <see cref="ReadingTextFieldReplace"/>.</param>
		public static string ReplaceFields(string readingText, IFormatProvider formatProvider, string predicatePartDecorator, ReadingTextFieldReplace replaceCallback)
		{
			int lastIndex = 0;
			formatProvider = formatProvider ?? CultureInfo.CurrentCulture;
			bool decoratePredicateText = !string.IsNullOrEmpty(predicatePartDecorator) && predicatePartDecorator != "{0}";
			string retVal = FieldAndPredicatePartRegex.Replace(
				readingText,
				delegate(Match match)
				{
					int index;
					string fieldReplace = null;
					if (replaceCallback != null &&
						int.TryParse(match.Groups[ReplaceFieldsMatchIndexGroupName].Value, NumberStyles.None, formatProvider, out index))
					{
						//Index param to the delegate
						fieldReplace = replaceCallback(index);
					}
					string predicatePart = match.Groups[ReplaceFieldsMatchPredicatePartGroupName].Value;
					if (decoratePredicateText &&
						!string.IsNullOrEmpty(predicatePart))
					{
						predicatePart = string.Format(formatProvider, predicatePartDecorator, predicatePart);
					}
					fieldReplace = (fieldReplace != null) ? fieldReplace : match.Groups[ReplaceFieldsMatchFieldGroupName].Value;
					lastIndex += match.Length;
					return (predicatePart != null) ? (predicatePart + fieldReplace) : fieldReplace;
				});
			if (decoratePredicateText)
			{
				int textLength = readingText.Length;
				if (lastIndex < textLength)
				{
					// Note that remaining is already part of retVal and needs to be stripped
					return retVal.Substring(0, retVal.Length - textLength + lastIndex) + string.Format(formatProvider, predicatePartDecorator, readingText.Substring(lastIndex));
				}
			}
			return retVal;
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
							@"(?n)(?<Field>((?<!\{)\{)(?<Index>[0-9]+)(\}(?!\})))",
							RegexOptions.Compiled),
						null);
					retVal = myFieldRegex;
				}
				return retVal;
			}
		}
		/// <summary>
		/// The regular expression used to find fields, indices, and predicate parts
		/// in a reading format string. Captures named groups corresponding
		/// to the <see cref="ReplaceFieldsMatchFieldGroupName"/>, <see cref="ReplaceFieldsMatchIndexGroupName"/>,
		/// and <see cref="ReplaceFieldsMatchPredicatePartGroupName"/> constants.
		/// Note that the final predicate part needs to be tracked separately after
		/// the final field.
		/// </summary>
		private static Regex FieldAndPredicatePartRegex
		{
			get
			{
				Regex retVal = myFieldAndPredicatePartRegex;
				if (retVal == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myFieldAndPredicatePartRegex,
						new Regex(
							@"(?n)\G(?<PredicatePart>.*?)(?<Field>((?<!\{)\{)(?<Index>[0-9]+)(\}(?!\})))",
							RegexOptions.Compiled),
						null);
					retVal = myFieldAndPredicatePartRegex;
				}
				return retVal;
			}
		}
		#endregion // Reading text utility fields and methods
		#region IXmlSerializable Implementation
		System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
		{
			// Schema is already validated in ORMCore.xsd
			return null;
		}
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string XmlNamespace = ORMCoreDomainModel.XmlNamespace;
			ISerializationContext serializationContext = ((ISerializationContextHost)Store).SerializationContext;

			while (reader.Read())
			{
				XmlNodeType nodeType = reader.NodeType;
				if (nodeType == XmlNodeType.Element)
				{
					if (!reader.IsEmptyElement)
					{
						if (reader.LocalName == "Reading" && reader.NamespaceURI == XmlNamespace)
						{
							while (reader.Read())
							{
								XmlNodeType childNodeType = reader.NodeType;
								if (childNodeType == XmlNodeType.Element)
								{
									if (!reader.IsEmptyElement)
									{
										if (reader.LocalName == "Data" && reader.NamespaceURI == XmlNamespace)
										{
											Text = reader.ReadElementString();
										}
										else
										{
											PassEndElement(reader);
										}
									}
								}
								else if (childNodeType == XmlNodeType.EndElement)
								{
									break;
								}
							}
						}
						else
						{
							PassEndElement(reader);
						}
					}
				}
				else if (nodeType == XmlNodeType.EndElement)
				{
					break;
				}
			}
		}
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			string namespaceUri = ORMCoreDomainModel.XmlNamespace;
			writer.WriteStartElement("orm", "Reading", namespaceUri);
			writer.WriteAttributeString("id", '_' + XmlConvert.ToString(Id).ToUpperInvariant());
			string readingText = Text;
			writer.WriteElementString("orm", "Data", namespaceUri, readingText);
			IList<RoleBase> roles = RoleCollection;
			int roleCount = roles.Count;

			VerbalizationHyphenBinder hyphenBinder = new VerbalizationHyphenBinder(this, null, null, null, "{0}\x0{1}", null);
			string alternateReadingText = hyphenBinder.ModifiedReadingText;
			if (alternateReadingText != null)
			{
				readingText = alternateReadingText;
			}
			int precedingIndex = -1;
			string pendingPredicateText = null;
			bool wroteExpandedData = false;
			int lastIndex = 0;
			// The regex gives leading text, not trailing, so postpone processing until
			// we have full data.
			bool testHyphenBinding = alternateReadingText != null;
			if (testHyphenBinding)
			{
				readingText = alternateReadingText;
			}
			Match visitMatch = FieldAndPredicatePartRegex.Match(readingText);
			while (visitMatch.Success)
			{
				GroupCollection groups = visitMatch.Groups;
				string currentIndexString = groups[ReplaceFieldsMatchIndexGroupName].Value;
				pendingPredicateText += groups[ReplaceFieldsMatchPredicatePartGroupName].Value;
				int currentIndex;
				if (int.TryParse(currentIndexString, NumberStyles.None, CultureInfo.InvariantCulture, out currentIndex) &&
					currentIndex >= 0 &&
					currentIndex < roleCount)
				{
					WriteRoleText(writer, precedingIndex, testHyphenBinding, ref hyphenBinder, ref pendingPredicateText, ref wroteExpandedData);
					precedingIndex = currentIndex;
				}
				else
				{
					// Error condition, just append the field text untouched to the predicate text
					pendingPredicateText += groups[ReplaceFieldsMatchFieldGroupName].Value;
				}
				lastIndex += visitMatch.Length;
				visitMatch = visitMatch.NextMatch();
			}
			bool hasTrailingText = lastIndex < readingText.Length;
			if (lastIndex < readingText.Length)
			{
				pendingPredicateText += readingText.Substring(lastIndex);
			}
			if (precedingIndex != -1 ||
				!string.IsNullOrEmpty(pendingPredicateText))
			{
				WriteRoleText(writer, precedingIndex, testHyphenBinding, ref hyphenBinder, ref pendingPredicateText, ref wroteExpandedData);
			}
			if (wroteExpandedData)
			{
				writer.WriteEndElement(); // Matches ExpandedData element
			}
			writer.WriteEndElement(); // Matches Reading element
		}
		private static void WriteRoleText(XmlWriter writer, int roleIndex, bool testHyphenBinding, ref VerbalizationHyphenBinder hyphenBinder, ref string pendingPredicateText, ref bool wroteExpandedData)
		{
			if (roleIndex == -1)
			{
				writer.WriteStartElement("orm", "ExpandedData", ORMCoreDomainModel.XmlNamespace);
				if (!string.IsNullOrEmpty(pendingPredicateText))
				{
					writer.WriteAttributeString("FrontText", pendingPredicateText);
					pendingPredicateText = null;
				}
				wroteExpandedData = true;
			}
			else
			{
				string hyphenBindingFormatString;
				if (testHyphenBinding &&
					null != (hyphenBindingFormatString = hyphenBinder.GetRoleFormatString(roleIndex)) &&
					hyphenBindingFormatString.Length > 1)
				{
					writer.WriteStartElement("orm", "RoleText", ORMCoreDomainModel.XmlNamespace);
					writer.WriteAttributeString("RoleIndex", roleIndex.ToString(CultureInfo.InvariantCulture));
					int separatorIndex = hyphenBindingFormatString.IndexOf('\0');
					if (separatorIndex > 0)
					{
						writer.WriteAttributeString("PreBoundText", hyphenBindingFormatString.Substring(0, separatorIndex));
					}
					if ((hyphenBindingFormatString.Length - separatorIndex) > 1)
					{
						writer.WriteAttributeString("PostBoundText", hyphenBindingFormatString.Substring(separatorIndex + 1));
					}
					if (!string.IsNullOrEmpty(pendingPredicateText))
					{
						writer.WriteAttributeString("FollowingText", pendingPredicateText);
						pendingPredicateText = null;
					}
					writer.WriteEndElement();
				}
				else if (!string.IsNullOrEmpty(pendingPredicateText))
				{
					writer.WriteStartElement("orm", "RoleText", ORMCoreDomainModel.XmlNamespace);
					writer.WriteAttributeString("RoleIndex", roleIndex.ToString(CultureInfo.InvariantCulture));
					writer.WriteAttributeString("FollowingText", pendingPredicateText);
					pendingPredicateText = null;
					writer.WriteEndElement();
				}
			}
		}
		/// <summary>
		/// Move the reader to the node immediately after the end element corresponding to the current open element
		/// </summary>
		/// <param name="reader">The XmlReader to advance</param>
		private static void PassEndElement(XmlReader reader)
		{
			if (!reader.IsEmptyElement)
			{
				bool finished = false;
				while (!finished && reader.Read())
				{
					switch (reader.NodeType)
					{
						case XmlNodeType.Element:
							PassEndElement(reader);
							break;

						case XmlNodeType.EndElement:
							finished = true;
							break;
					}
				}
			}
		}
		#endregion // IXmlSerializable Implementation
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// populates the initial values for the <see cref="Reading.Signature"/> property.
		/// </summary>
		public static IDeserializationFixupListener SignatureFixupListener
		{
			get
			{
				return new ReadingSignatureFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation. Properly initializes the Reading.Signature property
		/// </summary>
		private sealed class ReadingSignatureFixupListener : DeserializationFixupListener<FactType>
		{
			/// <summary>
			/// SignatureFixupListener constructor
			/// </summary>
			public ReadingSignatureFixupListener()
				: base((int)ORMDeserializationFixupPhase.GenerateElementNames)
			{
			}
			/// <summary>
			/// Process derivation elements
			/// </summary>
			/// <param name="element">A <see cref="FactTypeDerivationRule"/> element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(FactType element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					UpdateReadingSignatures(element);
				}
			}
		}
		#endregion // Deserialization Fixup
	}
	#endregion // Reading class
	#region TooFewReadingRolesError class
	[ModelErrorDisplayFilter(typeof(FactTypeDefinitionErrorCategory))]
	public partial class TooFewReadingRolesError
	{
		#region Base overrides
		/// <summary>
		/// Creates the error text.
		/// </summary>
		public override void GenerateErrorText()
		{
			Reading reading;
			ReadingOrder order;
			IModelErrorDisplayContext context;
			if (null != (reading = Reading) &&
				null != (order = reading.ReadingOrder) &&
				null != (context = order.FactType))
			{
				ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorReadingTooFewRolesMessage, context.ErrorDisplayContext ?? "", reading.Text));
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

		#endregion // Base overrides
	}
	#endregion // TooFewReadingRolesError class
	#region TooManyReadingRolesError class
	[ModelErrorDisplayFilter(typeof(FactTypeDefinitionErrorCategory))]
	public partial class TooManyReadingRolesError
	{
		#region Base overrides
		/// <summary>
		/// Creates the error text.
		/// </summary>
		public override void GenerateErrorText()
		{
			Reading reading;
			ReadingOrder order;
			IModelErrorDisplayContext context;
			if (null != (reading = Reading) &&
				null != (order = reading.ReadingOrder) &&
				null != (context = order.FactType))
			{
				ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorReadingTooManyRolesMessage, context.ErrorDisplayContext ?? "", reading.Text));
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
		#endregion // Base overrides
	}
	#endregion // TooManyReadingRolesError class
	#region ReadingRequiresUserModificationError class
	[ModelErrorDisplayFilter(typeof(FactTypeDefinitionErrorCategory))]
	public partial class ReadingRequiresUserModificationError
	{
		#region Base overrides
		/// <summary>
		/// Creates the error text.
		/// </summary>
		public override void GenerateErrorText()
		{
			Reading reading = Reading;
			ReadingOrder order;
			IModelErrorDisplayContext context;
			if (null != (reading = Reading) &&
				null != (order = reading.ReadingOrder) &&
				null != (context = order.FactType))
			{
				ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorReadingRequiresUserModificationMessage, context.ErrorDisplayContext ?? "", GenerateReadingText(reading.Text, order)));
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
	}
	#endregion // ReadingHasReadingRequiresUserModificationError class
}
