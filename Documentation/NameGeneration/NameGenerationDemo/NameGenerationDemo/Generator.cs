using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

namespace WindowsApplication2
{
	public static class Generator
	{
		static NORMA_Name_Generation options;
		internal static DataSet GenerateNames(NORMA_Name_Generation options)
		{
			Generator.options = options;

			columnAbbreviations = GetAbbreviations(true);
			tableAbbreviations = GetAbbreviations(false);

			DataSet ds = new DataSet();
			CreateEmployeeTable(ds);
			CreateProgramTable(ds);
			CreateProgramIsAlsoOnCampusTable(ds);
			CreateRoomTable(ds);

			return ds;
		}

		private static void CreateRoomTable(DataSet ds)
		{
			DataTable Room = new DataTable("Room");

			CreateColumn(Room, DoBuilding(), true, true, false);
			CreateColumn(Room, DoCampus(), true, true, false);
			CreateColumn(Room, DoRoom(), true, true, false);
			CreateColumn(Room, PredicateTextFilter("Has Window"), false, false, false);

			Room.TableName = FinalizeName(Room.TableName, false);

			AddTable(ds, Room);
		}

		private static void AddTable(DataSet ds, DataTable table)
		{
		TryAgain:
			try
			{
				ds.Tables.Add(table);
			}
			catch (DuplicateNameException)
			{
				string name = table.TableName;
				string n2 = EnsureDiff(name, name);
				ds.Tables[name].TableName = n2;
				name = EnsureDiff(name, n2);
				table.TableName = name;
				goto TryAgain;
			}
		}

		private static void CreateProgramIsAlsoOnCampusTable(DataSet ds)
		{
			DataTable ProgramIsAlsoOnCampus = new DataTable("Program " + PredicateTextFilter("Is Also On") + " Campus");

			CreateColumn(ProgramIsAlsoOnCampus, "Campus", true, false, true);
			CreateColumn(ProgramIsAlsoOnCampus, "Program", true, false, true);

			ProgramIsAlsoOnCampus.TableName = FinalizeName(ProgramIsAlsoOnCampus.TableName, false);

			AddTable(ds, ProgramIsAlsoOnCampus);
		}

		private static void CreateProgramTable(DataSet ds)
		{
			DataTable Program = new DataTable("Program");

			CreateColumn(Program, "Program_Code", true, false, false);
			CreateColumn(Program, SubType(), false, false, true);
			CreateColumn(Program, "Primary Address", false, false, true);

			Program.TableName = FinalizeName(Program.TableName, false);

			AddTable(ds, Program);
		}

		private static void CreateEmployeeTable(DataSet ds)
		{
			DataTable Employee = new DataTable("Employee");

			CreateColumn(Employee, "Employee_Nr", true, true, false);
			CreateColumn(Employee, SubType() + PredicateTextFilter(" Receives Commission"), false, false, false);
			CreateColumn(Employee, "Office " + DoBuilding(), false, false, true);
			CreateColumn(Employee, "Office " + DoCampus(), false, false, true);
			CreateColumn(Employee, "Office " + DoRoom(), false, false, true);

			Employee.TableName = FinalizeName(Employee.TableName, false);

			AddTable(ds, Employee);
		}

		private static string DoRoom()
		{
			return (options.RoomRoom ? "Room " : "") + "Room Number";
		}

		private static string DoCampus()
		{
			return (options.RoomRoom ? "Room " : "") + (options.RoomBuilding ? "Building " : "") + (options.RoomCampus ? "Campus " : "") + "Campus_Address";
		}

		private static string DoBuilding()
		{
			return (options.RoomRoom ? "Room " : "") + (options.RoomBuilding ? "Building " : "") + "Building Number";
		}

		private static string SubType()
		{
			string name = "";
			if (options.lbxSupertypesIncluded.Items.Contains("Employee"))
				name += "Employee ";
			return name + "Manager";
		}

		private static void CreateColumn(DataTable table, string name, bool isID, bool hasFK, bool isFK)
		{
			if (options.rbPrefixAll.Checked || (options.rbPrefixIDAlways.Checked && (isID))
				|| (options.rbPrefixIDIfFK.Checked && ((isID && hasFK))))
				name = table.TableName + " " + name;

			DataColumn column = new DataColumn(FinalizeName(name, true));

			AddColumn(table, column);
		}

		private static void AddColumn(DataTable dt, DataColumn column)
		{
		TryAgain:
			try
			{
				dt.Columns.Add(column);
			}
			catch (DuplicateNameException)
			{
				string name = column.ColumnName;
				string n2 = EnsureDiff(name, name);
				dt.Columns[name].ColumnName = n2;
				name = EnsureDiff(name, n2);
				column.ColumnName = name;
				goto TryAgain;
			}
		}

		private static string PredicateTextFilter(string text)
		{
			string newText = "";
			string[] words = text.Split(' ');
			for (int i = 0; i < words.Length; ++i)
			{
				if (!options.lbxOmitWords.Items.Contains(words[i]))
				{
					newText += words[i] + " ";
				}
			}
			return newText;
		}

		private static Dictionary<string, string> columnAbbreviations;
		private static Dictionary<string, string> tableAbbreviations;

