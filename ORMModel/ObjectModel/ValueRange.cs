#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
* Copyright � ORM Solutions, LLC. All rights reserved.                     *
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
	partial class ValueRange : IModelErrorOwner, IHasIndirectModelErrorOwner
	{
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
				MaxValueMismatchError max;
				MinValueMismatchError min;
				ValueRangeOutOfRangeError range;
				if (null != (max = MaxValueMismatchError))
				{
					yield return max;
				}
				if (null != (min = MinValueMismatchError))
				{
					yield return min;
				}
				if (null != (range = OutOfRangeError))
				{
					yield return range;
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
		protected static new void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			// Error validation done in ValueConstraint
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		/// <summary>
		/// Implements IModelErrorOwner.DelayValidateErrors
		/// </summary>
		protected static new void DelayValidateErrors()
		{
			// Error validation done in ValueConstraint
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner implementation
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { ValueConstraintHasValueRange.ValueRangeDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region Helper Methods
		/// <summary>
		/// Get the display text for this value range for
		/// the specified <paramref name="dataType"/>
		/// </summary>
		/// <param name="dataType">The <see cref="DataType"/> used to interpret values.</param>
		/// <returns>Formatted string</returns>
		/// <remarks>If an invariant min or max value is available and the culture-specific
		/// value does not match the invariant value for the provided datatype, then display
		/// a parsed form of the invariant value in place of the user-supplied min or max value.</remarks>
		public string GetDisplayText(DataType dataType)
		{
			string minInclusionMark;
			switch (MinInclusion)
			{
				case RangeInclusion.Open:
					minInclusionMark = ResourceStrings.ValueConstraintOpenInclusionLowerDelimiter;
					break;
				case RangeInclusion.Closed:
					minInclusionMark = ResourceStrings.ValueConstraintClosedInclusionLowerDelimiter;
					break;
				default:
					minInclusionMark = "";
					break;
			}
			string maxInclusionMark;
			switch (MaxInclusion)
			{
				case RangeInclusion.Open:
					maxInclusionMark = ResourceStrings.ValueConstraintOpenInclusionUpperDelimiter;
					break;
				case RangeInclusion.Closed:
					maxInclusionMark = ResourceStrings.ValueConstraintClosedInclusionUpperDelimiter;
					break;
				default:
					maxInclusionMark = "";
					break;
			}
			string minValue = MinValue;
			string maxValue = MaxValue;
			if (dataType != null)
			{
				minValue = dataType.NormalizeDisplayText(minValue, InvariantMinValue);
				maxValue = dataType.NormalizeDisplayText(maxValue, InvariantMaxValue);
			}
			bool minExists = minValue.Length != 0;
			bool maxExists = maxValue.Length != 0;

			// Delimit strings if needed
			// Since text is by far the minority of all the data types and
			// the only type to require string container marks, we do not put values inside 
			// those marks by default whenever a data type is not known.
			if (dataType != null && dataType is TextDataType)
			{
				string stringDelimiter = ResourceStrings.ValueConstraintStringDelimiter;
				if (minExists && minValue.Contains(stringDelimiter))
				{
					minValue = minValue.Replace(stringDelimiter, stringDelimiter + stringDelimiter);
				}
				minValue = stringDelimiter + minValue + stringDelimiter;
				if (maxExists && maxValue.Contains(stringDelimiter))
				{
					maxValue = maxValue.Replace(stringDelimiter, stringDelimiter + stringDelimiter);
				}
				maxValue = stringDelimiter + maxValue + stringDelimiter;
			}

			// Assemble values into a value range text
			string valueDelimiter = ResourceStrings.ValueConstraintValueDelimiter;
			if (minExists)
			{
				if (maxExists)
				{
					if (minValue == maxValue)
					{
						return minValue;
					}
					return string.Concat(minInclusionMark, minValue, valueDelimiter, maxValue, maxInclusionMark);
				}
				return string.Concat(minInclusionMark, minValue, valueDelimiter, maxInclusionMark);
			}
			else if (maxExists)
			{
				return string.Concat(minInclusionMark, valueDelimiter, maxValue, maxInclusionMark);
			}
			return minValue;
		}
		private static Regex myValueRangeRegex;
		private static string myValueRangePattern;
		private const string ValueRangeDelimiterGroupName = "RangeDelimiter";
		private const string ValueRangeLowerBoundGroupName = "MinValue";
		private const string ValueRangeUpperBoundGroupName = "MaxValue";
		private const string ValueRangeMinClosedInclusionGroupName = "MinClosedInclusion";
		private const string ValueRangeMaxClosedInclusionGroupName = "MaxClosedInclusion";
		private const string ValueRangeMinOpenInclusionGroupName = "MinOpenInclusion";
		private const string ValueRangeMaxOpenInclusionGroupName = "MaxOpenInclusion";
		/// <summary>
		/// A regular expression for extracting all parts of a value range
		/// </summary>
		private static Regex ValueRangeRegex
		{
			get
			{
				string valueRangePattern = ResourceStrings.ValueConstraintValueRangeRegexPattern;
				Regex retVal = myValueRangeRegex;
				if (valueRangePattern != myValueRangePattern)
				{
					Regex newRegex = new Regex(valueRangePattern, RegexOptions.Compiled);
					if ((object)retVal == System.Threading.Interlocked.CompareExchange<Regex>(ref myValueRangeRegex, newRegex, retVal))
					{
						// We used the current values, store them
						myValueRangePattern = valueRangePattern;
					}
					retVal = myValueRangeRegex;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Interpret value range text and create or update a corresponding <see cref="ValueRange"/> object.
		/// </summary>
		/// <param name="contextConstraint">The <see cref="ValueConstraint"/> context. If the <paramref name="existingValueRange"/>
		/// is not set and parsing succeeds, then a new range is created and added to the context constraint.</param>
		/// <param name="existingValueRange">An existing range to modify. If the text cannot be parsed, then the existing range is deleted.</param>
		/// <param name="rangeText">The text to interpret.</param>
		/// <returns>A <see cref="ValueRange"/>, or <see langword="null"/> if the text cannot be parsed.</returns>
		public static ValueRange ParseValueRangeText(ValueConstraint contextConstraint, ValueRange existingValueRange, string rangeText)
		{
			Match rangeMatch = ValueRangeRegex.Match(rangeText);
			bool invalid = true;
			if (rangeMatch.Success)
			{
				invalid = false;
				string minValue = "";
				string maxValue = "";
				RangeInclusion minInclusion = RangeInclusion.NotSet;
				RangeInclusion maxInclusion = RangeInclusion.NotSet;
				GroupCollection groups = rangeMatch.Groups;
				if (rangeMatch.Groups[ValueRangeDelimiterGroupName].Length != 0)
				{
					minValue = TrimStringMarkers(groups[ValueRangeLowerBoundGroupName].Value);
					maxValue = TrimStringMarkers(groups[ValueRangeUpperBoundGroupName].Value);
					bool haveMin = minValue.Length != 0;
					bool haveMax = maxValue.Length != 0;
					if (!haveMin && !haveMax)
					{
						// Treat as a single empty value, no reason to check inclusion
						if (!contextConstraint.IsText)
						{
							invalid = true;
						}
					}
					else
					{
						if (haveMin)
						{
							if (groups[ValueRangeMinOpenInclusionGroupName].Length != 0)
							{
								minInclusion = RangeInclusion.Open;
							}
							else if (groups[ValueRangeMinClosedInclusionGroupName].Length != 0)
							{
								minInclusion = RangeInclusion.Closed;
							}
						}
						if (haveMax)
						{
							if (groups[ValueRangeMaxOpenInclusionGroupName].Length != 0)
							{
								maxInclusion = RangeInclusion.Open;
							}
							else if (groups[ValueRangeMaxClosedInclusionGroupName].Length != 0)
							{
								maxInclusion = RangeInclusion.Closed;
							}
						}
					}
				}
				else
				{
					// Do not check inclusion on single elements
					minValue = maxValue = TrimStringMarkers(groups[ValueRangeLowerBoundGroupName].Value);
					if (minValue.Length == 0 && !contextConstraint.IsText)
					{
						invalid = true;
					}
				}
				if (!invalid)
				{
					bool newRange = existingValueRange == null;
					if (newRange)
					{
						existingValueRange = new ValueRange(contextConstraint.Partition);
					}
					existingValueRange.MinValue = minValue;
					existingValueRange.MaxValue = maxValue;
					existingValueRange.MinInclusion = minInclusion;
					existingValueRange.MaxInclusion = maxInclusion;
					if (newRange)
					{
						existingValueRange.ValueConstraint = contextConstraint;
					}
				}
			}
			if (invalid && existingValueRange != null)
			{
				existingValueRange.Delete();
				existingValueRange = null;
			}
			return existingValueRange;
		}
		/// <summary>
		/// Removes the left- and right-strings which denote a value as a string.
		/// </summary>
		/// <param name="value">The string to remove string delimiters from.</param>
		private static string TrimStringMarkers(string value)
		{
			string stringDelimiter = ResourceStrings.ValueConstraintStringDelimiter;
			value = value.Trim();
			if (value.StartsWith(stringDelimiter) && value.EndsWith(stringDelimiter))
			{
				int stringDelimiterLength = stringDelimiter.Length;
				value = value.Substring(stringDelimiterLength, value.Length - stringDelimiterLength - stringDelimiterLength);
				if (!string.IsNullOrEmpty(value) && value.Contains(stringDelimiter))
				{
					// Treat doubled delimiter characters as a single character.
					value = value.Replace(stringDelimiter + stringDelimiter, stringDelimiter);
				}
			}
			return value;
		}
		#endregion // Helper Methods
	}
	partial class ValueConstraint : IModelErrorOwner, IDefaultNamePattern
	{
		#region CustomStorage Handling
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
		private string GetDefinitionTextValue()
		{
			Definition currentDefinition = Definition;
			return (currentDefinition != null) ? currentDefinition.Text : String.Empty;
		}
		private string GetNoteTextValue()
		{
			Note currentNote = Note;
			return (currentNote != null) ? currentNote.Text : String.Empty;
		}
		#endregion // CustomStorage Handling
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
			LinkedElementCollection<ValueRange> ranges = ValueRangeCollection;
			int rangeCount = ranges.Count;
			for (int i = 0; i < rangeCount; ++i)
			{
				foreach (ModelErrorUsage rangeError in ((IModelErrorOwner)ranges[i]).GetErrorCollection(filter))
				{
					yield return rangeError;
				}
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				ConstraintDuplicateNameError duplicateName = DuplicateNameError;
				if (duplicateName != null)
				{
					yield return duplicateName;
				}

				ValueRangeOverlapError overlap;
				if (null != (overlap = ValueRangeOverlapError))
				{
					yield return overlap;
				}

				ValueConstraintValueTypeDetachedError detachedError;
				if (null != (detachedError = ValueTypeDetachedError))
				{
					yield return detachedError;
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
			VerifyValueRangeValues(notifyAdded);
			VerifyValueRangeOverlapError(notifyAdded);
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
			if (FrameworkDomainModel.DelayValidateElement(this, DelayValidateValueRangeValues))
			{
				OnTextChanged();
			}
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateValueRangeOverlapError);
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
		#region ValueMatch Validation
		/// <summary>
		/// Determine if the range endpoints satisfy the data type and manage mismatch errors
		/// </summary>
		/// <param name="range">The range to validate</param>
		/// <param name="dataType">The context data type</param>
		/// <param name="contextValueConstraint">The nearest context value constraint.</param>
		/// <param name="notifyAdded">Set during deserialization</param>
		private static void VerifyValueMatch(ValueRange range, DataType dataType, ValueConstraint contextValueConstraint, INotifyElementAdded notifyAdded)
		{
			bool needMinError = false;
			bool needMaxError = false;
			bool needRangeError = false;
			MinValueMismatchError minMismatch;
			MaxValueMismatchError maxMismatch;
			ValueRangeOutOfRangeError outOfRange;
			if (dataType != null)
			{
				string min = range.MinValue;
				bool haveMin = min.Length != 0;
				string max = range.MaxValue;
				bool haveMax = max.Length != 0;
				string normalizedParsedForm = "";
				if ((haveMin && !dataType.ParseNormalizeValue(min, range.InvariantMinValue, out normalizedParsedForm)) ||
					(!haveMin && !haveMax && !dataType.CanParseAnyValue))
				{
					needMinError = true;
					minMismatch = range.MinValueMismatchError;
					if (minMismatch == null)
					{
						minMismatch = new MinValueMismatchError(range.Partition);
						minMismatch.ValueRange = range;
						minMismatch.Model = dataType.Model;
						minMismatch.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(minMismatch, true);
						}
					}
					else
					{
						minMismatch.GenerateErrorText();
					}
				}
				else if (contextValueConstraint != null)
				{
					needRangeError = !contextValueConstraint.IsInRange(normalizedParsedForm, dataType);
				}

				if (min != max)
				{
					normalizedParsedForm = "";
					if (haveMax && !dataType.ParseNormalizeValue(max, range.InvariantMaxValue, out normalizedParsedForm))
					{
						needMaxError = true;
						maxMismatch = range.MaxValueMismatchError;
						if (maxMismatch == null)
						{
							maxMismatch = new MaxValueMismatchError(range.Partition);
							maxMismatch.ValueRange = range;
							maxMismatch.Model = dataType.Model;
							maxMismatch.GenerateErrorText();
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(maxMismatch, true);
							}
						}
						else
						{
							maxMismatch.GenerateErrorText();
						}
					}
					else if (!needRangeError && contextValueConstraint != null)
					{
						needRangeError = !contextValueConstraint.IsInRange(normalizedParsedForm, dataType);
					}
				}
			}
			if (!needMinError && null != (minMismatch = range.MinValueMismatchError))
			{
				minMismatch.Delete();
			}
			if (!needMaxError && null != (maxMismatch = range.MaxValueMismatchError))
			{
				maxMismatch.Delete();
			}

			if (needRangeError)
			{
				outOfRange = range.OutOfRangeError;
				if (outOfRange == null)
				{
					outOfRange = new ValueRangeOutOfRangeError(range.Partition);
					outOfRange.ValueRange = range;
					outOfRange.Model = dataType.Model;
					outOfRange.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(outOfRange, true);
					}
				}
				else
				{
					outOfRange.GenerateErrorText();
				}
			}
			else if (null != (outOfRange = range.OutOfRangeError))
			{
				outOfRange.Delete();
			}
		}
		/// <summary>
		/// Register descended value constraints and values for validation when
		/// a value constraint or default value changes.
		/// </summary>
		/// <param name="element">A <see cref="Role"/> or <see cref="ObjectType"/> with a modified value constraint or default value.</param>
		private static void DelayValidateDescendedValues(ModelElement element)
		{
			if (element.IsDeleted)
			{
				return;
			}
			ObjectType valueType;
			Role role;
			if (null != (valueType = element as ObjectType))
			{
				role = null;
			}
			else if (null != (role = element as Role))
			{
				valueType = role.ValueRoleValueType;
			}

			if (valueType != null)
			{
				// This could be slightly more efficient if we validate from a specific role.
				// However, a role needs to reverse walk value roles anyway to resolve the default
				// value and value constraints, so there is little benefit on starting part way down
				// a chain that is rarely more than a couple of links long.
				DelayValidateDescendedValueConstraints(valueType, null, null, false);
			}
		}
		/// <summary>
		/// Synchronize the <see cref="ValueRange.InvariantMinValue"/> and <see cref="ValueRange.InvariantMaxValue"/>
		/// values for a modified data type. If neither the culture-specific nor invariant form of the data matches then
		/// do not toss either piece of data.
		/// </summary>
		[DelayValidatePriority(1)]
		private static void DelayValidateInvariantValues(ModelElement element)
		{
			ValueConstraint valueConstraint;
			DataType dataType;
			if (element.IsDeleted ||
				null == (dataType = (valueConstraint = (ValueConstraint)element).DataType))
			{
				return;
			}
			bool updateInvariant = !dataType.CanParseAnyValue && dataType.IsCultureSensitive;
			foreach (ValueRange valueRange in valueConstraint.ValueRangeCollection)
			{
				if (updateInvariant)
				{
					string value = valueRange.MinValue;
					if (dataType.ParseNormalizeValue(value, valueRange.InvariantMinValue, out value))
					{
						valueRange.InvariantMinValue = value;
					}
					value = valueRange.MaxValue;
					if (dataType.ParseNormalizeValue(value, valueRange.InvariantMaxValue, out value))
					{
						valueRange.InvariantMaxValue = value;
					}
				}
				else
				{
					valueRange.InvariantMinValue = "";
					valueRange.InvariantMaxValue = "";
				}
			}
		}
		/// <summary>
		/// Synchronize <see cref="ObjectType.InvariantDefaultValue"/> values and downstream
		/// <see cref="Role.InvariantDefaultValue"/> settings for a modified data type.
		/// If neither the culture-specific nor invariant form of the data matches then do not toss
		/// either piece of data.
		/// </summary>
		[DelayValidatePriority(1)]
		private static void DelayValidateValueTypeInvariantDefaultValues(ModelElement element)
		{
			ObjectType valueType;
			DataType dataType;
			if (element.IsDeleted ||
				null == (dataType = (valueType = (ObjectType)element).DataType))
			{
				return;
			}

			ValueRoleVisitor roleVisitor;
			if (!dataType.CanParseAnyValue && dataType.IsCultureSensitive)
			{
				string value = valueType.DefaultValue;
				if (dataType.ParseNormalizeValue(value, valueType.InvariantDefaultValue, out value))
				{
					valueType.InvariantDefaultValue = value;
				}

				roleVisitor = delegate(Role role, PathedRole pathedRole, RolePathObjectTypeRoot pathRoot, ValueTypeHasDataType dataTypeLink, ValueConstraint currentValueConstraint, ValueConstraint previousValueConstraint, string defaultValue)
				{
					if (role != null)
					{
						role.InvariantDefaultValue = "";
					}
					return true;
				};
			}
			else
			{
				valueType.InvariantDefaultValue = "";
				roleVisitor = delegate(Role role, PathedRole pathedRole, RolePathObjectTypeRoot pathRoot, ValueTypeHasDataType dataTypeLink, ValueConstraint currentValueConstraint, ValueConstraint previousValueConstraint, string defaultValue)
				{
					if (role != null)
					{
						string value = valueType.DefaultValue;
						if (dataType.ParseNormalizeValue(value, role.InvariantDefaultValue, out value))
						{
							role.InvariantDefaultValue = value;
						}
					}
					return true;
				};
			}

			Role.WalkDescendedValueRoles(valueType, null, null, roleVisitor);
		}
		[DelayValidatePriority(2)] // We want this after reference scheme validation and invariant value validation
		private static void DelayValidateValueRangeValues(ModelElement element)
		{
			((ValueConstraint)element).VerifyValueRangeValues(null);
		}

		[DelayValidatePriority(4)] // We want this after all value constraints are verified
		private static void DelayValidateValueTypeDefaultValue(ModelElement element)
		{
			ValidateValueTypeDefaultValue((ObjectType)element, null);
		}

		[DelayValidatePriority(4)] // We want this after all value constraints are verified
		private static void DelayValidateRoleDefaultValue(ModelElement element)
		{
			ValidateRoleDefaultValue((Role)element, null);
		}

		/// <summary>
		/// Verify the default value state, adding and removing errors as apropriate
		/// </summary>
		public static void ValidateValueTypeDefaultValue(ObjectType valueType, INotifyElementAdded notifyAdded)
		{
			if (valueType.IsDeleted)
			{
				return;
			}

			DataType dataType = valueType.DataType;
			DefaultValueMismatchError mismatchError = valueType.DefaultValueMismatchError;
			DefaultValueOutOfRangeError rangeError = valueType.DefaultValueOutOfRangeError;
			bool hasMismatchError = false;
			bool hasRangeError = false;

			if (dataType != null)
			{
				string defaultValue = valueType.ResolvedDirectDefaultValue;
				if (defaultValue != null)
				{
					hasMismatchError = !dataType.ParseNormalizeValue(defaultValue, valueType.InvariantDefaultValue, out defaultValue);

					ValueConstraint valueConstraint;
					if (!hasMismatchError && null != (valueConstraint = valueType.ValueConstraint))
					{
						hasRangeError = !valueConstraint.IsInRange(defaultValue, dataType);
					}
				}
			}

			if (hasMismatchError)
			{
				if (mismatchError == null)
				{
					mismatchError = new DefaultValueMismatchError(valueType.Partition);
					mismatchError.ValueType = valueType;
					mismatchError.Model = valueType.Model;
					mismatchError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(mismatchError, true);
					}
				}
				else if (notifyAdded != null)
				{
					mismatchError.GenerateErrorText();
				}
			}
			else if (mismatchError != null)
			{
				mismatchError.Delete();
			}

			if (hasRangeError)
			{
				if (rangeError == null)
				{
					rangeError = new DefaultValueOutOfRangeError(valueType.Partition);
					rangeError.ValueType = valueType;
					rangeError.Model = valueType.Model;
					rangeError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(rangeError, true);
					}
				}
				else if (notifyAdded != null)
				{
					rangeError.GenerateErrorText();
				}
			}
			else if (rangeError != null)
			{
				rangeError.Delete();
			}
		}

		/// <summary>
		/// Verify the default value state, adding and removing errors as apropriate
		/// </summary>
		public static void ValidateRoleDefaultValue(Role role, INotifyElementAdded notifyAdded)
		{
			if (role.IsDeleted)
			{
				return;
			}

			FactType factType = role.FactType;
			bool hasMismatchError = false;
			bool hasRangeError = false;
			bool hasDetachedError = false;
			DefaultValueMismatchError mismatchError = role.DefaultValueMismatchError;
			DefaultValueOutOfRangeError rangeError = role.DefaultValueOutOfRangeError;
			DefaultValueValueTypeDetachedError detachedError = role.DefaultValueValueTypeDetachedError;

			// Unary defaults are managed implicitly by responding to changes in the UnaryPattern property.
			// They are also displayed as read only. It is not worth doing extra validation on this. If
			// we want to add it, use factType.ResolvedModel.GetPortableDataType(PortableDataType.LogicalTrueOrFalse)
			// to retrieve the data type to validate the default values.
			if (factType.UnaryPattern == UnaryValuePattern.NotUnary)
			{
				ObjectType valueType = role.ValueRoleValueType;
				DataType dataType;
				if (valueType == null)
				{
					hasDetachedError = role.ResolvedDefaultValue != null;
				}
				else if (null != (dataType = valueType.DataType))
				{
					string defaultValue = role.ResolvedDefaultValue;
					if (defaultValue != null)
					{
						hasMismatchError = !dataType.ParseNormalizeValue(defaultValue, role.InvariantDefaultValue, out defaultValue);

						ValueConstraint valueConstraint;
						if (!hasMismatchError && null != (valueConstraint = role.NearestAlethicValueConstraint))
						{
							hasRangeError = !valueConstraint.IsInRange(defaultValue, dataType);
						}
					}
				}
			}

			ORMModel model = null;
			if (hasMismatchError)
			{
				if (mismatchError == null)
				{
					mismatchError = new DefaultValueMismatchError(role.Partition);
					mismatchError.Role = role;
					mismatchError.Model = model ?? (model = role.FactType.ResolvedModel);
					mismatchError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(mismatchError, true);
					}
				}
				else if (notifyAdded != null)
				{
					mismatchError.GenerateErrorText();
				}
			}
			else if (mismatchError != null)
			{
				mismatchError.Delete();
			}

			if (hasRangeError)
			{
				if (rangeError == null)
				{
					rangeError = new DefaultValueOutOfRangeError(role.Partition);
					rangeError.Role = role;
					rangeError.Model = model ?? (model = role.FactType.ResolvedModel);
					rangeError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(rangeError, true);
					}
				}
				else if (notifyAdded != null)
				{
					rangeError.GenerateErrorText();
				}
			}
			else if (rangeError != null)
			{
				rangeError.Delete();
			}

			if (hasDetachedError)
			{
				if (detachedError == null)
				{
					detachedError = new DefaultValueValueTypeDetachedError(role.Partition);
					detachedError.Role = role;
					detachedError.Model = model ?? role.FactType.ResolvedModel;
					detachedError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(detachedError, true);
					}
				}
				else if (notifyAdded != null)
				{
					detachedError.GenerateErrorText();
				}
			}
			else if (detachedError != null)
			{
				detachedError.Delete();
			}
		}

		/// <summary>
		/// Determine if value ranges are valid and manage range and value type detached errors.
		/// </summary>
		private void VerifyValueRangeValues(INotifyElementAdded notifyAdded)
		{
			if (IsDeleted)
			{
				return;
			}
			DataType dataType = this.DataType;
			bool hasError = true;
			if (dataType != null)
			{
				hasError = false;
				ValueConstraint contextValueConstraint = null;
				RoleValueConstraint roleConstraint;
				if (null != (roleConstraint = this as RoleValueConstraint))
				{
					ValueConstraint deonticDummy;
					contextValueConstraint = roleConstraint.Role.RolePlayer.GetNearestValueConstraint(out deonticDummy);
				}
				LinkedElementCollection<ValueRange> ranges = ValueRangeCollection;
				int rangesCount = ranges.Count;
				for (int i = 0; i < rangesCount; ++i)
				{
					VerifyValueMatch(ranges[i], dataType, contextValueConstraint, notifyAdded);
				}
			}
			else if (this is ValueTypeValueConstraint)
			{
				// UNDONE: When we allow ValueConstraints directly on entity types then
				// this will change
				hasError = false;
			}

			ValueConstraintValueTypeDetachedError error = ValueTypeDetachedError;
			if (hasError)
			{
				if (error == null)
				{
					error = new ValueConstraintValueTypeDetachedError(Partition);
					error.ValueConstraint = this;
					error.Model = Model;
					error.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(error, true);
					}
				}
				else
				{
					error.GenerateErrorText();
				}
			}
			else if (error != null)
			{
				error.Delete();
			}
		}
		/// <summary>
		/// Helper function to validate value type constraints
		/// </summary>
		private static void DelayValidateAssociatedValueConstraints(ObjectType valueType)
		{
			if (valueType.IsDeleted)
			{
				return;
			}
			DelayValidateValueConstraint(valueType.ValueConstraint, true, false);
			DelayValidateDefaultValue(valueType, true, false);
			Role.WalkDescendedValueRoles(valueType, null, null, delegate(Role role, PathedRole pathedRole, RolePathObjectTypeRoot pathRoot, ValueTypeHasDataType dataTypeLink, ValueConstraint currentValueConstraint, ValueConstraint previousValueConstraint, string defaultValue)
			{
				DelayValidateValueConstraint(currentValueConstraint, true, false);
				if (pathedRole == null && pathRoot == null)
				{
					if (defaultValue != null)
					{
						DelayValidateDefaultValue(role, true, false);
					}
					ValueComparisonConstraint.DelayValidateAttachedConstraint(role);
				}
				return true;
			});
		}
		/// <summary>
		/// Validate errors on the specified value constraint
		/// </summary>
		/// <param name="valueConstraint">A <see cref="ValueConstraint"/> to validate. Can be null.</param>
		/// <param name="checkForDataTypeChange"><see langword="true"/> if the data type of the associated constraint
		/// may have changed.</param>
		/// <param name="checkDescendants">Validate downstream constraints and default values.</param>
		public static void DelayValidateValueConstraint(ValueConstraint valueConstraint, bool checkForDataTypeChange, bool checkDescendants)
		{
			if (valueConstraint != null && !valueConstraint.IsDeleting && !valueConstraint.IsDeleted)
			{
				bool updateText = false;
				if (checkForDataTypeChange &&
					FrameworkDomainModel.DelayValidateElement(valueConstraint, DelayValidateInvariantValues))
				{
					updateText = true;
				}
				if (FrameworkDomainModel.DelayValidateElement(valueConstraint, DelayValidateValueRangeValues))
				{
					updateText = true;
				}
				if (updateText)
				{
					// Add a text change the first time this is called
					valueConstraint.OnTextChanged();
				}
				FrameworkDomainModel.DelayValidateElement(valueConstraint, DelayValidateValueRangeOverlapError);
				if (checkDescendants)
				{
					ModelElement testElement = null;
					RoleValueConstraint roleValueConstraint;
					ValueTypeValueConstraint valueTypeValueConstraint;
					PathConditionRoleValueConstraint pathedRoleConstraint;
					PathConditionRootValueConstraint pathRootConstraint;
					if (null != (roleValueConstraint = valueConstraint as RoleValueConstraint))
					{
						testElement = roleValueConstraint.Role;
					}
					else if (null != (valueTypeValueConstraint = valueConstraint as ValueTypeValueConstraint))
					{
						testElement = valueTypeValueConstraint.ValueType;
					}
					else if (null != (pathedRoleConstraint = valueConstraint as PathConditionRoleValueConstraint))
					{
						testElement = pathedRoleConstraint.PathedRole.Role;
					}
					else if (null != (pathRootConstraint = valueConstraint as PathConditionRootValueConstraint))
					{
						ObjectType objectType = pathRootConstraint.PathRoot.RootObjectType;
						if (objectType.DataType != null)
						{
							testElement = objectType;
						}
						else
						{
							testElement = objectType.EntityTypeIdentifyingValueType;
						}
					}

					if (testElement != null)
					{
						FrameworkDomainModel.DelayValidateElement(testElement, DelayValidateDescendedValues);
					}
				}
			}
		}
		/// <summary>
		/// Validate default value errors for a value type
		/// </summary>
		/// <param name="valueType">An <see cref="ObjectType"/> to validate. Can be null.</param>
		/// <param name="checkForDataTypeChange"><see langword="true"/> if the data type of the value type
		/// may have changed.</param>
		/// <param name="checkDescendants">Validate downstream constraints and default values.</param>
		/// <remarks>Default value processing is very similar to value constraint processing so is
		/// maintained in this file.</remarks>
		public static void DelayValidateDefaultValue(ObjectType valueType, bool checkForDataTypeChange, bool checkDescendants)
		{
			if (valueType != null && !valueType.IsDeleted && !valueType.IsDeleting)
			{
				if (checkForDataTypeChange)
				{
					FrameworkDomainModel.DelayValidateElement(valueType, DelayValidateValueTypeInvariantDefaultValues);
				}
				FrameworkDomainModel.DelayValidateElement(valueType, DelayValidateValueTypeDefaultValue);
				if (checkDescendants)
				{
					FrameworkDomainModel.DelayValidateElement(valueType, DelayValidateDescendedValues);
				}
			}
		}
		/// <summary>
		/// Validate default value errors for a role
		/// </summary>
		/// <param name="role">An <see cref="Role"/> to validate. Can be null.</param>
		/// <param name="checkInvariant">The invariant value should be validated. This should
		/// be set if the value itself changed or the data type changed</param>
		/// <param name="checkDescendants">Validate downstream constraints and default values.</param>
		public static void DelayValidateDefaultValue(Role role, bool checkInvariant, bool checkDescendants)
		{
			if (role != null && !role.IsDeleted && !role.IsDeleting)
			{
				if (checkInvariant)
				{
					ObjectType valueType;
					DataType dataType;
					if (null != (valueType = role.ValueRoleValueType) &&
						null != (dataType = valueType.DataType))
					{
						string newDefault = role.DefaultValue;
						string invariantDefault = null;
						if (!string.IsNullOrEmpty(newDefault) && !dataType.CanParseAnyValue && dataType.IsCultureSensitive)
						{
							dataType.TryConvertToInvariant(newDefault, out invariantDefault);
						}
						role.InvariantDefaultValue = invariantDefault ?? "";
					}
				}

				FrameworkDomainModel.DelayValidateElement(role, DelayValidateRoleDefaultValue);
				if (checkDescendants)
				{
					FrameworkDomainModel.DelayValidateElement(role, DelayValidateDescendedValues);
				}
			}
		}
		/// <summary>
		/// Force validation of downstream value constraints. Helper function, calls
		/// <see cref="Role.WalkDescendedValueRoles(ObjectType, Role, UniquenessConstraint, ValueRoleVisitor)"/>
		/// </summary>
		/// <param name="anchorType">The <see cref="ObjectType"/> to begin walking from</param>
		/// <param name="unattachedRole">The alternate role</param>
		/// <param name="unattachedPreferredIdentifier">The alternate preferred identifier</param>
		/// <param name="checkForDataTypeChange"><see langword="true"/> if the data type of the associated constraint
		/// may have changed.</param>
		private static void DelayValidateDescendedValueConstraints(ObjectType anchorType, Role unattachedRole, UniquenessConstraint unattachedPreferredIdentifier, bool checkForDataTypeChange)
		{
			if (anchorType.ResolvedDirectDefaultValue != null && anchorType.ValueConstraint != null)
			{
				// For validation purposed, the default value of the anchor type is a
				// descendant of a value constraint on the value type.
				DelayValidateDefaultValue(anchorType, true, false);
			}
			Role.WalkDescendedValueRoles(anchorType, unattachedRole, unattachedPreferredIdentifier, delegate(Role role, PathedRole pathedRole, RolePathObjectTypeRoot pathRoot, ValueTypeHasDataType dataTypeLink, ValueConstraint currentValueConstraint, ValueConstraint previousValueConstraint, string defaultValue)
			{
				DelayValidateValueConstraint(currentValueConstraint, checkForDataTypeChange, false);
				if (pathedRole == null && pathRoot == null)
				{
					if (defaultValue != null)
					{
						DelayValidateDefaultValue(role, true, false);
					}
					ValueComparisonConstraint.DelayValidateAttachedConstraint(role);
				}
				return true; // Continue walking
			});
		}
		#region DataTypeRolePlayerChangeRule
		/// <summary>
		/// RolePlayerChangeRule: typeof(ValueTypeHasDataType)
		/// When the DataType is changed, recheck the instance values
		/// </summary>
		private static void DataTypeRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ValueTypeHasDataType.DataTypeDomainRoleId)
			{
				DelayValidateAssociatedValueConstraints(((ValueTypeHasDataType)e.ElementLink).ValueType);
			}
		}
		#endregion // DataTypeRolePlayerChangeRule
		#region DataTypeDeletingRule
		/// <summary>
		/// DeletingRule: typeof(ValueTypeHasDataType)
		/// If a DataType is being cleared from an ObjectType that is
		/// not being deleted, and the value type has downstream value roles,
		/// then create a new value type with that data type and set it as the
		/// reference mode for the object type.
		/// </summary>
		private static void DataTypeDeletingRule(ElementDeletingEventArgs e)
		{
			ValueTypeHasDataType link = (ValueTypeHasDataType)e.ModelElement;
			ObjectType oldValueType = link.ValueType;
			if (!oldValueType.IsDeleting)
			{
				ValueTypeHasValueConstraint valueTypeConstraintLink = ValueTypeHasValueConstraint.GetLinkToValueConstraint(oldValueType);
				bool hasValueConstraintOrDefaultValue = valueTypeConstraintLink != null || oldValueType.ResolvedDirectDefaultValue != null;
				if (!hasValueConstraintOrDefaultValue)
				{
					Role.WalkDescendedValueRoles(oldValueType, null, null, delegate(Role role, PathedRole pathedRole, RolePathObjectTypeRoot pathRoot, ValueTypeHasDataType dataTypeLink, ValueConstraint currentValueConstraint, ValueConstraint previousValueConstraint, string defaultValue)
					{
						if ((currentValueConstraint != null && !currentValueConstraint.IsDeleting) || defaultValue != null)
						{
							hasValueConstraintOrDefaultValue = true;
							return false; // Stop walking
						}
						return true;
					});
				}
				if (hasValueConstraintOrDefaultValue)
				{
					// Convert this value type into an entity type with a reference mode
					IHasAlternateOwner<ObjectType> toAlternateOwner;
					IAlternateElementOwner<ObjectType> alternateOwner = (null == (toAlternateOwner = oldValueType as IHasAlternateOwner<ObjectType>)) ? null : toAlternateOwner.AlternateOwner;
					DomainClassInfo alternateCtor = (alternateOwner != null) ? alternateOwner.GetOwnedElementClassInfo(typeof(ObjectType)) : null;
					ORMModel model = (alternateCtor == null) ? oldValueType.Model : null;
					if (alternateCtor != null || model != null)
					{
						// Get a unique name for a new value type
						INamedElementDictionaryOwner dictionaryOwner;
						INamedElementDictionary objectsDictionary;
						if (null == alternateCtor ||
							null == (dictionaryOwner = alternateOwner as INamedElementDictionaryOwner) ||
							null == (objectsDictionary = dictionaryOwner.FindNamedElementDictionary(typeof(ObjectType))))
						{
							objectsDictionary = model.ObjectTypesDictionary;
						}
						string newNamePattern = ResourceStrings.ValueTypeAutoCreateReferenceModeNamePattern;
						string baseName = oldValueType.Name;
						string newName = null;
						int i = 0;
						do
						{
							newName = string.Format(CultureInfo.InvariantCulture, newNamePattern, baseName, (i == 0) ? "" : i.ToString(CultureInfo.InvariantCulture));
							++i;
						} while (!objectsDictionary.GetElement(newName).IsEmpty);

						// Create the value type and attach it to a clone of the deleting datatype link
						Partition partition = oldValueType.Partition;
						PropertyAssignment[] assignments = new PropertyAssignment[] { new PropertyAssignment(ORMNamedElement.NameDomainPropertyId, newName) };
						ObjectType newValueType;
						if (alternateCtor != null)
						{
							// Create the object type subtype for this owner and attach it to the owner.
							newValueType = (ObjectType)partition.ElementFactory.CreateElement(alternateCtor);
							((IHasAlternateOwner<ObjectType>)newValueType).AlternateOwner = alternateOwner;
						}
						else
						{
							// Create a standard object type and attach it to the model
							newValueType = new ObjectType(partition, assignments);
							newValueType.Model = model;
						}

						// Set facet properties
						ValueTypeHasDataType newDataTypeLink = new ValueTypeHasDataType(
							partition,
							new RoleAssignment[] { new RoleAssignment(ValueTypeHasDataType.ValueTypeDomainRoleId, newValueType), new RoleAssignment(ValueTypeHasDataType.DataTypeDomainRoleId, link.DataType) },
							new PropertyAssignment[] { new PropertyAssignment(ValueTypeHasDataType.ScaleDomainPropertyId, link.Scale), new PropertyAssignment(ValueTypeHasDataType.LengthDomainPropertyId, link.Length) });

						// Transfer default value settings
						switch (oldValueType.DefaultState)
						{
							case DefaultValueState.UseValue:
								newValueType.DefaultValue = oldValueType.DefaultValue;
								break;
							case DefaultValueState.EmptyValue:
								newValueType.DefaultState = DefaultValueState.EmptyValue;
								break;
						}

						// Change the old ValueTypeValueConstraint to a new RoleValueConstraint
						RoleValueConstraint newValueConstraint = null;
						string preserveValueConstraintName = null;
						if (valueTypeConstraintLink != null)
						{
							// Move all links except the aggregating link from the old value constraint to
							// the new one. Note that this will also take care of moving any presentation elements
							ValueConstraint oldValueConstraint = valueTypeConstraintLink.ValueConstraint;
							preserveValueConstraintName = oldValueConstraint.Name;
							newValueConstraint = new RoleValueConstraint(partition);
							if (System.Text.RegularExpressions.Regex.IsMatch(preserveValueConstraintName, System.ComponentModel.TypeDescriptor.GetClassName(oldValueConstraint) + @"\d+"))
							{
								preserveValueConstraintName = null;
							}

							foreach (DomainRoleInfo roleInfo in oldValueConstraint.GetDomainClass().AllDomainRolesPlayed)
							{
								if (!roleInfo.OppositeDomainRole.IsEmbedding && roleInfo.Id != ValueConstraintHasDuplicateNameError.ValueConstraintDomainRoleId)
								{
									foreach (ElementLink transferLink in roleInfo.GetElementLinks(oldValueConstraint))
									{
										roleInfo.SetRolePlayer(transferLink, newValueConstraint);
									}
								}
							}

							// No we've pulled all useful information from the old value constraint we can go ahead
							// and delete it. Deleting the link will propagate to delete the constraint.
							valueTypeConstraintLink.Delete();
						}

						// Setting the ReferenceModeString property will do the bulk of the work
						oldValueType.ReferenceModeString = newName;

						if (newValueConstraint != null)
						{
							// Attach the new constraint to the appropriate opposite role
							newValueConstraint.Role = oldValueType.PreferredIdentifier.RoleCollection[0];
							if (preserveValueConstraintName != null)
							{
								Dictionary<object, object> contextInfo = partition.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
								object duplicateNamesKey = ORMModel.AllowDuplicateNamesKey;
								bool removeDuplicateNamesKey = false;
								try
								{
									if (!contextInfo.ContainsKey(duplicateNamesKey))
									{
										contextInfo[duplicateNamesKey] = null;
										removeDuplicateNamesKey = true;
									}
									newValueConstraint.Name = preserveValueConstraintName;
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
					}
				}
			}
			else
			{
				DelayValidateDescendedValueConstraints(oldValueType, null, null, true);
			}
		}
		#endregion // DataTypeDeletingRule
		#region DataTypeChangeRule
		/// <summary>
		/// ChangeRule: typeof(ValueTypeHasDataType)
		/// Checks first if the data type has been changed and then if the 
		/// value matches the datatype
		/// </summary>
		private static void DataTypeChangeRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == ValueTypeHasDataType.ScaleDomainPropertyId ||
				attributeId == ValueTypeHasDataType.LengthDomainPropertyId)
			{
				DelayValidateAssociatedValueConstraints(((ValueTypeHasDataType)e.ModelElement).ValueType);
			}
		}
		#endregion // DataTypeChangeRule
		#region ValueTypeValueConstraintAddedRule
		/// <summary>
		/// AddRule: typeof(ValueTypeHasValueConstraint)
		/// Checks if the new value range definition matches the data type
		/// </summary>
		private static void ValueTypeValueConstraintAddedRule(ElementAddedEventArgs e)
		{
			ValueConstraint valueConstraint = ((ValueTypeHasValueConstraint)e.ModelElement).ValueConstraint;
			DelayValidateValueConstraint(valueConstraint, valueConstraint.ValueRangeCollection.Count != 0, true);
		}
		#endregion // ValueTypeValueConstraintAddedRule
		#region ValueTypeValueConstraintDeletedRule
		/// <summary>
		/// Delete: typeof(ValueTypeHasValueConstraint)
		/// Checks if the new value range definition matches the data type
		/// </summary>
		private static void ValueTypeValueConstraintDeletedRule(ElementDeletedEventArgs e)
		{
			ObjectType valueType = ((ValueTypeHasValueConstraint)e.ModelElement).ValueType;
			if (!valueType.IsDeleted)
			{
				DelayValidateDefaultValue(valueType, false, true);
			}
		}
		#endregion // ValueTypeValueConstraintDeletedRule
		#region RoleValueConstraintAddedRule
		/// <summary>
		/// AddRule: typeof(RoleHasValueConstraint)
		/// Checks if the value range matches the specified date type
		/// </summary>
		private static void RoleValueConstraintAddedRule(ElementAddedEventArgs e)
		{
			ValueConstraint valueConstraint = ((RoleHasValueConstraint)e.ModelElement).ValueConstraint;
			DelayValidateValueConstraint(valueConstraint, valueConstraint.ValueRangeCollection.Count != 0, true);
		}
		#endregion // RoleValueConstraintAddedRule
		#region RoleValueConstraintDeletedRule
		/// <summary>
		/// DeleteRule: typeof(RoleHasValueConstraint)
		/// Checks if default value error status changes without a role value constraint
		/// </summary>
		private static void RoleValueConstraintDeletedRule(ElementDeletedEventArgs e)
		{
			Role role = ((RoleHasValueConstraint)e.ModelElement).Role;
			if (!role.IsDeleted)
			{
				DelayValidateDefaultValue(role, false, true);
			}
		}
		#endregion // RoleValueConstraintAddedRule
		#region  PathConditionRoleValueConstraintAddedRule
		/// <summary>
		/// AddRule: typeof(PathedRoleHasValueConstraint)
		/// Checks if the value range matches the specified date type
		/// </summary>
		private static void PathConditionRoleValueConstraintAddedRule(ElementAddedEventArgs e)
		{
			ValueConstraint valueConstraint = ((PathedRoleHasValueConstraint)e.ModelElement).ValueConstraint;
			DelayValidateValueConstraint(valueConstraint, valueConstraint.ValueRangeCollection.Count != 0, false);
		}
		#endregion // PathConditionRoleValueConstraintAddedRule
		#region  PathConditionRootValueConstraintAddedRule
		/// <summary>
		/// AddRule: typeof(RolePathRootHasValueConstraint)
		/// Checks if the value range matches the specified date type
		/// </summary>
		private static void PathConditionRootValueConstraintAddedRule(ElementAddedEventArgs e)
		{
			ValueConstraint valueConstraint = ((RolePathRootHasValueConstraint)e.ModelElement).ValueConstraint;
			DelayValidateValueConstraint(valueConstraint, valueConstraint.ValueRangeCollection.Count != 0, false);
		}
		#endregion // PathConditionRootValueConstraintAddedRule
		#region ObjectTypeRoleAdded
		/// <summary>
		/// AddRule: typeof(ObjectTypePlaysRole)
		/// Checks to see if the value on the role added matches the specified data type
		/// </summary>
		private static void ObjectTypeRoleAdded(ElementAddedEventArgs e)
		{
			ObjectType valueType = ((ObjectTypePlaysRole)e.ModelElement).RolePlayer;
			if (valueType.DataType != null)
			{
				DelayValidateAssociatedValueConstraints(valueType);
			}
		}
		#endregion // ObjectTypeRoleAdded
		#region ValueRangeAddedRule
		/// <summary>
		/// AddRule: typeof(ValueConstraintHasValueRange)
		/// Validate a value constraint when a new value range is added.
		/// </summary>
		private static void ValueRangeAddedRule(ElementAddedEventArgs e)
		{
			ValueConstraintHasValueRange link = (ValueConstraintHasValueRange)e.ModelElement;
			ValueConstraint valueConstraint = link.ValueConstraint;
			DataType dataType = valueConstraint.DataType;
			if (dataType != null)
			{
				string minInvariant = null;
				string maxInvariant = null;
				ValueRange range = link.ValueRange;
				if (!dataType.CanParseAnyValue && dataType.IsCultureSensitive)
				{
					dataType.TryConvertToInvariant(range.MinValue, out minInvariant);
					dataType.TryConvertToInvariant(range.MaxValue, out maxInvariant);
				}
				range.InvariantMinValue = minInvariant ?? "";
				range.InvariantMaxValue = maxInvariant ?? "";
			}
			DelayValidateValueConstraint(valueConstraint, false, true);
		}
		#endregion // ValueRangeAddedRule
		#region ValueRangeChangeRule
		/// <summary>
		/// ChangeRule: typeof(ValueRange)
		/// Validate values when a range value is changed
		/// </summary>
		private static void ValueRangeChangeRule(ElementPropertyChangedEventArgs e)
		{
			ValueRange range = (ValueRange)e.ModelElement;
			ValueConstraint valueConstraint = range.ValueConstraint;
			if (valueConstraint != null)
			{
				Guid propertyId = e.DomainProperty.Id;
				bool isMin;
				if (isMin = (propertyId == ValueRange.MinValueDomainPropertyId) ||
					(propertyId == ValueRange.MaxValueDomainPropertyId))
				{
					string invariantForm = null;
					DataType dataType = valueConstraint.DataType;
					if (null != (dataType = valueConstraint.DataType) &&
						!dataType.CanParseAnyValue &&
						dataType.IsCultureSensitive)
					{
						dataType.TryConvertToInvariant((string)e.NewValue, out invariantForm);
					}
					if (isMin)
					{
						range.InvariantMinValue = invariantForm ?? "";
					}
					else
					{
						range.InvariantMaxValue = invariantForm ?? "";
					}
				}
				DelayValidateValueConstraint(valueConstraint, false, true);
			}
		}
		#endregion // ValueRangeChangeRule
		#region ValueConstraintChangeRule
		/// <summary>
		/// ChangeRule: typeof(ValueConstraint)
		/// Translate the Text property
		/// </summary>
		private static void ValueConstraintChangeRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeGuid = e.DomainProperty.Id;
			if (attributeGuid == ValueConstraint.TextDomainPropertyId)
			{
				ValueConstraint valueConstraint = (ValueConstraint)e.ModelElement;
				//Set the new definition
				string newText = (string)e.NewValue;
				if (newText.Length == 0 || !valueConstraint.ParseDefinition(newText))
				{
					valueConstraint.Delete();
				}
			}
		}
		#endregion // ValueConstraintChangeRule
		#region PreferredIdentifierDeletingRule
		/// <summary>
		/// DeletingRule: typeof(EntityTypeHasPreferredIdentifier)
		/// Deleting a preferred identifier can force any descended value
		/// roles to no longer be value roles. Delete any attached value constraints
		/// in this case.
		/// </summary>
		private static void PreferredIdentifierDeletingRule(ElementDeletingEventArgs e)
		{
			ProcessPreferredIdentifierDeleting((EntityTypeHasPreferredIdentifier)e.ModelElement, null, null);
		}
		/// <summary>
		/// Common rule code for processing preferred identifier deletion
		/// </summary>
		/// <param name="link">The <see cref="EntityTypeHasPreferredIdentifier"/> being deleted or modified.</param>
		/// <param name="objectType">An alternate EntityType role player from the <paramref name="link"/>. Use the link's role player if this is <see langword="null"/></param>
		/// <param name="preferredIdentifier">An alternate UniquenessConstraint preferred identifier from the <paramref name="link"/>. Use the link's preferred identifier if this is <see langword="null"/></param>
		private static void ProcessPreferredIdentifierDeleting(EntityTypeHasPreferredIdentifier link, ObjectType objectType, UniquenessConstraint preferredIdentifier)
		{
			if (objectType == null)
			{
				objectType = link.PreferredIdentifierFor;
			}
			if (preferredIdentifier == null)
			{
				preferredIdentifier = link.PreferredIdentifier;
			}
			if (!objectType.IsDeleting)
			{
				DelayValidateDescendedValueConstraints(objectType, null, preferredIdentifier, true);
			}
		}
		#endregion // PreferredIdentifierDeletingRule
		#region PreferredIdentifierRolePlayerChangeRule
		/// <summary>
		/// RolePlayerChangeRule: typeof(EntityTypeHasPreferredIdentifier)
		/// RolePlayerChangeRule corresponding to <see cref="PreferredIdentifierDeletingRule"/>
		/// </summary>
		private static void PreferredIdentifierRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			EntityTypeHasPreferredIdentifier link = (EntityTypeHasPreferredIdentifier)e.ElementLink;
			if (e.DomainRole.Id == EntityTypeHasPreferredIdentifier.PreferredIdentifierForDomainRoleId)
			{
				ProcessPreferredIdentifierDeleting(link, (ObjectType)e.OldRolePlayer, null);
				DelayValidateDescendedValueConstraints((ObjectType)e.NewRolePlayer, null, null, true);
			}
			else
			{
				ProcessPreferredIdentifierDeleting(link, null, (UniquenessConstraint)e.OldRolePlayer);
				DelayValidateDescendedValueConstraints(link.PreferredIdentifierFor, null, null, true);
			}
		}
		#endregion // PreferredIdentifierRolePlayerChangeRule
		#region PreferredIdentifierRoleAddRule
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole), Priority=1;
		/// A rule to determine if a role has been added to a constraint
		/// that is acting as a preferred identifier
		/// </summary>
		private static void PreferredIdentifierRoleAddRule(ElementAddedEventArgs e)
		{
			UniquenessConstraint constraint;
			ObjectType identifiedObject;
			LinkedElementCollection<Role> roles;
			ConstraintRoleSequenceHasRole link = (ConstraintRoleSequenceHasRole)e.ModelElement;
			if (null != (constraint = link.ConstraintRoleSequence as UniquenessConstraint) &&
				null != (identifiedObject = constraint.PreferredIdentifierFor) &&
				(roles = constraint.RoleCollection).Count == 2) // Moving from 1 role to 2
			{
				// Find the original role
				Role originalRole = roles[0];
				if (originalRole == link.Role)
				{
					originalRole = roles[1];
				}
				ObjectType originalRolePlayer;
				RoleBase oppositeRoleBase;
				RoleProxy proxyRole;
				Role oppositeRole;
				if (null != (originalRolePlayer = originalRole.RolePlayer) &&
					null != (oppositeRoleBase = (null != (proxyRole = originalRole.Proxy)) ? proxyRole.OppositeRole : originalRole.OppositeOrUnaryRole) &&
					null != (oppositeRole = oppositeRoleBase.Role))
				{
					// This assert can fail incorrectly during element merge
					// Debug.Assert((proxyRole != null) ? (oppositeRole.RolePlayer == originalRole.FactType.NestingType) : oppositeRole.RolePlayer == identifiedObject);
					LinkedElementCollection<Role> playedRoles = identifiedObject.PlayedRoleCollection;
					int playedRolesCount = playedRoles.Count;
					for (int i = 0; i < playedRolesCount; ++i)
					{
						Role testRole = playedRoles[i];
						if (testRole != oppositeRole)
						{
							// Test by skipping the binary fact for the old part of the preferred identifier
							DelayValidateDescendedValueConstraints(originalRolePlayer, testRole, null, true);
						}
					}
				}
			}
		}
		#endregion // PreferredIdentifierRoleAddRule
		#region RolePlayerDeletingRule
		/// <summary>
		/// DeletingRule: typeof(ObjectTypePlaysRole)
		/// Deleting a role player link can eliminate the
		/// path between a downstream value role and a value type.
		/// </summary>
		private static void RolePlayerDeletingRule(ElementDeletingEventArgs e)
		{
			DelayValidateDescendedValueConstraints(((ObjectTypePlaysRole)e.ModelElement).RolePlayer, null, null, true);
		}
		#endregion // RolePlayerDeletingRule
		#region RolePlayerRolePlayerChangeRule
		/// <summary>
		/// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
		/// Changing a role player link from one object to another
		/// can eliminate or modify the path between a downstream
		/// value role and a value type.
		/// </summary>
		private static void RolePlayerRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ObjectTypePlaysRole.RolePlayerDomainRoleId)
			{
				Role changedRole = ((ObjectTypePlaysRole)e.ElementLink).PlayedRole;
				ObjectType oldRolePlayer = (ObjectType)e.OldRolePlayer;
				if (changedRole.IsValueRoleForAlternateRolePlayer(oldRolePlayer))
				{
					// If the old configuration did not have the changed role as a value
					// role then there will be no value roles descended from it.
					bool visited = false;
					Role.WalkDescendedValueRoles((ObjectType)e.NewRolePlayer, changedRole, null, delegate(Role role, PathedRole pathedRole, RolePathObjectTypeRoot pathRoot, ValueTypeHasDataType dataTypeLink, ValueConstraint currentValueConstraint, ValueConstraint previousValueConstraint, string defaultValue)
					{
						// If we get any callback here, then the role can still be a value role
						visited = true;
						// Make sure that this value constraint is compatible with
						// other constraints above it.
						DelayValidateValueConstraint(currentValueConstraint, true, false);
						if (pathedRole == null && pathRoot == null)
						{
							if (defaultValue != null)
							{
								DelayValidateDefaultValue(role, true, false);
							}
							ValueComparisonConstraint.DelayValidateAttachedConstraint(role);
						}
						return true;
					});
					if (!visited)
					{
						// The old role player supported values, the new one does not.
						// A downstream value constraints and defaults need an error state.
						DelayValidateDescendedValueConstraints(oldRolePlayer, changedRole, null, true);
					}
				}
			}
		}
		#endregion // RolePlayerRolePlayerChangeRule
		#endregion // ValueMatch Validation
		#region VerifyValueRangeOverlapError
		[DelayValidatePriority(3)] // We want this after reference scheme validation, invariant validation, and range validation
		private static void DelayValidateValueRangeOverlapError(ModelElement element)
		{
			((ValueConstraint)element).VerifyValueRangeOverlapError(null);
		}
		private struct RangeValueNode
		{
			public readonly string Value;
			public readonly bool IsLower;
			public readonly int Index;
			public readonly bool IsOpen;
			public RangeValueNode(string value, bool isLower, int index, bool isOpen)
			{
				Value = value;
				IsLower = isLower;
				Index = index;
				IsOpen = isOpen;
			}
		}
		private void VerifyValueRangeOverlapError(INotifyElementAdded notifyAdded)
		{
			bool hasError = false;
			if (IsDeleted)
			{
				return;
			}
			DataType dataType = DataType;
			if (dataType != null && dataType.CanCompare)
			{
				DataTypeRangeSupport rangeSupport = dataType.RangeSupport;
				LinkedElementCollection<ValueRange> ranges = ValueRangeCollection;
				int rangeCount = ranges.Count;
				string minValue;
				string maxValue;
				ValueRange range;
				switch (rangeSupport)
				{
					case DataTypeRangeSupport.None:
						{
							if (rangeCount > 1)
							{
								string[] singleValues = new string[rangeCount];
								int i = 0;
								for (; i < rangeCount; ++i)
								{
									range = ranges[i];

									// The lower node is sufficient
									minValue = range.MinValue;
									if (minValue.Length != 0 && !dataType.ParseNormalizeValue(minValue, range.InvariantMinValue, out minValue))
									{
										break;
									}
									singleValues[i] = minValue;
								}
								if (i == rangeCount)
								{
									Array.Sort<string>(
										singleValues,
										dataType.Compare);
									i = 1;
									for (; i < rangeCount; ++i)
									{
										if (singleValues[i - 1] == singleValues[i])
										{
											break;
										}
									}
									hasError = i < rangeCount;
								}
							}
							break;
						}
					case DataTypeRangeSupport.ContinuousEndPoints:
					case DataTypeRangeSupport.DiscontinuousEndPoints:
						{
							bool adjustEndPoints = rangeSupport == DataTypeRangeSupport.DiscontinuousEndPoints;
							bool isOpen;
							if (rangeCount == 1)
							{
								// The data is overlapping if the values are backwards
								range = ranges[0];
								minValue = range.MinValue;
								maxValue = range.MaxValue;
								if (minValue.Length != 0 && dataType.ParseNormalizeValue(minValue, range.InvariantMinValue, out minValue) &&
									maxValue.Length != 0 && dataType.ParseNormalizeValue(maxValue, range.InvariantMaxValue, out maxValue))
								{
									if (adjustEndPoints)
									{
										isOpen = range.MinInclusion == RangeInclusion.Open;
										if (!dataType.AdjustDiscontinuousLowerBound(ref minValue, ref isOpen))
										{
											hasError = true;
										}
										else
										{
											isOpen = range.MaxInclusion == RangeInclusion.Open;
											if (!dataType.AdjustDiscontinuousUpperBound(ref maxValue, ref isOpen))
											{
												hasError = true;
											}
										}
									}
									if (!hasError)
									{
										hasError = dataType.Compare(minValue, maxValue) > 0;
									}
								}
							}
							else
							{
								RangeValueNode[] nodes = new RangeValueNode[rangeCount + rangeCount];
								int index = 0;
								int leftIndex;
								int rightIndex;
								int i = 0;
								for (; i < rangeCount; ++i)
								{
									range = ranges[i];

									// Add the lower node
									minValue = range.MinValue;
									if (minValue.Length != 0 && !dataType.ParseNormalizeValue(minValue, range.InvariantMinValue, out minValue))
									{
										break;
									}
									isOpen = range.MinInclusion == RangeInclusion.Open;
									if (adjustEndPoints &&
										!(dataType.AdjustDiscontinuousLowerBound(ref minValue, ref isOpen)))
									{
										break;
									}
									nodes[index] = new RangeValueNode(minValue, true, i, isOpen);

									// Add the upper node
									maxValue = range.MaxValue;
									if (maxValue.Length != 0 && !dataType.ParseNormalizeValue(maxValue, range.InvariantMaxValue, out maxValue))
									{
										break;
									}
									isOpen = range.MaxInclusion == RangeInclusion.Open;
									if (adjustEndPoints &&
										!(dataType.AdjustDiscontinuousLowerBound(ref maxValue, ref isOpen)))
									{
										break;
									}
									nodes[index + 1] = new RangeValueNode(maxValue, false, i, isOpen);
									index += 2;
								}
								if (i == rangeCount)
								{
									Array.Sort<RangeValueNode>(
										nodes,
										delegate(RangeValueNode leftNode, RangeValueNode rightNode)
										{
											string leftValue = leftNode.Value;
											string rightValue = rightNode.Value;
											int retVal = 0;
											if (leftValue.Length == 0)
											{
												if (rightValue.Length != 0)
												{
													retVal = leftNode.IsLower ? -1 : 1;
												}
											}
											else if (rightValue.Length == 0)
											{
												retVal = rightNode.IsLower ? 1 : -1;
											}
											else
											{
												retVal = dataType.Compare(leftValue, rightValue);
											}
											if (retVal == 0)
											{
												leftIndex = leftNode.Index;
												rightIndex = rightNode.Index;
												if (leftIndex < rightIndex)
												{
													retVal = -1;
												}
												else if (leftIndex != rightIndex)
												{
													retVal = 1;
												}
												if (retVal == 0)
												{
													if (leftNode.IsLower)
													{
														if (!rightNode.IsLower)
														{
															retVal = -1;
														}
													}
													else if (rightNode.IsLower)
													{
														retVal = 1;
													}
												}
											}
											return retVal;
										});
									index = 0;
									RangeValueNode lower;
									RangeValueNode upper = default(RangeValueNode);
									i = 0;
									for (; i < rangeCount; ++i)
									{
										lower = nodes[index];
										if (!lower.IsLower)
										{
											break;
										}
										if (!lower.IsOpen &&
											!upper.IsOpen &&
											i != 0 &&
											lower.Value.Length != 0 &&
											upper.Value.Length != 0 &&
											0 == dataType.Compare(lower.Value, upper.Value))
										{
											break;
										}
										upper = nodes[index + 1];
										if (upper.IsLower || (upper.Index != lower.Index))
										{
											break;
										}
										index += 2;
									}
									hasError = i != rangeCount;
								}
							}
							break;
						}
				}
			}
			ValueRangeOverlapError error = ValueRangeOverlapError;
			if (hasError)
			{
				if (error == null)
				{
					error = new ValueRangeOverlapError(Partition);
					error.ValueConstraint = this;
					error.Model = DataType.Model;
					error.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(error, true);
					}
				}
			}
			else if (error != null)
			{
				error.Delete();
			}
		}

		/// <summary>
		/// Determine if a value is valid for this value constraint.
		/// </summary>
		/// <param name="normalizedValue">The value to test. dataType.ParseNormalizeValue has already been applied to this value.</param>
		/// <param name="dataType">The resolve data type.</param>
		public bool IsInRange(string normalizedValue, DataType dataType)
		{
			bool inRange = true;
			if (dataType == null)
			{
				dataType = DataType;
			}
			if (dataType != null &&
				dataType.CanCompare &&
				!(inRange = normalizedValue.Length == 0))
			{
				DataTypeRangeSupport rangeSupport = dataType.RangeSupport;
				LinkedElementCollection<ValueRange> ranges = ValueRangeCollection;
				int rangeCount = ranges.Count;
				string minValue;
				string maxValue;
				ValueRange range;
				switch (rangeSupport)
				{
					case DataTypeRangeSupport.None:
						{
							inRange = false;
							for (int i = 0; i < rangeCount; ++i)
							{
								range = ranges[i];

								// The lower node is sufficient
								minValue = range.MinValue;
								if ((minValue.Length == 0 || !dataType.ParseNormalizeValue(minValue, range.InvariantMinValue, out minValue)) &&
									minValue == normalizedValue)
								{
									inRange = true;
									break;
								}
							}
							break;
						}
					case DataTypeRangeSupport.ContinuousEndPoints:
					case DataTypeRangeSupport.DiscontinuousEndPoints:
						{
							bool adjustEndPoints = rangeSupport == DataTypeRangeSupport.DiscontinuousEndPoints;
							bool isOpen;
							inRange = false;
							for (int i = 0; i < rangeCount; ++i)
							{
								range = ranges[i];
								minValue = range.MinValue;
								maxValue = range.MaxValue;
								if (minValue.Length != 0 && dataType.ParseNormalizeValue(minValue, range.InvariantMinValue, out minValue))
								{
									isOpen = range.MinInclusion == RangeInclusion.Open;
									if (adjustEndPoints && !dataType.AdjustDiscontinuousLowerBound(ref minValue, ref isOpen))
									{
										continue;
									}

									int minCompare = dataType.Compare(minValue, normalizedValue);

									if (isOpen ? minCompare < 0 : minCompare <= 0)
									{
										if (maxValue.Length == 0)
										{
											inRange = true;
											break;
										}
										else if (dataType.ParseNormalizeValue(maxValue, range.InvariantMaxValue, out maxValue))
										{
											isOpen = range.MaxInclusion == RangeInclusion.Open;
											if (!adjustEndPoints || dataType.AdjustDiscontinuousUpperBound(ref maxValue, ref isOpen))
											{
												int maxCompare = dataType.Compare(normalizedValue, maxValue);
												if (isOpen ? maxCompare < 0 : maxCompare <= 0)
												{
													inRange = true;
													break;
												}
											}
										}
									}
								}
								else if (maxValue.Length != 0 && dataType.ParseNormalizeValue(maxValue, range.InvariantMaxValue, out maxValue))
								{
									isOpen = range.MaxInclusion == RangeInclusion.Open;
									if (!adjustEndPoints || dataType.AdjustDiscontinuousUpperBound(ref maxValue, ref isOpen))
									{
										int maxCompare = dataType.Compare(normalizedValue, maxValue);
										if (isOpen ? maxCompare < 0 : maxCompare <= 0)
										{
											inRange = true;
											break;
										}
									}
								}
							}
						}
						break;
				}
			}
			return inRange;
		}
		#endregion // VerifyValueRangeOverlapError
		#region CustomStorage handlers
		/// <summary>
		/// Helper method to get a multi-line representation of the <see cref="Text"/> property.
		/// </summary>
		/// <param name="maxColumns">The maximum number of value to display on a single row, or 0 for unlimited.</param>
		/// <param name="maxDisplayed">The maximum total number of items to display, or 0 for unlimited.</param>
		/// <param name="showModality">The returned string should return a deontic mark if applicable.</param>
		/// <returns>A multiline string, if requested.</returns>
		public string GetDisplayText(int maxColumns, int maxDisplayed, bool showModality)
		{
			LinkedElementCollection<ValueRange> ranges = ValueRangeCollection;
			int rangeCount = ranges.Count;
			if (rangeCount <= 0)
			{
				return String.Empty;
			}
			else if (rangeCount == 1)
			{
				return string.Concat((showModality && Modality == ConstraintModality.Deontic) ? ResourceStrings.ValueConstraintDefinitionContainerOpenDelimiterDeontic : ResourceStrings.ValueConstraintDefinitionContainerOpenDelimiter, ranges[0].GetDisplayText(DataType), ResourceStrings.ValueConstraintDefinitionContainerCloseDelimiter);
			}
			int totalValues = rangeCount;
			int totalSlots = rangeCount; // Possibly treat a trailing ellipsis in the total
			if (maxDisplayed > 0)
			{
				if (maxDisplayed < rangeCount)
				{
					totalValues = maxDisplayed;
					totalSlots = maxDisplayed + 1;
				}
			}
			if (maxColumns > 0)
			{
				// Adjust to balance the number of displayed items across rows. For example, if we have
				// 13 slots and 6 max columns, instead of {abcdef,ghijkl,m} we should show {abcde,fghij,klm}
				int itemsOnLastRow = totalSlots % maxColumns;
				if (itemsOnLastRow != 0)
				{
					maxColumns -= (maxColumns - itemsOnLastRow) / ((totalSlots + maxColumns - 1) / maxColumns);
				}
			}
			else
			{
				maxColumns = -1; // Standardize for loop
			}
			StringBuilder valueRangeText = new StringBuilder();
			valueRangeText.Append((showModality && Modality == ConstraintModality.Deontic) ? ResourceStrings.ValueConstraintDefinitionContainerOpenDelimiterDeontic : ResourceStrings.ValueConstraintDefinitionContainerOpenDelimiter);
			int currentColumn = 0;
			string rangeSeparator = null;
			DataType dataType = this.DataType;
			for (int i = 0; i < totalValues; ++i)
			{
				if (i != 0)
				{
					valueRangeText.Append(rangeSeparator ?? (rangeSeparator = DisplayedRangeSeparator));
					if (currentColumn == maxColumns)
					{
						valueRangeText.AppendLine();
						currentColumn = 0;
					}
				}
				++currentColumn;
				valueRangeText.Append(ranges[i].GetDisplayText(dataType));
			}
			if (totalSlots > totalValues)
			{
				valueRangeText.Append(rangeSeparator ?? (rangeSeparator = DisplayedRangeSeparator));
				if (currentColumn == maxColumns)
				{
					valueRangeText.AppendLine();
				}
				valueRangeText.Append(ResourceStrings.ReadingShapeEllipsis);
			}
			valueRangeText.Append(ResourceStrings.ValueConstraintDefinitionContainerCloseDelimiter);
			return valueRangeText.ToString();
		}
		private string GetTextValue()
		{
			return GetDisplayText(0, 0, false);
		}
		private void SetTextValue(string newValue)
		{
			// Handled by ValueConstraintChangeRule
		}
		private void OnTextChanged()
		{
			TransactionManager tmgr = Store.TransactionManager;
			if (tmgr.InTransaction)
			{
				TextChanged = tmgr.CurrentTransaction.SequenceNumber;
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
		#endregion // CustomStorage handlers
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// adds invariant values as needed.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new InvariantValueFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation.
		/// </summary>
		private sealed class InvariantValueFixupListener : DeserializationFixupListener<ValueConstraint>
		{
			/// <summary>
			/// InvariantValueFixupListener constructor
			/// </summary>
			public InvariantValueFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// Process ValueConstraint elements
			/// </summary>
			/// <param name="element">A ValueConstraint element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(ValueConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					LinkedElementCollection<ValueRange> ranges = element.ValueRangeCollection;
					if (ranges.Count == 0)
					{
						// Sanity check
						element.Delete();
						return;
					}

					DataType dataType;
					if (null != (dataType = element.DataType) &&
						!dataType.CanParseAnyValue &&
						dataType.IsCultureSensitive)
					{
						foreach (ValueRange valueRange in ranges)
						{
							string value;
							string invariantValue;
							if ((invariantValue = valueRange.InvariantMinValue).Length == 0 &&
								(value = valueRange.MinValue).Length != 0 &&
								dataType.TryConvertToInvariant(value, out invariantValue))
							{
								valueRange.InvariantMinValue = invariantValue;
							}
							if ((invariantValue = valueRange.InvariantMaxValue).Length == 0 &&
								(value = valueRange.MaxValue).Length != 0 &&
								dataType.TryConvertToInvariant(value, out invariantValue))
							{
								valueRange.InvariantMaxValue = invariantValue;
							}
						}

						// See comments in ValueTypeInstance.InvariantValueFixupListener regarding invariant forms of default values.
					}
				}
			}
		}
		#endregion // Deserialization Fixup
		#region ValueConstraint specific
		/// <summary>
		/// The data type associated with this value range definition
		/// </summary>
		public abstract DataType DataType { get;}
		/// <summary>
		/// Get the <see cref="IModelErrorDisplayContext"/> for this <see cref="ValueConstraint"/>
		/// </summary>
		public abstract IModelErrorDisplayContext ErrorDisplayContext { get;}
		/// <summary>
		/// Get the <see cref="ORMModel"/> for this <see cref="ValueConstraint"/>
		/// </summary>
		public abstract ORMModel Model { get;}
		/// <summary>
		/// Tests if the associated data type is a text type.
		/// </summary>
		public bool IsText
		{
			get
			{
				return DataType is TextDataType;
			}
		}
		/// <summary>
		/// Get the current format provider
		/// </summary>
		private CultureInfo CurrentCulture
		{
			get
			{
				// UNDONE: Consider storing a culture with the model, with a
				// user option to display and parse with either the native culture
				// or the model's stored culture.
				return CultureInfo.CurrentCulture;
			}
		}
		/// <summary>
		/// Get the displayed separator used between two ranges.
		/// </summary>
		private string DisplayedRangeSeparator
		{
			get
			{
				string retVal = CurrentCulture.TextInfo.ListSeparator;
				return !char.IsWhiteSpace(retVal[retVal.Length - 1]) ? retVal + " " : retVal;
			}
		}
		private static Regex myRangeSeparatorRegex;
		private const string RangeSeparatorGroupName = "Data";
		private static string myRangeSeparator;
		private static string myStringDelimiter;
		private Regex RangeSeparatorRegex
		{
			get
			{
				string rangeSeparator = CurrentCulture.TextInfo.ListSeparator;
				string stringDelimiter = ResourceStrings.ValueConstraintStringDelimiter;
				Regex retVal = myRangeSeparatorRegex;
				if (rangeSeparator != myRangeSeparator ||
					stringDelimiter != myStringDelimiter)
				{
					Regex newRegex = new Regex(
						string.Format(CultureInfo.InvariantCulture, @"(?n)\G(\s*(?<Data>([^{0}{1}]+|({0}.*?{0})+)*)\s*)(({1})|\z)", Regex.Escape(stringDelimiter), Regex.Escape(rangeSeparator)),
						RegexOptions.Compiled);
					if ((object)retVal == System.Threading.Interlocked.CompareExchange<Regex>(ref myRangeSeparatorRegex, newRegex, retVal))
					{
						// We used the current values, store them
						myStringDelimiter = stringDelimiter;
						myRangeSeparator = rangeSeparator;
					}
					retVal = myRangeSeparatorRegex;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Breaks a value range definition into value ranges and adds them
		/// to the ValueRangeCollection.
		/// </summary>
		/// <param name="newDefinition">The string containing a value range definition.</param>
		/// <returns>True if one or more value ranges were extracted from <paramref name="newDefinition"/>, otherwise false.</returns>
		protected bool ParseDefinition(string newDefinition)
		{
			// First, remove the container strings from the ends of the definition string
			newDefinition = TrimDefinitionMarkers(newDefinition);
			// UNDONE: Can we preserve this collection instead of clearing it?
			LinkedElementCollection<ValueRange> ranges = ValueRangeCollection;
			ranges.Clear();
			// Second, find the value ranges in the definition string
			// and add them to the collection
			if (newDefinition.Length != 0)
			{
				Match match = RangeSeparatorRegex.Match(newDefinition);
				while (match.Success)
				{
					string rangeData = match.Groups[RangeSeparatorGroupName].Value;
					// Note that the range data is already trimmed by the regex
					if (!string.IsNullOrEmpty(rangeData))
					{
						ValueRange.ParseValueRangeText(this, null, rangeData);
					}
					match = match.NextMatch();
				}
			}
			return ranges.Count != 0;
		}
		/// <summary>
		/// Helper method for matching value ranges. Called after an
		/// <see cref="IElementEquivalence"/> implementation has determined
		/// that this <see cref="ValueConstraint"/> and <paramref name="otherValueConstraint"/>
		/// correspond to the same instance.
		/// </summary>
		/// <param name="otherValueConstraint">The equivalent value constraint.</param>
		/// <param name="elementTracker">The <see cref="IEquivalentElementTracker"/> used
		/// to equate value ranges.</param>
		protected void MatchValueRanges(ValueConstraint otherValueConstraint, IEquivalentElementTracker elementTracker)
		{
			LinkedElementCollection<ValueRange> ranges = ValueRangeCollection;
			int rangeCount = ranges.Count;
			if (rangeCount != 0)
			{
				LinkedElementCollection<ValueRange> otherRanges = otherValueConstraint.ValueRangeCollection;
				int otherRangeCount = otherRanges.Count;
				if (otherRangeCount != 0)
				{
					BitTracker otherMatches = new BitTracker(otherRangeCount);
					DataType dataType = DataType;
					bool canCompare = dataType.CanCompare;
					for (int i = 0; i < rangeCount; ++i)
					{
						ValueRange range = ranges[i];
						string rangeMin = range.MinValue;
						bool minParsed;
						if (string.IsNullOrEmpty(rangeMin))
						{
							rangeMin = null;
							minParsed = false;
						}
						else
						{
							minParsed = dataType.ParseNormalizeValue(rangeMin, range.InvariantMinValue, out rangeMin);
						}
						string rangeMax = range.MaxValue;
						bool maxParsed;
						if (string.IsNullOrEmpty(rangeMax))
						{
							rangeMax = null;
							maxParsed = false;
						}
						else
						{
							maxParsed = dataType.ParseNormalizeValue(rangeMax, range.InvariantMaxValue, out rangeMax);
						}
						for (int j = 0; j < otherRangeCount; ++j)
						{
							if (!otherMatches[j])
							{
								ValueRange otherRange = otherRanges[j];
								// If the data types for the two elements are different, then
								// there is very little we can do at this point because we don't
								// know what the final data type resolution will be after merge
								// integration is complete. We use the current data type to
								// compare values, and leave it up to rules to sort out the
								// remaining issues after data type information is complete.
								string otherRangeBound = otherRange.MinValue;
								if (string.IsNullOrEmpty(otherRangeBound))
								{
									if (rangeMin != null)
									{
										continue;
									}
								}
								else if (minParsed != dataType.ParseNormalizeValue(otherRangeBound, otherRange.InvariantMinValue, out otherRangeBound))
								{
									continue;
								}
								else if (!((canCompare && minParsed) ? (dataType.Compare(rangeMin, otherRangeBound) == 0) : (rangeMin == otherRangeBound)))
								{
									continue;
								}
								otherRangeBound = otherRange.MaxValue;
								if (string.IsNullOrEmpty(otherRangeBound))
								{
									if (rangeMax != null)
									{
										continue;
									}
								}
								else if (maxParsed != dataType.ParseNormalizeValue(otherRangeBound, otherRange.InvariantMaxValue, out otherRangeBound))
								{
									continue;
								}
								else if (!((canCompare && maxParsed) ? (dataType.Compare(rangeMax, otherRangeBound) == 0) : (rangeMax == otherRangeBound)))
								{
									continue;
								}

								// Ignore endpoint inclusion properties for merging, consider these sufficient equivalent ranges to match.
								elementTracker.AddEquivalentElement(range, otherRange);
								otherMatches[j] = true;
								break;
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Removes the left- and right-strings which denote a value range definition.
		/// </summary>
		/// <param name="definition">The string to remove left and right value range definition strings from.</param>
		private static string TrimDefinitionMarkers(string definition)
		{
			definition = definition.Trim();
			string leftContainerMarkTrimmed = ResourceStrings.ValueConstraintDefinitionContainerOpenDelimiter;
			if (leftContainerMarkTrimmed.Length != 0)
			{
				leftContainerMarkTrimmed = leftContainerMarkTrimmed.Trim();
			}
			if (definition.StartsWith(leftContainerMarkTrimmed))
			{
				definition = definition.Substring(leftContainerMarkTrimmed.Length);
			}
			string rightContainerMarkTrimmed = ResourceStrings.ValueConstraintDefinitionContainerCloseDelimiter;
			if (rightContainerMarkTrimmed.Length != 0)
			{
				rightContainerMarkTrimmed = rightContainerMarkTrimmed.Trim();
			}
			if (definition.EndsWith(rightContainerMarkTrimmed))
			{
				definition = definition.Substring(0, definition.Length - rightContainerMarkTrimmed.Length);
			}
			return definition;
		}
		#endregion //ValueConstraint specific
	}
	#region ValueTypeValueConstraint class
	partial class ValueTypeValueConstraint : IHasIndirectModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// Override to retrieve the data type
		/// </summary>
		public override DataType DataType
		{
			get
			{
				return ValueType.DataType;
			}
		}
		/// <summary>
		/// Get the error display context of the <see cref="P:ValueType"/>
		/// </summary>
		public override IModelErrorDisplayContext ErrorDisplayContext
		{
			get
			{
				return ValueType;
			}
		}
		/// <summary>
		/// Get the associated <see cref="ORMModel"/>
		/// </summary>
		public override ORMModel Model
		{
			get
			{
				ObjectType valueType = ValueType;
				return valueType != null ? valueType.ResolvedModel : null;
			}
		}
		#endregion // Base overrides
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { ValueTypeHasValueConstraint.ValueConstraintDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
	}
	#endregion // ValueTypeValueConstraint class
	#region RoleValueConstraint class
	partial class RoleValueConstraint : IHasIndirectModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// Override to retrieve the data type
		/// </summary>
		public override DataType DataType
		{
			get
			{
				DataType retVal = null;
				Role role;
				ObjectType valueType;
				if (null != (role = Role) &&
					null != (valueType = role.ValueRoleValueType))
				{
					retVal = valueType.DataType;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Get the error display context of the <see cref="P:Role"/>
		/// </summary>
		public override IModelErrorDisplayContext ErrorDisplayContext
		{
			get
			{
				return Role;
			}
		}
		/// <summary>
		/// Get the associated <see cref="ORMModel"/>
		/// </summary>
		public override ORMModel Model
		{
			get
			{
				Role role;
				FactType factType;
				if (null != (role = Role) &&
					null != (factType = role.FactType))
				{
					return factType.ResolvedModel;
				}
				return null;
			}
		}
		#endregion // Base overrides
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { RoleHasValueConstraint.ValueConstraintDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
	}
	#endregion // RoleValueConstraint class
	#region PathConditionRoleValueConstraint class
	partial class PathConditionRoleValueConstraint : IHasIndirectModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// Retrieve the <see cref="DataType"/> from the <see cref="PathedRole"/>
		/// </summary>
		public override DataType DataType
		{
			get
			{
				DataType retVal = null;
				PathedRole pathedRole;
				ObjectType valueType;
				if (null != (pathedRole = PathedRole) &&
					null != (valueType = pathedRole.Role.ValueRoleValueType))
				{
					retVal = valueType.DataType;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Get the error display context of the containing <see cref="RolePathOwner"/>
		/// </summary>
		public override IModelErrorDisplayContext ErrorDisplayContext
		{
			get
			{
				PathedRole pathedRole = PathedRole;
				LeadRolePath leadRolePath = pathedRole.RolePath.RootRolePath;
				return leadRolePath != null ? leadRolePath.RootOwner as IModelErrorDisplayContext : null;
			}
		}
		/// <summary>
		/// Get the associated <see cref="ORMModel"/>
		/// </summary>
		public override ORMModel Model
		{
			get
			{
				PathedRole pathedRole;
				IRolePathOwner owner;
				if (null != (pathedRole = PathedRole) &&
					null != (owner = pathedRole.RolePath.RootOwner))
				{
					return owner.Model;
				}
				return null;
			}
		}
		#endregion // Base overrides
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { PathedRoleHasValueConstraint.ValueConstraintDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
	}
	#endregion // PathConditionRoleValueConstraint class
	#region PathConditionRootValueConstraint class
	partial class PathConditionRootValueConstraint : IHasIndirectModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// Retrieve the <see cref="DataType"/> from the <see cref="PathedRole"/>
		/// </summary>
		public override DataType DataType
		{
			get
			{
				DataType retVal = null;
				RolePathObjectTypeRoot pathRoot;
				if (null != (pathRoot = PathRoot))
				{
					retVal = pathRoot.RootObjectType.SingleValueDataType;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Get the error display context of the containing <see cref="RolePathOwner"/>
		/// </summary>
		public override IModelErrorDisplayContext ErrorDisplayContext
		{
			get
			{
				RolePathObjectTypeRoot pathRoot = PathRoot;
				LeadRolePath leadRolePath = pathRoot.RolePath.RootRolePath;
				return leadRolePath != null ? leadRolePath.RootOwner as IModelErrorDisplayContext : null;
			}
		}
		/// <summary>
		/// Get the associated <see cref="ORMModel"/>
		/// </summary>
		public override ORMModel Model
		{
			get
			{
				RolePathObjectTypeRoot pathRoot = PathRoot;
				return null != pathRoot ? pathRoot.RootObjectType.ResolvedModel : null;
			}
		}
		#endregion // Base overrides
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { RolePathRootHasValueConstraint.ValueConstraintDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
	}
	#endregion // PathConditionRootValueConstraint class
	#region ValueConstraintError class
	/// <summary>
	/// ValueConstraintError error abstract class
	/// </summary>
	public abstract partial class ValueConstraintError
	{
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
		/// <summary>
		/// Get the <see cref="ValueConstraint"/> associated with this error.
		/// </summary>
		public abstract ValueConstraint ContextValueConstraint { get;}
	}
	#endregion // ValueConstraintError class
	#region MinValueMismatchError class
	/// <summary>
	/// MinValueMismatchError class
	/// </summary>
	[ModelErrorDisplayFilter(typeof(DataTypeAndValueErrorCategory))]
	partial class MinValueMismatchError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Standard override
		/// </summary>
		public override void GenerateErrorText()
		{
			ValueConstraint valueConstraint = ValueRange.ValueConstraint;
			IModelErrorDisplayContext displayContext = valueConstraint != null ? valueConstraint.ErrorDisplayContext : null;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorValueRangeMinValueMismatchError, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorValueRangeMinValueMismatchErrorCompact;
			}
		}
		/// <summary>
		/// Get the associated <see cref="ValueConstraint"/>
		/// </summary>
		public override ValueConstraint ContextValueConstraint
		{
			get
			{
				return ValueRange.ValueConstraint;
			}
		}
		#endregion // Base overrides
		#region IRepresentModelElements Implementation
		/// <summary>
		/// Implements <see cref="IRepresentModelElements.GetRepresentedElements"/>
		/// </summary>
		protected new ModelElement[] GetRepresentedElements()
		{
			// Reimplement to get the grandparent ValueConstraint instead of the parent ValueRange
			return new ModelElement[] { this.ValueRange.ValueConstraint };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
	}
	#endregion // MinValueMismatchError class
	#region MaxValueMismatchError class
	/// <summary>
	/// MaxValueMismatchError class
	/// </summary>
	[ModelErrorDisplayFilter(typeof(DataTypeAndValueErrorCategory))]
	partial class MaxValueMismatchError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Standard override
		/// </summary>
		public override void GenerateErrorText()
		{
			ValueConstraint valueConstraint = ValueRange.ValueConstraint;
			IModelErrorDisplayContext displayContext = valueConstraint != null ? valueConstraint.ErrorDisplayContext : null;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorValueRangeMaxValueMismatchError, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorValueRangeMaxValueMismatchErrorCompact;
			}
		}
		/// <summary>
		/// Get the associated <see cref="ValueConstraint"/>
		/// </summary>
		public override ValueConstraint ContextValueConstraint
		{
			get
			{
				return ValueRange.ValueConstraint;
			}
		}
		#endregion // Base overrides
		#region IRepresentModelElements Implementation
		/// <summary>
		/// Implements <see cref="IRepresentModelElements.GetRepresentedElements"/>
		/// </summary>
		protected new ModelElement[] GetRepresentedElements()
		{
			// Reimplement to get the grandparent ValueConstraint instead of the parent ValueRange
			return new ModelElement[] { this.ValueRange.ValueConstraint };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
	}
	#endregion // MaxValueMismatchError class
	#region ValueConstraintValueTypeDetachedError class
	/// <summary>
	/// This is the model error message for value ranges that overlap
	/// </summary>
	[ModelErrorDisplayFilter(typeof(DataTypeAndValueErrorCategory))]
	partial class ValueConstraintValueTypeDetachedError
	{
		#region Base overrides
		/// <summary>
		/// GenerateErrorText
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = ValueConstraint.ErrorDisplayContext;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorValueConstraintValueTypeDetachedError, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorValueConstraintValueTypeDetachedErrorCompact;
			}
		}
		/// <summary>
		/// Get the associated <see cref="ValueConstraint"/>
		/// </summary>
		public override ValueConstraint ContextValueConstraint
		{
			get
			{
				return ValueConstraint;
			}
		}
		#endregion // Base overrides
	}
	#endregion // ValueConstraintValueTypeDetachedError class
	#region ValueRangeOverlapError
	/// <summary>
	/// This is the model error message for value ranges that overlap
	/// </summary>
	[ModelErrorDisplayFilter(typeof(DataTypeAndValueErrorCategory))]
	partial class ValueRangeOverlapError
	{
		#region Base overrides
		/// <summary>
		/// GenerateErrorText
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = ValueConstraint.ErrorDisplayContext;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorValueConstraintValueRangeOverlapError, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorValueConstraintValueRangeOverlapErrorCompact;
			}
		}
		/// <summary>
		/// Get the associated <see cref="ValueConstraint"/>
		/// </summary>
		public override ValueConstraint ContextValueConstraint
		{
			get
			{
				return ValueConstraint;
			}
		}
		#endregion // Base overrides
	}
	#endregion // ValueRangeOverlapError
	#region DefaultValueMismatchError class
	/// <summary>
	/// DefaultValueMismatchError class
	/// </summary>
	[ModelErrorDisplayFilter(typeof(DataTypeAndValueErrorCategory))]
	partial class DefaultValueMismatchError : IRepresentModelElements
	{
		#region Base overrides
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
		/// <summary>
		/// Standard override
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = (IModelErrorDisplayContext)Role ?? ValueType;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorDefaultValueMismatch, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorDefaultValueMismatchCompact;
			}
		}
		#endregion // Base overrides
		#region IRepresentModelElements Implementation
		/// <summary>
		/// Implements <see cref="IRepresentModelElements.GetRepresentedElements"/>
		/// </summary>
		protected new ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { (ModelElement)Role ?? ValueType };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
	}
	#endregion // DefaultValueMismatchError class
	#region DefaultValueOutOfRangeError class
	/// <summary>
	/// DefaultValueOutOfRangeError class
	/// </summary>
	[ModelErrorDisplayFilter(typeof(DataTypeAndValueErrorCategory))]
	partial class DefaultValueOutOfRangeError : IRepresentModelElements
	{
		#region Base overrides
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
		/// <summary>
		/// Standard override
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = (IModelErrorDisplayContext)Role ?? ValueType;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorDefaultValueOutOfRange, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorDefaultValueOutOfRangeCompact;
			}
		}
		#endregion // Base overrides
		#region IRepresentModelElements Implementation
		/// <summary>
		/// Implements <see cref="IRepresentModelElements.GetRepresentedElements"/>
		/// </summary>
		protected new ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { (ModelElement)Role ?? ValueType };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
	}
	#endregion // DefaultValueOutOfRangeError class
	#region DefaultValueValueTypeDetachedError class
	/// <summary>
	/// DefaultValueValueTypeDetachedError class
	/// </summary>
	[ModelErrorDisplayFilter(typeof(DataTypeAndValueErrorCategory))]
	partial class DefaultValueValueTypeDetachedError : IRepresentModelElements
	{
		#region Base overrides
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
		/// <summary>
		/// Standard override
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = Role;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorDefaultValueValueTypeDetached, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorDefaultValueValueTypeDetachedCompact;
			}
		}
		#endregion // Base overrides
		#region IRepresentModelElements Implementation
		/// <summary>
		/// Implements <see cref="IRepresentModelElements.GetRepresentedElements"/>
		/// </summary>
		protected new ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { Role };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
	}
	#endregion // DefaultValueValueTypeDetachedError class
	#region ValueConstraintOutOfRangeError class
	/// <summary>
	/// This is the model error message for value ranges that overlap
	/// </summary>
	[ModelErrorDisplayFilter(typeof(DataTypeAndValueErrorCategory))]
	partial class ValueRangeOutOfRangeError
	{
		#region Base overrides
		/// <summary>
		/// GenerateErrorText
		/// </summary>
		public override void GenerateErrorText()
		{
			ValueConstraint valueConstraint = ValueRange.ValueConstraint;
			IModelErrorDisplayContext displayContext = valueConstraint != null ? valueConstraint.ErrorDisplayContext : null;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorValueRangeOutOfRangeError, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorValueRangeOutOfRangeErrorCompact;
			}
		}
		/// <summary>
		/// Get the associated <see cref="ValueConstraint"/>
		/// </summary>
		public override ValueConstraint ContextValueConstraint
		{
			get
			{
				return ValueRange.ValueConstraint;
			}
		}
		#endregion // Base overrides
	}
	#endregion // ValueConstraintOutOfRangeError class
}
