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
			public const int ORMFileBaseName = 104;
			public const int OptionsCategory = 105;
			public const int OptionsGeneral = 106;
			public const int ORMProjectItems = 107;
			public const int EditorName = 108;
			public const int FactEditorName = 109;

			public const int AboutBoxIcon = 110;
			public const int ORMFileIcon = 111;

			// Corresponds to IDB_VERBALIZATIONTOOLBARIMAGES in PkgCmd.vsct.
			public const int VerbalizationToolbarImages = 124;
			// Corresponds to IDB_TOOLWINDOWICONS in PkgCmd.vsct.
			public const int ToolWindowIcons = 125;
			// Corresponds to IDB_TOOLWINDOWICONS32 in PkgCmd.vsct.
			public const int ToolWindowIcons32 = 126;

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
		}
	}
}
