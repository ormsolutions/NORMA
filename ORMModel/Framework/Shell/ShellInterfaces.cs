#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Modeling;

namespace ORMSolutions.ORMArchitect.Framework.Shell
{
	#region IFreeFormCommandProvider interface
	/// <summary>
	/// An interface to implement on a selected object to
	/// add free-form commands for that object. Free-form
	/// commands are generally placed at the top of a
	/// context menu. These commands do not have the same
	/// advantages as package commands, such as custom
	/// command placement and hot keys. This is designed as
	/// a very simple facility for adding commands to any selected node.
	/// </summary>
	public interface IFreeFormCommandProvider<ContextType>
		where ContextType : class
	{
		/// <summary>
		/// Get the number of free-form commands support by this object
		/// </summary>
		/// <param name="context">The context <typeparamref name="ContextType"/></param>
		/// <param name="targetElement">The target element to retrieve commands form.</param>
		/// <returns>Total command count</returns>
		int GetFreeFormCommandCount(ContextType context, object targetElement);
		/// <summary>
		/// Provide status for a free-form command. If the command is visible, then
		/// status must include text for the command.
		/// </summary>
		/// <param name="context">The context <typeparamref name="ContextType"/></param>
		/// <param name="targetElement">The target element to retrieve commands for.</param>
		/// <param name="command">The <see cref="MenuCommand"/> instance to provide status settings for.</param>
		/// <param name="commandIndex">The index of the requested command</param>
		void OnFreeFormCommandStatus(ContextType context, object targetElement, MenuCommand command, int commandIndex);
		/// <summary>
		/// Execute the specified command
		/// </summary>
		/// <param name="context">The context <typeparamref name="ContextType"/></param>
		/// <param name="targetElement">The target element to retrieve commands for.</param>
		/// <param name="commandIndex">The index of the requested command</param>
		void OnFreeFormCommandExecute(ContextType context, object targetElement, int commandIndex);
	}
	#endregion // IFreeFormCommandProvider interface
	#region IFreeFormCommandProviderService interface
	/// <summary>
	/// An interface implemented on a <see cref="DomainModel"/>
	/// to enable free-form commands to be added to any selected
	/// node. Provides a mechanism for retrieving an <see cref="IFreeFormCommandProvider{ContextType}"/>
	/// implementation to dynamically add basic commands for a
	/// selected element.
	/// </summary>
	public interface IFreeFormCommandProviderService<ContextType>
		where ContextType : class
	{
		/// <summary>
		/// Retrieve a <see cref="IFreeFormCommandProvider{ContextType}"/> to add commands to
		/// the specified <paramref name="targetElement"/>
		/// </summary>
		/// <param name="context">The context <typeparamref name="ContextType"/></param>
		/// <param name="targetElement">The target element to retrieve a command provider for.</param>
		/// <returns><see cref="IFreeFormCommandProvider{ContextType}"/> that provides commands for the
		/// specified <paramref name="targetElement"/>, or <see langword="null"/></returns>
		IFreeFormCommandProvider<ContextType> GetFreeFormCommandProvider(ContextType context, object targetElement);
	}
	#endregion // IFreeFormCommandProviderService interface
}
