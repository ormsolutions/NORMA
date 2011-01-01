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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio.VirtualTreeGrid;

namespace ORMSolutions.ORMArchitect.ORMCustomTool
{
	public sealed partial class Extender
	{
		private sealed partial class ORMCustomToolPropertyDescriptor : PropertyDescriptor
		{
			// TODO: Localize this.
			public ORMCustomToolPropertyDescriptor()
				: base("ORM Generator Selection", null)
			{
			}

			public override object GetEditor(Type editorBaseType)
			{
				return new ORMCustomToolUITypeEditor();
			}

			public override bool CanResetValue(object component)
			{
				return false;
			}

			public override Type ComponentType
			{
				get { return typeof(ORMCustomTool); }
			}

			public override object GetValue(object component)
			{
				return null;
			}

			public override bool IsReadOnly
			{
				get { return true; }
			}

			public override Type PropertyType
			{
				get { return typeof(string); }
			}

			public override void ResetValue(object component)
			{
				// Do nothing.
			}

			public override void SetValue(object component, object value)
			{
				// Do nothing.
			}

			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}
		}
	}
}
