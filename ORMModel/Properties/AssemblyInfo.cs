using System;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AssemblyTitle("Neumont.Tools.ORM.dll")]
[assembly: AssemblyProduct("Neumont ORM Architect for Visual Studio")]
[assembly: AssemblyDescription("Neumont ORM Architect for Visual Studio - Package DLL")]
[assembly: AssemblyCompany("Neumont University")]
[assembly: AssemblyCopyright("Copyright © Neumont University. All rights reserved.")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.DisableOptimizations)]
#else
[assembly: AssemblyConfiguration("Release")]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
#endif

