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
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Modeling;
namespace Neumont.Tools.ORM.Framework
{
	/// <summary>
	/// Contains general-purpose utility methods.
	/// </summary>
	public static class Utility
	{
		/// <summary>
		/// Sets the role player for a one-to-one relationship while enforcing the one-to-one pattern on both role players.
		/// </summary>
		public static void SetPropertyValidateOneToOne(ModelElement sourceRolePlayer, ModelElement newTargetRolePlayer, Guid sourceRoleGuid, Guid targetRoleGuid, Type linkType)
		{
			if (sourceRolePlayer == null)
			{
				throw new ArgumentNullException("sourceRolePlayer");
			}
			Store store = sourceRolePlayer.Store;
			bool sameTargetRolePlayer = false;
			DomainRoleInfo sourceRoleInfo = store.DomainDataDirectory.FindDomainRole(sourceRoleGuid);
			if (linkType == null)
			{
				linkType = sourceRoleInfo.DomainRelationship.ImplementationClass;
			}
			ReadOnlyCollection<ElementLink> links = DomainRoleInfo.GetElementLinks<ElementLink>(sourceRolePlayer, sourceRoleGuid);
			int linkCount = links.Count;
			if (linkCount != 0)
			{
				for (int i = linkCount - 1; i >= 0; --i)
				{
					ElementLink link = links[i];
					if (!link.IsDeleted)
					{
						ModelElement counterpart = sourceRoleInfo.OppositeDomainRole.GetRolePlayer(link);
						if (counterpart != null && counterpart == newTargetRolePlayer)
						{
							sameTargetRolePlayer = true;
						}
						else
						{
							link.Delete();
						}
						// break; // In theory we can break on the first one, but this guarantees that we rip any slop already in the model
					}
				}
			}
			if (newTargetRolePlayer != null)
			{
				// Check the relationship on the other end to enforce 1-1
				links = DomainRoleInfo.GetElementLinks<ElementLink>(newTargetRolePlayer, targetRoleGuid);
				linkCount = links.Count;
				if (linkCount != 0)
				{
					for (int i = linkCount - 1; i >= 0; --i)
					{
						ElementLink link = links[i];
						if (!link.IsDeleted)
						{
							ModelElement counterpart = sourceRoleInfo.GetRolePlayer(link);
							if (counterpart != null && counterpart == sourceRolePlayer)
							{
								sameTargetRolePlayer = true;
							}
							else
							{
								link.Delete();
							}
							// break; // In theory we can break on the first one, but this guarantees that we rip any slop already in the model
						}
					}
				}
			}
			if ((!sameTargetRolePlayer) && (newTargetRolePlayer != null))
			{
				store.ElementFactory.CreateElementLink(sourceRoleInfo.DomainRelationship,
					new RoleAssignment(sourceRoleGuid, sourceRolePlayer),
					new RoleAssignment(targetRoleGuid, newTargetRolePlayer));
			}
		}
	}
}
