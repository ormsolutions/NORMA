using System;
using System.Collections.Generic;
using System.Text;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;


namespace ExtensionExample
{
	public partial class MyCustomExtensionElement : IORMPropertyExtension
	{
		#region IORMPropertyExtension Members

		public ORMExtensionPropertySettings ExtensionPropertySettings
		{
			get { return ORMExtensionPropertySettings.MergeAsDirectProperty; }
		}		

		public Guid ExtensionDefaultAttribute
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public Store ExtensionStore
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		#endregion
}
	[RuleOn(typeof(Role))]
	public partial class ExtensionAddRule : AddRule
	{
		public override void ElementAdded(ElementAddedEventArgs e)
		{
			IORMExtendableElement extendable;
			if(null != (extendable = e.ModelElement as IORMExtendableElement))
			{
				MyCustomExtensionElement customElement = MyCustomExtensionElement.CreateMyCustomExtensionElement(e.ModelElement.Store);
				customElement.TestProperty = "It is impossible that UML Rocks!";
				ExtensionUtility.AddExtensionElement(customElement, extendable);
			}
		}
	}
}
