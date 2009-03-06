using System;
using System.Diagnostics;
using System.Security.Permissions;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

[assembly: CLSCompliant(true)]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AssemblyCompany("ORM Solutions, LLC")]

#if !NO_SECURITYPERMISSIONATTRIBUTE
[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
#endif //!NO_SECURITYPERMISSIONATTRIBUTE

#if !NO_ASSEMBLYCOPYRIGHTATTRIBUTE
[assembly: AssemblyCopyright("Copyright © ORM Solutions, LLC. Portions copyright © Neumont University. All rights reserved.")]
#endif //!NO_ASSEMBLYCOPYRIGHTATTRIBUTE

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints
#if DEBUG
	| DebuggableAttribute.DebuggingModes.DisableOptimizations
#endif //DEBUG
)]

#if !NO_ASSEMBLYCONFIGURATIONATTRIBUTE
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else //!DEBUG
[assembly: AssemblyConfiguration("Release")]
#endif //DEBUG
#endif //!NO_ASSEMBLYCONFIGURATIONATTRIBUTE

[assembly: CompilationRelaxations((CompilationRelaxations)
	((int)CompilationRelaxations.NoStringInterning /*0x8*/ |
	/*CompilationRelaxations.RelaxedArrayExceptions*/ 0x200 |
	/*CompilationRelaxations.RelaxedInvalidCastException*/ 0x80 |
	/*CompilationRelaxations.RelaxedNullReferenceException*/ 0x20 |
	/*CompilationRelaxations.RelaxedOverflowExceptions*/  0x800))]
