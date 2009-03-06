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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.Design
{
	/// <summary>
	/// An element picker to select data types for a value type.
	/// Associated with the ObjectType.DataTypeDisplay property
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class ColumnDataTypePicker : ElementPicker<ColumnDataTypePicker>
	{
		private sealed class DataTypeToStringComparer : IComparer<DataType>
		{
			public DataTypeToStringComparer()
			{
				myStringComparer = StringComparer.CurrentCulture;
			}
			private readonly StringComparer myStringComparer;
			public int Compare(DataType x, DataType y)
			{
				return myStringComparer.Compare(x.ToString(), y.ToString());
			}
		}
		/// <summary>
		/// Returns a list of data types that can be used by a column. The UnspecifiedDataType is never offered.
		/// </summary>
		/// <param name="context">ITypeDescriptorContext. Used to retrieve the selected instance</param>
		/// <param name="value">The current value</param>
		/// <returns>A list of candidates</returns>
		protected sealed override IList GetContentList(ITypeDescriptorContext context, object value)
		{
			Debug.Assert(!(value is object[]));
			Column instance = (Column)EditorUtility.ResolveContextInstance(context.Instance, true); // true to pick any element. We can use any element to get at the datatypes on the model
			ObjectType valueType = instance.AssociatedValueType;
			IList retVal = null;
			if (valueType != null)
			{
				LinkedElementCollection<DataType> dataTypes = valueType.Model.DataTypeCollection;
				retVal = dataTypes;
				int count = dataTypes.Count;
				int unspecifiedIndex = dataTypes.FindIndex(delegate(DataType testType){return testType is UnspecifiedDataType;});
				if (unspecifiedIndex >= 0 || count > 1)
				{
					DataType[] types;
					if (unspecifiedIndex >= 0)
					{
						int nextIndex = 0;
						types = new DataType[count - 1];
						for (int i = 0; i < count; ++i)
						{
							if (i == unspecifiedIndex)
							{
								continue;
							}
							types[nextIndex] = dataTypes[i];
							++nextIndex;
						}
					}
					else
					{
						types = new DataType[count];
						dataTypes.CopyTo(types, 0);
					}
					Array.Sort<DataType>(types, new DataTypeToStringComparer());
					retVal = types;
				}
			}
			return retVal;
		}
		private static readonly Size DefaultInitialControlSize = SetupDefaultInitialControlSize();
		private static Size SetupDefaultInitialControlSize()
		{
			return LastControlSizeStorage = new Size(192, 144);
		}
	}
}
