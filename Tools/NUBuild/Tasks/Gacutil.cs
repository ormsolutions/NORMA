using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Neumont.Tools.Build.Tasks
{
	/// <summary>
	/// Wrapper <see cref="ToolTask"/> for <c>gacutil.exe</c>.
	/// </summary>
	public class Gacutil : ToolTask
	{
		/// <summary>See <see cref="ToolTask.GenerateFullPathToTool"/>.</summary>
		protected override string GenerateFullPathToTool()
		{
			return ToolLocationHelper.GetPathToDotNetFrameworkSdkFile(this.ToolName, TargetDotNetFrameworkVersion.VersionLatest);
		}
		/// <summary>See <see cref="ToolTask.ToolName"/>.</summary>
		protected override string ToolName
		{
			get
			{
				return "gacutil.exe";
			}
		}
		/// <summary>See <see cref="ToolTask.GenerateCommandLineCommands"/>.</summary>
		protected override string GenerateCommandLineCommands()
		{
			CommandLineBuilder commandLineBuilder = new CommandLineBuilder();
			commandLineBuilder.AppendSwitch("/nologo");
			if (this.Force)
			{
				commandLineBuilder.AppendSwitch("/f");
			}
			commandLineBuilder.AppendSwitch(this.Uninstall ? "/u" : "/i");
			commandLineBuilder.AppendFileNameIfNotNull(this.Assembly);
			return commandLineBuilder.ToString();
		}

		private bool _force;
		/// <summary>
		/// Corresponds to the <c>/f</c> parameter.
		/// </summary>
		public bool Force
		{
			get
			{
				return this._force;
			}
			set
			{
				this._force = value;
			}
		}

		private bool _uninstall;
		/// <summary>
		/// <see langword="true"/> corresponds to <c>/u</c>.
		/// <see langword="false"/> corresponds to <c>/i</c>.
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

		private string _assembly;
		/// <summary>
		/// If <see cref="Uninstall"/> is <see langword="true"/>, corresponds to <c>assemblyName</c>,
		/// otherwise corresponds to <c>assemblyPath</c>.
		/// </summary>
		[Required]
		public string Assembly
		{
			get
			{
				return this._assembly;
			}
			set
			{
				this._assembly = value;
			}
		}
	}
}
