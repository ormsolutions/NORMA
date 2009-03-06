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
using Microsoft.VisualStudio.Modeling;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	partial class GenerationState
	{
		/// <summary>
		/// Get or create the singleton <see cref="GenerationState"/> for
		/// the provided <see cref="Store"/>. The store must have an active
		/// transaction when this method is called.
		/// </summary>
		public static GenerationState EnsureGenerationState(Store store)
		{
			foreach (GenerationState existingState in store.ElementDirectory.FindElements<GenerationState>())
			{
				return existingState;
			}
			return new GenerationState(store);
		}
	}
}
