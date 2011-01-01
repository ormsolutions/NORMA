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
		}
		#endregion // Constructor
#if VISUALSTUDIO_10_0
		private ElementLink GetLink(ModelElement element)
		{
			ReadOnlyCollection<ElementLink> links;
			if (null != element &&
				0 != (links = this.DomainRoleInfo.OppositeDomainRole.GetElementLinks(element)).Count)
			{
				return links[0];
			}
			return null;
		}
#else // VISUALSTUDIO_10_0
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
		/// <see cref="DomainRoleInfo.RolePlayer"/> of <see cref="DomainRoleInfo"/>.
		/// </summary>
		public override Type PropertyType
		{
			get
			{
				return DomainRoleInfo.RolePlayer.ImplementationClass;
			}
		}
		#endregion // PropertyType property
#endif // VISUALSTUDIO_10_0
		#region AllowNull property
#if VISUALSTUDIO_10_0
		/// <summary>
		/// Test if null is allowed for the provided link.
		/// </summary>
		/// <param name="link">The current element link for
		/// the component associated with this role player</param>
		/// <returns></returns>
		public virtual bool CanAllowNull(ElementLink link)
		{
			if (link == null)
			{
				return true;
			}
			DomainRoleInfo targetDomainRole = DomainRoleInfo;
			DomainRoleInfo sourceDomainRole = targetDomainRole.OppositeDomainRole;
			bool sourceIsOptional = sourceDomainRole.IsOptional;
			if (sourceIsOptional && targetDomainRole.IsOptional)
			{
				// We previously determined that we always allow null.
				return true;
			}
			if (sourceIsOptional)
			{
				// The source role is optional, but the target role isn't. However,
				// we need to check/ for other instances of this type of relationship
				// that satisfy that.
				if (targetDomainRole.Multiplicity == Multiplicity.OneMany)
				{
					ModelElement targetPlayer = targetDomainRole.GetRolePlayer(link);
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
#else
		/// <summary>
		/// Indicates whether the role player can be set to <see langword="null"/>
		/// (in other words, whether <see cref="RolePlayerPropertyDescriptor.Link"/> can be deleted), without
		/// considering read-only status.
		/// </summary>
		/// <remarks>
		/// Replaces <see cref="RolePlayerPropertyDescriptor.AllowNull"/>.
		/// </remarks>
		public new virtual bool AllowNull
		{
			get
			{
				DomainRoleInfo targetDomainRole = DomainRoleInfo;
				DomainRoleInfo sourceDomainRole = targetDomainRole.OppositeDomainRole;
				bool sourceIsOptional = sourceDomainRole.IsOptional;
				if (sourceIsOptional && targetDomainRole.IsOptional)
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
				if (sourceIsOptional)
				{
					// The source role is optional, but the target role isn't. However,
					// we need to check/ for other instances of this type of relationship
					// that satisfy that.
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
#endif
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
#if !VISUALSTUDIO_10_0
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
#endif // !VISUALSTUDIO_10_0
		#endregion // MapSourceRolePlayer property
		#region BeginTransaction method
		/// <summary>
		/// Begins a new <see cref="Transaction"/> with the <see cref="Transaction.Name"/> set to the
		/// <see cref="String"/> returned when calling <see cref="RolePlayerPropertyDescriptor.GetSetFieldString"/>
		/// with <see cref="RolePlayerPropertyDescriptor.DisplayName"/> as the parameter.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> to begin the transaction for.</param>
		/// <returns>A new <see cref="Transaction"/></returns>
		protected Transaction BeginTransaction(Store store)
		{
			return store.TransactionManager.BeginTransaction(this.GetSetFieldString(this.DisplayName));
		}
		#endregion // BeginTransaction method
		#region ShouldSerializeValue method
#if VISUALSTUDIO_10_0
		/// <summary>
		/// Returns <see langword="true"/> if <see cref="GetValue"/>
		/// is not <see langword="null"/>.
		/// </summary>
		public override bool ShouldSerializeValue(object component)
		{
			return GetValue(component) != null;
		}
#else
		/// <summary>
		/// Returns <see langword="true"/> if <see cref="RolePlayerPropertyDescriptor.Link"/>
		/// is not <see langword="null"/>.
		/// </summary>
		public override bool ShouldSerializeValue(object component)
		{
			return (this.Link != null);
		}
#endif
		#endregion // ShouldSerializeValue method
		#region CanResetValue method
#if VISUALSTUDIO_10_0
		/// <summary>
		/// Determines whether the <see cref="ElementLink"/> for this <param name="component"/> can be deleted.
		/// </summary>
		/// <remarks>
		/// Replaces <see cref="RolePlayerPropertyDescriptor.CanResetValue"/>.
		/// </remarks>
		public override bool CanResetValue(object component)
		{
			ElementLink link;
			return !this.IsReadOnly &&
				null != (link = GetLink(EditorUtility.ResolveContextInstance(component, false) as ModelElement)) &&
				CanAllowNull(link);
		}
#else
		/// <summary>
		/// Determines whether <see cref="RolePlayerPropertyDescriptor.Link"/> can be deleted.
		/// </summary>
		/// <remarks>
		/// Replaces <see cref="RolePlayerPropertyDescriptor.CanResetValue"/>.
		/// </remarks>
		public override bool CanResetValue(object component)
		{
			return (this.Link != null) && !this.IsReadOnly && this.AllowNull;
		}
#endif
		#endregion // CanResetValue method
		#region ResetValue method
#if !VISUALSTUDIO_10_0
		/// <summary>
		/// If <see cref="CanResetValue"/> returns <see langword="true"/>,
		/// deletes <see cref="RolePlayerPropertyDescriptor.Link"/>.
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
				using (Transaction transaction = this.BeginTransaction(elementLink.Store))
				{
					elementLink.Delete();
					transaction.Commit();
				}
			}
		}
#endif // !VISUALSTUDIO_10_0
		#endregion // ResetValue method
		#region GetValue method
#if VISUALSTUDIO_10_0
		/// <summary>
		/// Returns the current role player of <see cref="RolePlayerPropertyDescriptor.DomainRoleInfo"/>,
		/// or <see langword="null"/> if the current <see cref="ElementLink"/> associated with this property
		/// is <see langword="null"/>
		/// </summary>
		/// <remarks>
		/// Replaces <see cref="RolePlayerPropertyDescriptor.GetValue"/>.
		/// </remarks>
		public override object GetValue(object component)
		{
			return null != (component = EditorUtility.ResolveContextInstance(component, false)) ? base.GetValue(component) : null;
		}
#else
		/// <summary>
		/// Returns the current role player of <see cref="RolePlayerPropertyDescriptor.DomainRoleInfo"/>
		/// for <see cref="RolePlayerPropertyDescriptor.Link"/>, or <see langword="null"/> if
		/// <see cref="RolePlayerPropertyDescriptor.Link"/> is <see langword="null"/>.
		/// </summary>
		/// <remarks>
		/// Replaces <see cref="RolePlayerPropertyDescriptor.GetValue"/>.
		/// </remarks>
		public override object GetValue(object component)
		{
			ElementLink elementLink = this.Link;
			return (elementLink != null) ? DomainRoleInfo.GetRolePlayer(elementLink) : null;
		}
#endif
		#endregion // GetValue method
		#region SetValue method
#if VISUALSTUDIO_10_0
		/// <summary>
		/// Sets the new role player specified by <paramref name="value"/> for the associated
		/// <param name="component"/> or creates a new <see cref="ElementLink"/> for
		/// <see cref="RolePlayerPropertyDescriptor.RelationshipInfo"/> between <paramref name="component"/>
		/// and the opposite role player of <see cref="DomainRoleInfo"/> as represented by <param name="value"/>
		/// If <paramref name="value"/> is <see langword="null"/> or not a <see cref="ModelElement"/>,
		/// then the relationship is deleted.
		/// </summary>
		/// <remarks>
		/// Replaces <see cref="RolePlayerPropertyDescriptor.SetValue"/>.
		/// </remarks>
		public override void SetValue(object component, object value)
#else // VISUALSTUDIO_10_0
		/// <summary>
		/// Sets the new role player specified by <paramref name="value"/> for <see cref="RolePlayerPropertyDescriptor.Link"/>,
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
#endif // VISUALSTUDIO_10_0
		{
			if (this.IsReadOnly)
			{
				// We don't make any changes if we're read-only.
				return;
			}

			ModelElement targetPlayer = value as ModelElement;
#if VISUALSTUDIO_10_0
			ModelElement element = EditorUtility.ResolveContextInstance(component, false) as ModelElement;
			if (element == null)
			{
				// Can't continue
				return;
			}
			ElementLink elementLink = GetLink(element);
#else // VISUALSTUDIO_10_0
			if (targetPlayer == null)
			{
				// Defer to ResetValue.
				this.ResetValue(component);
				return;
			}
			ElementLink elementLink = this.Link;
#endif // VISUALSTUDIO_10_0
			Store store = elementLink.Store;
			using (Transaction transaction = this.BeginTransaction(store))
			{
#if VISUALSTUDIO_10_0
				if (elementLink == null)
				{
					if (targetPlayer != null)
					{
						DomainRoleInfo targetRoleInfo = DomainRoleInfo;
						store.ElementFactory.CreateElementLink(this.RelationshipInfo,
							new RoleAssignment(targetRoleInfo.OppositeDomainRole.Id, element),
							new RoleAssignment(targetRoleInfo.Id, targetPlayer));
					}
				}
				else if (targetPlayer == null)
				{
					elementLink.Delete();
				}
#else // VISUALSTUDIO_10_0
				if (elementLink == null)
				{
					DomainRoleInfo targetRoleInfo = DomainRoleInfo;
					store.ElementFactory.CreateElementLink(this.RelationshipInfo,
						new RoleAssignment(targetRoleInfo.OppositeDomainRole.Id, this.SourcePlayer),
						new RoleAssignment(targetRoleInfo.Id, targetPlayer));
				}
#endif // VISUALSTUDIO_10_0
				else
				{
					DomainRoleInfo.SetRolePlayer(elementLink, targetPlayer);
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
