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
	#region ReferenceModeKindPicker class
	/// <summary>
	/// An element picker to select reference mode kinds.
	/// Associated with the <see cref="ReferenceMode.KindDisplay"/> property.
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class ReferenceModeKindPicker : ElementPicker<ReferenceModeKindPicker>
	{
		/// <summary>
		/// Returns a list of available reference mode kinds.
		/// </summary>
		/// <param name="context">ITypeDescriptorContext. Used to retrieve the selected instance</param>
		/// <param name="value">The current value</param>
		/// <returns>A list of candidates</returns>
		protected sealed override IList GetContentList(ITypeDescriptorContext context, object value)
		{
			ReferenceMode instance = (ReferenceMode)EditorUtility.ResolveContextInstance(context.Instance, true);
			ReadOnlyCollection<ReferenceModeKind> candidates = instance.Store.ElementDirectory.FindElements<ReferenceModeKind>();
			int candidatesCount = candidates.Count;
			if (candidatesCount > 1)
			{
				// Make sure we're sorted
				ReferenceModeKind[] kinds = new ReferenceModeKind[candidatesCount];
				string[] localizedNames = Utility.GetLocalizedEnumNames(typeof(ReferenceModeType), true);
				candidates.CopyTo(kinds, 0);
				Array.Sort<ReferenceModeKind>(
					kinds,
					delegate(ReferenceModeKind leftKind, ReferenceModeKind rightKind)
					{
						return string.Compare(localizedNames[(int)leftKind.ReferenceModeType], localizedNames[(int)rightKind.ReferenceModeType], true, CultureInfo.CurrentCulture);
					});
				return kinds;
			}
			return candidates;
		}
	}
	#endregion // ReferenceModeKindPicker class
	#region ReferenceModePicker class
	/// <summary>
	/// An element picker to select the reference mode for an object type. Associated with the 
	/// <see cref="ObjectType.ReferenceModeDisplay"/> property.
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class ReferenceModePicker : ElementPicker<ReferenceModePicker>
	{
		private IList myModes;
		/// <summary>
		/// Returns a list of role player candidates for a fact type.
		/// The nesting type is filtered out of the list.
		/// </summary>
		/// <param name="context">ITypeDescriptorContext. Used to retrieve the selected instance</param>
		/// <param name="value">The current value</param>
		/// <returns>A list of candidates</returns>
		protected sealed override IList GetContentList(ITypeDescriptorContext context, object value)
		{
			ObjectType instance = (ObjectType)EditorUtility.ResolveContextInstance(context.Instance, true);
			IList candidates = instance.ResolvedModel.ReferenceModeCollection;
			int candidateCount = candidates.Count;
			if (candidateCount == 0)
			{
				// If it's empty, we don't need to do anything else
				return candidates;
			}
			else
			{
				// Make sure we're sorted, and only display the fully defined reference modes
				ReferenceMode[] modes = new ReferenceMode[candidateCount];
				if (candidateCount > 1)
				{
					candidates.CopyTo(modes, 0);
					Array.Sort<ReferenceMode>(
						modes,
						delegate(ReferenceMode leftMode, ReferenceMode rightMode)
						{
							// Sort by decorated name
							return string.Compare(leftMode.DecoratedName, rightMode.DecoratedName, true, CultureInfo.CurrentCulture);
						});
				}
				myModes = modes;
				string[] prettyStrings = new string[candidateCount];
				for (int i = 0; i < prettyStrings.Length; ++i)
				{
					ReferenceMode refMode = modes[i];
					prettyStrings[i] = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelReferenceModePickerFormatString, refMode.DecoratedName, refMode.GenerateValueTypeName(instance.Name));
				}
				candidates = prettyStrings;
			}
			return candidates;
		}
		/// <summary>
		///  Translates the formatted string from the drop down list to the corresponding
		///  reference mode
		/// </summary>
		/// <param name="newIndex"></param>
		/// <param name="newObject"></param>
		/// <returns></returns>
		protected sealed override object TranslateFromDisplayObject(int newIndex, object newObject)
		{
			return myModes[newIndex];
		}
		/// <summary>
		/// Translates from the reference mode object in the drop down list to the corresponding
		/// formatted string
		/// </summary>
		/// <param name="initialObject"></param>
		/// <param name="contentList"></param>
		/// <returns></returns>
		protected sealed override object TranslateToDisplayObject(object initialObject, IList contentList)
		{
			ReferenceMode mode = initialObject as ReferenceMode;
			if (mode != null)
			{
				int index = myModes.IndexOf(initialObject);
				if (index >= 0)
				{
					return contentList[index];
				}
			}
			return initialObject;
		}
	}
	#endregion // ReferenceModePicker class
}
