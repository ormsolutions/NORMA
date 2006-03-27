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
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;
using Neumont.Tools.ORM.Framework;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region ORMModelElement
	public abstract partial class ORMModelElement : IORMExtendableElement, IModelErrorOwner
	{
		#region Base overrides
		/// <summary>See <see cref="IORMExtendableElement.GetDisplayProperties"/></summary>
		public override PropertyDescriptorCollection GetDisplayProperties(ModelElement requestor, ref PropertyDescriptor defaultPropertyDescriptor)
		{
			return ExtendableElementUtility.MergeExtensionProperties(this, base.GetDisplayProperties(requestor, ref defaultPropertyDescriptor));
		}
		/// <summary>
		/// Get the default accessible name for this element. Defers to GetClassName
		/// </summary>
		public override string AccessibleName
		{
			get
			{
				return GetClassName();
			}
		}
		/// <summary>
		/// Get the default accessible value for this element. Defers to GetComponentName
		/// </summary>
		public override string AccessibleValue
		{
			get
			{
				return GetComponentName();
			}
		}
		#endregion // Base overrides
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements IModelErrorOwner.GetErrorCollection
		/// </summary>
		protected IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0 || (0 != (filter & ModelErrorUses.Verbalize)))
			{
				foreach (object error in GetCounterpartRolePlayers(ORMModelElementHasExtensionModelError.ExtendedElementMetaRoleGuid, ORMModelElementHasExtensionModelError.ExtensionModelErrorCollectionMetaRoleGuid))
				{
					yield return (ModelError)error;
				}
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		/// <summary>
		/// Implements IModelErrorOwner.ValidateErrors (empty implementation)
		/// </summary>
		protected static void ValidateErrors(INotifyElementAdded notifyAdded)
		{
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		/// <summary>
		/// Implements IModelErrorOwner.DelayValidateErrors (empty implementation)
		/// </summary>
		protected static void DelayValidateErrors()
		{
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
	}
	#endregion // ORMModelElement
	#region ORMNamedElement
	public abstract partial class ORMNamedElement : IORMExtendableElement, IModelErrorOwner
	{
		#region Base overrides
		/// <summary>See <see cref="IORMExtendableElement.GetDisplayProperties"/></summary>
		public override PropertyDescriptorCollection GetDisplayProperties(ModelElement requestor, ref PropertyDescriptor defaultPropertyDescriptor)
		{
			return ExtendableElementUtility.MergeExtensionProperties(this, base.GetDisplayProperties(requestor, ref defaultPropertyDescriptor));
		}
		/// <summary>
		/// Get the default accessible name for this element. Defers to GetClassName
		/// </summary>
		public override string AccessibleName
		{
			get
			{
				return GetClassName();
			}
		}
		/// <summary>
		/// Get the default accessible name for this element. Returns the Name.
		/// </summary>
		public override string AccessibleValue
		{
			get
			{
				return Name;
			}
		}
		#endregion // Base overrides
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements IModelErrorOwner.GetErrorCollection
		/// </summary>
		protected IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0 || (0 != (filter & ModelErrorUses.Verbalize)))
			{
				foreach (object error in GetCounterpartRolePlayers(ORMNamedElementHasExtensionModelError.ExtendedElementMetaRoleGuid, ORMNamedElementHasExtensionModelError.ExtensionModelErrorCollectionMetaRoleGuid))
				{
					yield return (ModelError)error;
				}
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		/// <summary>
		/// Implements IModelErrorOwner.ValidateErrors (empty implementation)
		/// </summary>
		protected static void ValidateErrors(INotifyElementAdded notifyAdded)
		{
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		/// <summary>
		/// Implements IModelErrorOwner.DelayValidateErrors (empty implementation)
		/// </summary>
		protected static void DelayValidateErrors()
		{
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
	}
	#endregion // ORMNamedElement
}
