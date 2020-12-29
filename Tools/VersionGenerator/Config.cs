#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
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
#endregion

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;

namespace Neumont.Tools.ORM.SDK
{
	internal static partial class VersionGenerator
	{
		private static class Config
		{
			// Pull in data on first access, not readonly static, to allow for dynamic config swapping
			private static Configuration _customConfig;
			private static bool _init;
			private static int _majorVersion;
			private static int _minorVersion;
			private static DateTime _releaseYearMonth;
			private static string _releaseType;
			private static DateTime _revisionStartYearMonth;
			private static DateTime _countQuartersFromYearMonth;
			private static string _gitCommand;
			private static string _gitCommandArgs;

			private static void EnsureInit()
			{
				if (!_init)
				{
					_init = true;
					NameValueCollection defaultSettings;
					KeyValueConfigurationCollection customSettings;
					if (_customConfig != null)
					{
						defaultSettings = null;
						customSettings = _customConfig.AppSettings.Settings;
					}
					else
					{
						defaultSettings = ConfigurationManager.AppSettings;
						customSettings = null;
					}
					_majorVersion = int.Parse(GetSetting("MajorVersion", defaultSettings, customSettings));
					_minorVersion = int.Parse(GetSetting("MinorVersion", defaultSettings, customSettings));
					_releaseYearMonth = DateTime.ParseExact(GetSetting("ReleaseYearMonth", defaultSettings, customSettings), "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault);
					_releaseType = GetSetting("ReleaseType", defaultSettings, customSettings);
					_revisionStartYearMonth = DateTime.ParseExact(GetSetting("RevisionStartYearMonth", defaultSettings, customSettings), "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault);
					string countQuartersFromYearMonthString = GetSetting("CountQuartersFromYearMonth", defaultSettings, customSettings);
					_countQuartersFromYearMonth = string.IsNullOrEmpty(countQuartersFromYearMonthString) ? DateTime.Today.AddYears(1) : DateTime.ParseExact(countQuartersFromYearMonthString, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault);
					_gitCommand = GetSetting("GitCommand", defaultSettings, customSettings);
					_gitCommandArgs = GetSetting("GitCommandArgs", defaultSettings, customSettings);
				}
			}
			private static string GetSetting(string name, NameValueCollection defaultSettings, KeyValueConfigurationCollection customSettings)
			{
				return defaultSettings != null ? defaultSettings[name] : customSettings[name].Value;
			}

			public static Configuration CustomConfig
			{
				get
				{
					return _customConfig;
				}
				set
				{
					_init = false;
					_customConfig = value;
				}
			}

			public static int MajorVersion
			{
				get
				{
					EnsureInit();
					return _majorVersion;
				}
			}

			public static int MinorVersion
			{
				get
				{
					EnsureInit();
					return _minorVersion;
				}
			}

			public static DateTime ReleaseYearMonth
			{
				get
				{
					EnsureInit();
					return _releaseYearMonth;
				}
			}

			public static string ReleaseType
			{
				get
				{
					EnsureInit();
					return _releaseType;
				}
			}

			public static DateTime RevisionStartYearMonth
			{
				get
				{
					EnsureInit();
					return _revisionStartYearMonth;
				}
			}

			public static DateTime CountQuartersFromYearMonth
			{
				get
				{
					EnsureInit();
					return _countQuartersFromYearMonth;
				}
			}

			public static string GitCommand
			{
				get
				{
					EnsureInit();
					return _gitCommand;
				}
			}
			public static string GitCommandArgs
			{
				get
				{
					EnsureInit();
					return _gitCommandArgs;
				}
			}
		}
		private class TemplateConfigurationSection : ConfigurationSection
		{
			[ConfigurationProperty("last")]
			public string LastGenerated
			{
				get { return this["last"] as string; }
				set { this["last"] = value; }
			}

			[ConfigurationProperty("templates")]
			[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "configuration element collections cannot be read-only")]
			[ConfigurationCollection(typeof(TemplateCollection))]
			public TemplateCollection Templates
			{
				get { return this["templates"] as TemplateCollection; }
				set { this["templates"] = value; }

			}
		}
		private class TemplateCollection : ConfigurationElementCollection
		{
			public override ConfigurationElementCollectionType CollectionType
			{
				get { return ConfigurationElementCollectionType.BasicMap; }
			}

			protected override bool IsElementName(string elementName)
			{
				return elementName == "template";
			}

			protected override ConfigurationElement CreateNewElement()
			{
				throw new InvalidOperationException("Unspecified element name.");
			}

			protected override ConfigurationElement CreateNewElement(string elementName)
			{
				switch (elementName)
				{
					case "template":
						return new TemplateElement();

					default:
						throw new InvalidOperationException("Unknown element name.");
				}
			}

			protected override object GetElementKey(ConfigurationElement element)
			{
				TemplateElement template = element as TemplateElement;
				if (template == null)
				{
					throw new InvalidOperationException(string.Format("Cannot derive key for element: {0}", element.GetType().FullName));
				}
				return template.Output;
			}
		}
		private class TemplateElement : ConfigurationElement
		{
			private string _content;
			[ConfigurationProperty("output")]
			public string Output
			{
				get { return this["output"] as string; }
				set { this["output"] = value; }
			}
			[ConfigurationProperty("major")]
			public int Major
			{
				get
				{
					int result;
					return int.TryParse(this["major"] as string, out result) ? result : -1;
				}
				set { this["major"] = value.ToString(); }
			}
			[ConfigurationProperty("minor")]
			public int Minor
			{
				get
				{
					int result;
					return int.TryParse(this["minor"] as string, out result) ? result : -1;
				}
				set { this["minor"] = value.ToString(); }
			}
			[ConfigurationProperty("build")]
			public int Build
			{
				get
				{
					int result;
					return int.TryParse(this["build"] as string, out result) ? result : -1;
				}
				set { this["build"] = value.ToString(); }
			}
			[ConfigurationProperty("revision")]
			public int Revision
			{
				get
				{
					int result;
					return int.TryParse(this["revision"] as string, out result) ? result : -1;
				}
				set { this["revision"] = value.ToString(); }
			}
			[ConfigurationProperty("hash")]
			public int Hash
			{
				get
				{
					int result;
					return int.TryParse(this["hash"] as string, out result) ? result : -1;
				}
				set { this["hash"] = value.ToString(); }
			}
			[ConfigurationProperty("yearMonth")]
			public int YearMonth
			{
				get
				{
					int result;
					return int.TryParse(this["yearMonth"] as string, out result) ? result : -1;
				}
				set { this["yearMonth"] = value.ToString(); }
			}
			[ConfigurationProperty("buildType")]
			public int BuildType
			{
				get
				{
					int result;
					return int.TryParse(this["buildType"] as string, out result) ? result : -1;
				}
				set { this["buildType"] = value.ToString(); }
			}
			[ConfigurationProperty("warning")]
			public int Warning
			{
				get
				{
					int result;
					return int.TryParse(this["warning"] as string, out result) ? result : -1;
				}
				set { this["warning"] = value.ToString(); }
			}
			public string Content
			{
				get { return _content; }
				set { _content = value; }
			}
			protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
			{
				//First get the attributes
				for (int i = 0, count = reader.AttributeCount; i < count; ++i)
				{
					reader.MoveToAttribute(i);
					this[reader.Name] = reader.Value;
				}
				reader.MoveToElement();
				_content = reader.ReadElementContentAsString();
			}
		}
	}
}
