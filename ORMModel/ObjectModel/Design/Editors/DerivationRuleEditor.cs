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
using System.Drawing;
using System.Security.Permissions;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.ObjectModel.Design
{
	/// <summary>
	/// An editor for a derivation rule dropdown
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class DerivationRuleEditor : MultilineTextEditor<DerivationRuleEditor>
	{
		private static readonly Size InitialControlSize = SetupInitialControlSize();
		private static Size SetupInitialControlSize()
		{
			return LastControlSizeStorage = new Size(DefaultInitialControlWidth, DefaultInitialControlHeight / 2);
		}
	}
}
