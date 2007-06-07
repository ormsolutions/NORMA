using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORMOialBridge
{
	public partial class OialModelIsForORMModel
	{
		private partial class ObjectTypeRule
		{
			/// <summary>
			/// Fires when an entity type changes to a value type.  Records a CHANGE event, not add, since the element has not been added.
			/// </summary>
			[RuleOn(typeof(ValueTypeHasDataType))]
			private partial class ValueTypeDataTypeAddRule : AddRule
			{
				public override void ElementAdded(ElementAddedEventArgs e)
				{
					ValueTypeHasDataType valueTypeHasDataType = (ValueTypeHasDataType)e.ModelElement;
					ObjectType objectType = (ObjectType)valueTypeHasDataType.ValueType;
					AddTransactedModelElement(objectType, OialModelElementAction.Change);
					ORMCoreDomainModel.DelayValidateElement(objectType.Model, OialModelIsForORMModel.DelayValidateModel);
				}
			}

			// We're deliberately ignoring the value type's data type change event

			/// <summary>
			/// Fires when a value type changes to an entity type.  Records a CHANGE event, not delete, since the element has not been deleted.
			/// </summary>
			[RuleOn(typeof(ValueTypeHasDataType))]
			private partial class ValueTypeDataTypeDeleteRule : DeleteRule
			{
				public override void ElementDeleted(ElementDeletedEventArgs e)
				{
					ValueTypeHasDataType valueTypeHasDataType = (ValueTypeHasDataType)e.ModelElement;
					ObjectType objectType = (ObjectType)valueTypeHasDataType.ValueType;
					AddTransactedModelElement(objectType, OialModelElementAction.Change);
					ORMCoreDomainModel.DelayValidateElement(objectType.Model, OialModelIsForORMModel.DelayValidateModel);
				}
			}
		}
	}
}
