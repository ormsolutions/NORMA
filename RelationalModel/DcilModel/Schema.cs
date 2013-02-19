#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using System.Collections.ObjectModel;
namespace ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase
{
	partial class Schema
	{
		#region Custom Storage handlers
		private string GetEditNameValue()
		{
			return Name;
		}
		private void SetEditNameValue(string value)
		{
			if (Store.TransactionActive)
			{
				if (string.IsNullOrEmpty(value))
				{
					// Name generators should listen to this property to regenerate
					// the name.
					CustomName = false;
				}
				else
				{
					CustomName = true;
					Name = value;
				}
			}
		}
		#endregion // Custom Storage handlers
	}
}
