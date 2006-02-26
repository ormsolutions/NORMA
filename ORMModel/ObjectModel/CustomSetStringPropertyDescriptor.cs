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
using Microsoft.VisualStudio.Modeling;
namespace Neumont.Tools.ORM.ObjectModel
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
