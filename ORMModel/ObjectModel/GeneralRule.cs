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
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	partial class GeneralRule
	{
		/// <summary>
		/// Determine the <see cref="ORMModel"/> for this element.
		/// If the element is owned by an alternate owner, then retrieve
		/// the model through that owner.
		/// </summary>
		public ORMModel ResolvedModel
		{
			get
			{
				IHasAlternateOwner<GeneralRule> toAlternateOwner;
				IAlternateElementOwner<GeneralRule> alternateOwner;
				return (null != (toAlternateOwner = this as IHasAlternateOwner<GeneralRule>) &&
					null != (alternateOwner = toAlternateOwner.AlternateOwner)) ?
						alternateOwner.Model :
						this.Model;
			}
		}
	}
}
