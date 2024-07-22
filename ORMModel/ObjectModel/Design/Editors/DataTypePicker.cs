#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
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

namespace ORMSolutions.ORMArchitect.Core.ObjectModel.Design
{
	/// <summary>
	/// An element picker to select data types for a value type.
	/// Associated with the ObjectType.DataTypeDisplay property
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[CLSCompliant(false)]
	public sealed class DataTypePicker : ElementPicker<DataTypePicker>
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
		/// Returns a list of data types that can be used by a value type.
		/// </summary>
		/// <param name="context">ITypeDescriptorContext. Used to retrieve the selected instance</param>
		/// <param name="value">The current value</param>
		/// <returns>A list of candidates</returns>
		protected sealed override IList GetContentList(ITypeDescriptorContext context, object value)
		{
			Debug.Assert(!(value is object[]));
			object resolvedInstance = EditorUtility.ResolveContextInstance(context.Instance, true); // true to pick any element. We can use any element to get at the datatypes on the model
			ORMModel model = null;
			ObjectType objectType;
			CustomReferenceMode refMode;
			if (null != (objectType = resolvedInstance as ObjectType))
			{
				model = objectType.ResolvedModel;
			}
			else if (null != (refMode = resolvedInstance as CustomReferenceMode))
			{
				model = refMode.Model;
			}

			if (model == null)
			{
				return new object[0];
			}

			IList<DataType> dataTypes = model.DataTypeCollection;
			int count = dataTypes.Count;
			if (count != 0)
			{
				DataType[] types = new DataType[count - DataType.ImplicitDataTypeCount];
				int iFilteredType = 0;
				for (int i = 0; i < count; ++i)
				{
					DataType dataType = dataTypes[i];
					if (!dataType.ImplicitOnly)
					{
						types[iFilteredType] = dataType;
						++iFilteredType;
					}
				}
				Array.Sort<DataType>(types, new DataTypeToStringComparer());
				return types;
			}
			return Array.Empty<DataType>();
		}
		/// <summary>
		/// Choose an initial size
		/// </summary>
		static DataTypePicker()
		{
			LastControlSizeStorage = new Size(192, 144);
		}
	}
}
