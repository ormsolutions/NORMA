#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © Matthew Curland. All rights reserved.                        *
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
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;

namespace Neumont.Tools.Modeling
{
	#region IFrameworkServices interface
	/// <summary>
	/// An interface that should be implemented by a <see cref="Store"/> that
	/// loads the domain models. This is meant to act as a base interface for
	/// tool-specific interfaces that derive from it.
	/// </summary>
	public interface IFrameworkServices
	{
		/// <summary>
		/// Retrieve the <see cref="IPropertyProviderService"/> service for registering
		/// and unregistering <see cref="PropertyProvider"/> delegates to enable extension
		/// properties to be added to any timeof element. Can be implemented using the
		/// <see cref="T:PropertyProviderService"/> class.
		/// </summary>
		IPropertyProviderService PropertyProviderService { get;}
		/// <summary>
		/// Retrieve the <see cref="INotifySurveyElementChanged"/> interface for this store.
		/// Can be implemented using an instance of the <see cref="SurveyTree"/> class which
		/// implements this interface.
		/// </summary>
		INotifySurveyElementChanged NotifySurveyElementChanged { get;}
	}
	#endregion // IFrameworkServices interface
	#region IRepresentedModelElements interface
	/// <summary>
	/// General mechanism to retrieve a ModelElement associated
	/// with an arbitrary object.
	/// </summary>
	public interface IRepresentModelElements
	{
		/// <summary>
		/// Retrieve the model element associated with this item.
		/// </summary>
		ModelElement[] GetRepresentedElements();
	}
	#endregion // IRepresentedModelElements interface
}
