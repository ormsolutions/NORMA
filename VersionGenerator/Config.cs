using System;
using System.Configuration;
using System.Globalization;
using System.Text;

namespace Neumont.Tools.ORM.SDK
{
	internal partial class VersionGenerator
	{
		private static class Config
		{
			public static readonly int MajorVersion = int.Parse(ConfigurationManager.AppSettings["MajorVersion"]);
			public static readonly int MinorVersion = int.Parse(ConfigurationManager.AppSettings["MinorVersion"]);
			public static readonly DateTime ReleaseYearMonth = DateTime.ParseExact(ConfigurationManager.AppSettings["ReleaseYearMonth"], "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault);
			public static readonly string ReleaseType = ConfigurationManager.AppSettings["ReleaseType"];
			public static readonly DateTime RevisionStartYearMonth = DateTime.ParseExact(ConfigurationManager.AppSettings["RevisionStartYearMonth"], "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault);
		}
	}
}
