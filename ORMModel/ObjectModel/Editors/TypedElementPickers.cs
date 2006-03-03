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
		/// The nesting type is filtered out of the list.
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
				ObjectType nestingType = instance.FactType.NestingType;
				foreach (ObjectType objType in candidates)
				{
					if (objType != nestingType)
					{
						types.Add(objType);
					}
				}
				types.Sort(delegate(ObjectType type1, ObjectType type2)
				{
					return string.Compare(type1.Name, type2.Name);
				});
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
			ObjectType[] roleTypes = null;
			IList<ObjectType> roleTypesList = null;
			IList candidates = instance.Store.ElementDirectory.GetElements(ObjectType.MetaClassGuid);
			int count = candidates.Count;
			if (count > 0)
			{
				List<ObjectType> types = new List<ObjectType>(count);
				foreach (ObjectType objType in candidates)
				{
					if (!objType.IsValueType && objType.PreferredIdentifier == null)
					{
						FactType nestedFact = objType.NestedFactType;
						if (nestedFact == null || object.ReferenceEquals(nestedFact, instance))
						{
							Debug.Assert(nestedFact == null || object.ReferenceEquals(value, objType)); // Should be equivalent condition to checking nestedFact against instance

							// Make sure that the nested type candidate is also not being used as a role player
							// This is backed up in the metamodel rules and other pickers.
							if (roleTypes == null)
							{
								RoleMoveableCollection roles = instance.RoleCollection;
								int roleCount = roles.Count;
								if (roleCount > 0)
								{
									roleTypes = new ObjectType[roleCount];
									for (int i = 0; i < roleCount; ++i)
									{
										roleTypes[i] = roles[i].RolePlayer;
									}
								}
								else
								{
									roleTypes = new ObjectType[0];
								}
								roleTypesList = roleTypes;
							}
							if (!roleTypesList.Contains(objType))
							{
								types.Add(objType);
							}
						}
					}
				}
				if (types.Count > 0)
				{
					types.Sort(delegate(ObjectType type1, ObjectType type2)
					{
						return string.Compare(type1.Name, type2.Name);
					});
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
			FactType[] roleFacts = null;
			IList<FactType> roleFactsList = null;
			int count = candidates.Count;
			if (count > 0)
			{
				List<FactType> types = new List<FactType>(count);
				foreach (FactType factType in candidates)
				{
					if ((factType is SubtypeFact) ||
						(factType.ImpliedByObjectification != null))
					{
						continue;
					}
					ObjectType nestingType = factType.NestingType;
					if (nestingType == null || object.ReferenceEquals(nestingType, instance))
					{
						Debug.Assert(nestingType == null || object.ReferenceEquals(value, factType)); // Should be equivalent condition to checking nestedFact against instance

						// Make sure that the nested fact does not parent a role with this
						// instance as a role player.
						// This is backed up in the metamodel rules and other pickers.
						if (roleFacts == null)
						{
							RoleMoveableCollection roles = instance.PlayedRoleCollection;
							int roleCount = roles.Count;
							if (roleCount > 0)
							{
								roleFacts = new FactType[roleCount];
								for (int i = 0; i < roleCount; ++i)
								{
									roleFacts[i] = roles[i].FactType;
								}
							}
							else
							{
								roleFacts = new FactType[0];
							}
							roleFactsList = roleFacts;
						}
						if (!roleFactsList.Contains(factType))
						{
							types.Add(factType);
						}
					}
				}
				if (types.Count > 0)
				{
					types.Sort(delegate(FactType type1, FactType type2)
					{
						return string.Compare(type1.Name, type2.Name);
					});
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
			DataTypeMoveableCollection dataTypes = instance.Model.DataTypeCollection;
			IList content = dataTypes;
			// Let's use the order that the types appear in the DataType.PortableDataType enum.
			// To change this, uncomment the lines in the region below.
			#region Sort list
//			int count = dataTypes.Count;
//			if (count > 0)
//			{
//				DataType[] types = new DataType[count];
//				for (int i = 0; i < count; ++i)
//				{
//					types[i] = dataTypes[i];
//				}
//				Array.Sort<DataType>(types, delegate(DataType type1, DataType type2)
//				{
//					return string.Compare(type1.ToString(), type2.ToString());
//				});
//				content = types;
//			}
			#endregion // Sort list
			return content;
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
			if (candidates.Count > 1)
			{
				// Make sure we're sorted
				List<ReferenceModeKind> kinds = new List<ReferenceModeKind>(candidates.Count);
				foreach (ReferenceModeKind refKind in candidates)
				{
					kinds.Add(refKind);
				}
				kinds.Sort(delegate(ReferenceModeKind kind1, ReferenceModeKind kind2)
				{
					return string.Compare(kind1.Name, kind2.Name);
				});
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
			IList rawModes = instance.Model.ReferenceModeCollection;
			IList candidates;
			string instanceName = instance.Name;
			string formatString = ResourceStrings.ModelReferenceModePickerFormatString; 
			if (rawModes.Count > 1)
			{
				// Make sure we're sorted
				List<ReferenceMode> modes = new List<ReferenceMode>(rawModes.Count);
				foreach (ReferenceMode mode in rawModes)
				{
					modes.Add(mode);
				}
				modes.Sort(delegate(ReferenceMode mode1, ReferenceMode mode2)
				{
					return string.Compare(mode1.Name, mode2.Name);
				});
				myModes = modes;
				int modeCount = modes.Count;
				string[] prettyStrings = new string[modeCount];
				for (int i = 0; i < modeCount; ++i)
				{
					ReferenceMode refMode = modes[i];
					prettyStrings[i] = string.Format(CultureInfo.InvariantCulture, formatString, refMode.Name, refMode.GenerateValueTypeName(instanceName));
				}
				candidates = prettyStrings;
			}
			else
			{
				myModes = rawModes;
				ReferenceMode refMode = (ReferenceMode)rawModes[0];
				candidates = new string[] { string.Format(CultureInfo.InvariantCulture, formatString, refMode.Name, refMode.GenerateValueTypeName(instanceName)) };
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
