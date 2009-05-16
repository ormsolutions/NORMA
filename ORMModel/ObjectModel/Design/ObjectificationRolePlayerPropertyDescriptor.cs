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
	#region ObjectificationRolePlayerPropertyDescriptor class
	/// <summary>
	/// <see cref="RolePlayerElementPropertyDescriptor"/> for <see cref="ObjectType.NestedFactType"/>
	/// (<see cref="ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.NestedFactType"/>) and
	/// <see cref="FactType.NestingType"/> (<see cref="ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.NestingType"/>).
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ObjectificationRolePlayerPropertyDescriptor : RolePlayerElementPropertyDescriptor
	{
		#region Constructor
		private static readonly Guid[] ObjectificationDomainRoleIds =
			new Guid[] { Objectification.NestedFactTypeDomainRoleId, Objectification.NestingTypeDomainRoleId };
		/// <summary>
		/// Initializes a new instance of <see cref="ObjectificationRolePlayerPropertyDescriptor"/>.
		/// </summary>
		public ObjectificationRolePlayerPropertyDescriptor(ModelElement sourcePlayer, DomainRoleInfo domainRole, Attribute[] sourceDomainRoleInfoAttributes)
			: base(sourcePlayer, domainRole, sourceDomainRoleInfoAttributes)
		{
			// The base class constructor has already checked domainRole for null.
			if (!Utility.IsDescendantOrSelf(domainRole, ObjectificationDomainRoleIds))
			{
				throw new ArgumentException();
			}
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
				ObjectType objectType = this.SourcePlayer as ObjectType;
				if (objectType != null)
				{
					Objectification objectification = objectType.Objectification;
					if (objectification != null)
					{
						return objectification.IsImplied;
					}
				}
				return base.IsReadOnly;
			}
		}
		#endregion // IsReadOnly property
		#region ResetValue method
		/// <summary>See <see cref="RolePlayerElementPropertyDescriptor.ResetValue"/>.</summary>
		public override void ResetValue(object component)
		{
			this.SetValue(component, null);
		}
		#endregion // ResetValue method
		#region SetValue method
		/// <summary>See <see cref="RolePlayerElementPropertyDescriptor.SetValue"/>.</summary>
		public override void SetValue(object component, object value)
		{
			if (this.IsReadOnly)
			{
				return;
			}
			IORMToolServices toolServices = null;
			ModelElement element;
			AutomatedElementFilterCallback callback = null;
			if (null != (element = component as ModelElement) &&
				null != (toolServices = element.Store as IORMToolServices))
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
				using (Transaction transaction = this.BeginTransaction())
				{
					ObjectType objectType = this.SourcePlayer as ObjectType;
					if (objectType != null)
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
					}
					else
					{
						FactType factType = this.SourcePlayer as FactType;
						Debug.Assert(factType != null, "SourcePlayer should only ever be an ObjectType or a FactType.");
						Objectification.CreateExplicitObjectification(factType, value as ObjectType);
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
	#endregion // ObjectificationRolePlayerPropertyDescriptor class
}
