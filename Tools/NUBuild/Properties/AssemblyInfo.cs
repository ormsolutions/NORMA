using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
#if NET_4_0
[assembly: AssemblyTitle("Neumont.Build.v4.0.dll")]
#else
[assembly: AssemblyTitle("Neumont.Build.dll")]
#endif
[assembly: AssemblyProduct("Neumont Build System")]
[assembly: AssemblyDescription("Neumont Build System - Main DLL")]

[assembly: Dependency("Microsoft.Build.Framework,", LoadHint.Always)]
#if NET_4_0
[assembly: Dependency("Microsoft.Build.Utilities.v4.0,", LoadHint.Always)]
[assembly: Dependency("Microsoft.Build.Tasks.v4.0,", LoadHint.Always)]
#else
[assembly: Dependency("Microsoft.Build.Utilities,", LoadHint.Always)]
[assembly: Dependency("Microsoft.Build.Tasks,", LoadHint.Always)]
#endif
[assembly: Dependency("System.Xml,", LoadHint.Always)]
