using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.Modeling;

namespace Neumont.Tools.ORMToORMAbstractionBridge
{
	public partial class AbstractionModelIsForORMModel
	{
		#region ObjectType Rules
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasDataType)
		/// Fires when an entity type changes to a value type.  Records a CHANGE event, not add, since the element has not been added.
		/// </summary>
		private static void ValueTypeDataTypeAddRule(ElementAddedEventArgs e)
		{
			ValueTypeHasDataType valueTypeHasDataType = (ValueTypeHasDataType)e.ModelElement;
			ObjectType objectType = (ObjectType)valueTypeHasDataType.ValueType;
			AddTransactedModelElement(objectType, OialModelElementAction.Change);
			ORMModel model = objectType.Model;
			if (model != null)
			{
				FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
			}
		}
		/// <summary>
		/// DeletingRule: typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasDataType)
		/// Fires when a value type changes to an entity type.  Records a CHANGE event, not delete, since the element has not been deleted.
		/// </summary>
		private static void ValueTypeDataTypeDeletingRule(ElementDeletingEventArgs e)
		{
			ValueTypeHasDataType valueTypeHasDataType = (ValueTypeHasDataType)e.ModelElement;
			ObjectType objectType = (ObjectType)valueTypeHasDataType.ValueType;
			if (!objectType.IsDeleting)
			{
				AddTransactedModelElement(objectType, OialModelElementAction.Change);
				ORMModel model = objectType.Model; // Note that if the model were in a deleting phase, then the object model would be as well, no need to check
				if (model != null)
				{
					FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
				}
			}
		}
		#endregion // ObjectType Rules
	}
}
