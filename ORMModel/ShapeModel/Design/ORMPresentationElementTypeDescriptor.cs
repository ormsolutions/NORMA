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
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Framework.Diagrams.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel.Design
{
	/// <summary>
	/// <see cref="PresentationElementTypeDescriptor"/> for ORM <see cref="PresentationElement"/>s of type <typeparamref name="TPresentationElement"/>.
	/// </summary>
	/// <typeparam name="TPresentationElement">
	/// The type of the ORM <see cref="PresentationElement"/> that this <see cref="PresentationElementTypeDescriptor{TPresentationElement,TModelElement}"/> is for.
	/// </typeparam>
	/// <typeparam name="TModelElement">
	/// The type of the ORM <see cref="ModelElement"/> that <typeparamref name="TPresentationElement"/> is associated with.
	/// </typeparam>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ORMPresentationElementTypeDescriptor<TPresentationElement, TModelElement> : PresentationElementTypeDescriptor<TPresentationElement, TModelElement>
		where TPresentationElement : PresentationElement
		where TModelElement : ModelElement
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ORMPresentationElementTypeDescriptor{TPresentationElement,TModelElement}"/> for
		/// the instance of <typeparamref name="TPresentationElement"/> specified by <paramref name="presentationElement"/>
		/// that is associated with the instance of <typeparamref name="TModelElement"/> specified by <paramref name="selectedElement"/>.
		/// </summary>
		public ORMPresentationElementTypeDescriptor(ICustomTypeDescriptor parent, TPresentationElement presentationElement, TModelElement selectedElement)
			: base(parent, presentationElement, selectedElement)
		{
		}

		/// <summary>
		/// Not used, don't look for them
		/// </summary>
		protected override bool IncludeEmbeddingRelationshipProperties(ModelElement requestor)
		{
			return false;
		}

		/// <summary>
		/// Let our *Display properties handle these
		/// </summary>
		protected override bool IncludeOppositeRolePlayerProperties(ModelElement requestor)
		{
			// UNDONE: We may want to lose the *Display properties. Need a way to filter
			// the contents of a RolePlayerPropertyDescriptor dropdown list
			// UNDONE: MSBUG RolePlayerPropertyDescriptor should respect the System.ComponentModel.EditorAttribute on the
			// generated property.
			return false;
		}
	}
}
