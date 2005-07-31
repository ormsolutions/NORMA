using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using System.Text.RegularExpressions;
using System.Text;

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
	public partial class ValueRange
	{
		#region variables
		private static readonly string valueDelim = ResourceStrings.ValueRangeDefinitionValueDelimiter;
		private static readonly string stringContainerString = ResourceStrings.ValueRangeDefinitionStringContainerPattern;
		private static readonly string leftStringContainerMark = GetLeftMark(stringContainerString);
		private static readonly string rightStringContainerMark = GetRightMark(stringContainerString);
		private static readonly string openInclusionContainerString = ResourceStrings.ValueRangeDefinitionOpenInclusionContainer;
		private static readonly string minOpenInclusionMark = GetLeftMark(openInclusionContainerString);
		private static readonly string maxOpenInclusionMark = GetRightMark(openInclusionContainerString);
		private static readonly string closedInclusionContainerString = ResourceStrings.ValueRangeDefinitionClosedInclusionContainer;
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
					minValue = String.Format(CultureInfo.InvariantCulture, stringContainerString , minValue);
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
		/// <returns>ValueRangeDefinition.IsText() if definition exists; otherwise, true.</returns>
		/// <remarks>Since text is by far the minority of all the data types and
		/// the only type to require string container marks, we do not put values inside 
		/// those marks by default whenever we can't determine the data type (i.e. no
		/// ValueRangeDefinition exists).</remarks>
		protected bool IsText()
		{
			if (this.ValueRangeDefinition != null)
			{
				return ValueRangeDefinition.IsText();
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
	}
	public partial class ValueRangeDefinition
	{
		#region variables
		private static readonly string defnContainerString = ResourceStrings.ValueRangeDefinitionDefinitionContainerPattern;
		private static readonly string leftContainerMark = defnContainerString.Substring(0, defnContainerString.IndexOf("{0}"));
		private static readonly string rightContainerMark = defnContainerString.Substring(defnContainerString.IndexOf("{0}") + 3);
		private static readonly string rangeDelim = ResourceStrings.ValueRangeDefinitionRangeDelimiter;
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
				// Handled by ValueRangeDefinitionChangeRule
				return;
			}
			base.SetValueForCustomStoredAttribute(attribute, newValue);
		}
		#endregion // CustomStorage handlers
		#region ValueRangeDefinition specific
		/// <summary>
		/// Tests if the associated data type is a text type.
		/// </summary>
		public abstract bool IsText();
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
		#endregion //ValueRangeDefinition specific
	}
	#region ValueTypeValueRangeDefinition class
	public partial class ValueTypeValueRangeDefinition
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
		#region ValueTypeValueRangeDefinitionChangeRule class
		[RuleOn(typeof(ValueTypeValueRangeDefinition))]
		private class ValueTypeValueRangeDefinitionChangeRule : ChangeRule
		{
			/// <summary>
			/// Translate the Text property
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				if (attributeGuid == ValueTypeValueRangeDefinition.TextMetaAttributeGuid)
				{
					ValueTypeValueRangeDefinition valueRangeDefn = e.ModelElement as ValueTypeValueRangeDefinition;
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
		#endregion // ValueTypeValueRangeDefinitionChangeRule class
	}
	#endregion // ValueTypeValueRangeDefinition class
	#region RoleValueRangeDefinition class
	public partial class RoleValueRangeDefinition
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
			Role role = this.Role;
			ObjectType objectType = role.RolePlayer;
			if (objectType != null)
			{
				return objectType.DataType is TextDataType;
			}
			return false;
		}
		#region RoleValueRangeDefinitionChangeRule class
		[RuleOn(typeof(RoleValueRangeDefinition))]
		private class RoleValueRangeDefinitionChangeRule : ChangeRule
		{
			/// <summary>
			/// Translate the Text property
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				if (attributeGuid == ValueRangeDefinition.TextMetaAttributeGuid)
				{
					RoleValueRangeDefinition valueRangeDefn = e.ModelElement as RoleValueRangeDefinition;
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
		#endregion // RoleValueRangeDefinitionChangeRule class
	}
	#endregion // RoleValueRangeDefinition class
}
