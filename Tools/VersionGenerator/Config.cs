#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
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
using System.Configuration;
using System.Globalization;
using System.Text;

namespace Neumont.Tools.ORM.SDK
{
	internal static partial class VersionGenerator
	{
		private static class Config
		{
			public static readonly int MajorVersion = int.Parse(ConfigurationManager.AppSettings["MajorVersion"]);
			public static readonly int MinorVersion = int.Parse(ConfigurationManager.AppSettings["MinorVersion"]);
			public static readonly DateTime ReleaseYearMonth = DateTime.ParseExact(ConfigurationManager.AppSettings["ReleaseYearMonth"], "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault);
			public static readonly string ReleaseType = ConfigurationManager.AppSettings["ReleaseType"];
			public static readonly DateTime RevisionStartYearMonth = DateTime.ParseExact(ConfigurationManager.AppSettings["RevisionStartYearMonth"], "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault);
			private static readonly string CountQuartersFromYearMonthString = ConfigurationManager.AppSettings["CountQuartersFromYearMonth"];
			public static readonly DateTime CountQuartersFromYearMonth = string.IsNullOrEmpty(CountQuartersFromYearMonthString) ? DateTime.Today.AddYears(1) : DateTime.ParseExact(CountQuartersFromYearMonthString, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault);
		}
	}
}
