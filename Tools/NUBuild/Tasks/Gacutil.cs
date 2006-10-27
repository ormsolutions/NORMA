#region zlib/libpng Copyright Notice
/**************************************************************************\
* Neumont Build Framework                                                  *
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
