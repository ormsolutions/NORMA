using System;
using Microsoft.VisualStudio.Modeling;
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// A helper class to enable returning custom values from
	/// the ElementPropertyDescriptor.GetSetFieldString function.
	/// The values appear in the undo/redo dropdowns in the shell.
	/// </summary>
	public class CustomSetStringPropertyDescriptor : ElementPropertyDescriptor
	{
		private string mySetString;
		/// <summary>
		/// Create a new CustomSetStringPropertyDescriptor
		/// </summary>
		/// <param name="setString">The set string value</param>
		/// <param name="modelElement">forwarded to base</param>
		/// <param name="metaAttributeInfo">forwarded to base</param>
		/// <param name="requestor">forwarded to base</param>
		/// <param name="attributes">forwarded to base</param>
		public CustomSetStringPropertyDescriptor(string setString, ModelElement modelElement, MetaAttributeInfo metaAttributeInfo, ModelElement requestor, Attribute[] attributes) : base(modelElement, metaAttributeInfo, requestor, attributes)
		{
			mySetString = setString;
		}
		/// <summary>
		/// Return the user provided value for the set string.
		/// This shows up as the transaction name in shell's
		/// undo/redo dropdowns
		/// </summary>
		/// <param name="caption">ignored</param>
		/// <returns>setSetring value from constructor</returns>
		protected override string GetSetFieldString(string caption)
		{
			return mySetString;
		}
	}
}