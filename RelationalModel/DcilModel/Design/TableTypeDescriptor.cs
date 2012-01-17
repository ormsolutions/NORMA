#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase;

namespace ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.Design
{
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="ObjectType"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class TableTypeDescriptor<TModelElement> : ConceptualDatabaseElementTypeDescriptor<TModelElement>
		where TModelElement : Table
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of <see cref="TableTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public TableTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
		}
		#endregion // Constructor
		#region Base overrides
		/// <summary>
		/// Create property descriptors that only allow merging of DataType facet properties
		/// when the <see cref="P:ObjectType.DataType"/> instances are equal.
		/// </summary>
		protected override ElementPropertyDescriptor CreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainPropertyInfo, Attribute[] attributes)
		{
			if (domainPropertyInfo.Id == Table.NameDomainPropertyId)
			{
				return new NamePropertyDescriptor(this, requestor, domainPropertyInfo, attributes);
			}
			return base.CreatePropertyDescriptor(requestor, domainPropertyInfo, attributes);
		}
		#endregion // Base overrides
		#region NamePropertyDescriptor class
		/// <summary>
		/// A property descriptor that ties display and reset capabilities
		/// on the column name to both the Name and CustomName properties.
		/// </summary>
		private sealed class NamePropertyDescriptor : ElementPropertyDescriptor
		{
			public NamePropertyDescriptor(ElementTypeDescriptor owner, ModelElement modelElement, DomainPropertyInfo domainProperty, Attribute[] attributes)
				: base(owner, modelElement, domainProperty, attributes)
			{
			}
			public override bool CanResetValue(object component)
			{
				return ((Table)this.ModelElement).CustomName;
			}
			public override void ResetValue(object component)
			{
				this.SetValue(component, "");
			}
			public override bool ShouldSerializeValue(object component)
			{
				return ((Table)this.ModelElement).CustomName;
			}
			public override void SetValue(object component, object value)
			{
				Table table;
				Store store;
				string newName;
				if (null == (table = ModelElement as Table) ||
					null == (store = Utility.ValidateStore(table.Store)) ||
					null == (newName = value as string) ||
					newName == table.Name)
				{
					return;
				}
				using (Transaction t = store.TransactionManager.BeginTransaction(ElementPropertyDescriptor.GetSetValueTransactionName(this.DisplayName)))
				{
					if (string.IsNullOrEmpty(newName))
					{
						// Name generators should listen to this property to regenerate
						// the name.
						table.CustomName = false;
					}
					else
					{
						table.CustomName = true;
						table.Name = newName;
					}
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
		}
		#endregion // NamePropertyDescriptor class
	}
}
