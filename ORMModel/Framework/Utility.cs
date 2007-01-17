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

namespace Neumont.Tools.Modeling
{
	/// <summary>
	/// Contains general-purpose utility methods.
	/// </summary>
	public static class Utility
	{
		#region GetCombinedHashCode methods
		/// <summary>
		/// Combines multiple hash codes (as returned from implementations of <see cref="Object.GetHashCode"/>)
		/// together in a way that results in an <see cref="Int32"/> value suitable for use as a hash code.
		/// </summary>
		/// <param name="hashCode1">
		/// The first hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode2">
		/// The second hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <returns>
		/// The <see cref="Int32"/> value suitable for use as a hash code that results from combining the hash codes specified by
		/// <paramref name="hashCode1"/> and <paramref name="hashCode2"/>.
		/// </returns>
		public static int GetCombinedHashCode(int hashCode1, int hashCode2)
		{
			return unchecked((int)((uint)hashCode1 ^
				(((uint)hashCode2 >> 1) | ((uint)hashCode2 << (32 - 1)))));
		}
		/// <summary>
		/// Combines multiple hash codes (as returned from implementations of <see cref="Object.GetHashCode"/>)
		/// together in a way that results in an <see cref="Int32"/> value suitable for use as a hash code.
		/// </summary>
		/// <param name="hashCode1">
		/// The first hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode2">
		/// The second hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode3">
		/// The third hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <returns>
		/// The <see cref="Int32"/> value suitable for use as a hash code that results from combining the hash codes specified by
		/// <paramref name="hashCode1"/>, <paramref name="hashCode2"/>, and <paramref name="hashCode3"/>.
		/// </returns>
		public static int GetCombinedHashCode(int hashCode1, int hashCode2, int hashCode3)
		{
			return unchecked((int)((uint)hashCode1 ^
				(((uint)hashCode2 >> 1) | ((uint)hashCode2 << (32 - 1))) ^
				(((uint)hashCode3 >> 2) | ((uint)hashCode3 << (32 - 2)))));
		}
		/// <summary>
		/// Combines multiple hash codes (as returned from implementations of <see cref="Object.GetHashCode"/>)
		/// together in a way that results in an <see cref="Int32"/> value suitable for use as a hash code.
		/// </summary>
		/// <param name="hashCode1">
		/// The first hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode2">
		/// The second hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode3">
		/// The third hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode4">
		/// The fourth hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <returns>
		/// The <see cref="Int32"/> value suitable for use as a hash code that results from combining the hash codes specified by
		/// <paramref name="hashCode1"/>, <paramref name="hashCode2"/>, <paramref name="hashCode3"/>, and <paramref name="hashCode4"/>.
		/// </returns>
		public static int GetCombinedHashCode(int hashCode1, int hashCode2, int hashCode3, int hashCode4)
		{
			return unchecked((int)((uint)hashCode1 ^
				(((uint)hashCode2 >> 1) | ((uint)hashCode2 << (32 - 1))) ^
				(((uint)hashCode3 >> 2) | ((uint)hashCode3 << (32 - 2))) ^
				(((uint)hashCode4 >> 3) | ((uint)hashCode4 << (32 - 3)))));
		}
		/// <summary>
		/// Combines multiple hash codes (as returned from implementations of <see cref="Object.GetHashCode"/>)
		/// together in a way that results in an <see cref="Int32"/> value suitable for use as a hash code.
		/// </summary>
		/// <param name="hashCode1">
		/// The first hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode2">
		/// The second hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode3">
		/// The third hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode4">
		/// The fourth hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <param name="hashCode5">
		/// The fifth hash code (<see cref="Int32"/> value returned from an implementation of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <returns>
		/// The <see cref="Int32"/> value suitable for use as a hash code that results from combining the hash codes specified by
		/// <paramref name="hashCode1"/>, <paramref name="hashCode2"/>, <paramref name="hashCode3"/>, <paramref name="hashCode4"/>, and
		/// <paramref name="hashCode5"/>.
		/// </returns>
		public static int GetCombinedHashCode(int hashCode1, int hashCode2, int hashCode3, int hashCode4, int hashCode5)
		{
			return unchecked((int)((uint)hashCode1 ^
				(((uint)hashCode2 >> 1) | ((uint)hashCode2 << (32 - 1))) ^
				(((uint)hashCode3 >> 2) | ((uint)hashCode3 << (32 - 2))) ^
				(((uint)hashCode4 >> 3) | ((uint)hashCode4 << (32 - 3))) ^
				(((uint)hashCode5 >> 4) | ((uint)hashCode4 << (32 - 4)))));
		}
		/// <summary>
		/// Combines multiple hash codes (as returned from implementations of <see cref="Object.GetHashCode"/>)
		/// together in a way that results in an <see cref="Int32"/> value suitable for use as a hash code.
		/// </summary>
		/// <param name="hashCodes">
		/// The hash codes (<see cref="Int32"/> values returned from implementations of <see cref="Object.GetHashCode"/>) to be combined.
		/// </param>
		/// <returns>
		/// The <see cref="Int32"/> value suitable for use as a hash code that results from combining the specified <paramref name="hashCodes"/>.
		/// </returns>
		public static int GetCombinedHashCode(params int[] hashCodes)
		{
			unchecked
			{
				if (hashCodes == null || hashCodes.Length <= 0)
				{
					return 0;
				}
				uint hashCode = (uint)hashCodes[0];
				for (int i = 1; i < hashCodes.Length; i++)
				{
					hashCode ^= (uint)RotateRight(hashCodes[i], i);
				}
				return (int)hashCode;
			}
		}
		#endregion // GetCombinedHashCode methods

		#region RotateRight method
		/// <summary>
		/// Returns the result of rotating the bits in the <see cref="Int32"/> value specified by <paramref name="value"/>
		/// the number of places specified by <paramref name="places"/>.
		/// </summary>
		/// <param name="value">
		/// The <see cref="Int32"/> value for which the bits should be rotated.
		/// </param>
		/// <param name="places">
		/// The <see cref="Int32"/> value specifying the number of places to rotate the bits in <paramref name="value"/>.
		/// </param>
		/// <returns>
		/// The result of rotating the bits in the <see cref="Int32"/> value specified by <paramref name="value"/>
		/// the number of places specified by <paramref name="places"/>.
		/// </returns>
		/// <remarks>
		/// The rotation is performed without sign extension. That is, the most significant bit is not treated specially.
		/// </remarks>
		public static int RotateRight(int value, int places)
		{
			// NOTE: If changes are made to this method, they will most likely also need to be made to the inlined
			// versions of it in the various GetCombinedHashCode overloads.

			// The IL that is generated for this method does not actually contain any casts. The casts
			// below simply instruct the compiler to perform the operations without sign extension.
			return unchecked((int)(((uint)value >> places) | ((uint)value << (32 - places))));
		}
		#endregion // RotateRight method

		#region SetPropertyValidateOneToOne method
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
		#endregion // SetPropertyValidateOneToOne method
	}
}
