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
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;
using System;
using System.Collections.ObjectModel;

namespace Neumont.Tools.RelationalModels.ConceptualDatabase
{
	partial class Column : IRedirectVerbalization
	{
		#region IRedirectVerbalization implementation
		/// <summary>
		/// The guid for the role in the bridge model we want to defer verbalization to. Lifted
		/// from the generated bridge code.
		/// </summary>
		private static readonly Guid ColumnBridgeRoleId = new Guid(0xbc7ea8a8, 0x8772, 0x4ca4, 0xb9, 0x14, 0xb7, 0x8b, 0x4b, 0x58, 0x33, 0x38);

		/// <summary>
		/// Implements <see cref="IRedirectVerbalization.SurrogateVerbalizer"/>. Defers to the
		/// first bridge link element, if the bridge is loaded.
		/// </summary>
		protected IVerbalize SurrogateVerbalizer
		{
			get
			{
				// Look up the associated bridge links by guid, this project does not reference
				// the bridge elements directly, so the type is not available.
				DomainRoleInfo roleInfo;
				ReadOnlyCollection<ElementLink> links;
				IVerbalize retVal = null;
				if (null != (roleInfo = Store.DomainDataDirectory.FindDomainRole(ColumnBridgeRoleId)) &&
					0 != (links = roleInfo.GetElementLinks(this)).Count)
				{
					retVal = links[0] as IVerbalize;
				}
				return retVal;
			}
		}
		IVerbalize IRedirectVerbalization.SurrogateVerbalizer
		{
			get
			{
				return SurrogateVerbalizer;
			}
		}
		#endregion // IRedirectVerbalization implementation
	}
	partial class UniquenessConstraintIncludesColumn : IRedirectVerbalization
	{
		#region IRedirectVerbalization implementation
		/// <summary>
		/// Implements <see cref="IRedirectVerbalization.SurrogateVerbalizer"/>. Defers to the
		/// redirection on the column.
		/// </summary>
		protected IVerbalize SurrogateVerbalizer
		{
			get
			{
				return (Column as IRedirectVerbalization).SurrogateVerbalizer;
			}
		}
		IVerbalize IRedirectVerbalization.SurrogateVerbalizer
		{
			get
			{
				return SurrogateVerbalizer;
			}
		}
		#endregion // IRedirectVerbalization implementation
	}
	partial class UniquenessConstraint : IRedirectVerbalization
	{
		#region IRedirectVerbalization implementation
		/// <summary>
		/// The guid for the role in the bridge model we want to defer verbalization to. Lifted
		/// from the generated bridge code.
		/// </summary>
		private static readonly Guid UniquenessBridgeRoleId = new Guid(0x2c58f935, 0xe736, 0x401f, 0xb2, 0x2f, 0xe3, 0x8f, 0x71, 0xab, 0x33, 0xe5);

		/// <summary>
		/// Implements <see cref="IRedirectVerbalization.SurrogateVerbalizer"/>. Defers to the
		/// first bridge link element, if the bridge is loaded.
		/// </summary>
		protected IVerbalize SurrogateVerbalizer
		{
			get
			{
				// Look up the associated bridge links by guid, this project does not reference
				// the bridge elements directly, so the type is not available.
				DomainRoleInfo roleInfo;
				ReadOnlyCollection<ElementLink> links;
				IVerbalize retVal = null;
				if (null != (roleInfo = Store.DomainDataDirectory.FindDomainRole(UniquenessBridgeRoleId)) &&
					0 != (links = roleInfo.GetElementLinks(this)).Count)
				{
					retVal = links[0] as IVerbalize;
				}
				return retVal;
			}
		}
		IVerbalize IRedirectVerbalization.SurrogateVerbalizer
		{
			get
			{
				return SurrogateVerbalizer;
			}
		}
		#endregion // IRedirectVerbalization implementation
	}
}
