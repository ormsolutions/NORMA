using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyTitle("Neumont.Build.dll")]
[assembly: AssemblyProduct("Neumont Build System")]
[assembly: AssemblyDescription("Neumont Build System - Main DLL")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0.0")]
[assembly: AssemblyVersion(
#if TOOLS_2_0
"2.0.0.0"
#elif TOOLS_3_5
"3.5.0.0"
#elif TOOLS_4_0
"4.0.0.0"
#elif TOOLS_12_0
"12.0.0.0"
#elif TOOLS_14_0
"14.0.0.0"
#elif TOOLS_15_1
"15.1.0.0"
#else
NEW_TOOLS_VERSION
#endif
)]
