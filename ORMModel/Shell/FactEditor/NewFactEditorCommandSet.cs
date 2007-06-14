using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

using Microsoft.VisualStudio.Modeling.Shell;

namespace Neumont.Tools.ORM.Shell.FactEditor
{
	class NewFactEditorCommandSet : CommandSet, IDisposable
	{
		#region Static Methods
		/// <summary>
		/// Create a command set for the fact editor. This should be called when the package loads
		/// </summary>
		/// <param name="serviceProvider">The package service provider.</param>
		public static CommandSet CreateCommandSet(IServiceProvider serviceProvider)
		{
			return new NewFactEditorCommandSet(serviceProvider);
		}
		#endregion

		#region Private Members
		private List<MenuCommand> m_Commands;
		#endregion

		#region Constructors
		public NewFactEditorCommandSet(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			m_Commands = new List<MenuCommand>();
			// UNDONE: We'd like this to work, but haven't gotten it to fire yet
			//m_Commands.Add(new MenuCommand(
			//                  Test,
			//                  FactEditorCommandIds.AddFactFromEditor
			//              )
			//);
		}
		#endregion

		#region Overriden Methods
		protected override IList<MenuCommand> GetMenuCommands()
		{
			return m_Commands;
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Removes commands from the package.
		/// </summary>
		public void Dispose()
		{
			if (m_Commands != null)
			{
				RemoveCommands(m_Commands);
			}
			m_Commands = null;
		}

		/// <summary>
		/// Called to remove a set of commands. This should be called
		/// by Dispose.
		/// </summary>
		/// <param name="commands">Commands to add</param>
		protected virtual void RemoveCommands(IList<MenuCommand> commands)
		{
			IMenuCommandService menuService = base.MenuService;
			if (menuService != null)
			{
				foreach (MenuCommand menuCommand in commands)
				{
					menuService.RemoveCommand(menuCommand);
				}
			}
		}
		#endregion

		//private void Test(Object sender, EventArgs e)
		//{
		//    System.Diagnostics.Trace.WriteLine(">>>>> Add fact from editor called! <<<<<");
		//}

		public class FactEditorCommandIds
		{
			/// <summary>
			/// The global identifier for the command set used by the fact editor.
			/// keep in sync with SatDll\PkgCmd.ctc
			/// </summary>
			public static readonly Guid GuidFactEditorCommandSet = new Guid("30E071AF-F796-404d-BF87-A645F417791E");

			public static readonly CommandID AddFactFromEditor = new CommandID(GuidFactEditorCommandSet, CmdIdAddFactFromEditor);

			private const int CmdIdAddFactFromEditor = 0x2000;
		}
	}
}
