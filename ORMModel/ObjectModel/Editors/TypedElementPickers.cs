using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using Northface.Tools.ORM.ObjectModel;
namespace Northface.Tools.ORM.ObjectModel.Editors
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
			IList<ObjectType> roleTypes = null;
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
							}
							if (!roleTypes.Contains(objType))
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
			IList<FactType> roleFacts = null;
			int count = candidates.Count;
			if (count > 0)
			{
				List<FactType> types = new List<FactType>(count);
				foreach (FactType factType in candidates)
				{
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
						}
						if (!roleFacts.Contains(factType))
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
	}
	#endregion // NestedFactTypePicker class
}