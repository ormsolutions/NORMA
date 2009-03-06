#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
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
		#region ValueRangeChangeRule rule
		/// <summary>
		/// ChangeRule: typeof(ValueRange)
		/// Translate the Text property
		/// </summary>
		private static void ValueRangeChangeRule(ElementPropertyChangedEventArgs e)
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
		#endregion // ValueRangeChangeRule rule
		#region CustomStorage handlers
		private string GetTextValue()
		{
			string minInclusionMarkToUse = minInclusionMarks[(int)this.MinInclusion];
			string maxInclusionMarkToUse = maxInclusionMarks[(int)this.MaxInclusion];
			string minValue = MinValue;
			string maxValue = MaxValue;
			bool minExists = (minValue.Length != 0);
			bool maxExists = (maxValue.Length != 0);
			bool isText = IsText;
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
		protected bool IsText
		{
			get
			{
				ValueConstraint constraint = ValueConstraint;
				return (constraint != null) ? constraint.IsText : false;
			}
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
		#region ValueMatch Validation
		private static void VerifyValueMatch(ValueRange range, DataType dataType, INotifyElementAdded notifyAdded)
		{
			bool needMinError = false;
			bool needMaxError = false;
			MinValueMismatchError minMismatch;
			MaxValueMismatchError maxMismatch;
			if (dataType != null)
			{
				string min = range.MinValue;
				string max = range.MaxValue;
				if (min.Length != 0 && !dataType.CanParse(min))
				{
					needMinError = true;
					minMismatch = range.MinValueMismatchError;
					if (minMismatch == null)
					{
						minMismatch = new MinValueMismatchError(range.Store);
						minMismatch.ValueRange = range;
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
					if (max.Length != 0 && !dataType.CanParse(max))
					{
						needMaxError = true;
						maxMismatch = range.MaxValueMismatchError;
						if (maxMismatch == null)
						{
							maxMismatch = new MaxValueMismatchError(range.Store);
							maxMismatch.ValueRange = range;
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
			if (!needMinError && null != (minMismatch = range.MinValueMismatchError))
			{
				minMismatch.Delete();
			}
			if (!needMaxError && null != (maxMismatch = range.MaxValueMismatchError))
			{
				maxMismatch.Delete();
			}
		}
		[DelayValidatePriority(1)] // We want this after reference scheme validation
		private static void DelayValidateValueRangeValues(ModelElement element)
		{
			(element as ValueConstraint).VerifyValueRangeValues(null);
		}
		private void VerifyValueRangeValues(INotifyElementAdded notifyAdded)
		{
			if (IsDeleted)
			{
				return;
			}
			DataType dataType = this.DataType;
			RoleValueConstraint roleConstraint = null;
			bool hasError = true;
			if (dataType != null)
			{
				hasError = false;
				LinkedElementCollection<ValueRange> ranges = ValueRangeCollection;
				int rangesCount = ranges.Count;
				for (int i = 0; i < rangesCount; ++i)
				{
					VerifyValueMatch(ranges[i], dataType, notifyAdded);
				}
			}
			else if (null == (roleConstraint = (this as RoleValueConstraint)))
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
					error = new ValueConstraintValueTypeDetachedError(Store);
					error.ValueConstraint = this;
					error.Model = roleConstraint.Role.FactType.Model;
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
		/// <param name="valueType"></param>
		private static void DelayValidateAssociatedValueConstraints(ObjectType valueType)
		{
			if (valueType.IsDeleted)
			{
				return;
			}
			DelayValidateValueConstraint(valueType.ValueConstraint);
			Role.WalkDescendedValueRoles(valueType, null, delegate(Role role, ValueTypeHasDataType dataTypeLink, RoleValueConstraint currentValueConstraint, ValueConstraint previousValueConstraint)
			{
				DelayValidateValueConstraint(currentValueConstraint);
				return true;
			});
		}
		/// <summary>
		/// Validate errors on the specified value constraint
		/// </summary>
		/// <param name="constraint">Constraint to validate. Can be null.</param>
		public static void DelayValidateValueConstraint(ValueConstraint constraint)
		{
			if (constraint != null && !constraint.IsDeleting && !constraint.IsDeleted)
			{
				if (FrameworkDomainModel.DelayValidateElement(constraint, DelayValidateValueRangeValues))
				{
					// Add a text change the first time this is called
					constraint.OnTextChanged();
				}
				FrameworkDomainModel.DelayValidateElement(constraint, DelayValidateValueRangeOverlapError);
			}
		}
		/// <summary>
		/// Force validation of downstream value constraints. Helper function, calls
		/// <see cref="Role.WalkDescendedValueRoles(ObjectType, Role, ValueRoleVisitor)"/>
		/// </summary>
		/// <param name="anchorType">The <see cref="ObjectType"/> to begin walking from</param>
		/// <param name="unattachedRole">The alternate role</param>
		private static void DelayValidateDescendedValueConstraints(ObjectType anchorType, Role unattachedRole)
		{
			Role.WalkDescendedValueRoles(anchorType, unattachedRole, delegate(Role role, ValueTypeHasDataType dataTypeLink, RoleValueConstraint currentValueConstraint, ValueConstraint previousValueConstraint)
			{
				DelayValidateValueConstraint(currentValueConstraint);
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
				DelayValidateAssociatedValueConstraints((e.ElementLink as ValueTypeHasDataType).ValueType);
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
			ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
			ObjectType oldValueType = link.ValueType;
			if (!oldValueType.IsDeleting)
			{
				ValueTypeHasValueConstraint valueTypeConstraintLink = ValueTypeHasValueConstraint.GetLinkToValueConstraint(oldValueType);
				bool hasValueConstraint = valueTypeConstraintLink != null;
				if (!hasValueConstraint)
				{
					Role.WalkDescendedValueRoles(oldValueType, null, delegate(Role role, ValueTypeHasDataType dataTypeLink, RoleValueConstraint currentValueConstraint, ValueConstraint previousValueConstraint)
					{
						if (currentValueConstraint != null && !currentValueConstraint.IsDeleting)
						{
							hasValueConstraint = true;
							return false; // Stop walking
						}
						return true;
					});
				}
				if (hasValueConstraint)
				{
					// Convert this value type into an entity type with a reference mode
					ORMModel model = oldValueType.Model;
					if (model != null)
					{
						// Get a unique name for a new value type
						INamedElementDictionary existingObjectsDictionary = model.ObjectTypesDictionary;
						string newNamePattern = ResourceStrings.ValueTypeAutoCreateReferenceModeNamePattern;
						string baseName = oldValueType.Name;
						string newName = null;
						int i = 0;
						do
						{
							newName = string.Format(CultureInfo.InvariantCulture, newNamePattern, baseName, (i == 0) ? "" : i.ToString(CultureInfo.InvariantCulture));
							++i;
						} while (!existingObjectsDictionary.GetElement(newName).IsEmpty);

						// Create the value type and attach it to a clone of the deleting datatype link
						Partition partition = model.Partition;
						ObjectType newValueType = new ObjectType(partition, new PropertyAssignment[] { new PropertyAssignment(ORMNamedElement.NameDomainPropertyId, newName) });
						ValueTypeHasDataType newDataTypeLink = new ValueTypeHasDataType(
							partition,
							new RoleAssignment[] { new RoleAssignment(ValueTypeHasDataType.ValueTypeDomainRoleId, newValueType), new RoleAssignment(ValueTypeHasDataType.DataTypeDomainRoleId, link.DataType) },
							new PropertyAssignment[] { new PropertyAssignment(ValueTypeHasDataType.ScaleDomainPropertyId, link.Scale), new PropertyAssignment(ValueTypeHasDataType.LengthDomainPropertyId, link.Length) });

						// Attach the new value type to the model
						newValueType.Model = model;

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
								try
								{
									contextInfo[ORMModel.AllowDuplicateNamesKey] = null;
									newValueConstraint.Name = preserveValueConstraintName;
								}
								finally
								{
									contextInfo.Remove(ORMModel.AllowDuplicateNamesKey);
								}
							}
						}
					}
				}
			}
			else
			{
				DelayValidateDescendedValueConstraints(oldValueType, null);
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
				DelayValidateAssociatedValueConstraints((e.ModelElement as ValueTypeHasDataType).ValueType);
			}
		}
		#endregion // DataTypeChangeRule
		#region ValueConstraintAddRule
		/// <summary>
		/// AddRule: typeof(ValueTypeHasValueConstraint)
		/// Checks if the new value range definition matches the data type
		/// </summary>
		private static void ValueConstraintAddRule(ElementAddedEventArgs e)
		{
			DelayValidateValueConstraint((e.ModelElement as ValueTypeHasValueConstraint).ValueConstraint);
		}
		#endregion // ValueConstraintAddRule
		#region RoleValueConstraintAddedRule
		/// <summary>
		/// AddRule: typeof(RoleHasValueConstraint)
		/// Checks if the the value range matches the specified date type
		/// </summary>
		private static void RoleValueConstraintAddedRule(ElementAddedEventArgs e)
		{
			DelayValidateValueConstraint((e.ModelElement as RoleHasValueConstraint).ValueConstraint);
		}
		#endregion // RoleValueConstraintAddedRule
		#region ObjectTypeRoleAdded
		/// <summary>
		/// AddRule: typeof(ObjectTypePlaysRole)
		/// Checks to see if the value on the role added matches the specified data type
		/// </summary>
		private static void ObjectTypeRoleAdded(ElementAddedEventArgs e)
		{
			ObjectType valueType = (e.ModelElement as ObjectTypePlaysRole).RolePlayer;
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
			DelayValidateValueConstraint((e.ModelElement as ValueConstraintHasValueRange).ValueConstraint);
		}
		#endregion // ValueRangeAddedRule
		#region ValueRangeChangeRule
		/// <summary>
		/// ChangeRule: typeof(ValueRange)
		/// Validate values when any non-calculated properties change
		/// </summary>
		private static void ValueRangeChangeRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeGuid = e.DomainProperty.Id;
			if (attributeGuid != ValueRange.TextDomainPropertyId)
			{
				DelayValidateValueConstraint((e.ModelElement as ValueRange).ValueConstraint);
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
				ValueConstraint valueConstraint = e.ModelElement as ValueConstraint;
				//Set the new definition
				string newText = (string)e.NewValue;
				if (newText.Length == 0)
				{
					valueConstraint.Delete();
				}
				else
				{
					valueConstraint.ParseDefinition((string)e.NewValue);
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
			ProcessPreferredIdentifierDeleting(e.ModelElement as EntityTypeHasPreferredIdentifier, null);
		}
		/// <summary>
		/// Common rule code for processing preferred identifier deletion
		/// </summary>
		/// <param name="link">The <see cref="EntityTypeHasPreferredIdentifier"/> being deleted or modified.</param>
		/// <param name="objectType">An alternate EntityType role player from the <paramref name="link"/>. Use the link's role player if this is <see langword="null"/></param>
		private static void ProcessPreferredIdentifierDeleting(EntityTypeHasPreferredIdentifier link, ObjectType objectType)
		{
			if (objectType == null)
			{
				objectType = link.PreferredIdentifierFor;
			}
			if (!objectType.IsDeleting)
			{
				DelayValidateDescendedValueConstraints(objectType, null);
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
			Guid changedRoleGuid = e.DomainRole.Id;
			ObjectType oldObjectType = null;
			if (changedRoleGuid == EntityTypeHasPreferredIdentifier.PreferredIdentifierForDomainRoleId)
			{
				oldObjectType = (ObjectType)e.OldRolePlayer;
			}
			ProcessPreferredIdentifierDeleting(e.ElementLink as EntityTypeHasPreferredIdentifier, oldObjectType);
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
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
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
					null != (oppositeRoleBase = (null != (proxyRole = originalRole.Proxy)) ? proxyRole.OppositeRole : originalRole.OppositeRole) &&
					null != (oppositeRole = oppositeRoleBase.Role))
				{
					Debug.Assert((proxyRole != null) ? (oppositeRole.RolePlayer == originalRole.FactType.NestingType) : oppositeRole.RolePlayer == identifiedObject);
					LinkedElementCollection<Role> playedRoles = identifiedObject.PlayedRoleCollection;
					int playedRolesCount = playedRoles.Count;
					for (int i = 0; i < playedRolesCount; ++i)
					{
						Role testRole = playedRoles[i];
						if (testRole != oppositeRole)
						{
							// Test by skipping the binary fact for the old part of the preferred identifier
							DelayValidateDescendedValueConstraints(originalRolePlayer, testRole);
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
			DelayValidateDescendedValueConstraints((e.ModelElement as ObjectTypePlaysRole).RolePlayer, null);
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
				Role changedRole = (e.ElementLink as ObjectTypePlaysRole).PlayedRole;
				ObjectType oldRolePlayer = (ObjectType)e.OldRolePlayer;
				if (changedRole.IsValueRoleForAlternateRolePlayer(oldRolePlayer))
				{
					// If the old configuration did not have the changed role as a value
					// role then there will be no value roles descended from it.
					bool visited = false;
					Role.WalkDescendedValueRoles((ObjectType)e.NewRolePlayer, changedRole, delegate(Role role, ValueTypeHasDataType dataTypeLink, RoleValueConstraint currentValueConstraint, ValueConstraint previousValueConstraint)
					{
						// If we get any callback here, then the role can still be a value role
						visited = true;
						if (currentValueConstraint != null && !currentValueConstraint.IsDeleting)
						{
							// Make sure that this value constraint is compatible with
							// other constraints above it.
							DelayValidateValueConstraint(currentValueConstraint);
						}
						return true;
					});
					if (!visited)
					{
						// The old role player supported values, the new one does not.
						// Delete any downstream value constraints.
						DelayValidateDescendedValueConstraints(oldRolePlayer, changedRole);
					}
				}
			}
		}
		#endregion // RolePlayerRolePlayerChangeRule
		#endregion // ValueMatch Validation
		#region VerifyValueRangeOverlapError
		[DelayValidatePriority(2)] // We want this after reference scheme validation and range validation
		private static void DelayValidateValueRangeOverlapError(ModelElement element)
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
		#region ValueConstraint specific
		/// <summary>
		/// The data type associated with this value range definition
		/// </summary>
		public abstract DataType DataType { get;}
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
		/// Override to retrieve the data type
		/// </summary>
		public override DataType DataType
		{
			get
			{
				DataType retVal = null;
				Role role;
				Role[] valueRoles;
				if (null != (role = Role) &&
					null != (valueRoles = role.GetValueRoles()))
				{
					retVal = valueRoles[0].RolePlayer.DataType;
				}
				return retVal;
			}
		}
		#endregion // Base overrides
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
	[ModelErrorDisplayFilter(typeof(DataTypeAndValueErrorCategory))]
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
			string currentText = ErrorText;
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
				ErrorText = newText;
			}
		}
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
	}
	#endregion // MinValueMismatchError class
	#region MaxValueMismatchError class
	/// <summary>
	/// MaxValueMismatchError class
	/// </summary>
	[ModelErrorDisplayFilter(typeof(DataTypeAndValueErrorCategory))]
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
			string currentText = ErrorText;
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
				ErrorText = newText;
			}
		}
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
	}
	#endregion // MaxValueMismatchError class
	#region ValueConstraintValueTypeDetachedError class
	/// <summary>
	/// This is the model error message for value ranges that overlap
	/// </summary>
	[ModelErrorDisplayFilter(typeof(DataTypeAndValueErrorCategory))]
	public partial class ValueConstraintValueTypeDetachedError
	{
		#region Base overrides
		/// <summary>
		/// GenerateErrorText
		/// </summary>
		public override void GenerateErrorText()
		{
			ValueConstraint defn = ValueConstraint;
			RoleValueConstraint roleDefn;
			string newText = "";
			string currentText = ErrorText;
			if (null != (roleDefn = defn as RoleValueConstraint))
			{
				Role attachedRole = roleDefn.Role;
				FactType roleFact = attachedRole.FactType;
				int index = roleFact.RoleCollection.IndexOf(attachedRole) + 1;
				string name = roleFact.Name;
				string model = this.Model.Name;
				newText = string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorRoleValueTypeDetachedError, model, name, index);
			}
			if (currentText != newText)
			{
				ErrorText = newText;
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
		#endregion // Base overrides
	}
	#endregion // ValueConstraintValueTypeDetachedError class
	#region ValueRangeOverlapError
	/// <summary>
	/// This is the model error message for value ranges that overlap
	/// </summary>
	[ModelErrorDisplayFilter(typeof(DataTypeAndValueErrorCategory))]
	public partial class ValueRangeOverlapError
	{
		#region Base overrides
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
			string currentText = ErrorText;
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
				ErrorText = newText;
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
		#endregion // Base overrides
	}
	#endregion // ValueRangeOverlapError
}
