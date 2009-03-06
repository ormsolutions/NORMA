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
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel.Design
{
	/// <summary>
	/// An element picker to select role players. Associated with the Role.RolePlayerDisplay property
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class RolePlayerPicker : ElementPicker<RolePlayerPicker>
	{
		/// <summary>
		/// Returns a list of role player candidates for a fact type.
		/// The nesting types of this fact type and of implied objectifications are filtered out of the list.
		/// </summary>
		/// <param name="context">ITypeDescriptorContext. Used to retrieve the selected instance</param>
		/// <param name="value">The current value</param>
		/// <returns>A list of candidates</returns>
		protected sealed override IList GetContentList(ITypeDescriptorContext context, object value)
		{
			Role instance = (Role)EditorUtility.ResolveContextInstance(context.Instance, true);
			ReadOnlyCollection<ObjectType> candidates = instance.Store.ElementDirectory.FindElements<ObjectType>();
			int candidatesCount = candidates.Count;
			switch (candidatesCount)
			{
				case 0:
					break;
				case 1:
					{
						ObjectType objType = candidates[0]; 
						Objectification objectification;
						if (objType.IsImplicitBooleanValue ||
							(null != (objectification = objType.Objectification) && !objectification.IsImplied && objectification == instance.FactType.Objectification))
						{
							candidates = null;
						}
					}
					break;
				default:
					{
						// Make sure we're sorted
						List<ObjectType> types = new List<ObjectType>(candidatesCount);
						Objectification thisObjectification = instance.FactType.Objectification;
						foreach (ObjectType objType in candidates)
						{
							Objectification objectification = objType.Objectification;
							if (!objType.IsImplicitBooleanValue && 
								(null == (objectification = objType.Objectification) || (objectification != thisObjectification && !objectification.IsImplied)))
							{
								types.Add(objType);
							}
						}
						if (types.Count > 1)
						{
							types.Sort(NamedElementComparer<ObjectType>.CurrentCulture);
						}
						return types;
					}
			}
			return candidates;
		}
		/// <summary>
		/// The text for the first 'null' item in the
		/// dropdown. Clicking this item is equivalent to
		/// setting the property value to null.
		/// </summary>
		protected sealed override string NullItemText
		{
			get
			{
				return ResourceStrings.RolePlayerPickerNullItemText;
			}
		}
	}
}
