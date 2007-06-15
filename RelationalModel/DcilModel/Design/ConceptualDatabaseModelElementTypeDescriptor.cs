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
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.RelationalModels.ConceptualDatabase;

namespace Neumont.Tools.RelationalModels.ConceptualDatabase.Design
{
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="ModelElement"/>s of type <typeparamref name="TModelElement"/>.
	/// </summary>
	/// <typeparam name="TModelElement">
	/// The type of the <see cref="ModelElement"/> that this <see cref="ConceptualDatabaseElementTypeDescriptor{TModelElement}"/> is for.
	/// </typeparam>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ConceptualDatabaseElementTypeDescriptor<TModelElement> : ElementTypeDescriptor<TModelElement>
		where TModelElement : ModelElement
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ConceptualDatabaseElementTypeDescriptor{TModelElement}"/> for
		/// the instance of <typeparamref name="TModelElement"/> specified by <paramref name="selectedElement"/>.
		/// </summary>
		public ConceptualDatabaseElementTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
		}

		/// <summary>
		/// Not used, don't look for them.
		/// </summary>
		protected override bool IncludeEmbeddingRelationshipProperties(ModelElement requestor)
		{
			return false;
		}
		/// <summary>
		/// We're not ready to let the user arbitrarily pick opposite relationships
		/// </summary>
		protected override bool IncludeOppositeRolePlayerProperties(ModelElement requestor)
		{
			return false;
		}
	}
}
