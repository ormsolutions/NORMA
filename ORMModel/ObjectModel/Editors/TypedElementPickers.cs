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
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;
namespace Neumont.Tools.ORM.ObjectModel.Editors
{
	#region RolePlayerPicker class
	/// <summary>
	/// An element picker to select role players. Associated with the Role.RolePlayerDisplay property
	/// </summary>
	public class RolePlayerPicker : ElementPicker
	{
		/// <summary>
		/// Returns a list of role player candidates for a fact type.
		/// The nesting types of this fact type and of implied objectifications are filtered out of the list.
		/// </summary>
		/// <param name="context">ITypeDescriptorContext. Used to retrieve the selected instance</param>
		/// <param name="value">The current value</param>
		/// <returns>A list of candidates</returns>
		protected override IList GetContentList(ITypeDescriptorContext context, object value)
		{
			Role instance = (Role)EditorUtility.ResolveContextInstance(context.Instance, true);
			IList candidates = instance.Store.ElementDirectory.GetElements(ObjectType.MetaClassGuid);
			if (candidates.Count > 1)
			{
				// Make sure we're sorted
				List<ObjectType> types = new List<ObjectType>(candidates.Count);
				Objectification thisObjectification = instance.FactType.Objectification;
				foreach (ObjectType objType in candidates)
				{
					Objectification objectification = objType.Objectification;
					if (objectification != null && objectification != thisObjectification && !objectification.IsImplied)
					{
						types.Add(objType);
					}
				}
				if (types.Count > 1)
				{
					types.Sort(NamedElementComparer<ObjectType>.CurrentCulture);
				}
				candidates = types;
			}
			return candidates;
		}
		/// <summary>
		/// The text for the first 'null' item in the
		/// dropdown. Clicking this item is equivalent to
		/// setting the property value to null.
		/// </summary>
		protected override string NullItemText
		{
			get
			{
				return ResourceStrings.RolePlayerPickerNullItemText;
			}
		}
		private static Size myLastControlSize = Size.Empty;
		/// <summary>
		/// Manage control size independently
		/// </summary>
		protected override Size LastControlSize
		{
			get { return myLastControlSize; }
			set	{ myLastControlSize = value; }
		}
	}
	#endregion // RolePlayerPicker class
	#region NestingTypePicker class
	/// <summary>
	/// An element picker to select nesting types for a fact type.
	/// Associated with the FactType.NestingTypeDisplay property
	/// </summary>
	public class NestingTypePicker : ElementPicker
	{
		/// <summary>
		/// Returns a list of entity types that can be used to nest
		/// a fact type. Types already in use as nesting types, as well
		/// as roleplayers of the current fact are filtered out of the list.
		/// </summary>
		/// <param name="context">ITypeDescriptorContext. Used to retrieve the selected instance</param>
		/// <param name="value">The current value</param>
		/// <returns>A list of candidates</returns>
		protected override IList GetContentList(ITypeDescriptorContext context, object value)
		{
			Debug.Assert(!(value is object[]));
			FactType instance = (FactType)EditorUtility.ResolveContextInstance(context.Instance, false); // false indicates this should not be called in multiselect mode.
			if (instance.ImpliedByObjectification != null)
			{
				// Implied FactTypes can't be objectified, so we just return an empty list
				return new ObjectType[0];
			}
			IList candidates = instance.Store.ElementDirectory.GetElements(ObjectType.MetaClassGuid);
			int count = candidates.Count;
			if (count > 0)
			{
				IComparer<ObjectType> comparer = HashCodeComparer<ObjectType>.Instance;
				RoleBaseMoveableCollection roles = instance.RoleCollection;
				ObjectType[] rolePlayers = new ObjectType[roles.Count];
				for (int i = 0; i < rolePlayers.Length; ++i)
				{
					rolePlayers[i] = (roles[i] as Role).RolePlayer;
				}
				Array.Sort(rolePlayers, comparer);

				List<ObjectType> types = new List<ObjectType>(count);
				foreach (ObjectType objType in candidates)
				{
					if (!objType.IsValueType)
					{
						FactType nestedFact = objType.NestedFactType;
						// We only show the ObjectType if it is not already nesting a FactType, or if it is explicitly nesting this FactType
						if (nestedFact == null || ((object)nestedFact == instance && !nestedFact.Objectification.IsImplied))
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
				candidates = types;
			}
			return candidates;
		}
		/// <summary>
		/// The text for the first 'null' item in the
		/// dropdown. Clicking this item is equivalent to
		/// setting the property value to null.
		/// </summary>
		protected override string NullItemText
		{
			get
			{
				return ResourceStrings.NestingTypePickerNullItemText;
			}
		}
		private static Size myLastControlSize = Size.Empty;
		/// <summary>
		/// Manage control size independently
		/// </summary>
		protected override Size LastControlSize
		{
			get { return myLastControlSize; }
			set { myLastControlSize = value; }
		}
	}
	#endregion // NestingTypePicker class
	#region NestedFactTypePicker class
	/// <summary>
	/// An element picker to select nested fact types for an entity type.
	/// Associated with the ObjectType.NestedFactTypeDisplay property
	/// </summary>
	public class NestedFactTypePicker : ElementPicker
	{
		private sealed class FactTypeNameComparer : IComparer<FactType>
		{
			public FactTypeNameComparer()
			{
				myStringComparer = StringComparer.CurrentCulture;
			}
			private readonly StringComparer myStringComparer;
			public int Compare(FactType x, FactType y)
			{
				return myStringComparer.Compare(x.Name, y.Name);
			}
		}
		/// <summary>
		/// Returns a list of fact types that can be nested by an entity type.
		/// Currently nested fact types are filtered out, as well as fact types
		/// where the current object type is a role player.
		/// </summary>
		/// <param name="context">ITypeDescriptorContext. Used to retrieve the selected instance</param>
		/// <param name="value">The current value</param>
		/// <returns>A list of candidates</returns>
		protected override IList GetContentList(ITypeDescriptorContext context, object value)
		{
			Debug.Assert(!(value is object[]));
			ObjectType instance = (ObjectType)EditorUtility.ResolveContextInstance(context.Instance, false); // false indicates this should not be called in multiselect mode.
			IList candidates = instance.Store.ElementDirectory.GetElements(FactType.MetaClassGuid);
			int count = candidates.Count;
			if (count > 0)
			{
				IComparer<FactType> comparer = HashCodeComparer<FactType>.Instance;
				RoleMoveableCollection playedRoles = instance.PlayedRoleCollection;
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
					types.Sort(new FactTypeNameComparer());
				}
				candidates = types;
			}
			return candidates;
		}
		/// <summary>
		/// The text for the first 'null' item in the
		/// dropdown. Clicking this item is equivalent to
		/// setting the property value to null.
		/// </summary>
		protected override string NullItemText
		{
			get
			{
				return ResourceStrings.NestedFactTypePickerNullItemText;
			}
		}
		private static Size myLastControlSize = Size.Empty;
		/// <summary>
		/// Manage control size independently
		/// </summary>
		protected override Size LastControlSize
		{
			get { return myLastControlSize; }
			set { myLastControlSize = value; }
		}
	}
	#endregion // NestedFactTypePicker class
	#region DataTypePicker class
	/// <summary>
	/// An element picker to select data types for a value type.
	/// Associated with the ObjectType.DataTypeDisplay property
	/// </summary>
	public class DataTypePicker : ElementPicker
	{
		private sealed class DataTypeToStringComparer : IComparer<DataType>
		{
			public DataTypeToStringComparer()
			{
				myStringComparer = StringComparer.CurrentCulture;
			}
			private readonly StringComparer myStringComparer;
			public int Compare(DataType x, DataType y)
			{
				return myStringComparer.Compare(x.ToString(), y.ToString());
			}
		}
		/// <summary>
		/// Returns a list of data types that can be used by a value type.
		/// </summary>
		/// <param name="context">ITypeDescriptorContext. Used to retrieve the selected instance</param>
		/// <param name="value">The current value</param>
		/// <returns>A list of candidates</returns>
		protected override IList GetContentList(ITypeDescriptorContext context, object value)
		{
			Debug.Assert(!(value is object[]));
			ObjectType instance = (ObjectType)EditorUtility.ResolveContextInstance(context.Instance, true); // true to pick any element. We can use any element to get at the datatypes on the model
			IList dataTypes = instance.Model.DataTypeCollection;
			int count = dataTypes.Count;
			if (count > 1)
			{
				DataType[] types = new DataType[count];
				dataTypes.CopyTo(types, 0);
				Array.Sort<DataType>(types, new DataTypeToStringComparer());
				dataTypes = types;
			}
			return dataTypes;
		}
		private static Size myLastControlSize = new Size(192, 144);
		/// <summary>
		/// Manage control size independently
		/// </summary>
		protected override Size LastControlSize
		{
			get { return myLastControlSize; }
			set { myLastControlSize = value; }
		}
	}
	#endregion // DataTypePicker class
	#region ReferenceModeKindPicker class
	/// <summary>
	/// An element picker to select reference mode kinds. Associated with the ReferenceMode.KindDisplay property
	/// </summary>
	public class ReferenceModeKindPicker : ElementPicker
	{
		/// <summary>
		/// Returns a list of role player candidates for a fact type.
		/// The nesting type is filtered out of the list.
		/// </summary>
		/// <param name="context">ITypeDescriptorContext. Used to retrieve the selected instance</param>
		/// <param name="value">The current value</param>
		/// <returns>A list of candidates</returns>
		protected override IList GetContentList(ITypeDescriptorContext context, object value)
		{
			ReferenceMode instance = (ReferenceMode)EditorUtility.ResolveContextInstance(context.Instance, true);
			IList candidates = instance.Store.ElementDirectory.GetElements(ReferenceModeKind.MetaClassGuid);
			int candidatesCount = candidates.Count;
			if (candidatesCount > 1)
			{
				// Make sure we're sorted
				ReferenceModeKind[] kinds = new ReferenceModeKind[candidatesCount];
				candidates.CopyTo(kinds, 0);
				Array.Sort<ReferenceModeKind>(kinds, NamedElementComparer<ReferenceModeKind>.CurrentCulture);
				candidates = kinds;
			}
			return candidates;
		}
		private static Size myLastControlSize = Size.Empty;
		/// <summary>
		/// Manage control size independently
		/// </summary>
		protected override Size LastControlSize
		{
			get { return myLastControlSize; }
			set { myLastControlSize = value; }
		}
	}
	#endregion // ReferenceModeKindPicker class
	#region ReferenceModePicker class
	/// <summary>
	/// An element picker to select the reference mode for an object type. Associated with the 
	/// ObjectType.ReferenceModeDisplay property
	/// </summary>
	public class ReferenceModePicker : ElementPicker
	{
		private IList myModes;
		/// <summary>
		/// Returns a list of role player candidates for a fact type.
		/// The nesting type is filtered out of the list.
		/// </summary>
		/// <param name="context">ITypeDescriptorContext. Used to retrieve the selected instance</param>
		/// <param name="value">The current value</param>
		/// <returns>A list of candidates</returns>
		protected override IList GetContentList(ITypeDescriptorContext context, object value)
		{
			ObjectType instance = (ObjectType)EditorUtility.ResolveContextInstance(context.Instance, true);
			IList candidates = instance.Model.ReferenceModeCollection;
			int candidatesCount = candidates.Count;
			if (candidatesCount == 0)
			{
				// If it's empty, we don't need to do anything else
				return candidates;
			}
			else if (candidatesCount > 1)
			{
				// Make sure we're sorted
				ReferenceMode[] modes = new ReferenceMode[candidatesCount];
				candidates.CopyTo(modes, 0);
				Array.Sort<ReferenceMode>(modes, NamedElementComparer<ReferenceMode>.CurrentCulture);
				myModes = modes;
				string[] prettyStrings = new string[candidatesCount];
				for (int i = 0; i < prettyStrings.Length; ++i)
				{
					ReferenceMode refMode = modes[i];
					prettyStrings[i] = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelReferenceModePickerFormatString, refMode.Name, refMode.GenerateValueTypeName(instance.Name));
				}
				candidates = prettyStrings;
			}
			else
			{
				myModes = candidates;
				ReferenceMode refMode = (ReferenceMode)candidates[0];
				candidates = new string[] { string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelReferenceModePickerFormatString, refMode.Name, refMode.GenerateValueTypeName(instance.Name)) };
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
		protected override object TranslateFromDisplayObject(int newIndex, object newObject)
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
		protected override object TranslateToDisplayObject(object initialObject, IList contentList)
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
		private static Size myLastControlSize = Size.Empty;
		/// <summary>
		/// Manage control size independently
		/// </summary>
		protected override Size LastControlSize
		{
			get { return myLastControlSize; }
			set { myLastControlSize = value; }
		}
	}
	#endregion // ReferenceModePicker class
}
