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
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Win32;
using System.Reflection;
#if VISUALSTUDIO_15_0
using Microsoft.VisualStudio.Shell.Interop;
#endif

namespace ORMSolutions.ORMArchitect.ORMCustomTool
{
	public sealed partial class ORMCustomTool
	{
#if VISUALSTUDIO_15_0
		private static Dictionary<string, IORMGenerator> _ormGenerators;
		internal static IDictionary<string, IORMGenerator> GetORMGenerators(IServiceProvider serviceProvider)
		{
			Dictionary<string, IORMGenerator> generators = _ormGenerators;
			if (generators == null)
			{
				generators = new Dictionary<string, IORMGenerator>();
				string registryRoot = null;
				try
				{
					IVsShell shell = serviceProvider.GetService(typeof(SVsShell)) as IVsShell;
					object registryRootObj;
					shell.GetProperty((int)__VSSPROPID.VSSPROPID_VirtualRegistryRoot, out registryRootObj);
					registryRoot = registryRootObj.ToString();
				}
				catch (Exception ex)
				{
					// TODO: Localize message.
					ReportError("WARNING: Exception ocurred while trying to read registry root in ORMCustomTool:", ex);
				}

				if (registryRoot != null)
				{
					LoadGeneratorsFromRoot(Registry.CurrentUser, generators, registryRoot + "_Config");
				}

				if (null != System.Threading.Interlocked.CompareExchange(ref _ormGenerators, generators, null))
				{
					generators = _ormGenerators;
				}
			}
			return generators;
		}
#else
		private static readonly Dictionary<string, IORMGenerator> _ormGenerators;
		// TODO: Change this back to private once the Control has been nested inside us...
		internal static IDictionary<string, IORMGenerator> ORMGenerators
		{
			get
			{
				return _ormGenerators;
			}
		}

		static ORMCustomTool()
		{
			Dictionary<string, IORMGenerator> generators = new Dictionary<string, IORMGenerator>(StringComparer.OrdinalIgnoreCase);
			_ormGenerators = generators;

			LoadGeneratorsFromRoot(Registry.LocalMachine, generators);
			LoadGeneratorsFromRoot(Registry.CurrentUser, generators);
		}
#endif

		private static void LoadGeneratorsFromRoot(RegistryKey rootKey, IDictionary<string, IORMGenerator> generators
#if VISUALSTUDIO_15_0
			, string baseKey
#endif
			)
		{
			RegistryKey generatorsKey = null;
			try
			{
#if VISUALSTUDIO_15_0
				using (RegistryKey localRootKey = rootKey.OpenSubKey(baseKey, RegistryKeyPermissionCheck.ReadSubTree))
				{
					LoadGenerators(generatorsKey = localRootKey.OpenSubKey(GENERATORS_REGISTRYROOT, RegistryKeyPermissionCheck.ReadSubTree), generators, localRootKey);
				}
#else
				LoadGenerators(generatorsKey = rootKey.OpenSubKey(GENERATORS_REGISTRYROOT, RegistryKeyPermissionCheck.ReadSubTree), generators);
#endif
			}
			catch (Exception ex)
			{
				// TODO: Localize message.
				ReportError("WARNING: Exception ocurred while trying to load generators in ORMCustomTool:", ex);
			}
			finally
			{
				if (generatorsKey != null)
				{
					generatorsKey.Close();
				}
			}
		}

		private static void LoadGenerators(RegistryKey generatorsKey, IDictionary<string, IORMGenerator> generators
#if VISUALSTUDIO_15_0
			// Allow resolution of reg: prefixed transform URI's
			, RegistryKey rootKey
#endif
			)
		{
			if (generatorsKey == null)
			{
				return;
			}
			string[] generatorNames = generatorsKey.GetSubKeyNames();
			for (int i = 0; i < generatorNames.Length; i++)
			{
				string generatorName = generatorNames[i];
				RegistryKey generatorKey = null;
				try
				{
					generatorKey = generatorsKey.OpenSubKey(generatorName, RegistryKeyPermissionCheck.ReadSubTree);
					string type = generatorKey.GetValue("Type", null) as string;
					IORMGenerator ormGenerator = null;
					if (String.Equals(type, "XSLT", StringComparison.OrdinalIgnoreCase))
					{
						ormGenerator = new XslORMGenerator(generatorKey
#if VISUALSTUDIO_15_0
							, rootKey
#endif
						);
					}
					else if (String.Equals(type, "Class", StringComparison.OrdinalIgnoreCase))
					{
						ormGenerator = LoadGeneratorClass(generatorKey);
					}

					if (ormGenerator != null)
					{
						System.Diagnostics.Debug.Assert(String.Equals(generatorName, ormGenerator.OfficialName, StringComparison.OrdinalIgnoreCase));
						//ormGenerator.ORMCustomTool = this;
						generators.Add(ormGenerator.OfficialName, ormGenerator);
					}
				}
				catch (Exception ex)
				{
					// TODO: Localize message.
					ReportError("WARNING: Exception ocurred while trying to load generator \"" + generatorName + "\" in ORMCustomTool:", ex);
				}
				finally
				{
					if (generatorKey != null)
					{
						generatorKey.Close();
					}
				}
			}
		}

		private static IORMGenerator LoadGeneratorClass(RegistryKey generatorKey)
		{
			string typeName = generatorKey.GetValue("Class", null) as string;
			string assembly = generatorKey.GetValue("Assembly", null) as string;
			string codeBase = generatorKey.GetValue("CodeBase", null) as string;

			if (string.IsNullOrEmpty(typeName))
			{
				// TODO: Localize message.
				throw new InvalidOperationException("No 'Class' value was specified.");
			}

			AssemblyName assemblyName;
			if (!string.IsNullOrEmpty(assembly))
			{
				assemblyName = new AssemblyName(assembly);
			}
			else
			{
				if (string.IsNullOrEmpty(codeBase))
				{
					// TODO: Localize message.
					throw new InvalidOperationException("Neither an 'Assembly' nor 'CodeBase' value was specified.");
				}
				assemblyName = new AssemblyName();
			}
			assemblyName.CodeBase = codeBase;

			return (IORMGenerator)Activator.CreateInstance(Assembly.Load(assemblyName).GetType(typeName, true, false), generatorKey);
		}
	}
}
