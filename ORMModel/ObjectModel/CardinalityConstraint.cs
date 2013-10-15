#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using System.Text.RegularExpressions;
using System.Text;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region CardinalityConstraint class
	partial class CardinalityConstraint : IDefaultNamePattern, IModelErrorOwner
	{
		#region Range Parsing
		private static string GetEditableRangeText(CardinalityRange range)
		{
			int lower = range.LowerBound;
			int upper = range.UpperBound;
			CultureInfo culture = CultureInfo.CurrentCulture;
			if (lower == upper)
			{
				return lower.ToString();
			}
			else if (lower == 0)
			{
				return string.Format(culture, "..{0}", upper);
			}
			else if (upper == -1)
			{
				return string.Format(culture, "{0}..", lower);
			}
			return string.Format(culture, "{0}..{1}", lower, upper);
		}
		private struct BasicRange
		{
			#region Regex
			private static Regex myRangeSeparatorRegex;
			private const string RangeSeparatorBound1 = "Bound1";
			private const string RangeSeparatorBound2 = "Bound2";
			private const string RangeSeparatorOp1 = "Op1";
			private const string RangeSeparatorOp2 = "Op2";
			private static string myRangeSeparator;
			private static Regex RangeSeparatorRegex
			{
				get
				{
					string rangeSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
					Regex retVal = myRangeSeparatorRegex;
					if (retVal == null ||
						rangeSeparator != myRangeSeparator)
					{
						string touchUpSeparator = rangeSeparator;
						if (touchUpSeparator != ",")
						{
							// Given that the us-en culture is so common and the default language
							// for this tool, use a comma along with whatever else the current culture
							// so that the users have a better chance of getting it right. Unlike value
							// ranges, there is no danger of conflict with other language characters here
							// because we're only parsing decimal digits.
							touchUpSeparator = Regex.Escape(touchUpSeparator) + "|,";
						}
						Regex newRegex = new Regex(
							string.Format(CultureInfo.InvariantCulture, @"(?n)\G\s*(?<Op1>\.\.|>=?|=|<=?)?\s*(?<Bound1>\d+)\s*((?<Op2>\.\.)\s*(?<Bound2>\d+)?\s*)?(({0})|\z)", rangeSeparator),
							RegexOptions.Compiled);
						if ((object)retVal == System.Threading.Interlocked.CompareExchange<Regex>(ref myRangeSeparatorRegex, newRegex, retVal))
						{
							// We used the current values, store them
							myRangeSeparator = rangeSeparator;
						}
						retVal = myRangeSeparatorRegex;
					}
					return retVal;
				}
			}
			#endregion // Regex
			#region Member Variables
			public readonly int LowerBound;
			public readonly int UpperBound;
			#endregion // Member Variables
			#region Constructor
			private BasicRange(int lowerBound, int upperBound)
			{
				LowerBound = lowerBound;
				UpperBound = (upperBound == int.MaxValue) ? -1 : upperBound;
			}
			#endregion // Constructor
			#region Static functions
			private static void ThrowInvalidRangeException()
			{
				throw new ArgumentException(ResourceStrings.ModelExceptionCardinalityConstraintInvalidRangeText);
			}
			/// <summary>
			/// Parse user entered range text
			/// </summary>
			/// <param name="rangeText">The text to parse.</param>
			/// <param name="callback">Callback for each successfully parsed range.</param>
			/// <returns><see langword="false"/> if no ranges are specified, <see langword="true"/> otherwise.</returns>
			public static bool ParseRangeText(string rangeText, Action<BasicRange> callback)
			{
				CultureInfo culture = CultureInfo.CurrentCulture;
				Match match;
				if (string.IsNullOrEmpty(rangeText) ||
					!(match = RangeSeparatorRegex.Match(rangeText)).Success)
				{
					return false;
				}
				int prevMin = -1;
				int prevMax = -1;
				bool retVal = false;
				while (match.Success)
				{
					GroupCollection groups = match.Groups;
					string bound2Raw = groups[RangeSeparatorBound2].Value;
					bool unboundedLower = false;
					bool unboundedUpper = false;
					bool discreteValue = false;
					int boundIncrement = 0;
					bool hasOp2 = !string.IsNullOrEmpty(groups[RangeSeparatorOp2].Value);
					bool hasSecondBound = !string.IsNullOrEmpty(bound2Raw);
					switch (groups[RangeSeparatorOp1].Value)
					{
						case "..":
						case "<=":
							unboundedLower = true;
							break;
						case "<":
							unboundedLower = true;
							boundIncrement = -1;
							break;
						case ">":
							unboundedUpper = true;
							boundIncrement = 1;
							break;
						case ">=":
							unboundedUpper = true;
							break;
						case "=":
							discreteValue = true;
							break;
					}
					bool leadOperator = unboundedLower || unboundedUpper || discreteValue;
					if (leadOperator && (hasOp2 || hasSecondBound))
					{
						ThrowInvalidRangeException();
					}
					int bound;
					int lower = -1;
					int upper = -1;
					// Note that bound1 is the only value required by the regex, so we must have it
					// if there is a match.
					if (!int.TryParse(groups[RangeSeparatorBound1].Value, NumberStyles.None, CultureInfo.CurrentCulture, out bound))
					{
						ThrowInvalidRangeException();
					}
					else if (unboundedUpper || (hasOp2 && !hasSecondBound))
					{
						lower = bound + boundIncrement;
						upper = int.MaxValue;
					}
					else if (unboundedLower)
					{
						lower = 0;
						upper = bound + boundIncrement;
					}
					else if (discreteValue)
					{
						lower = upper = bound;
					}
					else
					{
						lower = bound;
						if (!hasSecondBound)
						{
							upper = lower;
						}
					}

					if (hasSecondBound)
					{
						if (!int.TryParse(bound2Raw, NumberStyles.None, CultureInfo.CurrentCulture, out bound) ||
							bound < lower)
						{
							ThrowInvalidRangeException();
						}
						upper = bound;
					}
					if (lower < 0 ||
						lower <= prevMin ||
						(prevMax != -1 && (lower <= prevMax || upper <= prevMax)))
					{
						ThrowInvalidRangeException();
					}
					// This is always the default cardinality and does not need a constraint.
					// If this is the only range specified, then there will be no callback and
					// the constraint will be deleted. If there is a second range then it necessarily
					// overlaps, so an exception will be thrown.
					if (!(lower == 0 && upper == int.MaxValue))
					{
						callback(new BasicRange(lower, upper));
						retVal = true;
					}
					prevMin = lower;
					prevMax = upper;
					match = match.NextMatch();
				}
				return retVal;
			}
			#endregion // Static functions
		}
		private delegate CardinalityConstraint ConstraintCreator();
		/// <summary>
		/// The default display for an empty cardinality constraint.
		/// </summary>
		public const string DefaultCardinalityDisplay = "0..";
		/// <summary>
		/// Update the cardinality constraint for an <see cref="ObjectType"/>.
		/// </summary>
		/// <param name="objectType">The <see cref="ObjectType"/> to process.</param>
		/// <param name="rangeText">The unparsed range text to parse into cardinality ranges.</param>
		public static void UpdateCardinality(ObjectType objectType, string rangeText)
		{
			if (!UpdateCardinality(
					rangeText,
					objectType.Cardinality,
					delegate()
					{
						ObjectTypeCardinalityConstraint constraint = new ObjectTypeCardinalityConstraint(objectType.Partition);
						constraint.ObjectType = objectType;
						return constraint;
					}))
			{
				objectType.Cardinality = null;
			}
		}
		/// <summary>
		/// Update the cardinality constraint for a <see cref="Role"/>.
		/// </summary>
		/// <param name="role">The <see cref="Role"/> to process.</param>
		/// <param name="rangeText">The unparsed range text to parse into cardinality ranges.</param>
		public static void UpdateCardinality(Role role, string rangeText)
		{
			if (!UpdateCardinality(
					rangeText,
					role.Cardinality,
					delegate()
					{
						UnaryRoleCardinalityConstraint constraint = new UnaryRoleCardinalityConstraint(role.Partition);
						constraint.UnaryRole = role;
						return constraint;
					}))
			{
				role.Cardinality = null;
			}
		}
		private static bool UpdateCardinality(string rangeText, CardinalityConstraint constraint, ConstraintCreator creator)
		{
			LinkedElementCollection<CardinalityRange> ranges;
			int rangeCount;
			Partition partition;
			if (constraint != null)
			{
				rangeCount = (ranges = constraint.RangeCollection).Count;
				partition = constraint.Partition;
			}
			else
			{
				ranges = null;
				rangeCount = 0;
				partition = null;
			}
			int nextRange = 0;
			bool retVal = BasicRange.ParseRangeText(
				rangeText,
				delegate(BasicRange basicRange)
				{
					if (ranges == null)
					{
						ranges = (constraint = creator()).RangeCollection;
						partition = constraint.Partition;
					}
					if (nextRange == rangeCount)
					{
						++rangeCount;
						ranges.Add(new CardinalityRange(partition, new PropertyAssignment(CardinalityRange.LowerBoundDomainPropertyId, basicRange.LowerBound), new PropertyAssignment(CardinalityRange.UpperBoundDomainPropertyId, basicRange.UpperBound)));
					}
					else
					{
						CardinalityRange range = ranges[nextRange];
						range.LowerBound = basicRange.LowerBound;
						range.UpperBound = basicRange.UpperBound;
					}
					++nextRange;
				});
			if (nextRange < rangeCount)
			{
				ranges.RemoveRange(nextRange, rangeCount - nextRange);
			}
			return retVal;
		}
		/// <summary>
		/// Get an editable display of the current ranges in this constraint.
		/// An editable constraint display must be editable with standard keyboard
		/// characters, so we cannot use symbols such as unicode 2264 (less than or equal)
		/// or 2265 (greater than or equal) to express the editable form of the value ranges.
		/// Therefore, we use ..n for a range with a zero lower bound and n.. for a
		/// range with no upper bound. We will also parse &lt;n as ..(n-1) and &lt;=n as ..n,
		/// as well as &gt;n and &gt;=n.
		/// </summary>
		/// <param name="constraint">The constraint to test. If this is not specified
		/// then return the default range.</param>
		/// <returns>The editable range, or the default range of '0..'.</returns>
		public static string GetEditableRangeDisplay(CardinalityConstraint constraint)
		{
			if (constraint != null)
			{
				LinkedElementCollection<CardinalityRange> ranges = constraint.RangeCollection;
				int count = ranges.Count;
				if (count == 1)
				{
					return GetEditableRangeText(ranges[0]);
				}
				else
				{
					string[] rangeFields = new string[count];
					for (int i = 0; i < count; ++i)
					{
						rangeFields[i] = GetEditableRangeText(ranges[i]);
					}
					return string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator + " ", rangeFields);
				}
			}
			return "0..";
		}
		#endregion // Range Parsing
		#region CustomStorage Handling
		private string GetTextValue()
		{
			return GetEditableRangeDisplay(this);
		}
		private void SetTextValue(string value)
		{
			if (!Store.InUndoRedoOrRollback)
			{
				if (!UpdateCardinality(value, this, null))
				{
					this.Delete();
				}
			}
		}
		private long GetTextChangedValue()
		{
			TransactionManager tmgr = Store.TransactionManager;
			if (tmgr.InTransaction)
			{
				// Subtract 1 so that we get a difference in the transaction log
				return unchecked(tmgr.CurrentTransaction.SequenceNumber - 1);
			}
			else
			{
				return 0L;
			}
		}
		private void SetTextChangedValue(long newValue)
		{
			// Nothing to do, we're just trying to create a transaction log entry
		}
		private string GetDefinitionTextValue()
		{
			Definition currentDefinition = Definition;
			return (currentDefinition != null) ? currentDefinition.Text : String.Empty;
		}
		private void SetDefinitionTextValue(string newValue)
		{
			if (!Store.InUndoRedoOrRollback)
			{
				Definition definition = Definition;
				if (definition != null)
				{
					definition.Text = newValue;
				}
				else if (!string.IsNullOrEmpty(newValue))
				{
					Definition = new Definition(Partition, new PropertyAssignment(Definition.TextDomainPropertyId, newValue));
				}
			}
		}
		private string GetNoteTextValue()
		{
			Note currentNote = Note;
			return (currentNote != null) ? currentNote.Text : String.Empty;
		}
		private void SetNoteTextValue(string newValue)
		{
			if (!Store.InUndoRedoOrRollback)
			{
				Note note = Note;
				if (note != null)
				{
					note.Text = newValue;
				}
				else if (!string.IsNullOrEmpty(newValue))
				{
					Note = new Note(Partition, new PropertyAssignment(Note.TextDomainPropertyId, newValue));
				}
			}
		}
		#endregion // CustomStorage Handling
		#region Element equivalence helper
		/// <summary>
		/// Helper method for matching value ranges. Called after an
		/// <see cref="IElementEquivalence"/> implementation has determined
		/// that this <see cref="CardinalityConstraint"/> and <paramref name="otherCardinalityConstraint"/>
		/// correspond to the same instance.
		/// </summary>
		/// <param name="otherCardinalityConstraint">The equivalent value constraint.</param>
		/// <param name="elementTracker">The <see cref="IEquivalentElementTracker"/> used
		/// to equate ranges.</param>
		protected void MatchRanges(CardinalityConstraint otherCardinalityConstraint, IEquivalentElementTracker elementTracker)
		{
			LinkedElementCollection<CardinalityRange> ranges = RangeCollection;
			int rangeCount = ranges.Count;
			if (rangeCount != 0)
			{
				LinkedElementCollection<CardinalityRange> otherRanges = otherCardinalityConstraint.RangeCollection;
				int otherRangeCount = otherRanges.Count;
				if (otherRangeCount != 0)
				{
					BitTracker otherMatches = new BitTracker(otherRangeCount);
					for (int i = 0; i < rangeCount; ++i)
					{
						CardinalityRange range = ranges[i];
						int lower = range.LowerBound;
						int upper = range.UpperBound;
						for (int j = 0; j < otherRangeCount; ++j)
						{
							if (!otherMatches[j])
							{
								CardinalityRange otherRange = otherRanges[j];
								if (otherRange.LowerBound == lower && otherRange.UpperBound == upper)
								{
									elementTracker.AddEquivalentElement(range, otherRange);
									otherMatches[j] = true;
									break;
								}
							}
						}
					}
				}
			}
		}
		#endregion // Element equivalence helper
		#region Rule Methods
		private static void AddDelayedValidation(CardinalityConstraint constraint)
		{
			if (!constraint.IsDeleted)
			{
				if (FrameworkDomainModel.DelayValidateElement(constraint, DelayValidateRanges))
				{
					constraint.OnTextChanged();
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(CardinalityConstraint)
		/// Trigger a text change notification when the modality changes
		/// </summary>
		private static void CardinalityChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == CardinalityConstraint.ModalityDomainPropertyId)
			{
				((CardinalityConstraint)e.ModelElement).OnTextChanged();
			}
		}
		/// <summary>
		/// AddRule: typeof(CardinalityConstraintHasRange)
		/// </summary>
		private static void CardinalityRangeAddedRule(ElementAddedEventArgs e)
		{
			AddDelayedValidation(((CardinalityConstraintHasRange)e.ModelElement).Constraint);
		}
		/// <summary>
		/// ChangeRule: typeof(CardinalityRange)
		/// </summary>
		private static void CardinalityRangeChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid propertyId = e.DomainProperty.Id;
			CardinalityConstraint constraint;
			if ((propertyId == CardinalityRange.UpperBoundDomainPropertyId ||
				propertyId == CardinalityRange.LowerBoundDomainPropertyId) &&
				null != (constraint = ((CardinalityRange)e.ModelElement).CardinalityConstraint))
			{
				AddDelayedValidation(constraint);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(CardinalityConstraintHasRange)
		/// </summary>
		private static void CardinalityRangeDeletedRule(ElementDeletedEventArgs e)
		{
			AddDelayedValidation(((CardinalityConstraintHasRange)e.ModelElement).Constraint);
		}
		/// <summary>
		/// AddRule: typeof(ObjectTypeHasCardinalityConstraint)
		/// </summary>
		private static void ObjectTypeCardinalityAddedRule(ElementAddedEventArgs e)
		{
			AddDelayedValidation(((ObjectTypeHasCardinalityConstraint)e.ModelElement).CardinalityConstraint);
		}
		/// <summary>
		/// AddRule: typeof(UnaryRoleHasCardinalityConstraint)
		/// </summary>
		private static void UnaryRoleCardinalityAddedRule(ElementAddedEventArgs e)
		{
			UnaryRoleHasCardinalityConstraint roleCardinalityLink = (UnaryRoleHasCardinalityConstraint)e.ModelElement;
			AddDelayedValidation(roleCardinalityLink.CardinalityConstraint);
			// Validate unary-only requirement after unary binarization is complete
			FrameworkDomainModel.DelayValidateElement(roleCardinalityLink, DelayValidateRoleCardinalityUnaryOnly);
		}
		private static void DelayValidateRoleCardinalityUnaryOnly(ModelElement element)
		{
			FactType factType;
			Role unaryRole;
			if (!element.IsDeleted &&
				null != (factType = (unaryRole = ((UnaryRoleHasCardinalityConstraint)element).UnaryRole).FactType) &&
				factType.UnaryRole != unaryRole)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionUnaryRoleCardinalityConstraintUnaryOnly);
			}
		}
		private static void DelayValidateRanges(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				((CardinalityConstraint)element).ValidateRanges(null);
			}
		}
		/// <summary>
		/// Verify that ranges are ordered and non-overlapping and handle the
		/// corresponding error.
		/// </summary>
		private void ValidateRanges(INotifyElementAdded notifyAdded)
		{
			LinkedElementCollection<CardinalityRange> ranges = RangeCollection;
			int rangeCount = ranges.Count;
			int prevMin = -1;
			int prevMax = -1;
			bool hasError = false;
			for (int i = 0; i < rangeCount; ++i)
			{
				CardinalityRange range = ranges[i];
				int lower = range.LowerBound;
				int upper = range.UpperBound;
				if (upper == -1)
				{
					upper = int.MaxValue;
				}
				if (lower < 0 ||
					lower <= prevMin ||
					lower <= prevMax ||
					upper <= prevMax)
				{
					hasError = true;
					break;
				}
				prevMin = lower;
				prevMax = upper;
			}
			CardinalityRangeOverlapError overlapError = this.CardinalityRangeOverlapError;
			if (hasError)
			{
				if (overlapError == null)
				{
					overlapError = new CardinalityRangeOverlapError(Partition);
					overlapError.CardinalityConstraint = this;
					overlapError.Model = this.Model;
					overlapError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(overlapError, true);
					}
				}
			}
			else if (overlapError != null)
			{
				overlapError.Delete();
			}
		}
		private void OnTextChanged()
		{
			TransactionManager tmgr = Store.TransactionManager;
			if (tmgr.InTransaction)
			{
				TextChanged = tmgr.CurrentTransaction.SequenceNumber;
			}
		}
		#endregion // Rule Methods
		#region IModelErrorOwner implementation
		/// <summary>
		/// Implements IModelErrorOwner.GetErrorCollection
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				CardinalityRangeOverlapError overlapError = CardinalityRangeOverlapError;
				if (overlapError != null)
				{
					yield return overlapError;
				}
				ConstraintDuplicateNameError duplicateName = DuplicateNameError;
				if (duplicateName != null)
				{
					yield return duplicateName;
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
			// Calls added here need corresponding delayed calls in DelayValidateErrors
			ValidateRanges(notifyAdded);
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
			if (FrameworkDomainModel.DelayValidateElement(this, DelayValidateRanges))
			{
				OnTextChanged();
			}
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner implementation
		#region IDefaultNamePattern Implementation
		/// <summary>
		/// Implements <see cref="IDefaultNamePattern.DefaultNamePattern"/>
		/// Get the standard (empty) default name pattern
		/// </summary>
		protected static string DefaultNamePattern
		{
			get
			{
				return null;
			}
		}
		string IDefaultNamePattern.DefaultNamePattern
		{
			get
			{
				return DefaultNamePattern;
			}
		}
		/// <summary>
		/// Implements <see cref="IDefaultNamePattern.DefaultNameResettable"/> by
		/// marking constraint names as resettable.
		/// </summary>
		protected static bool DefaultNameResettable
		{
			get
			{
				return true;
			}
		}
		bool IDefaultNamePattern.DefaultNameResettable
		{
			get
			{
				return DefaultNameResettable;
			}
		}
		#endregion // IDefaultNamePattern Implementation
		#region CardinalityConstraint specific
		/// <summary>
		/// Get the <see cref="IModelErrorDisplayContext"/> for this <see cref="CardinalityConstraint"/>
		/// </summary>
		public abstract IModelErrorDisplayContext ErrorDisplayContext { get;}
		/// <summary>
		/// The <see cref="ORMModel"/> associated with this constraint.
		/// </summary>
		public abstract ORMModel Model { get;}
		#endregion // CardinalityConstraint specific
	}
	#endregion // CardinalityConstraint class
	#region ObjectTypeCardinalityConstraint class
	partial class ObjectTypeCardinalityConstraint
	{
		#region Base Overrides
		/// <summary>
		/// Provide the model associated with this constraint.
		/// </summary>
		public override ORMModel Model
		{
			get
			{
				ObjectType objectType;
				return (null != (objectType = this.ObjectType)) ? objectType.ResolvedModel : null;
			}
		}
		/// <summary>
		/// Return the display context for the parent object type.
		/// </summary>
		public override IModelErrorDisplayContext ErrorDisplayContext
		{
			get
			{
				return ObjectType;
			}
		}
		#endregion // Base Overrides
	}
	#endregion // ObjectTypeCardinalityConstraint class
	#region UnaryRoleCardinalityConstraint class
	partial class UnaryRoleCardinalityConstraint
	{
		#region Base overrides
		/// <summary>
		/// Provide the model associated with this constraint.
		/// </summary>
		public override ORMModel Model
		{
			get
			{
				Role unaryRole;
				FactType factType;
				return (null != (unaryRole = this.UnaryRole) && null != (factType = unaryRole.FactType)) ? factType.ResolvedModel : null;
			}
		}
		/// <summary>
		/// Return the display context for the parent role.
		/// </summary>
		public override IModelErrorDisplayContext ErrorDisplayContext
		{
			get
			{
				return UnaryRole;
			}
		}
		#endregion // Base overrides
	}
	#endregion // UnaryRoleCardinalityConstraint class
	#region CardinalityRangeOverlapError class
	[ModelErrorDisplayFilter(typeof(ConstraintStructureErrorCategory))]
	partial class CardinalityRangeOverlapError
	{
		#region Base overrides
		/// <summary>
		/// Generate error text using display context information.
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = CardinalityConstraint.ErrorDisplayContext;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorCardinalityConstraintCardinalityRangeOverlapError, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Regenerate error text on owner and model name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange;
			}
		}
		#endregion // Base overrides
	}
	#endregion // CardinalityRangeOverlapError class
}
