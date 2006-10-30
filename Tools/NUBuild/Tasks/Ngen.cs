#region zlib/libpng Copyright Notice
/**************************************************************************\
* Neumont Build System                                                     *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* This software is provided 'as-is', without any express or implied        *
* warranty. In no event will the authors be held liable for any damages    *
* arising from the use of this software.                                   *
*                                                                          *
* Permission is granted to anyone to use this software for any purpose,    *
* including commercial applications, and to alter it and redistribute it   *
* freely, subject to the following restrictions:                           *
*                                                                          *
* 1. The origin of this software must not be misrepresented; you must not  *
*    claim that you wrote the original software. If you use this software  *
*    in a product, an acknowledgment in the product documentation would be *
*    appreciated but is not required.                                      *
*                                                                          *
* 2. Altered source versions must be plainly marked as such, and must not  *
*    be misrepresented as being the original software.                     *
*                                                                          *
* 3. This notice may not be removed or altered from any source             *
*    distribution.                                                         *
\**************************************************************************/
#endregion

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
		/// Corresponds to <c>assemblyName</c>.
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
