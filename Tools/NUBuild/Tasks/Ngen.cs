using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Neumont.Tools.Build.Tasks
{
	/// <summary>
	/// Wrapper <see cref="ToolTask"/> for <c>ngen.exe</c>.
	/// </summary>
	public class Ngen : ToolTask
	{
		/// <summary>See <see cref="ToolTask.GenerateFullPathToTool"/>.</summary>
		protected override string GenerateFullPathToTool()
		{
			return ToolLocationHelper.GetPathToDotNetFrameworkFile(this.ToolName, TargetDotNetFrameworkVersion.VersionLatest);
		}
		/// <summary>See <see cref="ToolTask.ToolName"/>.</summary>
		protected override string ToolName
		{
			get
			{
				return "ngen.exe";
			}
		}
		/// <summary>See <see cref="ToolTask.GenerateCommandLineCommands"/>.</summary>
		protected override string GenerateCommandLineCommands()
		{
			CommandLineBuilder commandLineBuilder = new CommandLineBuilder();
			commandLineBuilder.AppendSwitchIfNotNull(this.Uninstall ? "uninstall " : "install ", this.AssemblyName);
			if (this.NoDependencies)
			{
				commandLineBuilder.AppendSwitch("/NoDependencies");
			}
			commandLineBuilder.AppendSwitch("/nologo");
			commandLineBuilder.AppendSwitch("/verbose");
			return commandLineBuilder.ToString();
		}

		private bool _noDependencies;
		/// <summary>
		/// Corresponds to the <c>/NoDependencies</c> parameter.
		/// </summary>
		public bool NoDependencies
		{
			get
			{
				return this._noDependencies;
			}
			set
			{
				this._noDependencies = value;
			}
		}

		private bool _uninstall;
		/// <summary>
		/// <see langword="true"/> corresponds to <c>uninstall</c>.
		/// <see langword="false"/> corresponds to <c>install</c>.
		/// </summary>
		public bool Uninstall
		{
			get
			{
				return this._uninstall;
			}
			set
			{
				this._uninstall = value;
			}
		}

		private string _assemblyName;
		/// <summary>
		/// If <see cref="Uninstall"/> is <see langword="true"/>, corresponds to <c>assemblyName</c>,
		/// otherwise corresponds to <c>assemblyPath</c>.
		/// </summary>
		[Required]
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
			set
			{
				this._assemblyName = value;
			}
		}
	}
}
