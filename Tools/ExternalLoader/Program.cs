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
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.Load;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

/*
This is an example of using the NORMAResolver class to enable use of the NORMA
assemblies outside of the local Visual Studio installation. In order to do this
the methods that use types from the NORMA assembly cannot be invoked until a
NORMAResolver instance exists. Any class that is instantiated before the resolver
runs also cannot have fields that are typed in the NORMA assemblies or the Visual
Studio assemblies it relies on.

After NORMA is updated make sure you run Visual Studio once. The actual changes to
the registry this is reading appear to be made when Visual Studio launches, not when
the VSIXInstaller completes.

The NORMAResolver class can be dropped into any project that needs to bind to a NORMA installation.
However, this project (including NORMAResolver.cs) is designed to be used within the NORMA build system
just like other extensions. If you have this file then you likely have the full git source as well. To emulate:
1) Even if you don't do a full NORMA build you still need to run the BuildDevToolsVS20xx.bat
   to establish the referenced multi-targeting imports used in the project file. If you are using
   a setup build make sure you run just this batch file (after the VS20xx batch file in the VS20xx
   developer command prompt to establish your environment. You'll need an admin prompt to build dev tools.).
2) If you wish to use a setup install of NORMA then you can run NORMAAssembliesFromSetup.bat to
   back copy the current files from a NORMA installation into the build directory.
3) If you are using an installed NORMA, be sure to set the NORMAOfficial=1 environment variable
   so you reference the correct assembly. Projects should always be opened with 'devenv ExternalLoader.csproj'
   from the command line after your environment is set.
3) Your project file should run as a subdirectory of the NORMA source code, just like this one. There
   are two references to the NORMA root directory that must be adjusted in the project file for everything to
   work smoothly. If you are not two-levels deep then you need to adjust NORMATrunkDir near the top and
   the reference to the core assembly (look for ORMModel in the project file). You might want to reference
   NORMAResolver.cs using a relative path as well so you can be assured of getting changes.
4) If you need to add other references I recommend adding them by hand in the project file XML. Right
   click on the project node to unload the project so you can edit it, right click again to edit, then
   right click to reload when you're done. You can base these references on an existing NORMA project file
   (ORMModel\ORMModel.csproj will have most of the SDK references).
5) Given that the whole point of this exercise is to NOT rely on locally installed files I recommend
   using the <Private>False</Private> tag inside your any reference you add. This corresponds to the
   CopyLocal=False property on a reference in the IDE and indicates that your program can resolve assemblies
   without copying them with the program.
6) Note that DefineConstants needs to emulate the pattern shown in this project file so values from the
   imported target files are not lost.
7) This sample takes two arguments (a file name and a contained object type name). You can set these in the
   Debug tab of the Project Properties (right click on the project in Solution Explorer and click Properties).
   Use the "Command Line Arguments" field to identify your inputs.
*/

namespace ORMSolutions.ORMArchitect.Utility.NORMAResolver
{
	/// <summary>
	/// Sample program using the NORMAResolver class and NORMA-build-integrated project settings.
	/// </summary>
	class Program
	{
		static void Main(string[] args)
		{
			DeferredMain(new NORMAResolver(true), args);
		}
		private static void DeferredMain(NORMAResolver resolver, string[] args)
		{
			// Sample code, provide an .orm file and an object type name and return the verbalization
			// for that object type (with no hyperlinks and no fact type list)
			ExtensionLoader extensions = resolver.ExtensionLoader;

			if (args.Length != 2)
			{
				Console.WriteLine("Provide the name of an .ORM file and an object type name.");
			}
			else if (!File.Exists(args[0]))
			{
				Console.WriteLine("File '" + args[0] + "' not found.");
			}
			else
			{
				// VerbalizationManager copy gives us clean options (shown for demonstration purposes, customizing the original is fine too.)
				VerbalizationManager customVerbalizer = new VerbalizationManager(resolver.VerbalizationManager);
				customVerbalizer.CustomSnippetsIdentifiers.Add(new VerbalizationSnippetsIdentifier(typeof(CoreVerbalizationSnippetType), "VerbalizationBrowser", "en-US", "NoHyperlinks"));
				customVerbalizer.CustomOptions.Add(CoreVerbalizationOption.ObjectTypeNameDisplay, ObjectTypeNameVerbalizationStyle.SeparateCombinedNames);
				customVerbalizer.CustomOptions.Add(CoreVerbalizationOption.FactTypesWithObjectType, false);

				// Use the non-generative loader (the true argument) for verbalization to skip loading diagrams and other
				// models that are used for editor support.
				ModelLoader loader = new ModelLoader(resolver.ExtensionLoader, customVerbalizer, true);
				Console.WriteLine("Loading model from '" + args[0] + "'.");
				Store store = loader.Load(args[0]);
				var model = store.ElementDirectory.FindElements<ORMModel>().First();
				var objectType = model.ObjectTypesDictionary.GetElement(args[1]).FirstElement;
				if (objectType == null)
				{
					Console.WriteLine("Object type '" + args[1] + "' not found.");
				}
				else
				{
					StringBuilder sb = new StringBuilder();
					StringWriter writer = new StringWriter(sb);
					customVerbalizer.Verbalize(store, writer, "VerbalizationBrowser", new object[] { objectType }, null /*BlockVerbalizeExcept<ObjectType>.Instance*/, false);
					if (sb.Length != 0)
					{
						Console.WriteLine(sb.ToString());
					}
					else
					{
						Console.WriteLine("No verbalization returned.");
					}
				}
			}
			Console.Write("Press any key to continue...");
			Console.ReadKey();
		}

		/// <summary>
		/// Example of a child filter
		/// </summary>
		/// <typeparam name="T"></typeparam>
		private sealed class BlockVerbalizeExcept<T> : IVerbalizeFilterChildren
			where T : class, IVerbalize
		{
			public static IVerbalizeFilterChildren Instance = new BlockVerbalizeExcept<T>();
			private BlockVerbalizeExcept() { }
			CustomChildVerbalizer IVerbalizeFilterChildren.FilterChildVerbalizer(object child, VerbalizationSign sign)
			{
				T typedChild = child as T;
				return typedChild != null ? CustomChildVerbalizer.VerbalizeInstance(typedChild) : CustomChildVerbalizer.Block;
			}
		}
	}
}
