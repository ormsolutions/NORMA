#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object Role Modeling Architect for Visual Studio                 *
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using System.Text.RegularExpressions;
using System.Text;
using Neumont.Tools.ORM.Framework;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region RangeInclusion enum
	/// <summary>
	/// Standard range value inclusion categories
	/// </summary>
	[CLSCompliant(true)]
	public enum RangeInclusion
	{
		/// <summary>
		/// Default inclusion type
		/// </summary>
		NotSet,
		/// <summary>
		/// Indicates the specific value is not included
		/// in the range.
		/// </summary>
		Open,
		/// <summary>
		/// Indicates the specific value is included
		/// in the range.
		/// </summary>
		Closed
	}
	#endregion // RangeInclusion enum
	public partial class ValueRange : IModelErrorOwner
	{
		#region variables
		private static readonly string valueDelim = ResourceStrings.ValueConstraintValueDelimiter;
		private static readonly string stringContainerString = ResourceStrings.ValueConstraintStringContainerPattern;
		private static readonly string leftStringContainerMark = GetLeftMark(stringContainerString);
		private static readonly string rightStringContainerMark = GetRightMark(stringContainerString);
		private static readonly string openInclusionContainerString = ResourceStrings.ValueConstraintOpenInclusionContainer;
		private static readonly string minOpenInclusionMark = GetLeftMark(openInclusionContainerString);
		private static readonly string maxOpenInclusionMark = GetRightMark(openInclusionContainerString);
		private static readonly string closedInclusionContainerString = ResourceStrings.ValueConstraintClosedInclusionContainer;
		private static readonly string minClosedInclusionMark = GetLeftMark(closedInclusionContainerString);
		private static readonly string maxClosedInclusionMark = GetRightMark(closedInclusionContainerString);
		private static readonly string[] minInclusionMarks = new string[] { "", minOpenInclusionMark, minClosedInclusionMark };
		private static readonly string[] maxInclusionMarks = new string[] { "", maxOpenInclusionMark, maxClosedInclusionMark };
		private static string GetLeftMark(string containerString)
		{
			return containerString.Substring(0, containerString.IndexOf("{0}"));
		}
		private static string GetRightMark(string containerString)
		{
			return containerString.Substring(containerString.IndexOf("{0}") + 3);
		}
		#endregion // variables
		IEnumerable<ModelError> IModelErrorOwner.ErrorCollection
		{
			get
			{
				return ErrorCollection;
			}
		}
		/// <summary>
		/// Implements IModelErrorOwner.ErrorCollection
		/// </summary>
		[CLSCompliant(false)]
		protected IEnumerable<ModelError> ErrorCollection
		{
			get
			{
				MaxValueMismatchError max;
				MinValueMismatchError min;
				if (null != (max = MaxValueMismatchError))
				{
					yield return max;
				}
				if (null != (min = MinValueMismatchError))
				{
					yield return min;
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="notifyAdded"></param>
		protected void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			// Calls added here need corresponding delayed calls in DelayValidateErrors
			VerifyValueMatch(notifyAdded);
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		/// <summary>
		/// Implements IModelErrorOwner.DelayValidateErrors
		/// </summary>
		protected static void DelayValidateErrors()
		{
			// UNDONE: DelayedValidation (ValueRange)
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		private void VerifyValueMatch(INotifyElementAdded notifyAdded)
		{
			DataType dataType = ValueConstraint.DataType;
			bool needMinError = false;
			bool needMaxError = false;
			MinValueMismatchError minMismatch;
			MaxValueMismatchError maxMismatch;
			if (dataType != null)
			{
				string min = MinValue;
				string max = MaxValue;
				if (min.Length != 0 && !dataType.CanParse(min))
				{
					needMinError = true;
					minMismatch = MinValueMismatchError;
					if (minMismatch == null)
					{
						minMismatch = MinValueMismatchError.CreateMinValueMismatchError(Store);
						minMismatch.Model = dataType.Model;
						minMismatch.ValueRange = this;
						minMismatch.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(minMismatch, true);
						}
					}
					minMismatch.GenerateErrorText();
				}
				if (min != max)
				{
					if (max.Length != 0 && !dataType.CanParse(MaxValue))
					{
						needMaxError = true;
						maxMismatch = MaxValueMismatchError;
						if (maxMismatch == null)
						{
							maxMismatch = MaxValueMismatchError.CreateMaxValueMismatchError(Store);
							maxMismatch.Model = dataType.Model;
							maxMismatch.ValueRange = this;
							maxMismatch.GenerateErrorText();
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(maxMismatch, true);
							}
						}
						maxMismatch.GenerateErrorText();
					}
				}
			}
			if (!needMinError && null != (minMismatch = MinValueMismatchError))
			{
				minMismatch.Remove();
			}
			if (!needMaxError && null != (maxMismatch = MaxValueMismatchError))
			{
				maxMismatch.Remove();
			}
		}
		private static void ValidateValueConstraintForRule(ValueConstraint valueConstraint)
		{
			if (valueConstraint == null)
			{
				return;
			}
			ValueRangeMoveableCollection ranges = valueConstraint.ValueRangeCollection;
			int rangesCount = ranges.Count;
			for (int i = 0; i < rangesCount; ++i)
			{
				ranges[i].VerifyValueMatch(null);
			}
		}
		[RuleOn(typeof(ValueTypeHasDataType), FireTime = TimeToFire.LocalCommit)]
		private class DataTypeAddRule : AddRule
		{
			/// <summary>
			/// Test if the changed value does not match the specified data type.
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
				ObjectType valueType = link.ValueTypeCollection;
				ValidateValueConstraintForRule(valueType.ValueConstraint);
				RoleMoveableCollection roles = valueType.PlayedRoleCollection;
				int rolesCount = roles.Count;
				for (int i = 0; i < rolesCount; ++i)
				{
					ValidateValueConstraintForRule(roles[i].ValueConstraint);
				}
			}
		}
		[RuleOn(typeof(ValueTypeHasDataType))]
		private class DataTypeChangeRule: ChangeRule
		{
			/// <summary>
			/// checks first if the data type has been chagned and then test if the 
			/// value matches the datatype
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
				ObjectType valueType = link.ValueTypeCollection;
				ValidateValueConstraintForRule(valueType.ValueConstraint);
				RoleMoveableCollection roles = valueType.PlayedRoleCollection;
				int rolesCount = roles.Count;
				for (int i = 0; i < rolesCount; ++i)
				{
					ValidateValueConstraintForRule(roles[i].ValueConstraint);
				}
			}
		}
		[RuleOn(typeof(ValueTypeHasValueConstraint), FireTime = TimeToFire.LocalCommit)]
		private class ValueConstraintAddRule : AddRule
		{
			/// <summary>
			/// checks if the new value range definition matches the data type
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ValueTypeHasValueConstraint link = e.ModelElement as ValueTypeHasValueConstraint;
				ObjectType valueType = link.ValueType;
				ValidateValueConstraintForRule(valueType.ValueConstraint);
				RoleMoveableCollection roles = valueType.PlayedRoleCollection;
				int rolesCount = roles.Count;
				for (int i = 0; i < rolesCount; ++i)
				{
					ValidateValueConstraintForRule(roles[i].ValueConstraint);
				}
			}
		}
		[RuleOn(typeof(RoleHasValueConstraint), FireTime = TimeToFire.LocalCommit)]
		private class RoleValueConstraintAdded : AddRule
		{
			/// <summary>
			/// checks if the the value range matches the specified date type
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				RoleHasValueConstraint link = e.ModelElement as RoleHasValueConstraint;
				ObjectType valueType = link.Role.RolePlayer;
				ValidateValueConstraintForRule(valueType.ValueConstraint);
				RoleMoveableCollection roles = valueType.PlayedRoleCollection;
				int rolesCount = roles.Count;
				for (int i = 0; i < rolesCount; ++i)
				{
					ValidateValueConstraintForRule(roles[i].ValueConstraint);
				}
			}
		}
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime= TimeToFire.LocalCommit)]
		private class ObjectTypeRoleAdded : AddRule
		{
			/// <summary>
			/// checks to see if the value on the role added matches the specified data type
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				ObjectType valueType = link.RolePlayer;
				ValidateValueConstraintForRule(valueType.ValueConstraint);
				RoleMoveableCollection roles = valueType.PlayedRoleCollection;
				int rolesCount = roles.Count;
				for (int i = 0; i < rolesCount; ++i)
				{
					ValidateValueConstraintForRule(roles[i].ValueConstraint);
				}
			}
		}
		[RuleOn(typeof(ValueConstraintHasValueRange), FireTime = TimeToFire.LocalCommit)]
		private class ValueRangeAdded : AddRule
		{
			public override void  ElementAdded(ElementAddedEventArgs e)
			{
				ValueConstraintHasValueRange link = e.ModelElement as ValueConstraintHasValueRange;
				link.ValueRangeCollection.VerifyValueMatch(null);
			}
		}
		[RuleOn(typeof(ObjectTypePlaysRole))]
		#region ValueRangeChangeRule class
		[RuleOn(typeof(ValueRange))]
		private class ValueRangeChangeRule : ChangeRule
		{
			/// <summary>
			/// Translate the Text property
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				if (attributeGuid == ValueRange.TextMetaAttributeGuid)
				{
					ValueRange vr = e.ModelElement as ValueRange;
					Debug.Assert(vr != null);
					string newValue = e.NewValue as string;
					//Set the min- and max-inclusion
					string minInclusion;
					string maxInclusion;
					newValue = StripInclusions(out minInclusion, out maxInclusion, newValue);
					if (minInclusion.Length != 0)
					{
						vr.MinInclusion = minInclusion.Equals(minOpenInclusionMark) ? RangeInclusion.Open : RangeInclusion.Closed;
					}
					if (maxInclusion.Length != 0)
					{
						vr.MaxInclusion = maxInclusion.Equals(maxOpenInclusionMark) ? RangeInclusion.Open : RangeInclusion.Closed;
					}
					//Split the range, if one exists, and set the values
					if (newValue.Contains(valueDelim))
					{
						int currentPos = newValue.IndexOf(valueDelim);
						string value = TrimStringMarkers(newValue.Substring(0, currentPos));
						vr.MinValue = string.Format(CultureInfo.InvariantCulture, value);
						currentPos += valueDelim.Length;
						value = TrimStringMarkers(newValue.Substring(currentPos, newValue.Length - currentPos));
						vr.MaxValue = string.Format(CultureInfo.InvariantCulture, value);
					}
					//Not a range? Set the value as both the min and max
					else
					{
						vr.MinValue = vr.MaxValue = string.Format(CultureInfo.InvariantCulture, TrimStringMarkers(newValue));
					}
				}
			}
		}
		#endregion // ValueRangeChangeRule class
		#region CustomStorage handlers
		/// <summary>
		/// Standard override. Retrieve values for calculated properties.
		/// </summary>
		public override object GetValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == TextMetaAttributeGuid)
			{
				string retVal;
				string minInclusionMarkToUse = minInclusionMarks[(int)this.MinInclusion];
				string maxInclusionMarkToUse = maxInclusionMarks[(int)this.MaxInclusion];
				string minValue = MinValue;
				string maxValue = MaxValue;
				bool minExists = (minValue.Length != 0);
				bool maxExists = (maxValue.Length != 0);
				// put values in string container if need to
				if (minExists && IsText())
				{
					minValue = String.Format(CultureInfo.InvariantCulture, stringContainerString, minValue);
				}
				if (maxExists && IsText())
				{
					maxValue = String.Format(CultureInfo.InvariantCulture, stringContainerString, maxValue);
				}
				// Assemble values into a value range text
				if (minExists && maxExists && !minValue.Equals(maxValue))
				{
					retVal = string.Concat(minInclusionMarkToUse, minValue, valueDelim, maxValue, maxInclusionMarkToUse);
				}
				else if (minExists && !maxExists)
				{
					retVal = string.Concat(minInclusionMarkToUse, minValue, valueDelim, maxInclusionMarkToUse);
				}
				else if (minExists && ((maxExists && minValue.Equals(maxValue)) || !maxExists))
				{
					retVal = minValue;
				}
				else
				{
					retVal = string.Concat(minInclusionMarkToUse, valueDelim, maxValue, maxInclusionMarkToUse);
				}
				return retVal;
			}
			return base.GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Tests if the associated data type is a text type.
		/// </summary>
		/// <returns>ValueConstraint.IsText() if definition exists; otherwise, true.</returns>
		/// <remarks>Since text is by far the minority of all the data types and
		/// the only type to require string container marks, we do not put values inside 
		/// those marks by default whenever we can't determine the data type (i.e. no
		/// ValueConstraint exists).</remarks>
		protected bool IsText()
		{
			if (this.ValueConstraint != null)
			{
				return ValueConstraint.IsText();
			}
			return false;
		}
		/// <summary>
		/// Standard override. Defer to GetValueForCustomStoredAttribute.
		/// </summary>
		protected override object GetOldValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			return GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Standard override. All custom storage properties are derived, not
		/// stored. Actual changes are handled in FactTypeChangeRule.
		/// </summary>
		public override void SetValueForCustomStoredAttribute(MetaAttributeInfo attribute, object newValue)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == TextMetaAttributeGuid)
			{
				// Handled by ValueRangeChangeRule
				return;
			}
			base.SetValueForCustomStoredAttribute(attribute, newValue);
		}
		/// <summary>
		/// Removes the left- and right-strings which denote a value as a string.
		/// </summary>
		/// <param name="range">The string to remove left and right string delimiters from.</param>
		public static string TrimStringMarkers(string range)
		{
			range = range.Trim();
			string leftStringContainerMarkTrimmed = leftStringContainerMark.Trim();
			if (range.StartsWith(leftStringContainerMarkTrimmed))
			{
				range = range.Remove(0, leftStringContainerMarkTrimmed.Length);
			}
			string rightStringContainerMarkTrimmed = rightStringContainerMark.Trim();
			if (range.EndsWith(rightStringContainerMarkTrimmed))
			{
				range = range.Remove(range.Length - rightStringContainerMarkTrimmed.Length);
			}
			return range;
		}
		/// <summary>
		/// Pops the inclusion strings off the ends of the range and stores them
		/// in the appropriate out string.
		/// </summary>
		/// <param name="minInclusion">
		/// The string to put the minInclusion mark from the rangeString into.
		/// Will return "" if no mark exists.
		/// </param>
		/// <param name="maxInclusion">
		/// The string to put the maxInclusion mark from the rangeString into.
		/// Will return "" if no mark exists.
		/// </param>
		/// <param name="range">The string to remove inclusion marks from.</param>
		private static string StripInclusions(out string minInclusion, out string maxInclusion, string range)
		{
			minInclusion = "";
			maxInclusion = "";
			range = range.Trim();
			//Take care of the min-marks
			string minOpenInclusionMarkTrimmed = minOpenInclusionMark.Trim();
			string minClosedInclusionMarkTrimmed = minClosedInclusionMark.Trim();
			if (range.StartsWith(minOpenInclusionMarkTrimmed))
			{
				range = range.Remove(0, minOpenInclusionMarkTrimmed.Length);
				minInclusion = minOpenInclusionMark;
			}
			else if (range.StartsWith(minClosedInclusionMarkTrimmed))
			{
				range = range.Remove(0, minClosedInclusionMarkTrimmed.Length);
				minInclusion = minClosedInclusionMark;
			}
			//Take care of the max-marks
			string maxOpenInclusionMarkTrimmed = maxOpenInclusionMark.Trim();
			string maxClosedInclusionMarkTrimmed = maxClosedInclusionMark.Trim();
			if (range.EndsWith(maxOpenInclusionMarkTrimmed))
			{
				range = range.Remove(range.Length - maxOpenInclusionMarkTrimmed.Length);
				maxInclusion = maxOpenInclusionMark;
			}
			else if (range.EndsWith(maxClosedInclusionMarkTrimmed))
			{
				range = range.Remove(range.Length - maxClosedInclusionMarkTrimmed.Length);
				maxInclusion = maxClosedInclusionMark;
			}
			return range;
		}
		#endregion // CustomStorage handlers
	}
	public partial class ValueConstraint
	{
		#region variables
		private static readonly string defnContainerString = ResourceStrings.ValueConstraintDefinitionContainerPattern;
		private static readonly string leftContainerMark = defnContainerString.Substring(0, defnContainerString.IndexOf("{0}"));
		private static readonly string rightContainerMark = defnContainerString.Substring(defnContainerString.IndexOf("{0}") + 3);
		private static readonly string rangeDelim = ResourceStrings.ValueConstraintRangeDelimiter;
		private static readonly string delimSansSpace = rangeDelim.Trim();
		private static readonly string delimSansSpaceEscaped = Regex.Escape(delimSansSpace);
		#endregion // variables
		#region CustomStorage handlers
		/// <summary>
		/// Standard override. Retrieve values for calculated properties.
		/// </summary>
		public override object GetValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == TextMetaAttributeGuid)
			{
				ValueRangeMoveableCollection ranges = ValueRangeCollection;
				int rangeCount = ranges.Count;
				string retVal = "";
				if (rangeCount == 1)
				{
					retVal = string.Concat(leftContainerMark, ranges[0].Text, rightContainerMark);
				}
				else if (rangeCount > 1)
				{
					StringBuilder valueRangeText = new StringBuilder();
					valueRangeText.Append(leftContainerMark);
					for (int i = 0; i < rangeCount; ++i)
					{
						if (i != 0)
						{
							valueRangeText.Append(rangeDelim);
						}
						valueRangeText.Append(ranges[i].Text);
					}
					valueRangeText.Append(rightContainerMark);
					retVal = valueRangeText.ToString();
				}
				return retVal;
			}
			return base.GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Standard override. Defer to GetValueForCustomStoredAttribute.
		/// </summary>
		protected override object GetOldValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			return GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Standard override. All custom storage properties are derived, not
		/// stored. Actual changes are handled in FactTypeChangeRule.
		/// </summary>
		public override void SetValueForCustomStoredAttribute(MetaAttributeInfo attribute, object newValue)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == TextMetaAttributeGuid)
			{
				// Handled by ValueConstraintChangeRule
				return;
			}
			base.SetValueForCustomStoredAttribute(attribute, newValue);
		}
		#endregion // CustomStorage handlers
		#region ValueConstraint specific
		/// <summary>
		/// Tests if the associated data type is a text type.
		/// </summary>
		public abstract bool IsText();
		/// <summary>
		/// The data type associated with this value range definition
		/// </summary>
		public abstract DataType DataType { get;}
		/// <summary>
		/// Breaks a value range definition into value ranges and adds them
		/// to the ValueRangeCollection.
		/// </summary>
		/// <param name="newDefinition">The string containing a value range definition.</param>
		protected void ParseDefinition(string newDefinition)
		{
			ValueRangeMoveableCollection vrColl = this.ValueRangeCollection;
			// First, remove the container strings from the ends of the definition string
			newDefinition = TrimDefinitionMarkers(newDefinition);
			// Second, find the value ranges in the definition string
			// and add them to the collection
			if(newDefinition.Length != 0)
			{
				vrColl.Clear();
				string[] ranges = Regex.Split(newDefinition, delimSansSpaceEscaped);
				for (int i = 0; i < ranges.Length; ++i)
				{
					string s = ranges[i].Trim();
					ValueRange valueRange = ValueRange.CreateValueRange(Store);
					valueRange.Text = s;
					vrColl.Add(valueRange);
				}
			}
		}
		/// <summary>
		/// Removes the left- and right-strings which denote a value range definition.
		/// </summary>
		/// <param name="definition">The string to remove left and right value range definition strings from.</param>
		public static string TrimDefinitionMarkers(string definition){
			definition = definition.Trim();
			string leftContainerMarkTrimmed = leftContainerMark.Trim();
			if (definition.StartsWith(leftContainerMarkTrimmed))
			{
				definition = definition.Remove(0, leftContainerMarkTrimmed.Length);
			}
			string rightContainerMarkTrimmed = rightContainerMark.Trim();
			if (definition.EndsWith(rightContainerMarkTrimmed))
			{
				definition = definition.Remove(definition.Length - rightContainerMarkTrimmed.Length);
			}
			return definition;
		}
		#endregion //ValueConstraint specific
	}
	#region ValueTypeValueConstraint class
	public partial class ValueTypeValueConstraint
	{
		/// <summary>
		/// Tests if the associated data type is a text type.
		/// </summary>
		/// <returns>
		/// Returns true if the associated object's datatype is a
		/// TextDataType; otherwise, false.
		/// </returns>
		public override bool IsText()
		{
			return ValueType.DataType is TextDataType;
		}
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
		#region ValueTypeValueConstraintChangeRule class
		[RuleOn(typeof(ValueTypeValueConstraint))]
		private class ValueTypeValueConstraintChangeRule : ChangeRule
		{
			/// <summary>
			/// Translate the Text property
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				if (attributeGuid == ValueTypeValueConstraint.TextMetaAttributeGuid)
				{
					ValueTypeValueConstraint valueRangeDefn = e.ModelElement as ValueTypeValueConstraint;
					//Set the new definition
					string newText = (string)e.NewValue;
					if (newText.Length == 0)
					{
						valueRangeDefn.Remove();
					}
					else
					{
						valueRangeDefn.ParseDefinition((string)e.NewValue);
					}
				}
			}
		}
		#endregion // ValueTypeValueConstraintChangeRule class
	}
	#endregion // ValueTypeValueConstraint class
	#region RoleValueConstraint class
	public partial class RoleValueConstraint
	{
		/// <summary>
		/// Tests if the associated data type is a text type.
		/// </summary>
		/// <returns>
		/// Returns true if the associated object's datatype is a
		/// TextDataType; otherwise, false.
		/// </returns>
		public override bool IsText()
		{
			DataType testType = DataType;
			return (testType != null) ? (testType is TextDataType) : false;
		}
		/// <summary>
		/// Override to retrieve the data type
		/// </summary>
		public override DataType DataType
		{
			get
			{
				DataType retVal = null;
				Role role = Role;
				if (role != null)
				{
					ObjectType objectType = role.RolePlayer;
					if (objectType != null)
					{
						retVal = objectType.DataType;
					}
				}
				return retVal;
			}
		}
		#region RoleValueConstraintChangeRule class
		[RuleOn(typeof(RoleValueConstraint))]
		private class RoleValueConstraintChangeRule : ChangeRule
		{
			/// <summary>
			/// Translate the Text property
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				if (attributeGuid == ValueConstraint.TextMetaAttributeGuid)
				{
					RoleValueConstraint valueRangeDefn = e.ModelElement as RoleValueConstraint;
					string newText = (string)e.NewValue;
					if (newText.Length == 0)
					{
						valueRangeDefn.Remove();
					}
					else
					{
						valueRangeDefn.ParseDefinition((string)e.NewValue);
					}
				}
			}
		}
		#endregion // RoleValueConstraintChangeRule class
	}
	#endregion // RoleValueConstraint class
	/// <summary>
	/// 
	/// </summary>
	public abstract partial class ValueMismatchError 
	{ 
		/// <summary>
		/// 
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange;
			}
		}
		
	}
	/// <summary>
	/// 
	/// </summary>
	public partial class MinValueMismatchError : IRepresentModelElements
	{ 
		/// <summary>
		/// 
		/// </summary>
		public override void GenerateErrorText()
		{
			ValueConstraint defn = ValueRange.ValueConstraint;
			RoleValueConstraint roleDefn;
			ValueTypeValueConstraint valueDefn;
			string value = null;
			string newText = null;
			string currentText = Name;
			if (null != (roleDefn = defn as RoleValueConstraint))
			{
				Role attachedRole = roleDefn.Role;
				FactType roleFact = attachedRole.FactType;
				int index = roleFact.RoleCollection.IndexOf(attachedRole) + 1;
				string name = roleFact.Name;
				string model = this.Model.Name;
				newText = string.Format(CultureInfo.CurrentUICulture, ResourceStrings.ModelErrorRoleValueRangeMinValueMismatchError, model, name, index);
			}
			else if (null != (valueDefn = defn as ValueTypeValueConstraint))
			{
				value = valueDefn.ValueType.Name;
				string model = this.Model.Name;
				newText = string.Format(CultureInfo.CurrentUICulture, ResourceStrings.ModelErrorValueRangeMinValueMismatchError, value, model);
			}
			if (currentText != newText)
			{
				Name = newText;
			}
		}
		//we need to find out what object this value range is associated with.  if the value range is the ref mode for the object, then
		// return that object, otherwise return...
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { this.ValueRange.ValueConstraint };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
	}
	/// <summary>
	/// 
	/// </summary>
	public partial class MaxValueMismatchError : IRepresentModelElements
	{
		/// <summary>
		/// 
		/// </summary>
		public override void GenerateErrorText()
		{
			ValueConstraint defn = ValueRange.ValueConstraint;
			RoleValueConstraint roleDefn;
			ValueTypeValueConstraint valueDefn;
			string value = null;
			string newText = null;
			string currentText = Name;
			if (null != (roleDefn = defn as RoleValueConstraint))
			{
				Role attachedRole = roleDefn.Role;
				FactType roleFact = attachedRole.FactType;
				int index = roleFact.RoleCollection.IndexOf(attachedRole) + 1;
				string name = roleFact.Name;
				string model = this.Model.Name;
				newText = string.Format(CultureInfo.CurrentUICulture, ResourceStrings.ModelErrorRoleValueRangeMaxValueMismatchError, model, name, index);
			}
			else if (null != (valueDefn = defn as ValueTypeValueConstraint))
			{
				value = valueDefn.ValueType.Name;
				string model = this.Model.Name;
				newText = string.Format(CultureInfo.CurrentUICulture, ResourceStrings.ModelErrorValueRangeMaxValueMismatchError, value, model);
			}
			if (currentText != newText)
			{
				Name = newText;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { this.ValueRange.ValueConstraint };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
	}
}
