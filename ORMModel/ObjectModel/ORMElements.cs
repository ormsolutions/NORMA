#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
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
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region ORMModelElement
	public abstract partial class ORMModelElement : IORMExtendableElement, IModelErrorOwner, ISurveyNodeReferenceDeletion
	{
		/// <summary>
		/// The <see cref="IORMToolServices"/> for this <see cref="ORMModelElement"/>.
		/// </summary>
		public IORMToolServices ORMToolServices
		{
			get
			{
				return (IORMToolServices)Store;
			}
		}
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements IModelErrorOwner.GetErrorCollection
		/// </summary>
		protected IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0 || (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary))))
			{
				foreach (ModelError modelError in ORMModelElementHasExtensionModelError.GetExtensionModelErrorCollection(this))
				{
					yield return new ModelErrorUsage(modelError);
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
		#region ISurveyNodeReferenceDeletion Implementation
		/// <summary>
		/// Implements <see cref="ISurveyNodeReferenceDeletion.PreserveSurveyNodeReference"/>.
		/// By default, references to elements are preserved if the element is not deleted.
		/// </summary>
		protected bool PreserveSurveyNodeReference(ISurveyNodeReference reference)
		{
			return !this.IsDeleted;
		}
		bool ISurveyNodeReferenceDeletion.PreserveSurveyNodeReference(ISurveyNodeReference reference)
		{
			return PreserveSurveyNodeReference(reference);
		}
		#endregion // ISurveyNodeReferenceDeletion Implementation
	}
	#endregion // ORMModelElement
	#region ORMNamedElement
	public abstract partial class ORMNamedElement
	{
		#region Base overrides
		/// <summary>
		/// Override to use our own name handling
		/// </summary>
		protected override void MergeConfigure(ElementGroup elementGroup)
		{
			// UNDONE: Consider providing an ElementNameProvider tied into the
			// appropriate named element dictionary.
			// Do nothing here. The base calls SetUniqueName, but we want
			// all unique name handling to go through our NamedElementDictionary
		}
		#endregion // Base overrides
	}
	#endregion // ORMNamedElement
}
