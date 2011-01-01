#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
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
using System.Diagnostics;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel.Design
{
	#region ObjectifiedFactTypePropertyDescriptor class
	/// <summary>
	/// <see cref="RolePlayerElementPropertyDescriptor"/> for <see cref="ObjectType.NestedFactType"/>
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ObjectifiedFactTypePropertyDescriptor : RolePlayerElementPropertyDescriptor
	{
		#region Constructor
		private readonly bool myIsReadOnly;
		/// <summary>
		/// Initializes a new instance of <see cref="ObjectifiedFactTypePropertyDescriptor"/>.
		/// </summary>
		public ObjectifiedFactTypePropertyDescriptor(ObjectType sourcePlayer, DomainRoleInfo domainRole, Attribute[] sourceDomainRoleInfoAttributes)
			: base(sourcePlayer, domainRole, sourceDomainRoleInfoAttributes)
		{
			// The base class constructor has already checked domainRole for null.
			if (domainRole.Id != Objectification.NestedFactTypeDomainRoleId)
			{
				throw new ArgumentException();
			}
			Objectification objectification;
			myIsReadOnly = null != (objectification = sourcePlayer.Objectification) && objectification.IsImplied;
		}
		#endregion // Constructor
		#region IsReadOnly property
		/// <summary>
		/// Ensure that the <see cref="ObjectType.NestedFactType"/> property is read-only when
		/// <see cref="Objectification"/> is not <see langword="null"/> and
		/// <see cref="ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.IsImplied"/> is <see langword="true"/>.
		/// </summary>
		public override bool IsReadOnly
		{
			get
			{
				return myIsReadOnly;
			}
		}
		#endregion // IsReadOnly property
		#region ResetValue method
		/// <summary>Delete the objectification relationship.</summary>
		public override void ResetValue(object component)
		{
			this.SetValue(component, null);
		}
		#endregion // ResetValue method
		#region SetValue method
		/// <summary>See <see cref="RolePlayerElementPropertyDescriptor.SetValue"/>.</summary>
		public override void SetValue(object component, object value)
		{
			ObjectType objectType;
			if (myIsReadOnly ||
				null == (objectType = EditorUtility.ResolveContextInstance(component, false) as ObjectType))
			{
				return;
			}
			IORMToolServices toolServices;
			AutomatedElementFilterCallback callback = null;
			Store store = objectType.Store;
			if (null != (toolServices = store as IORMToolServices))
			{
				callback = delegate(ModelElement filterElement)
				{
					FactType factType;
					return filterElement is ObjectType || (null != (factType = filterElement as FactType) && null == factType.ImpliedByObjectification) ?
						AutomatedElementDirective.NeverIgnore :
						AutomatedElementDirective.None;
				};
				toolServices.AutomatedElementFilter += callback;
			}
			try
			{
				using (Transaction transaction = BeginTransaction(store))
				{
					FactType factType = value as FactType;
					if (factType != null)
					{
						Objectification.CreateExplicitObjectification(factType, objectType);
					}
					else
					{
						objectType.NestedFactType = null;
					}
					if (transaction.HasPendingChanges)
					{
						transaction.Commit();
					}
				}
			}
			finally
			{
				if (toolServices != null)
				{
					toolServices.AutomatedElementFilter -= callback;
				}
			}
		}
		#endregion // SetValue method
	}
	#endregion // ObjectifiedFactTypePropertyDescriptor class
	#region ObjectifyingEntityTypePropertyDescriptor class
	/// <summary>
	/// <see cref="RolePlayerElementPropertyDescriptor"/> for <see cref="FactType.NestingType"/>
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ObjectifyingEntityTypePropertyDescriptor : RolePlayerElementPropertyDescriptor
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of <see cref="ObjectifyingEntityTypePropertyDescriptor"/>.
		/// </summary>
		public ObjectifyingEntityTypePropertyDescriptor(FactType sourcePlayer, DomainRoleInfo domainRole, Attribute[] sourceDomainRoleInfoAttributes)
			: base(sourcePlayer, domainRole, sourceDomainRoleInfoAttributes)
		{
			// The base class constructor has already checked domainRole for null.
			if (domainRole.Id != Objectification.NestingTypeDomainRoleId)
			{
				throw new ArgumentException();
			}
		}
		#endregion // Constructor
		#region ResetValue method
		/// <summary>Delete the objectification relationship.</summary>
		public override void ResetValue(object component)
		{
			this.SetValue(component, null);
		}
		#endregion // ResetValue method
		#region SetValue method
		/// <summary>See <see cref="RolePlayerElementPropertyDescriptor.SetValue"/>.</summary>
		public override void SetValue(object component, object value)
		{
			FactType factType;
			if (null == (factType = EditorUtility.ResolveContextInstance(component, false) as FactType))
			{
				return;
			}
			IORMToolServices toolServices = null;
			AutomatedElementFilterCallback callback = null;
			Store store = factType.Store;
			if (null != (toolServices = store as IORMToolServices))
			{
				callback = delegate(ModelElement filterElement)
				{
					FactType filterFactType;
					return filterElement is ObjectType || (null != (filterFactType = filterElement as FactType) && null == filterFactType.ImpliedByObjectification) ?
						AutomatedElementDirective.NeverIgnore :
						AutomatedElementDirective.None;
				};
				toolServices.AutomatedElementFilter += callback;
			}
			try
			{
				using (Transaction transaction = BeginTransaction(store))
				{
					Objectification.CreateExplicitObjectification(factType, value as ObjectType);
					if (transaction.HasPendingChanges)
					{
						transaction.Commit();
					}
				}
			}
			finally
			{
				if (toolServices != null)
				{
					toolServices.AutomatedElementFilter -= callback;
				}
			}
		}
		#endregion // SetValue method
	}
	#endregion // ObjectifyingEntityTypePropertyDescriptor class
}