		private static string FinalizeName(string name, bool column)
		{
			string newText = options.rbSubjectAreaPrefix.Checked ? options.txtSubjectArea.Text : "";

			name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToLower(name);
			Dictionary<string, string> abbreviations = column ? columnAbbreviations : tableAbbreviations;
			foreach (string s in abbreviations.Keys)
				name = name.Replace(s, abbreviations[s]);

			string space;
			if (column)
				space = options.columnSpace;
			else
				space = options.tableSpace;
			Case c;
			if (column)
				c = options.columnCase;
			else
				c = options.tableCase;
			string[] words = name.Split(' ');
			for (int i = 0; i < words.Length; ++i)
			{
				if (i != 0)
					newText += space;
				if (c == Case.Pascal)
					newText += System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[i]);
				else if (c == Case.Camel)
					if (i == 0)
						newText += System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToLower(words[i]);
					else
						newText += System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[i]);
				else
					newText += words[i];
			}

			if (c == Case.Lower)
				newText = newText.ToLower();
			else if (c == Case.Upper)
				newText = newText.ToUpper();

			newText += (options.rbSubjectAreaSuffix.Checked ? options.txtSubjectArea.Text : "");
			if ((column && options.shortenColumn) || (!column && options.shortenTable))
			{
				int maxLength;
				if (column)
					maxLength = options.columnMaxLength;
				else
					maxLength = options.tableMaxLength;

				newText = shorten(newText, maxLength, space);
			}

			return newText;
		}

		private static Regex myIsNumber;
		private static Dictionary<string, int> myCounts;
		private static string EnsureDiff(string name, string longerThan)
		{
			if (longerThan != null && longerThan.StartsWith(name))
			{
				bool isSame = name == longerThan;
				if (!isSame)
				{
					if (myIsNumber == null)
					{
						myIsNumber = new Regex(@"\d+", RegexOptions.RightToLeft | RegexOptions.Compiled);
					}
					string endNumbers = longerThan.Substring(name.Length);
					Match longerThanMatches = myIsNumber.Match(endNumbers);
					if (longerThanMatches.Success && longerThanMatches.Index + longerThanMatches.Length == endNumbers.Length)
					{
						isSame = true;
					}
				}

				if (isSame)
				{
					if (myCounts == null)
					{
						myCounts = new Dictionary<string, int>();
					}
					int curCount;
					if (myCounts.ContainsKey(name))
					{
						curCount = myCounts[name];
						++myCounts[name];
					}
					else
					{
						curCount = 1;
						myCounts.Add(name, 2);
					}
					name += curCount.ToString();
				}
			}
			return name;
		}

		private static Regex lowerCaseVowelPattern = new Regex("[aeiou]");
		private static Regex splitPattern = new Regex("[\\t\\n\\x0B\\r\\x85\\p{Z}]+");
		private static Regex nonwordCharacterPattern = new Regex("[^\\p{Ll}\\p{Lu}\\p{Lt}\\p{Lo}\\p{Nd}\\p{Pc}\\p{Lm}]");
		private static Regex notUpperCaseLetterOrNumberPattern = new Regex("[^\\p{Lu}\\p{N}]");
		private static string shorten(string name, int maxLength, string spaceChar)
		{
			// If the name is too long, we need to reduce its length
			if (name.Length > maxLength)
			{
				// First, we try to get the name down to size by stripping out certain separator characters
				// and replacing them with IdentifierCasing instead

				string[] tokens = splitPattern.Split(name);
				StringBuilder nameBuilder = new StringBuilder();

				for (int j = 0; j < tokens.Length; j++)
				{
					string token = tokens[j];
					string[] subtokens = token.Split(new string[] { spaceChar }, StringSplitOptions.RemoveEmptyEntries);

					for (int i = 0; i < subtokens.Length; i++)
					{
						token = subtokens[i];
						nameBuilder.Append(token[0].ToString().ToUpper());
						nameBuilder.Append(token, 1, token.Length - 1);
					}
				}

				// Check if the name is still too long
				if (name.Length > maxLength)
				{
					// OK, time for more drastic measures. Now we strip out the lower-case vowels.
					name = lowerCaseVowelPattern.Replace(name, "");

					// Check if the name is still too long
					if (name.Length > maxLength)
					{
						// Still too long? Get rid of all the nonword characters.
						name = nonwordCharacterPattern.Replace(name, "");

						// Check if the name is still too long
						if (name.Length > maxLength)
						{
							// At this point, we remove everything that isn't an upper-case letter or a number.
							name = notUpperCaseLetterOrNumberPattern.Replace(name, "");

							// Check if the name is still too long
							if (name.Length > maxLength)
							{
								// OK, that's about all we can do. Time to chop off the excess.
								name = name.Substring(0, maxLength);
							}
						}
					}
				}
			}

			return name;
		}

		public enum Case
		{
			Camel,
			Pascal,
			Upper,
			Lower,
		}

		private static Dictionary<string, string> GetAbbreviations(bool column)
		{
			Dictionary<string, string> retVal = new Dictionary<string, string>();
			foreach (System.Windows.Forms.DataGridViewRow row in options.dgvGlobalAbbreviations.Rows)
				AddRow(retVal, row);

			Dictionary<string, string> other;
			if (column)
				other = options.columnAbbreviations;
			else
				other = options.tableAbbreviations;

			foreach (string key in other.Keys)
				if (retVal.ContainsKey(key))
					retVal[key] = other[key];
				else
					retVal.Add(key, other[key]);

			return retVal;
		}

		public static void AddRow(Dictionary<string, string> abbreviations, System.Windows.Forms.DataGridViewRow row)
		{
			if (!string.IsNullOrEmpty(row.Cells[0].Value as string))
				abbreviations.Add(row.Cells[0].Value.ToString().ToLower(), row.Cells[1].Value.ToString().ToLower());
		}
	}
}
