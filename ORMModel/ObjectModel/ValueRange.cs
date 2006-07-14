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
	public partial class ValueRange : IModelErrorOwner, IHasIndirectModelErrorOwner
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
				if (null != (max = MaxValueMismatchError))
				{
					yield return max;
				}
				if (null != (min = MinValueMismatchError))
				{
					yield return min;
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
			VerifyValueMatch(notifyAdded);
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
			// UNDONE: DelayedValidation (ValueRange)
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner implementation
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
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { ValueConstraintHasValueRange.ValueRangeDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region ValueMatch Validation
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
						minMismatch = new MinValueMismatchError(Store);
						minMismatch.ValueRange = this;
						minMismatch.Model = dataType.Model;
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
							maxMismatch = new MaxValueMismatchError(Store);
							maxMismatch.ValueRange = this;
							maxMismatch.Model = dataType.Model;
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
				minMismatch.Delete();
			}
			if (!needMaxError && null != (maxMismatch = MaxValueMismatchError))
			{
				maxMismatch.Delete();
			}
		}
		private static void ValidateValueConstraintForRule(ValueConstraint valueConstraint)
		{
			if (valueConstraint == null)
			{
				return;
			}
			LinkedElementCollection<ValueRange> ranges = valueConstraint.ValueRangeCollection;
			int rangesCount = ranges.Count;
			for (int i = 0; i < rangesCount; ++i)
			{
				ranges[i].VerifyValueMatch(null);
			}
		}
		#region ValueTypeHasDataType rule
		[RuleOn(typeof(ValueTypeHasDataType), FireTime = TimeToFire.LocalCommit)]
		private sealed class DataTypeAddRule : AddRule
		{
			/// <summary>
			/// Test if the changed value does not match the specified data type.
			/// </summary>
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
				ObjectType valueType = link.ValueType;
				ValidateValueConstraintForRule(valueType.ValueConstraint);
				LinkedElementCollection<Role> roles = valueType.PlayedRoleCollection;
				int rolesCount = roles.Count;
				for (int i = 0; i < rolesCount; ++i)
				{
					ValidateValueConstraintForRule(roles[i].ValueConstraint);
				}
			}
		}
		#endregion // ValueTypeHasDataType rule
		#region DataTypeChangeRule rule
		[RuleOn(typeof(ValueTypeHasDataType))]
		private sealed class DataTypeChangeRule : ChangeRule
		{
			/// <summary>
			/// checks first if the data type has been chagned and then test if the 
			/// value matches the datatype
			/// </summary>
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
				ObjectType valueType = link.ValueType;
				ValidateValueConstraintForRule(valueType.ValueConstraint);
				LinkedElementCollection<Role> roles = valueType.PlayedRoleCollection;
				int rolesCount = roles.Count;
				for (int i = 0; i < rolesCount; ++i)
				{
					ValidateValueConstraintForRule(roles[i].ValueConstraint);
				}
			}
		}
		#endregion // DataTypeChangeRule rule
		#region ValueConstraintAddRule rule
		[RuleOn(typeof(ValueTypeHasValueConstraint), FireTime = TimeToFire.LocalCommit)]
		private sealed class ValueConstraintAddRule : AddRule
		{
			/// <summary>
			/// checks if the new value range definition matches the data type
			/// </summary>
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ValueTypeHasValueConstraint link = e.ModelElement as ValueTypeHasValueConstraint;
				ObjectType valueType = link.ValueType;
				ValidateValueConstraintForRule(valueType.ValueConstraint);
				LinkedElementCollection<Role> roles = valueType.PlayedRoleCollection;
				int rolesCount = roles.Count;
				for (int i = 0; i < rolesCount; ++i)
				{
					ValidateValueConstraintForRule(roles[i].ValueConstraint);
				}
			}
		}
		#endregion // ValueConstraintAddRule rule
		#region RoleValueConstraintAdded rule
		[RuleOn(typeof(RoleHasValueConstraint), FireTime = TimeToFire.LocalCommit)]
		private sealed class RoleValueConstraintAdded : AddRule
		{
			/// <summary>
			/// checks if the the value range matches the specified date type
			/// </summary>
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				RoleHasValueConstraint link = e.ModelElement as RoleHasValueConstraint;
				ObjectType valueType = link.Role.RolePlayer;
				ValidateValueConstraintForRule(valueType.ValueConstraint);
				LinkedElementCollection<Role> roles = valueType.PlayedRoleCollection;
				int rolesCount = roles.Count;
				for (int i = 0; i < rolesCount; ++i)
				{
					ValidateValueConstraintForRule(roles[i].ValueConstraint);
				}
			}
		}
		#endregion // RoleValueConstraintAdded rule
		#region ObjectTypeRoleAdded rule
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime= TimeToFire.LocalCommit)]
		private sealed class ObjectTypeRoleAdded : AddRule
		{
			/// <summary>
			/// checks to see if the value on the role added matches the specified data type
			/// </summary>
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				ObjectType valueType = link.RolePlayer;
				ValidateValueConstraintForRule(valueType.ValueConstraint);
				LinkedElementCollection<Role> roles = valueType.PlayedRoleCollection;
				int rolesCount = roles.Count;
				for (int i = 0; i < rolesCount; ++i)
				{
					ValidateValueConstraintForRule(roles[i].ValueConstraint);
				}
			}
		}
		#endregion // ObjectTypeRoleAdded rule
		#region ValueRangeAdded rule
		[RuleOn(typeof(ValueConstraintHasValueRange), FireTime = TimeToFire.LocalCommit)]
		private sealed class ValueRangeAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ValueConstraintHasValueRange link = e.ModelElement as ValueConstraintHasValueRange;
				link.ValueRange.VerifyValueMatch(null);
			}
		}
		#endregion // ValueRangeAdded rule
		#region ValueRangeChangeRule rule
		[RuleOn(typeof(ValueRange))]
		private sealed class ValueRangeChangeRule : ChangeRule
		{
			/// <summary>
			/// Translate the Text property
			/// </summary>
			/// <param name="e"></param>
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				Guid attributeGuid = e.DomainProperty.Id;
				if (attributeGuid == ValueRange.TextDomainPropertyId)
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
		#endregion // ValueRangeChangeRule rule
		#endregion // ValueMatch Validation
		#region CustomStorage handlers
		private string GetTextValue()
		{
			string minInclusionMarkToUse = minInclusionMarks[(int)this.MinInclusion];
			string maxInclusionMarkToUse = maxInclusionMarks[(int)this.MaxInclusion];
			string minValue = MinValue;
			string maxValue = MaxValue;
			bool minExists = (minValue.Length != 0);
			bool maxExists = (maxValue.Length != 0);
			bool isText = IsText();
			// put values in string container if need to
			if (minExists && isText)
			{
				minValue = String.Format(CultureInfo.InvariantCulture, stringContainerString, minValue);
			}
			if (maxExists && isText)
			{
				maxValue = String.Format(CultureInfo.InvariantCulture, stringContainerString, maxValue);
			}
			// Assemble values into a value range text
			if (minExists && maxExists && !minValue.Equals(maxValue))
			{
				return string.Concat(minInclusionMarkToUse, minValue, valueDelim, maxValue, maxInclusionMarkToUse);
			}
			else if (minExists && !maxExists)
			{
				return string.Concat(minInclusionMarkToUse, minValue, valueDelim, maxInclusionMarkToUse);
			}
			else if (minExists && ((maxExists && minValue.Equals(maxValue)) || !maxExists))
			{
				return minValue;
			}
			else
			{
				return string.Concat(minInclusionMarkToUse, valueDelim, maxValue, maxInclusionMarkToUse);
			}
		}
		private void SetTextValue(string newValue)
		{
			// Handled by ValueRangeChangeRule
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
	public partial class ValueConstraint : IModelErrorOwner
	{
		#region variables
		private static readonly string defnContainerString = ResourceStrings.ValueConstraintDefinitionContainerPattern;
		private static readonly string leftContainerMark = defnContainerString.Substring(0, defnContainerString.IndexOf("{0}"));
		private static readonly string rightContainerMark = defnContainerString.Substring(defnContainerString.IndexOf("{0}") + 3);
		private static readonly string rangeDelim = ResourceStrings.ValueConstraintRangeDelimiter;
		private static readonly string delimSansSpace = rangeDelim.Trim();
		private static readonly string delimSansSpaceEscaped = Regex.Escape(delimSansSpace);
		#endregion // variables
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
				foreach (ModelErrorUsage rangeError in (ranges[i] as IModelErrorOwner).GetErrorCollection(filter))
				{
					yield return rangeError;
				}
			}
			if (0 != (filter & ModelErrorUses.Verbalize))
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
			ORMCoreModel.DelayValidateElement(this, DelayValidateValueRangeOverlapError);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner implementation
		#region VerifyValueRangeOverlapError
		private void DelayValidateValueRangeOverlapError(ModelElement element)
		{
			(element as ValueConstraint).VerifyValueRangeOverlapError(null);
		}
		private struct RangeValueNode
		{
			private string myValue;
			private bool myIsLower;
			private int myIndex;
			private RangeInclusion myInclusion;
			public RangeValueNode(string value, bool isLower, int index, RangeInclusion inclusion)
			{
				myValue = value;
				myIsLower = isLower;
				myIndex = index;
				myInclusion = inclusion;
			}
			public string Value
			{
				get
				{
					return myValue;
				}
			}
			public bool IsLower
			{
				get
				{
					return myIsLower;
				}
			}
			public int Index
			{
				get
				{
					return myIndex;
				}
			}
			public RangeInclusion Inclusion
			{
				get
				{
					return myInclusion;
				}
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
			if (dataType != null && dataType.CanCompare == true)
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
								// UNDONE: None of the single elements should be equal
							}
							break;
						}
					case DataTypeRangeSupport.Closed:
					case DataTypeRangeSupport.Open:
						{
							bool alwaysClosed = rangeSupport == DataTypeRangeSupport.Closed;
							if (rangeCount == 1)
							{
								// The data is overlapping if the values are backwards
								range = ranges[0];
								minValue = range.MinValue;
								maxValue = range.MaxValue;
								if (minValue.Length != 0 && dataType.CanParse(minValue) &&
									maxValue.Length != 0 && dataType.CanParse(maxValue))
								{
									hasError = dataType.Compare(minValue, maxValue) > 0;
								}
							}
							else
							{
								RangeValueNode[] nodes = new RangeValueNode[rangeCount + rangeCount];
								int index = 0;
								int leftIndex;
								int rightIndex;
								RangeInclusion inclusion = RangeInclusion.Closed;
								bool keepGoing = true;
								for (int i = 0; i < rangeCount; ++i)
								{
									range = ranges[i];

									// Add the lower node
									if (!alwaysClosed)
									{
										inclusion = range.MinInclusion;
										if (inclusion == RangeInclusion.NotSet)
										{
											inclusion = RangeInclusion.Closed;
										}
									}
									minValue = range.MinValue;
									if (minValue.Length != 0 && !dataType.CanParse(minValue))
									{
										keepGoing = false;
										break;
									}

									nodes[index] = new RangeValueNode(minValue, true, i, inclusion);

									// Add the upper node
									maxValue = range.MaxValue;
									if (maxValue.Length != 0 && !dataType.CanParse(maxValue))
									{
										keepGoing = false;
										break;
									}
									if (!alwaysClosed)
									{
										inclusion = range.MaxInclusion;
										if (inclusion == RangeInclusion.NotSet)
										{
											inclusion = RangeInclusion.Closed;
										}
									}
									nodes[index + 1] = new RangeValueNode(maxValue, false, i, inclusion);
									index += 2;
								}
								if (keepGoing)
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
									for (int i = 0; i < rangeCount; ++i)
									{
										lower = nodes[index];
										if (!lower.IsLower)
										{
											keepGoing = false;
											break;
										}
										if ((alwaysClosed ||(lower.Inclusion == RangeInclusion.Closed && upper.Inclusion == RangeInclusion.Closed)) &&
											i != 0 &&
											lower.Value.Length != 0 &&
											upper.Value.Length != 0 &&
											0 == dataType.Compare(lower.Value, upper.Value))
										{
											keepGoing = false;
											break;
										}
										upper = nodes[index + 1];
										if (upper.IsLower || (upper.Index != lower.Index))
										{
											keepGoing = false;
											break;
										}
										index += 2;
									}
									hasError = !keepGoing;
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
					error = new ValueRangeOverlapError(Store);
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
		#endregion // VerifyValueRangeOverlapError
		#region CustomStorage handlers
		private string GetTextValue()
		{
			LinkedElementCollection<ValueRange> ranges = ValueRangeCollection;
			int rangeCount = ranges.Count;
			if (rangeCount <= 0)
			{
				return String.Empty;
			}
			else if (rangeCount == 1)
			{
				return string.Concat(leftContainerMark, ranges[0].Text, rightContainerMark);
			}
			else
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
				return valueRangeText.ToString();
			}
		}
		private void SetTextValue(string newValue)
		{
			// Handled by ValueConstraintChangeRule
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
			LinkedElementCollection<ValueRange> vrColl = this.ValueRangeCollection;
			// First, remove the container strings from the ends of the definition string
			newDefinition = TrimDefinitionMarkers(newDefinition);
			// Second, find the value ranges in the definition string
			// and add them to the collection
			if (newDefinition.Length != 0)
			{
				vrColl.Clear();
				string[] ranges = Regex.Split(newDefinition, delimSansSpaceEscaped);
				for (int i = 0; i < ranges.Length; ++i)
				{
					string s = ranges[i].Trim();
					ValueRange valueRange = new ValueRange(Store);
					valueRange.Text = s;
					vrColl.Add(valueRange);
				}
			}
		}
		/// <summary>
		/// Removes the left- and right-strings which denote a value range definition.
		/// </summary>
		/// <param name="definition">The string to remove left and right value range definition strings from.</param>
		public static string TrimDefinitionMarkers(string definition)
		{
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
	public partial class ValueTypeValueConstraint : IHasIndirectModelErrorOwner
	{
		#region Base overrides
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
		#endregion // Base overrides
		#region ValueTypeValueConstraintChangeRule class
		[RuleOn(typeof(ValueTypeValueConstraint))]
		private sealed class ValueTypeValueConstraintChangeRule : ChangeRule
		{
			/// <summary>
			/// Translate the Text property
			/// </summary>
			/// <param name="e"></param>
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				Guid attributeGuid = e.DomainProperty.Id;
				if (attributeGuid == ValueTypeValueConstraint.TextDomainPropertyId)
				{
					ValueTypeValueConstraint valueRangeDefn = e.ModelElement as ValueTypeValueConstraint;
					//Set the new definition
					string newText = (string)e.NewValue;
					if (newText.Length == 0)
					{
						valueRangeDefn.Delete();
					}
					else
					{
						valueRangeDefn.ParseDefinition((string)e.NewValue);
					}
				}
			}
		}
		#endregion // ValueTypeValueConstraintChangeRule class
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
	public partial class RoleValueConstraint : IHasIndirectModelErrorOwner
	{
		#region Base overrides
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
		#endregion // Base overrides
		#region RoleValueConstraintChangeRule class
		[RuleOn(typeof(RoleValueConstraint))]
		private sealed class RoleValueConstraintChangeRule : ChangeRule
		{
			/// <summary>
			/// Translate the Text property
			/// </summary>
			/// <param name="e"></param>
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				Guid attributeGuid = e.DomainProperty.Id;
				if (attributeGuid == ValueConstraint.TextDomainPropertyId)
				{
					RoleValueConstraint valueRangeDefn = e.ModelElement as RoleValueConstraint;
					string newText = (string)e.NewValue;
					if (newText.Length == 0)
					{
						valueRangeDefn.Delete();
					}
					else
					{
						valueRangeDefn.ParseDefinition((string)e.NewValue);
					}
				}
			}
		}
		#endregion // RoleValueConstraintChangeRule class
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
	#region ValueMismatchError class
	/// <summary>
	/// ValueMismatch error abstract class
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
	#endregion // ValueMismatchError class
	#region MinValueMismatchError class
	/// <summary>
	/// MinValueMismatchError class
	/// </summary>
	public partial class MinValueMismatchError : IRepresentModelElements
	{
		/// <summary>
		/// Standard override
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
				newText = string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorRoleValueRangeMinValueMismatchError, model, name, index);
			}
			else if (null != (valueDefn = defn as ValueTypeValueConstraint))
			{
				value = valueDefn.ValueType.Name;
				string model = this.Model.Name;
				newText = string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorValueRangeMinValueMismatchError, value, model);
			}
			if (currentText != newText)
			{
				Name = newText;
			}
		}
		/// <summary>
		/// Implements IRepresentModelElements.GetRepresentedElements
		/// </summary>
		protected ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { this.ValueRange.ValueConstraint };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
	}
	#endregion // MinValueMismatchError class
	#region MaxValueMismatchError class
	/// <summary>
	/// MaxValueMismatchError class
	/// </summary>
	public partial class MaxValueMismatchError : IRepresentModelElements
	{
		/// <summary>
		/// Standard override
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
				newText = string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorRoleValueRangeMaxValueMismatchError, model, name, index);
			}
			else if (null != (valueDefn = defn as ValueTypeValueConstraint))
			{
				value = valueDefn.ValueType.Name;
				string model = this.Model.Name;
				newText = string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorValueRangeMaxValueMismatchError, value, model);
			}
			if (currentText != newText)
			{
				Name = newText;
			}
		}
		/// <summary>
		/// Implements IRepresentModelElements.GetRepresentedElements
		/// </summary>
		protected ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { this.ValueRange.ValueConstraint };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
	}
	#endregion // MaxValueMismatchError class
	#region ValueRangeOverlapError
	/// <summary>
	/// This is the model error message for value ranges that overlap
	/// </summary>
	public partial class ValueRangeOverlapError : IRepresentModelElements
	{
		/// <summary>
		/// GenerateErrorText
		/// </summary>
		public override void GenerateErrorText()
		{
			ValueConstraint defn = ValueConstraint;
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
				newText = string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorRoleValueRangeOverlapError, model, name, index);
			}
			else if (null != (valueDefn = defn as ValueTypeValueConstraint))
			{
				value = valueDefn.ValueType.Name;
				string model = this.Model.Name;
				newText = string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorValueTypeValueRangeOverlapError, value, model);
			}
			if (currentText != newText)
			{
				Name = newText;
			}
		}
		/// <summary>
		/// RegenerateEvents
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange;
			}
		}
		#region IRepresentModelElements Members
		/// <summary>
		/// GetRepresentedElements
		/// </summary>
		/// <returns></returns>

		public ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { this.ValueConstraint };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Members
	}
	#endregion // ValueRangeOverlapError
}
