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
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Shell.Interop;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.Design
{
	#region ORMEditorUtility class
	/// <summary>
	/// Static helper functions to use with <see cref="System.Drawing.Design.UITypeEditor"/>
	/// implementations.
	/// </summary>
	public static class ORMEditorUtility
	{
		/// <summary>
		/// Find the FactType associated with the specified instance. The
		/// instance will first be stripped of any wrapping objects by the
		/// <see cref="EditorUtility.ResolveContextInstance"/> method.
		/// </summary>
		/// <param name="instance">The selected object returned by ITypeDescriptorContext.Instance</param>
		/// <returns>A FactType, or null if the item is not associated with a FactType.</returns>
		public static FactType ResolveContextFactType(object instance)
		{
			instance = EditorUtility.ResolveContextInstance(instance, false);
			FactType retval = null;
			ModelElement elem;

			if (null != (elem = instance as ModelElement))
			{
				FactType fact;
				Role role;
				SetConstraint internalConstraint;
				Reading reading;
				ReadingOrder readingOrder;
				ObjectType objType;
				if (null != (fact = elem as FactType))
				{
					retval = fact;
				}
				else if (null != (role = elem as Role))
				{
					//this one coming straight through on the selection so handling
					//and returning here.
					retval = role.FactType;
				}
				else if (null != (internalConstraint = elem as SetConstraint))
				{
					if (internalConstraint.Constraint.ConstraintIsInternal)
					{
						LinkedElementCollection<FactType> facts = internalConstraint.FactTypeCollection;
						if (facts.Count == 1)
						{
							retval = facts[0];
						}
					}
				}
				else if (null != (reading = elem as Reading))
				{
					readingOrder = reading.ReadingOrder;
					if (null != readingOrder)
					{
						retval = readingOrder.FactType;
					}
				}
				else if (null != (readingOrder = elem as ReadingOrder))
				{
					retval = readingOrder.FactType;
				}
				else if (null != (objType = elem as ObjectType))
				{
					retval = objType.NestedFactType;
				}
			}
			// Handle weird teardown scenarios where the Store is going away
			return (retval != null && retval.Store != null) ? retval : null;
		}
	}
	#endregion // ORMEditorUtility class
}
