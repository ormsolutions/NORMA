using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;

namespace Northface.Tools.ORM.ObjectModel
{
	public partial class FactType : INamedElementDictionaryChild
	{
		#region FactType Specific
		/// <summary>
		/// Get a read-only list of FactConstraint links. To get the
		/// constraints from here, use the ConstraintCollection property on the returned
		/// object. To get to the roles, use the ConstrainedRoleCollection property.
		/// </summary>
		[CLSCompliant(false)]
		public IList<ExternalFactConstraint> ExternalFactConstraintCollection
		{
			get
			{
				IList untypedList = GetElementLinks(ExternalFactConstraint.FactTypeCollectionMetaRoleGuid);
				int elementCount = untypedList.Count;
				ExternalFactConstraint[] typedList = new ExternalFactConstraint[elementCount];
				untypedList.CopyTo(typedList, 0);
				return typedList;
			}
		}
		#endregion // FactType Specific
		#region Customize property display
		/// <summary>
		/// Distinguish between an objectified and a
		/// normal fact type.
		/// </summary>
		public override string GetClassName()
		{
			return (NestingType == null) ? ResourceStrings.FactType : ResourceStrings.ObjectifiedFactType;
		}
		#endregion // Customize property display
		#region CustomStorage handlers
		/// <summary>
		/// Standard override. All custom storage properties are derived, not
		/// stored. Actual changes are handled in FactTypeChangeRule.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <param name="newValue">object</param>
		public override void SetValueForCustomStoredAttribute(MetaAttributeInfo attribute, object newValue)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == NestingTypeDisplayMetaAttributeGuid)
			{
				// Handled by FactTypeChangeRule
				return;
			}
			base.SetValueForCustomStoredAttribute(attribute, newValue);
		}
		/// <summary>
		/// Standard override. Retrieve values for calculated properties.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <returns></returns>
		public override object GetValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == NestingTypeDisplayMetaAttributeGuid)
			{
				return NestingType;
			}
			return base.GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Standard override. Defer to GetValueForCustomStoredAttribute.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <returns></returns>
		protected override object GetOldValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			return GetValueForCustomStoredAttribute(attribute);
		}
		#endregion // CustomStorage handlers
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			GetRoleGuids(out parentMetaRoleGuid, out childMetaRoleGuid);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'ModelHasFactType' naming set.
		/// </summary>
		/// <param name="parentMetaRoleGuid">Guid</param>
		/// <param name="childMetaRoleGuid">Guid</param>
		protected void GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			parentMetaRoleGuid = ModelHasFactType.ModelMetaRoleGuid;
			childMetaRoleGuid = ModelHasFactType.FactTypeCollectionMetaRoleGuid;
		}
		#endregion // INamedElementDictionaryChild implementation
		#region RoleChangeRule class
		[RuleOn(typeof(FactType))]
		private class FactTypeChangeRule : ChangeRule
		{
			/// <summary>
			/// Forward through the property grid property to the underlying
			/// nesting type property
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				if (attributeGuid == FactType.NestingTypeDisplayMetaAttributeGuid)
				{
					(e.ModelElement as FactType).NestingType = e.NewValue as ObjectType;
				}
			}
		}
		#endregion // RoleChangeRule class
		#region FactTypeHasReading rule classes
		[RuleOn(typeof(FactTypeHasReading))]
		private class FactTypeHasReadingAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasReading link = e.ModelElement as FactTypeHasReading;
				FactType theFact = link.FactType;
				ReadingMoveableCollection factReadings = theFact.ReadingCollection;
				int roleCount = factReadings.Count;
				ReadingMoveableCollection readings = theFact.ReadingCollection;
				if (readings.Count == 1)
				{
					Reading onlyReading = readings[0];
					if (!onlyReading.IsPrimary)
					{
						onlyReading.IsPrimary = true;
					}
				}
				else
				{
					//if more than one reading and the new one is set to be the
					//primary one then setting any others that are primary to false.
					Reading newReading = link.ReadingCollection;
					if (newReading.IsPrimary)
					{
						Reading r;
						for (int i = 0; i < roleCount; ++i)
						{
							r = factReadings[i];
							if (!object.ReferenceEquals(r, newReading))
							{
								if (r.IsPrimary)
								{
									r.IsPrimary = false;
									//UNDONE:break? should only be one.
								}
							}
						}
					}
				}
			}
		}

		[RuleOn(typeof(FactTypeHasReading))]
		private class FactTypeHasReadingRemoved : RemoveRule
		{
			/// <summary>
			/// deals with the primary reading being removed by selecting the first
			/// reading in the list if there are any left.
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				//TODO:test
				FactTypeHasReading link = e.ModelElement as FactTypeHasReading;
				FactType fact = link.FactType;
				Reading read = link.ReadingCollection;
				if (read.IsPrimary)
				{
					ReadingMoveableCollection allReadings = fact.ReadingCollection;
					if (allReadings.Count > 0)
					{
						allReadings[0].IsPrimary = true;
					}
				}
			}
		}

		#endregion FactTypeReadingRoleRemoved rule class
		#region Reading facade method
		/// <summary>
		/// Adds a reading to the fact.
		/// </summary>
		/// <param name="readingText">The text of the reading to add.</param>
		/// <returns>The reading that was added.</returns>
		public Reading AddReading(string readingText)
		{
			RoleMoveableCollection factRoles = RoleCollection;
			int roleCount = factRoles.Count;
			if(!Reading.IsValidReadingText(readingText, roleCount))
			{
				throw new ArgumentException(ResourceStrings.ModelExceptionFactAddReadingInvalidReadingText, "readingText");
			}

			Store theStore = this.Store;
			bool setIsPrimary = ReadingCollection.Count == 0;
			AttributeAssignment[] attrList = new AttributeAssignment[setIsPrimary ? 2 : 1];

			attrList[0] = new AttributeAssignment(Reading.TextMetaAttributeGuid, readingText, theStore);
			if (setIsPrimary)
			{
				attrList[1] = new AttributeAssignment(Reading.IsPrimaryMetaAttributeGuid, true, theStore);
			}
			
			Reading retval = Reading.CreateAndInitializeReading(theStore, attrList);
			RoleMoveableCollection readingRoles = retval.RoleCollection;
			for (int i = 0; i < roleCount; ++i)
			{
				readingRoles.Add(factRoles[i]);
			}

			return retval;
		}
		#endregion // Reading facade method
	}
}