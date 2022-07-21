#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
* can be found in the file CPL.txt at the root of this distribution.       *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/
# endregion

using System;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using ORMSolutions.ORMArchitect.Core.Load;
using System.Runtime.InteropServices;

namespace ORMSolutions.ORMArchitect.Utility
{
	/// <summary>
	/// Locate a NORMA installation and resolve necessary assembly locations to
	/// load NORMA files using the current Visual Studio installation, but from
	/// outside Visual Studio.
	/// </summary>
	internal class NORMAResolver
	{
		#region P/Invoke Declarations
		private static class WinDecl
		{
			[Flags]
			public enum RegSAM
			{
				KEY_ALL_ACCESS = 0xF003F,
				KEY_CREATE_LINK = 0x0020,
				KEY_CREATE_SUB_KEY = 0x0004,
				KEY_ENUMERATE_SUB_KEYS = 0x0008,
				KEY_EXECUTE = 0x20019,
				KEY_NOTIFY = 0x0010,
				KEY_QUERY_VALUE = 0x0001,
				KEY_READ = 0x20019,
				KEY_SET_VALUE = 0x0002,
				KEY_WOW64_32KEY = 0x0200,
				KEY_WOW64_64KEY = 0x0100,
				KEY_WRITE = 0x20006
			}

			[DllImport("advapi32.dll", SetLastError = true)]
			public static extern int RegLoadAppKey(String hiveFile, out IntPtr hKey, RegSAM samDesired, int options, int reserved);
		}
		#endregion // P/Invoke Declarations

		/// <summary>
		/// Directory prefix for VS data, appended to the local application directory.
		/// </summary>
		private const string VSDataDirPattern =
#if VISUALSTUDIO_17_0
			@"17.0_*";
#elif VISUALSTUDIO_16_0
			@"16.0_*";
#elif VISUALSTUDIO_15_0
			@"15.0_*";
#endif

		/// <summary>
		/// Directory prefix for VS data, appended to the local application directory.
		/// </summary>
		private const string NORMAVSVersionSuffix =
#if VISUALSTUDIO_17_0
			@"VS2022";
#elif VISUALSTUDIO_16_0
			@"VS2019";
#elif VISUALSTUDIO_15_0
			@"VS2017";
#endif

		private const string NORMAPublicKeyToken =
#if NORMA_Official
			"fd9102b3e2835fba";
#else
			"957d5b7d5e79e25f";
#endif

		private static string NORMACoreAssemblyName = "ORMSolutions.ORMArchitect.Core." + NORMAVSVersionSuffix + ", Version=1.0.0.0, Culture=neutral, PublicKeyToken=" + NORMAPublicKeyToken;

		// Fields are untyped so the types don't resolve when the class is created.
		private object myExtensionLoader = null;
		private object myVerbalizationManager = null;

		/// <summary>
		/// Find the NORMA installation to get an <see cref="ExtensionLoader"/> and (optionally)
		/// <see cref="VerbalizationManager"/>
		/// </summary>
		/// <param name="initializeVerbalization">The <see cref="VerbalizationManager"/> property will be initialized.</param>
		public NORMAResolver(bool initializeVerbalization = false)
		{
			string[] directories = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Microsoft\VisualStudio", VSDataDirPattern);
			int directoryCount = directories != null ? directories.Length : 0;
			if (directoryCount != 0)
			{
				if (directoryCount > 1)
				{
					Array.Sort<string>(directories, (left, right) =>
					{
						int lengthCompare = left.Length - right.Length;
						return lengthCompare == 0 ? String.Compare(left, right, StringComparison.OrdinalIgnoreCase) : lengthCompare;
					});
				}
				for (int i = 0; i < directoryCount; ++i)
				{
					if (TryLoadNORMA(directories[i], normaRegKey =>
					{
						// Initialization is in a separate function so that the assembly resolver is in place before the type is used.
						LoadNORMA(normaRegKey, initializeVerbalization);
					}))
					{
						break;
					}
				}
			}
		}

