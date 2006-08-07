using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyTitle("Neumont.Tools.Build.dll")]
[assembly: AssemblyProduct("Neumont University Build System")]
[assembly: AssemblyDescription("Neumont University Build System - Main DLL")]

[assembly: Dependency("Microsoft.Build.Framework,", LoadHint.Always)]
[assembly: Dependency("Microsoft.Build.Utilities,", LoadHint.Always)]
[assembly: Dependency("Microsoft.Build.Tasks,", LoadHint.Always)]
[assembly: Dependency("System.Xml,", LoadHint.Always)]
