using System;
using System.Reflection;
namespace ExtensionExample
{
	#region Attach rules to ExtensionDomainModel model
	public partial class ExtensionDomainModel
	{
		/// <summary>
		/// Generated code to attach rules to the store.
		/// </summary>
		protected override Type[] AllMetaModelTypes()
		{
			if (!(Neumont.Tools.ORM.ObjectModel.ORMMetaModel.InitializingToolboxItems))
			{
				return Type.EmptyTypes;
			}
			Type[] retVal = new Type[]{
				typeof(ExtensionAddRule)};
			System.Diagnostics.Debug.Assert(!(((System.Collections.IList)retVal).Contains(null)), "One or more rule types failed to resolve. The file and/or package will fail to load.");
			return retVal;
		}
	}
	#endregion // Attach rules to ExtensionDomainModel model
}
