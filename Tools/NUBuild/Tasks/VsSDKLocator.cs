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
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Build.Tasks;
using Microsoft.Win32;

namespace Neumont.Build.Tasks
{
	/// <summary>
	/// Obtains installation information and locations for the latest installed version of the Visual Studio SDK.
	/// </summary>
	public class VsSDKLocator : Task
	{
		#region Execute method
		/// <summary>See <see cref="Task.Execute"/>.</summary>
		public override bool Execute()
		{
			const string VsSDKInstallDirEnvironmentVariable = "VsSDKInstall";
			const string VsSDKVersionsRegistryPath = @"SOFTWARE\Microsoft\VisualStudio\VSIP";
			const string VsSDKInstallDirRegistryValue = "InstallDir";
			const string VsSDKIncludeFilesSubdirectory = @"VisualStudioIntegration\Common\Inc";
			const string VsSDKToolsSubdirectory = @"VisualStudioIntegration\Tools\Bin";
			const string VsSDKVsctAssemblyName = "VSCT, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
			const string VsSDKVsctFileName = "VSCT.exe";
			const string VsSDKVsctPrereleasePath = @"Prerelease\VSCT\" + VsSDKVsctFileName;

			TaskLoggingHelper log = base.Log;

			// Try to find the VsSDK installation directory by first checking the environment variable, and then by checking the latest version listed in the registry.
			string installDir = Environment.GetEnvironmentVariable(VsSDKInstallDirEnvironmentVariable);
			if (!string.IsNullOrEmpty(installDir))
			{
				log.LogMessage(MessageImportance.Low, "Using Visual Studio SDK installation directory from \"" + VsSDKInstallDirEnvironmentVariable + "\" environment variable: \"{0}\"", installDir);
			}
			else
			{
				using (RegistryKey vsipRegistryKey = Registry.LocalMachine.OpenSubKey(VsSDKVersionsRegistryPath, RegistryKeyPermissionCheck.ReadSubTree))
				{
					if (vsipRegistryKey != null)
					{
						string[] subKeyNames = vsipRegistryKey.GetSubKeyNames();
						SortedList<Version, string> vsipVersions = new SortedList<Version, string>(subKeyNames.Length);
						foreach (string versionSubKeyName in subKeyNames)
						{
							try
							{
								Version version = new Version(versionSubKeyName);
								vsipVersions.Add(version, versionSubKeyName);
							}
							// Ignore any ArgumentExceptions and FormatExceptions, since Version doesn't have a TryParse method.
							catch (ArgumentException) { }
							catch (FormatException) { }
						}

						int latestVersionIndex = vsipVersions.Count - 1;
						if (latestVersionIndex >= 0)
						{
							string latestVersionSubKeyName = vsipVersions.Values[latestVersionIndex];
							using (RegistryKey latestVersionRegistryKey = vsipRegistryKey.OpenSubKey(latestVersionSubKeyName, RegistryKeyPermissionCheck.ReadSubTree))
							{
								// This will only ever be null if somebody has deleted it since we initially got the list of subkey names, but just in case...
								if (latestVersionRegistryKey != null)
								{
									installDir = latestVersionRegistryKey.GetValue(VsSDKInstallDirRegistryValue, null) as string;
									log.LogMessage(MessageImportance.Low, "Using Visual Studio SDK installation directory from registry for version \"{0}\": \"{1}\"", latestVersionSubKeyName, installDir);
								}
							}
						}
					}
				}
			}

			if (string.IsNullOrEmpty(installDir))
			{
				log.LogError("Unable to find Visual Studio SDK installation directory.");
				return false;
			}

			DirectoryInfo installDirInfo = new DirectoryInfo(installDir);
			if (!installDirInfo.Exists)
			{
				log.LogError("Visual Studio SDK installation directory \"{0}\" does not exist.", installDir);
				return false;
			}
			installDir = this._installationDirectory = installDirInfo.FullName;

			// Get the include files directory
			DirectoryInfo includesDirInfo = new DirectoryInfo(Path.Combine(installDir, VsSDKIncludeFilesSubdirectory));
			if (includesDirInfo.Exists)
			{
				string includesDir = this._includesDirectory = includesDirInfo.FullName;
				log.LogMessage(MessageImportance.Low, "Visual Studio SDK include files directory found at \"{0}\".", includesDir);
			}
			else
			{
				log.LogWarning("Visual Studio SDK include files directory \"{0}\" does not exist.", includesDirInfo.FullName);
			}

			// Get the tools directory
			DirectoryInfo toolsDirInfo = new DirectoryInfo(Path.Combine(installDir, VsSDKToolsSubdirectory));
			if (toolsDirInfo.Exists)
			{
				string toolsDir = this._toolsDirectory = toolsDirInfo.FullName;
				log.LogMessage(MessageImportance.Low, "Visual Studio SDK tools directory found at \"{0}\".", toolsDir);
			}
			else
			{
				log.LogWarning("Visual Studio SDK tools directory \"{0}\" does not exist.", toolsDirInfo.FullName);
			}

			// NOTE: We initially did all of this so that we could determine at run-time where the VSCT assembly was located,
			// but in the end we just GAC'd them, so this isn't strictly needed any more.

			// Get the path to the VSCT assembly
			// First try loading it based on the assembly name
			FileInfo vsctFileInfo = null;
			try
			{
				Assembly vsctAssembly = Assembly.Load(VsSDKVsctAssemblyName);
				vsctFileInfo = new FileInfo(vsctAssembly.Location);
			}
			catch (FileNotFoundException)
			{
				// Ignore, this just means that the runtime couldn't find it
			}
			// We probably don't have to check that the path given for Assembly.Location actually exists, but just in case...
			if (vsctFileInfo == null || !vsctFileInfo.Exists)
			{
				log.LogMessage(MessageImportance.Low, "VSCT assembly not found by Assembly.Load.");

				vsctFileInfo = new FileInfo(Path.Combine(installDir, VsSDKVsctPrereleasePath));
				if (!vsctFileInfo.Exists)
				{
					log.LogMessage("VSCT assembly not found at \"{0}\".", vsctFileInfo.FullName);

					// If VSCT.exe doesn't exist in the Prerelease directory, try the tools directory
					vsctFileInfo = new FileInfo(Path.Combine(toolsDirInfo.FullName, VsSDKVsctFileName));
					if (!vsctFileInfo.Exists)
					{
						log.LogMessage("VSCT assembly not found at \"{0}\".", vsctFileInfo.FullName);

						// If it isn't in the tools directory either, try searching for it everywhere under the VsSDK installation directory
						FileInfo[] foundFiles = installDirInfo.GetFiles(VsSDKVsctFileName, SearchOption.AllDirectories);
						if (foundFiles.Length > 0)
						{
							// UNDONE: Does it matter if we found more than one? Which one should we choose?
							vsctFileInfo = foundFiles[0];
						}
					}
				}
			}
			if (vsctFileInfo.Exists)
			{
				string vsctPath = this._vsctPath = vsctFileInfo.FullName;
				log.LogMessage(MessageImportance.Low, "VSCT assembly found at \"{0}\".", vsctPath);
			}
			else
			{
				log.LogWarning("VSCT assembly could not be found.");
			}

			return true;
		}
		#endregion // Execute method

		#region Properties
		private string _installationDirectory;
		/// <summary>
		/// The directory into which the latest version of the Visual Studio SDK is installed.
		/// </summary>
		[Output]
		public string InstallationDirectory
		{
			get
			{
				return this._installationDirectory;
			}
		}

		private string _toolsDirectory;
		/// <summary>
		/// The tools directory for the latest installed version of the Visual Studio SDK.
		/// </summary>
		[Output]
		public string ToolsDirectory
		{
			get
			{
				return this._toolsDirectory;
			}
		}

		private string _includesDirectory;
		/// <summary>
		/// The include files directory for the latest installed version of the Visual Studio SDK.
		/// </summary>
		[Output]
		public string IncludesDirectory
		{
			get
			{
				return this._includesDirectory;
			}
		}

		private string _vsctPath;
		/// <summary>
		/// The full path to the VSCT assembly for the latest installed version of the Visual Studio SDK.
		/// </summary>
		[Output]
		public string VsctPath
		{
			get
			{
				return this._vsctPath;
			}
		}
		#endregion // Properties
	}
}
