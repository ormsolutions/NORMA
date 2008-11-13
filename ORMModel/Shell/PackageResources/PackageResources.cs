namespace Neumont.Tools.ORM.Shell
{
	internal static class PackageResources
	{
		/// <summary>
		/// Provides access to the resource IDs for various package resources.
		/// </summary>
		public static class Id
		{
			// Keep in sync with VSPackage.resx.

			public const int ORMFileDisplayName = 102;
			public const int ORMFileDescription = 103;
			// 104 is available
			public const int OptionsCategory = 105;
			public const int OptionsGeneral = 106;
			// 107 is available
			public const int EditorName = 108;
			public const int FactEditorName = 109;

			public const int AboutBoxIcon = 110;
			public const int ORMFileIcon = 111;

			// Favor for the DatabaseImport project, the resources
			// need to be in a package.
			// public const int ORMFileDBImportDisplayName = 112;
			// public const int ORMFileDBImportDescription = 113;

			public const int ToolWindowIcons = 125;

			public const int PackageLoadKey = 150;

			public const int CTMenu = 1000;
		}

		/// <summary>
		/// Provides access to the indexes for tool window icons.
		/// </summary>
		public static class ToolWindowIconIndex
		{
			public const int None = -1;

			// Keep in sync with PkgCmd.vsct. Note that the values here are the values from PkgCmd.vsct minus one.

			public const int VerbalizationBrowser = 0;
			public const int ReadingEditor = 1;
			public const int ReferenceModeEditor = 2;
			public const int FactEditor = 3;
			public const int ModelBrowser = 4;
			public const int NotesEditor = 5;
			public const int PopulationEditor = 6;
			public const int ContextWindow = 7;
			public const int DefinitionEditor = 8;
			public const int DiagramSpy = 9;
		}
	}
}