		/// <summary>
		/// Get the <see cref="ExtensionLoader"/> class
		/// </summary>
		public ExtensionLoader ExtensionLoader
		{
			get
			{
				return (ExtensionLoader)myExtensionLoader;
			}
		}

		/// <summary>
		/// Get the <see cref="VerbalizationManager"/> instance
		/// </summary>
		/// <remarks>This will be null if the initializeVerbalization flag on
		/// the constructor was not set.</remarks>
		public VerbalizationManager VerbalizationManager
		{
			get
			{
				return (VerbalizationManager)myVerbalizationManager;
			}
		}

		/// <summary>
		/// Do the actual loading. This is called after the assembly resolver is in place.
		/// </summary>
		/// <param name="NORMASettings">Key corresponding to the "$RootKey$\ORM Solutions\Natural ORM Architect" key in the VS registry.</param>
		/// <param name="initializeVerbalization">Load a VerbalizationManager while we have the NORMA registry key open.</param>
		private void LoadNORMA(RegistryKey NORMASettings, bool initializeVerbalization)
		{
			myExtensionLoader = new ExtensionLoader(ExtensionModelData.LoadFromRegistry(@"Designer\Extensions", NORMASettings, NORMASettings)); // Same key twice means to ignore second key
			if (initializeVerbalization)
			{
				myVerbalizationManager = VerbalizationManager.LoadFromRegistry(@"Designer\DesignerSettings", null, NORMASettings, NORMASettings);
			}
		}

		private static bool TryLoadNORMA(string VSUserDir, Action<RegistryKey> loadFrom)
		{
			IntPtr hKey;
			int hresult = WinDecl.RegLoadAppKey(VSUserDir + @"\privateregistry.bin", out hKey, WinDecl.RegSAM.KEY_READ, 0, 0);
			if (hresult == 0)
			{
				using (var regRoot = RegistryKey.FromHandle(new SafeRegistryHandle(hKey, true)))
				{
					using (var configRoot = regRoot.OpenSubKey(string.Format(@"Software\Microsoft\VisualStudio\{0}_Config", new DirectoryInfo(VSUserDir).Name)))
					{
						if (configRoot != null)
						{
							using (var NORMAPackageKey = configRoot.OpenSubKey(@"Packages\{efddc549-1646-4451-8a51-e5a5e94d647c}"))
							{
								if (NORMAPackageKey != null)
								{
									string package = NORMAPackageKey.GetValue("CodeBase") as string;
									if (!string.IsNullOrEmpty(package) && File.Exists(package) && AssemblyName.GetAssemblyName(package).FullName == NORMACoreAssemblyName)
									{
										using (var NORMAConfig = configRoot.OpenSubKey(@"ORM Solutions\Natural ORM Architect"))
										{
											if (NORMAConfig != null)
											{
												string VSInstall = (string)configRoot.GetValue("InstallDir"); // Has trailing backslash
												string[] probeDirs = new string[]
												{
													// The normal case is only the private assemblies is hit, but adding other directories is reasonable.
													VSInstall + @"PrivateAssemblies\",
													VSInstall + @"PublicAssemblies\",
													new FileInfo(package).Directory.FullName + @"\"
												};
												AppDomain.CurrentDomain.AssemblyResolve += (object sender, ResolveEventArgs args) =>
												{
													string assemblyName = args.Name;
													if (assemblyName == NORMACoreAssemblyName)
													{
														return Assembly.LoadFrom(package);
													}
													string fileName = new AssemblyName(args.Name).Name + ".dll";
													for (int i = 0; i < probeDirs.Length; ++i)
													{
														string probeFile = probeDirs[i] + fileName;
														if (File.Exists(probeFile))
														{
															return Assembly.LoadFrom(probeFile);
														}
													}
													return null;
												};
												loadFrom(NORMAConfig);
												return true;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return false;
		}
	}
}
