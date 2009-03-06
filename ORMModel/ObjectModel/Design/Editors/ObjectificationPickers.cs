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
	#region NestingTypePicker class
	/// <summary>
	/// An element picker to select nesting types for a fact type.
	/// Associated with the FactType.NestingTypeDisplay property
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class NestingTypePicker : ElementPicker<NestingTypePicker>
	{
		/// <summary>
		/// Returns a list of entity types that can be used to nest
		/// a fact type. Types already in use as nesting types, as well
		/// as roleplayers of the current fact are filtered out of the list.
		/// </summary>
		/// <param name="context">ITypeDescriptorContext. Used to retrieve the selected instance</param>
		/// <param name="value">The current value</param>
		/// <returns>A list of candidates</returns>
		protected sealed override IList GetContentList(ITypeDescriptorContext context, object value)
		{
			Debug.Assert(!(value is object[]));
			FactType instance = (FactType)EditorUtility.ResolveContextInstance(context.Instance, false); // false indicates this should not be called in multiselect mode.
			if (instance.ImpliedByObjectification != null)
			{
				// Implied FactTypes can't be objectified, so we just return an empty list
				return new ObjectType[0];
			}
			ReadOnlyCollection<ObjectType> candidates = instance.Store.ElementDirectory.FindElements<ObjectType>();
			int count = candidates.Count;
			if (count > 0)
			{
				IComparer<ObjectType> comparer = HashCodeComparer<ObjectType>.Instance;
				LinkedElementCollection<RoleBase> roles = instance.RoleCollection;
				ObjectType[] rolePlayers = new ObjectType[roles.Count];
				for (int i = 0; i < rolePlayers.Length; ++i)
				{
					rolePlayers[i] = ((Role)roles[i]).RolePlayer;
				}
				Array.Sort(rolePlayers, comparer);

				List<ObjectType> types = new List<ObjectType>(count);
				foreach (ObjectType objType in candidates)
				{
					if (!objType.IsValueType)
					{
						FactType nestedFact = objType.NestedFactType;
						// We only show the ObjectType if it is not already nesting a FactType, or if it is explicitly nesting this FactType
						if (nestedFact == null || (nestedFact == instance && !nestedFact.Objectification.IsImplied))
						{
							// Make sure that the nested type candidate is also not being used as a role player
							// This is backed up in the metamodel rules and other pickers.
							if (Array.BinarySearch(rolePlayers, objType, comparer) < 0)
							{
								types.Add(objType);
							}
						}
					}
				}
				if (types.Count > 1)
				{
					types.Sort(NamedElementComparer<ObjectType>.CurrentCulture);
				}
				return types;
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
				return ResourceStrings.NestingTypePickerNullItemText;
			}
		}
	}
	#endregion // NestingTypePicker class
	#region NestedFactTypePicker class
	/// <summary>
	/// An element picker to select nested fact types for an entity type.
	/// Associated with the ObjectType.NestedFactTypeDisplay property
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class NestedFactTypePicker : ElementPicker<NestedFactTypePicker>
	{
		/// <summary>
		/// Returns a list of fact types that can be nested by an entity type.
		/// Currently nested fact types are filtered out, as well as fact types
		/// where the current object type is a role player.
		/// </summary>
		/// <param name="context">ITypeDescriptorContext. Used to retrieve the selected instance</param>
		/// <param name="value">The current value</param>
		/// <returns>A list of candidates</returns>
		protected sealed override IList GetContentList(ITypeDescriptorContext context, object value)
		{
			Debug.Assert(!(value is object[]));
			ObjectType instance = (ObjectType)EditorUtility.ResolveContextInstance(context.Instance, false); // false indicates this should not be called in multiselect mode.
			ReadOnlyCollection<FactType> candidates = instance.Store.ElementDirectory.FindElements<FactType>();
			int count = candidates.Count;
			if (count > 0)
			{
				IComparer<FactType> comparer = HashCodeComparer<FactType>.Instance;
				LinkedElementCollection<Role> playedRoles = instance.PlayedRoleCollection;
				FactType[] playedFacts = new FactType[playedRoles.Count];
				for (int i = 0; i < playedFacts.Length; ++i)
				{
					playedFacts[i] = playedRoles[i].FactType;
				}
				Array.Sort(playedFacts, comparer);

				List<FactType> types = new List<FactType>(count);
				// Always include the current value in the list if it is not null
				if (value != null)
				{
					types.Add(value as FactType);
				}
				foreach (FactType factType in candidates)
				{
					// SubtypeFacts and facts implied by an objectification can't be objectified
					if (factType is SubtypeFact || factType.ImpliedByObjectification != null)
					{
						continue;
					}
					Objectification objectification = factType.Objectification;
					if (objectification == null || objectification.IsImplied)
					{
						// Make sure that the nested fact does not parent a role with this
						// instance as a role player.
						// This is backed up in the metamodel rules and other pickers.
						if (Array.BinarySearch(playedFacts, factType, comparer) < 0)
						{
							types.Add(factType);
						}
					}
				}
				if (types.Count > 1)
				{
					types.Sort(NamedElementComparer<FactType>.CurrentCulture);
				}
				return types;
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
				return ResourceStrings.NestedFactTypePickerNullItemText;
			}
		}
	}
	#endregion // NestedFactTypePicker class
}
