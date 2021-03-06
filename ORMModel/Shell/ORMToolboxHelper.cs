#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
* Copyright � ORM Solutions, LLC. All rights reserved.                     *
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
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework.Diagrams.Design;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	public sealed partial class ORMShapeToolboxHelper
	{
		private int myObjectTypeCount;
		private int myFactTypeCount;
		private int myUniquenessConstraintCount;
		/// <summary>See <see cref="ORMShapeToolboxHelperBase.CreateElementToolPrototype"/>.</summary>
		protected sealed override ElementGroupPrototype CreateElementToolPrototype(Store store, Guid domainClassId)
		{
			// WARNING: This method is _extremely_ order-sensitive. If the order that the toolbox items are listed
			// in the .dsl file changes, or if the DSL Tools text template that is used to generate ORMShapeModelToolboxHelperBase
			// changes, this method will most likely need to be changed as well.

			ElementGroup group = null;
			bool unknownItem = false;

			if (domainClassId.Equals(ObjectType.DomainClassId))
			{
				group = new ElementGroup(store);
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
				group = new ElementGroup(store);
				Debug.Assert(myFactTypeCount < 3);
				AddFactType(store, group, ++myFactTypeCount);
			}
			else if (domainClassId.Equals(ExclusiveOrConstraintCoupler.DomainClassId))
			{
				group = new ElementGroup(store);
				MandatoryConstraint mandatory = new MandatoryConstraint(store, null);
				group.AddGraph(mandatory, true);
				ExclusionConstraint exclusion = new ExclusionConstraint(store, null);
				group.AddGraph(exclusion, true);
				group.AddGraph(new ExclusiveOrConstraintCoupler(mandatory, exclusion), false);
			}
			else if (domainClassId.Equals(UniquenessConstraint.DomainClassId))
			{
				group = new ElementGroup(store);
				if (myUniquenessConstraintCount == 0)
				{
					// Add this here so that we can distinguish between internal and external uniqueness
					// constraints without unpacking the model. We want to merge internals into a fact
					// and externals into the model.
					group.UserData = ORMModel.InternalUniquenessConstraintUserDataKey;
					group.AddGraph(UniquenessConstraint.CreateInternalUniquenessConstraint(store.DefaultPartition), true);
				}
				else
				{
					Debug.Assert(myUniquenessConstraintCount == 1);
					group.AddGraph(new UniquenessConstraint(store), true);
				}
				myUniquenessConstraintCount++;
			}
			return (group == null || unknownItem) ? base.CreateElementToolPrototype(store, domainClassId) : group.CreatePrototype();
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
#if VISUALSTUDIO_10_0
		/// <summary>
		/// Provide dynamic toolbox items. Note that we do not use the
		/// static toolbox provide semantics provided with the DSL 2010
		/// generators because the list of toolbox items is considered
		/// variable based on the <see cref="IModelingToolboxItemProvider"/>
		/// extension interface.
		/// </summary>
		public override IList<ModelingToolboxItem> CreateToolboxItems()
		{
			IList<ModelingToolboxItem> retVal = base.CreateToolboxItems();
			Store store = ToolboxStore;
			using (Transaction t = store.TransactionManager.BeginTransaction("CreateToolboxItems"))
			{
				// Retrieve the specified ToolboxItem from the DSL
				retVal.Add(GetToolboxItem("EntityTypeToolboxItem", store));
				retVal.Add(GetToolboxItem("ValueTypeToolboxItem", store));
				retVal.Add(GetToolboxItem("ObjectifiedFactTypeToolboxItem", store));
				retVal.Add(GetToolboxItem("UnaryFactTypeToolboxItem", store));
				retVal.Add(GetToolboxItem("BinaryFactTypeToolboxItem", store));
				retVal.Add(GetToolboxItem("TernaryFactTypeToolboxItem", store));
				retVal.Add(GetToolboxItem("RoleConnectorToolboxItem", store));
				retVal.Add(GetToolboxItem("SubtypeConnectorToolboxItem", store));
				retVal.Add(GetToolboxItem("InternalUniquenessConstraintToolboxItem", store));
				retVal.Add(GetToolboxItem("ExternalUniquenessConstraintToolboxItem", store));
				retVal.Add(GetToolboxItem("EqualityConstraintToolboxItem", store));
				retVal.Add(GetToolboxItem("ExclusionConstraintToolboxItem", store));
				retVal.Add(GetToolboxItem("InclusiveOrConstraintToolboxItem", store));
				retVal.Add(GetToolboxItem("ExclusiveOrConstraintToolboxItem", store));
				retVal.Add(GetToolboxItem("SubsetConstraintToolboxItem", store));
				retVal.Add(GetToolboxItem("FrequencyConstraintToolboxItem", store));
				retVal.Add(GetToolboxItem("RingConstraintToolboxItem", store));
				retVal.Add(GetToolboxItem("ValueComparisonConstraintToolboxItem", store));
				retVal.Add(GetToolboxItem("ExternalConstraintConnectorToolboxItem", store));
				retVal.Add(GetToolboxItem("ModelNoteToolboxItem", store));
				retVal.Add(GetToolboxItem("ModelNoteConnectorToolboxItem", store));
			}
			return retVal;
		}
#endif // VISUALSTUDIO_10_0
	}
}
