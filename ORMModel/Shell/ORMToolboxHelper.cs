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
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.ShapeModel
{
	public sealed partial class ORMShapeModelToolboxHelper
	{
		private int myObjectTypeCount;
		private int myFactTypeCount;
		private int myUniquenessConstraintCount;
		/// <summary>See <see cref="ORMShapeModelToolboxHelperBase.CreateElementToolPrototype"/>.</summary>
		protected sealed override ElementGroupPrototype CreateElementToolPrototype(Store store, Guid domainClassId)
		{
			// WARNING: This method is _extremely_ order-sensitive. If the order that the toolbox items are listed
			// in the .dsl file changes, or if the DSL Tools text template that is used to generate ORMShapeModelToolboxHelperBase
			// changes, this method will most likely need to be changed as well.

			ElementGroup group = new ElementGroup(store);
			bool unknownItem = false;

			if (domainClassId.Equals(ObjectType.DomainClassId))
			{
				ObjectType objectType = new ObjectType(store);
				group.AddGraph(objectType, true);
				switch (myObjectTypeCount++)
				{
					case 0:
						// EntityType - We don't need to do anything else...
						break;
					case 1:
						// ValueType
						// Do not try to set the IsValueType property here. IsValueType picks
						// up the default data type for the model, which can only be done
						// when the model is known. Instead, flag the element so that it
						// can be set during MergeRelate on the model.
						group.UserData = ORMModel.ValueTypeUserDataKey;
						break;
					case 2:
						// ObjectifiedFactType
						group.AddGraph(new Objectification(objectType, AddFactType(store, group, 2)), false);
						break;
					default:
						unknownItem = true;
						break;
				}
			}
			else if (domainClassId.Equals(FactType.DomainClassId))
			{
				Debug.Assert(myFactTypeCount < 3);
				AddFactType(store, group, ++myFactTypeCount);
			}
			else if (domainClassId.Equals(UniquenessConstraint.DomainClassId))
			{
				if (myUniquenessConstraintCount == 0)
				{
					// Add this here so that we can distinguish between internal and external uniqueness
					// constraints without unpacking the model. We want to merge internals into a fact
					// and externals into the model.
					group.UserData = ORMModel.InternalUniquenessConstraintUserDataKey;
					group.AddGraph(UniquenessConstraint.CreateInternalUniquenessConstraint(store), true);
				}
				else
				{
					Debug.Assert(myUniquenessConstraintCount == 1);
					group.AddGraph(new UniquenessConstraint(store), true);
				}
				myUniquenessConstraintCount++;
			}
			else if (domainClassId.Equals(EqualityConstraint.DomainClassId))
			{
				group.AddGraph(new EqualityConstraint(store), true);
			}
			else if (domainClassId.Equals(ExclusionConstraint.DomainClassId))
			{
				group.AddGraph(new ExclusionConstraint(store), true);
			}
			else if (domainClassId.Equals(MandatoryConstraint.DomainClassId))
			{
				group.AddGraph(new MandatoryConstraint(store), true);
			}
			else if (domainClassId.Equals(SubsetConstraint.DomainClassId))
			{
				group.AddGraph(new SubsetConstraint(store), true);
			}
			else if (domainClassId.Equals(FrequencyConstraint.DomainClassId))
			{
				group.AddGraph(new FrequencyConstraint(store), true);
			}
			else if (domainClassId.Equals(RingConstraint.DomainClassId))
			{
				group.AddGraph(new RingConstraint(store), true);
			}
			else
			{
				unknownItem = true;
			}
			Debug.Assert(!unknownItem, "Unexpected toolbox item type.");
			return unknownItem ? null : group.CreatePrototype();
		}
		/// <summary>
		/// Creates a new <see cref="FactType"/> with the specified <paramref name="arity"/>.
		/// </summary>
		/// <param name="store">
		/// The <see cref="Store"/> in which the new <see cref="FactType"/> should be created.
		/// </param>
		/// <param name="group">
		/// The <see cref="ElementGroup"/> to which the new <see cref="FactType"/> and its <see cref="Role"/>s should
		/// be added.
		/// </param>
		/// <param name="arity">
		/// The number of <see cref="Role"/>s that the new <see cref="FactType"/> should contain.
		/// </param>
		/// <returns>
		/// The newly created <see cref="FactType"/>.
		/// </returns>
		/// <remarks>
		/// The new <see cref="FactType"/> is added to <paramref name="group"/> as a root element.
		/// </remarks>
		private static FactType AddFactType(Store store, ElementGroup group, int arity)
		{
			FactType factType = new FactType(store, null);
			group.AddGraph(factType, true);
			LinkedElementCollection<RoleBase> roles = factType.RoleCollection;
			for (int i = 0; i < arity; i++)
			{
				Role role = new Role(store);
				roles.Add(role);
				group.AddGraph(role);
			}
			return factType;
		}
	}
}
