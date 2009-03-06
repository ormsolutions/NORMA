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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace ORMSolutions.ORMArchitect.Framework.Design
{
	#region RolePlayerElementPropertyDescriptor class
	/// <summary>
	/// Improved version of <see cref="RolePlayerPropertyDescriptor"/>.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class RolePlayerElementPropertyDescriptor : RolePlayerPropertyDescriptor
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of <see cref="RolePlayerElementPropertyDescriptor"/>.
		/// </summary>
		public RolePlayerElementPropertyDescriptor(ModelElement sourcePlayer, DomainRoleInfo domainRole, Attribute[] sourceDomainRoleInfoAttributes)
			: base(sourcePlayer, domainRole, sourceDomainRoleInfoAttributes)
		{
			DomainRoleInfo sourceDomainRole = this.mySourceDomainRole = domainRole.OppositeDomainRole;
			// If both roles are optional, we will always allow null.
			this.myAlwaysAllowNull = sourceDomainRole.IsOptional && domainRole.IsOptional;
		}
		#endregion // Constructor

		#region SourceDomainRole property
		private readonly DomainRoleInfo mySourceDomainRole;
		/// <summary>
		/// The <see cref="DomainRoleInfo"/> for the source role. Corresponds to the role played by
		/// the <see cref="ModelElement"/> that was passed to
		/// <see cref="RolePlayerElementPropertyDescriptor(ModelElement,DomainRoleInfo,Attribute[])"/>.
		/// </summary>
		public DomainRoleInfo SourceDomainRole
		{
			get
			{
				return this.mySourceDomainRole;
			}
		}
		#endregion // SourceDomainRole property

		#region TargetDomainRole property
		/// <summary>
		/// The <see cref="DomainRoleInfo"/> for the target role. Corresponds to the role opposite
		/// to the role played by the <see cref="ModelElement"/> that was passed to
		/// <see cref="RolePlayerElementPropertyDescriptor(ModelElement,DomainRoleInfo,Attribute[])"/>.
		/// </summary>
		public DomainRoleInfo TargetDomainRole
		{
			get
			{
				return base.DomainRoleInfo;
			}
		}
		#endregion // TargetDomainRole property

		#region ComponentType property
		/// <summary>
		/// Returns the <see cref="Type"/> of <see cref="RolePlayerPropertyDescriptor.SourcePlayer"/>.
		/// </summary>
		public override Type ComponentType
		{
			get
			{
				return this.SourcePlayer.GetType();
			}
		}
		#endregion // ComponentType property

		#region PropertyType property
		/// <summary>
		/// Returns the <see cref="DomainClassInfo.ImplementationClass"/> of the
		/// <see cref="DomainRoleInfo.RolePlayer"/> of <see cref="TargetDomainRole"/>.
		/// </summary>
		public override Type PropertyType
		{
			get
			{
				return this.TargetDomainRole.RolePlayer.ImplementationClass;
			}
		}
		#endregion // PropertyType property

		#region Link property
		/// <summary>
		/// The <see cref="Link"/> that this
		/// <see cref="RolePlayerElementPropertyDescriptor"/> is currently for.
		/// </summary>
		/// <remarks>
		/// Replaces <see cref="RolePlayerPropertyDescriptor.Link"/>.
		/// </remarks>
		public new ElementLink Link
		{
			get
			{
				ReadOnlyCollection<ElementLink> elementLinks = this.SourceDomainRole.GetElementLinks<ElementLink>(this.SourcePlayer);
				return (elementLinks.Count > 0) ? elementLinks[0] : null;
			}
		}
		#endregion // Link property

		#region AllowNull property
		private readonly bool myAlwaysAllowNull;
		/// <summary>
		/// Indicates whether the role player can be set to <see langword="null"/>
		/// (in other words, whether <see cref="Link"/> can be deleted), without
		/// considering read-only status.
		/// </summary>
		/// <remarks>
		/// Replaces <see cref="RolePlayerPropertyDescriptor.AllowNull"/>.
		/// </remarks>
		public new virtual bool AllowNull
		{
			get
			{
				if (this.myAlwaysAllowNull)
				{
					// We previously determined that we always allow null.
					return true;
				}
				ElementLink elementLink = this.Link;
				if (elementLink == null)
				{
					// If we currently don't have a link,
					// we won't force one to be added.
					return true;
				}
				if (this.SourceDomainRole.IsOptional)
				{
					DomainRoleInfo targetDomainRole = this.TargetDomainRole;
					// The source role is optional, but the target role isn't (otherwise
					// alwaysAllowNull would have been true). However, we need to check
					// for other instances of this type of relationship that satisfy that.
					if (targetDomainRole.Multiplicity == Multiplicity.OneMany)
					{
						ModelElement targetPlayer = targetDomainRole.GetRolePlayer(elementLink);
						if (targetDomainRole.GetElementLinks<ElementLink>(targetPlayer).Count > 1)
						{
							// There is at least one other instance of this type of relationship,
							// which means that the "mandatory" on the target role would still be
							// satisfied even if this link were to be deleted.
							return true;
						}
					}
				}
				return false;
			}
		}
		#endregion // AllowNull property

		#region RolePlayerMenuCommands property
		/// <summary>
		/// Blocks editor access to <see cref="RolePlayerPropertyDescriptor.RolePlayerMenuCommands"/>.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This property is not supported.", true)]
		public new Collection<RolePlayerMenuCommand> RolePlayerMenuCommands
		{
			get
			{
				throw new NotSupportedException();
			}
		}
		#endregion // RolePlayerMenuCommands property

		#region MapSourceRolePlayer property
		/// <summary>
		/// Blocks editor access to <see cref="RolePlayerPropertyDescriptor.MapSourceRolePlayer"/>.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This property is not supported.", true)]
		public new SourceRolePlayerMapFunction MapSourceRolePlayer
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}
		#endregion // MapSourceRolePlayer property

		#region BeginTransaction method
		/// <summary>
		/// Begins a new <see cref="Transaction"/> with the <see cref="Transaction.Name"/> set to the
		/// <see cref="String"/> returned when calling <see cref="RolePlayerPropertyDescriptor.GetSetFieldString"/>
		/// with <see cref="RolePlayerPropertyDescriptor.DisplayName"/> as the parameter.
		/// </summary>
		/// <returns></returns>
		protected Transaction BeginTransaction()
		{
			return this.Store.TransactionManager.BeginTransaction(this.GetSetFieldString(this.DisplayName));
		}
		#endregion // BeginTransaction method

		#region ShouldSerializeValue method
		/// <summary>
		/// Returns <see langword="true"/> if <see cref="Link"/>
		/// is not <see langword="null"/>.
		/// </summary>
		public override bool ShouldSerializeValue(object component)
		{
			return (this.Link != null);
		}
		#endregion // ShouldSerializeValue method

		#region CanResetValue method
		/// <summary>
		/// Determines whether <see cref="Link"/> can be deleted.
		/// </summary>
		/// <remarks>
		/// Replaces <see cref="RolePlayerPropertyDescriptor.CanResetValue"/>.
		/// </remarks>
		public override bool CanResetValue(object component)
		{
			return (this.Link != null) && !this.IsReadOnly && this.AllowNull;
		}
		#endregion // CanResetValue method

		#region ResetValue method
		/// <summary>
		/// If <see cref="CanResetValue"/> returns <see langword="true"/>,
		/// deletes <see cref="Link"/>.
		/// </summary>
		/// <remarks>
		/// Replaces <see cref="RolePlayerPropertyDescriptor.ResetValue"/>.
		/// </remarks>
		public override void ResetValue(object component)
		{
			ElementLink elementLink = this.Link;
			// We defer to CanResetValue rather than checking the conditions ourself
			// so that a derived type can add or alter the conditions without
			// having to override this method as well.
			if (elementLink != null && this.CanResetValue(component))
			{
				using (Transaction transaction = this.BeginTransaction())
				{
					elementLink.Delete();
					transaction.Commit();
				}
			}
		}
		#endregion // ResetValue method

		#region GetValue method
		/// <summary>
		/// Returns the current role player of <see cref="TargetDomainRole"/>
		/// for <see cref="Link"/>, or <see langword="null"/> if
		/// <see cref="Link"/> is <see langword="null"/>.
		/// </summary>
		/// <remarks>
		/// Replaces <see cref="RolePlayerPropertyDescriptor.GetValue"/>.
		/// </remarks>
		public override object GetValue(object component)
		{
			ElementLink elementLink = this.Link;
			return (elementLink != null) ? this.TargetDomainRole.GetRolePlayer(elementLink) : null;
		}
		#endregion // GetValue method

		#region SetValue method
		/// <summary>
		/// Sets the new role player specified by <paramref name="value"/> for <see cref="Link"/>,
		/// or creates a new <see cref="ElementLink"/> for
		/// <see cref="RolePlayerPropertyDescriptor.RelationshipInfo"/> between
		/// <see cref="RolePlayerPropertyDescriptor.SourcePlayer"/> and <paramref name="value"/>.
		/// If <paramref name="value"/> is <see langword="null"/> or not a <see cref="ModelElement"/>,
		/// <see cref="ResetValue"/> is called.
		/// </summary>
		/// <remarks>
		/// Replaces <see cref="RolePlayerPropertyDescriptor.SetValue"/>.
		/// </remarks>
		public override void SetValue(object component, object value)
		{
			ModelElement targetPlayer = value as ModelElement;
			if (targetPlayer == null)
			{
				// Defer to ResetValue.
				this.ResetValue(component);
				return;
			}

			if (this.IsReadOnly)
			{
				// We don't make any changes if we're read-only.
				return;
			}
			
			ElementLink elementLink = this.Link;
			using (Transaction transaction = this.BeginTransaction())
			{
				if (elementLink == null)
				{
					this.Store.ElementFactory.CreateElementLink(this.RelationshipInfo,
						new RoleAssignment(this.SourceDomainRole.Id, this.SourcePlayer),
						new RoleAssignment(this.TargetDomainRole.Id, targetPlayer));
				}
				else
				{
					this.TargetDomainRole.SetRolePlayer(elementLink, targetPlayer);
				}
				if (transaction.HasPendingChanges)
				{
					transaction.Commit();
				}
			}
		}
		#endregion // SetValue method
	}
	#endregion // RolePlayerElementPropertyDescriptor class
}
