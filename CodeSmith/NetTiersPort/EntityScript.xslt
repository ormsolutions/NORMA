<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:csc="urn:nettiers:CommonSqlCode"
	extension-element-prefixes="csc">
	<msxsl:script implements-prefix="csc" language="CSharp">
		// Enum definitions
		<![CDATA[
		/// <summary>
		/// Indicates the style of Pascal casing to be used
		/// </summary>
		private enum PascalCasingStyle
		{
			/// <summary>
			/// No pascal casing is applied
			/// </summary>
			None,
			
			/// <summary>
			/// Original .NetTiers styling (pre SVN553)
			/// </summary>
			Style1,
			
			/// <summary>
			/// New styling that handles uppercase (post SVN552)
			/// </summary>
			Style2,
		}
		private enum ReturnFields
		{
			EntityName,
			PropertyName,
			FieldName,
			Id,
			CSType,
			FriendlyName
		}

		private enum ClassNameFormat
		{
			None,
			Base,
			Abstract,
			Interface,
			Key,
			Column,
			Comparer,
			EventHandler,
			EventArgs,
			Partial,
			PartialAbstract,
			PartialAbstractService,
			PartialCollection,
			PartialProviderBase,
			PartialUnitTest,
			Service,
			AbstractService,
			Proxy,
			Enum,
			Struct,
			Collection,
			AbstractCollection,
			CollectionProperty,
			ViewCollection,
			Provider,
			ProviderInterface,
			ProviderBase,
			UnitTest,
			Repository,
			AbstractRepository
		}
		]]>
		// Initial settings
		<![CDATA[
		private bool _changeUnderscoreToPascalCase = true;
		public void SetChangeUnderscoreToPascalCase(bool value)
		{
			_changeUnderscoreToPascalCase = value;
		}
		private PascalCasingStyle _usePascalCasing = PascalCasingStyle.Style2;
		public void SetUsePascalCasing(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				_usePascalCasing = (PascalCasingStyle)Enum.Parse(typeof(PascalCasingStyle), value);
			}
		}
		private string _entityFormat = "{0}";
		public void SetEntityFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1)
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "EntityFormat");
				}
				_entityFormat = value;
			}
		}
		private string _entityKeyFormat = "{0}Key";
		public void SetEntityKeyFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "EntityKeyFormat");
				}
				_entityKeyFormat = value;
			}
		}
		private string _entityDataFormat 	= "{0}EntityData";
		public void SetEntityDataFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "EntityDataFormat");
				}
				_entityDataFormat = value;
			}
		}
		private string _collectionFormat = "{0}Collection";
		public void SetCollectionFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "CollectionFormat");
				}
				_collectionFormat = value;
			}
		}
		private string _providerFormat = "{0}Provider";
		public void SetProviderFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "ProviderFormat");
				}
				_providerFormat = value;
			}
		}
		private string _interfaceFormat = "I{0}";
		public void SetInterfaceFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "InterfaceFormat");
				}
				_interfaceFormat = value;
			}
		}
		private string _baseClassFormat = "{0}Base";
		public void SetBaseClassFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "BaseClassFormat");
				}
				_baseClassFormat = value;
			}
		}
		private string _enumFormat = "{0}List";
		public void SetEnumFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "EnumFormat");
				}
				_enumFormat = value;
			}
		}
		private string _manyToManyFormat = "{0}From{1}";
		public void SetManyToManyFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1 || value.IndexOf("{1}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the patterns {0} and {1} to be valid.", "ManyToManyFormat");
				}
				_manyToManyFormat = value;
			}
		}
		private string _serviceClassNameFormat = "{0}Service";
		public void SetServiceClassNameFormat(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (value.IndexOf("{0}") == -1) 
				{
					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "ServiceClassNameFormat");
				}
				_serviceClassNameFormat = value;
			}
		}
		private string _safeNamePrefix = "SafeName_";
		public void SetSafeNamePrefix(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				_serviceClassNameFormat = value;
			}
		}
		private string _genericListFormat = "TList<{0}>";
		private string _genericViewFormat = "VList<{0}>";
		private string _unitTestFormat = "{0}Test";
		private string _strippedTablePrefixes = "tbl;tbl_";
		private string _strippedTableSuffixes= "_t";
		]]>
		// Specific casing queries
		<![CDATA[
		public string GetFieldNameForColumn(string ownerName, string tableName, string columnName)
		{
			return GetAliasName(ownerName, tableName, columnName, ReturnFields.FieldName);
		}
		public string GetClassNameForTable(string ownerName, string tableName, string classNameFormat)
		{
			ClassNameFormat format = ClassNameFormat.None;
			if (!string.IsNullOrEmpty(classNameFormat))
			{
				format = (ClassNameFormat)Enum.Parse(typeof(ClassNameFormat), classNameFormat);
			}
			return GetFormattedClassName(GetAliasName(ownerName, tableName, null, ReturnFields.EntityName), format);
		}
		public string GetPropertyNameForColumn(string ownerName, string tableName, string columnName)
		{
			return GetAliasName(ownerName, tableName, columnName, ReturnFields.PropertyName);
		}
		public string GetManyToManyName(string combinedColumnNames, string ownerName, string tableName)
		{
			return string.Format(_manyToManyFormat, combinedColumnNames, GetClassNameForTable(ownerName, tableName, ""));
		}
		]]>
		// Casing routines
		<![CDATA[
		/// <summary>
		/// Get the camel cased version of a name.  
		/// If the name is all upper case, change it to all lower case
		/// </summary>
		/// <param name="name">Name to be changed</param>
		/// <returns>CamelCased version of the name</returns>
		private string GetCamelCaseName(string name)
		{
			if (name == null)
				return string.Empty;
			// first get the PascalCase version of the name
			string pascalName = GetPascalCaseName(name);
			// now lowercase the first character to transform it to camelCase
			return pascalName.Substring(0, 1).ToLower() + pascalName.Substring(1);
		}

		/// <summary>
		/// Get the Pascal cased version of a name.  
		/// </summary>
		/// <param name="name">Name to be changed</param>
		/// <returns>PascalCased version of the name</returns>
		private string GetPascalCaseName(string name)
		{
			string result = name;
			switch (_usePascalCasing)
			{
				case PascalCasingStyle.Style1 :
					result = GetPascalCaseNameStyle1(name);
					break;
				case PascalCasingStyle.Style2 :
					result = GetPascalCaseNameStyle2(name);
					break;
			}
			return result;
		}
		/// <summary>
		/// Get the Pascal cased version of a name.  
		/// </summary>
		private string GetPascalCaseNameStyle1(string name)
		{
			string[] splitNames;
			name = name.Trim();
			if (_changeUnderscoreToPascalCase)
			{
				char[] splitter = {'_', ' '};
				splitNames = name.Split(splitter);
			}
			else
			{
				char[] splitter =  {' '};
				splitNames = name.Split(splitter);
			}
			
			string pascalName = "";
			foreach (string s in splitNames)
			{
				if (s.Length > 0)
				{
					pascalName += s.Substring(0, 1).ToUpper() + s.Substring(1);
				}
			}

			return pascalName;
		}
		/// <summary>
		/// Gets the pascal case name of a string.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		private string GetPascalCaseNameStyle2(string name)
		{
			string pascalName = string.Empty;
			// UNDONE: a-zA-Z is too restrictive here
			string notStartingAlpha = Regex.Replace(name, "^[^a-zA-Z]+", string.Empty);
			string workingString = ToLowerExceptCamelCase(notStartingAlpha);
			pascalName = RemoveSeparatorAndCapNext(workingString);

			return pascalName;
		}
		/// <summary>
		/// Converts a pascal string to a spaced string
		/// </summary>
		private static string PascalToSpaced(string name)
		{
			// ignore missing text
			if (string.IsNullOrEmpty(name))
				return string.Empty;
			// split the words
			Regex regex = new Regex("(?<=[a-z])(?<x>[A-Z])|(?<=.)(?<x>[A-Z])(?=[a-z])");
			name = regex.Replace(name, " ${x}");
			// get rid of any underscores or dashes
			name = name.Replace("_", string.Empty);
			return name.Replace("-", string.Empty);
		}
		private static string ToLowerExceptCamelCase(string input)
		{
			char[] chars = input.ToCharArray();
			char[] origChars = input.ToCharArray();

			for (int i = 0; i < chars.Length; i++)
			{
				int left = (i > 0 ? i - 1 : i);
				int right = (i < chars.Length - 1 ? i + 1 : i);

				if (i != left &&
						i != right)
				{
					if (Char.IsUpper(chars[i]) &&
							Char.IsLetter(chars[left]) &&
							Char.IsUpper(chars[left]))
					{
						chars[i] = Char.ToLower(chars[i], System.Globalization.CultureInfo.InvariantCulture);
					}
					else if (Char.IsUpper(chars[i]) &&
							Char.IsLetter(chars[right]) &&
							Char.IsUpper(chars[right]) &&
							Char.IsUpper(origChars[left]))
					{
						chars[i] = Char.ToLower(chars[i], System.Globalization.CultureInfo.InvariantCulture);
					}
					else if (Char.IsUpper(chars[i]) &&
							!Char.IsLetter(chars[right]))
					{
						chars[i] = Char.ToLower(chars[i], System.Globalization.CultureInfo.InvariantCulture);
					}
				}

				string x = new string(chars);
			}

			if (chars.Length > 0)
			{
				chars[chars.Length - 1] = Char.ToLower(chars[chars.Length - 1], System.Globalization.CultureInfo.InvariantCulture);
			}

			return new string(chars);
		}
		/// <summary>
		/// Removes the separator and capitalises next character.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		private string RemoveSeparatorAndCapNext(string input)
		{
			char[] splitter = new char[] { '-', '_', ' ' }; // potential chars to split on
			string workingString = input.TrimEnd(splitter);
			char[] chars = workingString.ToCharArray();

			if (chars.Length > 0)
			{
				int under = workingString.IndexOfAny(splitter);
				while (under > -1)
				{
					chars[under + 1] = Char.ToUpper(chars[under + 1], System.Globalization.CultureInfo.InvariantCulture);
					workingString = new String(chars);
					under = workingString.IndexOfAny(splitter, under + 1);
				}

				chars[0] = Char.ToUpper(chars[0], System.Globalization.CultureInfo.InvariantCulture);

				workingString = new string(chars);
			}
			string regexReplacer = "[" + new string(_changeUnderscoreToPascalCase ? new char[] { '-', '_', ' ' } : new char[] { ' ' }) + "]";

			return Regex.Replace(workingString, regexReplacer, string.Empty);
		}
	]]>
		// Class name helpers
		<![CDATA[
		private string GetFormattedClassName(string name, ClassNameFormat format)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			switch (format)
			{
				case ClassNameFormat.None:
					return name;

				case ClassNameFormat.Base:
				case ClassNameFormat.Abstract:
					return string.Format(_baseClassFormat, name);

				case ClassNameFormat.Interface:
					return string.Format("I{0}", name);

				case ClassNameFormat.Key:
					return string.Format(_entityKeyFormat, name);

				case ClassNameFormat.Column:
					return string.Format("{0}Column", name);

				case ClassNameFormat.Comparer:
					return string.Format("{0}Comparer", name);

				case ClassNameFormat.EventHandler:
					return string.Format("{0}EventHandler", name);

				case ClassNameFormat.EventArgs:
					return string.Format("{0}EventArgs", name);

				case ClassNameFormat.Partial:
					return string.Format("{0}.generated", name);

				case ClassNameFormat.PartialAbstract:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.Abstract), ClassNameFormat.Partial);

				case ClassNameFormat.PartialCollection:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.Collection), ClassNameFormat.Partial);

				case ClassNameFormat.PartialProviderBase:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.ProviderBase), ClassNameFormat.Partial);

				case ClassNameFormat.PartialUnitTest:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.UnitTest), ClassNameFormat.Partial);

				case ClassNameFormat.PartialAbstractService:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.AbstractService), ClassNameFormat.Partial);

				case ClassNameFormat.Service:
					return string.Format(_serviceClassNameFormat, name);

				case ClassNameFormat.AbstractService:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.Service), ClassNameFormat.Abstract);

				case ClassNameFormat.Proxy:
					return string.Format("{0}Services", name);

				case ClassNameFormat.Enum:
					return string.Format(_enumFormat, name);

				case ClassNameFormat.Struct:
					return string.Format(_entityDataFormat, name);

				case ClassNameFormat.Collection:
					return string.Format(_genericListFormat, name);

				case ClassNameFormat.AbstractCollection:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.Collection), ClassNameFormat.Abstract);

				case ClassNameFormat.CollectionProperty:
					return string.Format(_collectionFormat, name);

				case ClassNameFormat.ViewCollection:
					return string.Format(_genericViewFormat, name);

				case ClassNameFormat.Provider:
				case ClassNameFormat.Repository:
					return string.Format(_providerFormat, name);

				case ClassNameFormat.AbstractRepository:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.Repository), ClassNameFormat.Abstract);

				case ClassNameFormat.ProviderInterface:
					return string.Format(_interfaceFormat, GetFormattedClassName(name, ClassNameFormat.Provider));

				case ClassNameFormat.ProviderBase:
					return GetFormattedClassName(GetFormattedClassName(name, ClassNameFormat.Provider), ClassNameFormat.Base);

				case ClassNameFormat.UnitTest:
					return string.Format(_unitTestFormat, name);

				default:
					throw new ArgumentOutOfRangeException("format");
			}
		}
		/// <summary>
		/// This function get the alias name for this object name.
		/// </summary>
		/// <remark>This function should not be called directly, but via the GetClassName.</remark>
		private string GetAliasName(string owner, string obj, string item, ReturnFields returnType)
		{
			// UNDONE: Note that this is skipping the NameConversionType values in .NETTiers. This
			// is simply NameConversionType.None
			string name = string.Empty;
			// get the name
			if (!string.IsNullOrEmpty(obj) && string.IsNullOrEmpty(item)) // table/view names
			{
				name = obj;
				char[] delims = new char[] {',', ';'};
				// strip the prefix
				string[] strips = _strippedTablePrefixes.ToLower().Split(delims);
				foreach(string strip in strips)
					if (name.ToLower().StartsWith(strip))
						{
							name = name.Remove(0, strip.Length);
							continue;
						}
				// strip the suffix
				strips = _strippedTableSuffixes.Split(delims);
				foreach(string strip in strips)
				{
					if (name.ToLower().EndsWith(strip))
					{
						name = name.Remove(name.Length - strip.Length, strip.Length);
						continue;
					}
				}
			}
			else if (!string.IsNullOrEmpty(obj) && !string.IsNullOrEmpty(item)) // column names
			{
				name = item;
			}
			else
			{
				throw new ArgumentNullException();
			}

			// return the formatted name
			switch (returnType)
			{
				case ReturnFields.EntityName:
				case ReturnFields.PropertyName:
					name = GetCSharpSafeName(name);
					return GetPascalCaseName(name); // class and property names are pascal-cased
				case ReturnFields.FieldName:
					name = GetCSharpSafeName(name);
					return GetCamelCaseName(name); // fields (private member variables) are camel-cased
				case ReturnFields.FriendlyName:
					return PascalToSpaced(GetPascalCaseName(name)); // just return the pascal name with spaces
				case ReturnFields.Id:
				case ReturnFields.CSType:
				default:
					return string.Empty; // what should happen here, exactly?
			}
		}
		/// <summary>
		/// Gets a C Sharp safe version of the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		private string GetCSharpSafeName( string name )
		{
			string result = name;

			// we must have something to start with!
			if (!IsValidCSharpName( result ))
			{
				result = _safeNamePrefix + result;

				// replace any non valid char with an underscore
				// UNDONE: a-zA-Z0-9 is too restrictive here
				result = Regex.Replace( result, "[^a-zA-Z0-9_]", "_" );
			}

			return result;
		}
		private static string[] _csharpKeywords = PopulateCSharpKeywords();
		private static string[] PopulateCSharpKeywords()
		{
			string[] names = new string[] 
			{
					"abstract","event", "new", "struct", 
					"as", "explicit", "null", "switch",
					"base", "extern", "object", "this",
					"bool", "false", "operator", "throw",
					"break", "finally", "out", "true",
					"byte", "fixed", "override", "try",
					"case", "float", "params", "typeof",
					"catch", "for", "private", "uint",
					"char", "foreach", "protected", "ulong",
					"checked", "goto", "public", "unchecked",
					"class", "if", "readonly", "unsafe",
					"const", "implicit", "ref", "ushort",
					"continue","in","return","using",
					"decimal","int","sbyte","virtual",
					"default","interface","sealed","volatile",
					"delegate","internal","short","void",
					"do","is","sizeof","while",
					"double","lock","stackalloc",
					"else","long","static",
					"enum","namespace", "string"
			};
			Array.Sort(names, CaseInsensitiveComparer.DefaultInvariant);
			return names;
		}
		/// <summary>
		/// Determines whether specified name is valid in C#.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>
		/// 	<c>true</c> if the name is valid; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsValidCSharpName( string name )
		{
			// we assume that the name is invalid
			bool result = false;

			// we must have something to start with!
			if (!string.IsNullOrEmpty(name))
			{
				// the first char must not be a digit
				if (!char.IsDigit(name, 0))
				{
					// check if its a reserved C# keyword
					// Note this is changed from the .nettiers codebase. There is no need to use IndexOf here.
					if (Array.BinarySearch(_csharpKeywords, name, CaseInsensitiveComparer.DefaultInvariant) < 0)
					{
						// only letters, digits and underscores are allowed
						// we're also allowing spaces and dashes as the 
						// user has the option of suppressing those
						// UNDONE: a-zA-Z0-9 is too restrictive here
						Regex validChars = new Regex(@"[^a-zA-Z0-9_\s-]");
						result = !validChars.IsMatch(name);
					}
				}
			}
			return result;
		}
		]]>
	</msxsl:script>
</xsl:stylesheet>
