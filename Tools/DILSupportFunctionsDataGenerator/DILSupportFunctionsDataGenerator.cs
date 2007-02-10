#region zlib/libpng Copyright Notice
/**************************************************************************\
* Database Intermediate Language                                           *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* This software is provided 'as-is', without any express or implied        *
* warranty. In no event will the authors be held liable for any damages    *
* arising from the use of this software.                                   *
*                                                                          *
* Permission is granted to anyone to use this software for any purpose,    *
* including commercial applications, and to alter it and redistribute it   *
* freely, subject to the following restrictions:                           *
*                                                                          *
* 1. The origin of this software must not be misrepresented; you must not  *
*    claim that you wrote the original software. If you use this software  *
*    in a product, an acknowledgment in the product documentation would be *
*    appreciated but is not required.                                      *
*                                                                          *
* 2. Altered source versions must be plainly marked as such, and must not  *
*    be misrepresented as being the original software.                     *
*                                                                          *
* 3. This notice may not be removed or altered from any source             *
*    distribution.                                                         *
\**************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Neumont.Tools.DIL.Unicode;

namespace Neumont.Tools.DIL
{
	public static class Program
	{
		public static void Main()
		{
			System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

			CodePoint.LoadUnicodeData();

			const int BUFFER_SIZE = 500 * 1024;
			using (StreamWriter writer = new StreamWriter(File.Create("GeneratedDILSupportFunctions.js", BUFFER_SIZE, FileOptions.SequentialScan), Encoding.UTF8, BUFFER_SIZE))
			{
				const string GENERATEDCODE_START = "/**** GENERATED CODE STARTS HERE ****/";
				const string GENERATEDCODE_END = "/**** GENERATED CODE ENDS HERE ****/";

				writer.AutoFlush = false;

				writer.WriteLine(writer.NewLine);

				writer.WriteLine(GENERATEDCODE_START);
				GeneratePatterns(writer);
				writer.WriteLine(GENERATEDCODE_END);

				writer.WriteLine(writer.NewLine);
				writer.WriteLine(writer.NewLine);

				writer.WriteLine(GENERATEDCODE_START);
				GenerateUppercaseMappings(writer);
				writer.WriteLine(GENERATEDCODE_END);

			}
		}

		private static void GeneratePatterns(StreamWriter writer)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			{
				Console.Write("Generating 'split' pattern...");
				StringBuilder splitPatternBuilder = new StringBuilder(200);
				splitPatternBuilder.Append(@"var splitPattern = /[\t\n\x0B\r\x85\x20");
				foreach (CodePoint codePoint in CodePoint.CodePoints)
				{
					if (codePoint.Value > 0xFFFF)
					{
						break;
					}
					if ((codePoint.GeneralCategory & GeneralCategory.Z) != 0 && codePoint.Value != 0x20)
					{
						splitPatternBuilder.AppendFormat(invariantCulture, @"\u{0:X4}", codePoint.Value);
					}
				}
				splitPatternBuilder.Append(@"]+/;");
				writer.WriteLine(splitPatternBuilder.ToString());
				Console.WriteLine(" Done!");
			}
			{
				Console.Write("Generating 'nonword character' pattern...");
				StringBuilder nonwordCharacterPatternBuilder = new StringBuilder(100000);
				nonwordCharacterPatternBuilder.Append(@"var nonwordCharacterPattern = /[^");
				foreach (CodePoint codePoint in CodePoint.CodePoints)
				{
					if (codePoint.Value > 0xFFFF)
					{
						break;
					}
					GeneralCategory generalCategory = codePoint.GeneralCategory;
					if ((codePoint.GeneralCategory & GeneralCategory.L) != 0 || generalCategory == GeneralCategory.Nd || generalCategory == GeneralCategory.Pc)
					{
						nonwordCharacterPatternBuilder.AppendFormat(invariantCulture, @"\u{0:X4}", codePoint.Value);
					}
				}
				nonwordCharacterPatternBuilder.Append(@"]/g;");
				writer.WriteLine(nonwordCharacterPatternBuilder.ToString());
				Console.WriteLine(" Done!");
			}
			{
				Console.Write("Generating 'not upper case letter or number' pattern...");
				StringBuilder notUpperCaseLetterOrNumberPatternBuilder = new StringBuilder(100000);
				notUpperCaseLetterOrNumberPatternBuilder.Append(@"var notUpperCaseLetterOrNumberPattern = /[^");
				foreach (CodePoint codePoint in CodePoint.CodePoints)
				{
					if (codePoint.Value > 0xFFFF)
					{
						break;
					}
					GeneralCategory generalCategory = codePoint.GeneralCategory;
					if ((generalCategory & GeneralCategory.N) != 0 || generalCategory == GeneralCategory.Lu)
					{
						notUpperCaseLetterOrNumberPatternBuilder.AppendFormat(invariantCulture, @"\u{0:X4}", codePoint.Value);
					}
				}
				notUpperCaseLetterOrNumberPatternBuilder.Append(@"]/g;");
				writer.WriteLine(notUpperCaseLetterOrNumberPatternBuilder.ToString());
				Console.WriteLine(" Done!");
			}
			{
				Console.Write("Generating 'regular identifier' pattern...");
				StringBuilder regularIdentifierPatternBuilder = new StringBuilder(200000);
				foreach (CodePoint codePoint in CodePoint.CodePoints)
				{
					if (codePoint.Value > 0xFFFF)
					{
						break;
					}
					GeneralCategory generalCategory = codePoint.GeneralCategory;
					if ((generalCategory & GeneralCategory.L) != 0 || generalCategory == GeneralCategory.Nl)
					{
						regularIdentifierPatternBuilder.AppendFormat(invariantCulture, @"\u{0:X4}", codePoint.Value);
					}
				}
				string letterOrNumberLetter = regularIdentifierPatternBuilder.ToString();
				regularIdentifierPatternBuilder.Length = 0;
				regularIdentifierPatternBuilder.Append(@"var regularIdentifierPattern = new RegExp(""^([");
				regularIdentifierPatternBuilder.Append(letterOrNumberLetter);
				regularIdentifierPatternBuilder.Append(@"][");
				regularIdentifierPatternBuilder.Append(letterOrNumberLetter);
				regularIdentifierPatternBuilder.Append(@"\u00B7");
				foreach (CodePoint codePoint in CodePoint.CodePoints)
				{
					if (codePoint.Value > 0xFFFF)
					{
						break;
					}
					switch (codePoint.GeneralCategory)
					{
						case GeneralCategory.Mn:
						case GeneralCategory.Mc:
						case GeneralCategory.Nd:
						case GeneralCategory.Pc:
						case GeneralCategory.Cf:
							regularIdentifierPatternBuilder.AppendFormat(invariantCulture, @"\u{0:X4}", codePoint.Value);
							break;
					}
				}
				regularIdentifierPatternBuilder.Append(@"]{0,"" + (MAX_IDENTIFIER_LENGTH - 1) + ""})$"");");
				writer.WriteLine(regularIdentifierPatternBuilder.ToString());
				Console.WriteLine(" Done!");
			}
		}

		private struct SingleCharCaseMapping
		{
			public SingleCharCaseMapping(uint codePointValue, uint uppercaseMappingValue)
			{
				this.CodePointValue = codePointValue;
				this.UppercaseMappingValue = uppercaseMappingValue;
			}
			public readonly uint CodePointValue;
			public readonly uint UppercaseMappingValue;
			/// <summary>
			/// The value subtracted from <see cref="CodePointValue"/> to get <see cref="UppercaseMappingValue"/>.
			/// </summary>
			public uint Difference
			{
				get
				{
					return CodePointValue - UppercaseMappingValue;
				}
			}
		}
		private struct StringCaseMapping
		{
			public StringCaseMapping(SingleCharCaseMapping singleCharCaseMapping)
			{
				this.CodePointValue = singleCharCaseMapping.CodePointValue;
				this.UppercaseMappingValue = singleCharCaseMapping.UppercaseMappingValue;
				this.UppercaseMappingValues = null;
			}
			public StringCaseMapping(uint codePointValue, CodePoint[] uppercaseMapping)
			{
				this.CodePointValue = codePointValue;
				this.UppercaseMappingValues = new uint[uppercaseMapping.Length];
				for (int i = 0; i < uppercaseMapping.Length; i++)
				{
					this.UppercaseMappingValues[i] = uppercaseMapping[i].Value;
				}
				this.UppercaseMappingValue = 0;
			}
			public readonly uint CodePointValue;
			public readonly uint UppercaseMappingValue;
			public readonly uint[] UppercaseMappingValues;
		}
		private static void GenerateUppercaseMappings(StreamWriter writer)
		{
			// NOTE: In order to keep the generated code shorter, we leave out some line breaks in the case statements. This is intentional.

			string newLine = writer.NewLine;
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			Console.Write("Generating uppercase mappings switch...");

			StringBuilder builder = new StringBuilder(200000);
			
			#region Preparation
			List<SingleCharCaseMapping> singleCharMappings = new List<SingleCharCaseMapping>(100000);
			List<StringCaseMapping> stringMappings = new List<StringCaseMapping>(10000);
			foreach (CodePoint codePoint in CodePoint.CodePoints)
			{
				if (codePoint.Value > 0xFFFF)
				{
					break;
				}
				CodePoint[] uppercaseMapping = CodePoint.GetUppercaseMapping(codePoint);
				if (uppercaseMapping != null)
				{
					if (uppercaseMapping.Length == 1)
					{
						singleCharMappings.Add(new SingleCharCaseMapping(codePoint.Value, uppercaseMapping[0].Value));
					}
					else
					{
						System.Diagnostics.Debug.Assert(uppercaseMapping.Length > 1);
						stringMappings.Add(new StringCaseMapping(codePoint.Value, uppercaseMapping));
					}
				}
			}

			// Add the special cases for the second halves of surrogate pairs that map to the second halves of other surrogate pairs
			for (uint x = 0xDC28; x <= 0xDC4F; x++)
			{
				singleCharMappings.Add(new SingleCharCaseMapping(x, x - 0x28));
			}
			#endregion // Preparation

			#region singleCharMappings
			// Sort the single char mappings by the difference first, then by the source code point value
			singleCharMappings.Sort(delegate(SingleCharCaseMapping x, SingleCharCaseMapping y)
			{
				int diff = x.Difference.CompareTo(y.Difference);
				return (diff != 0) ? diff : x.CodePointValue.CompareTo(y.CodePointValue);
			});

			const int CASES_PER_LINE = 8;
			int singleCharMappingsCount = singleCharMappings.Count;
			int singleCharMappingsCountMinusOne = singleCharMappingsCount - 1;
			for (int i = 0, casesOnLine = 0; i < singleCharMappingsCount; i++)
			{
				SingleCharCaseMapping singleCharMapping = singleCharMappings[i];
				uint difference = singleCharMapping.Difference;
				bool isNewLine = (casesOnLine == 0);
				bool writeCaseBody = (i == singleCharMappingsCountMinusOne || difference != singleCharMappings[i + 1].Difference);
				bool isLastCaseOnLine = (writeCaseBody || casesOnLine == CASES_PER_LINE - 1);

				// When we only have a single case, we handle it separately
				if (writeCaseBody && (i == 0 || difference != singleCharMappings[i - 1].Difference))
				{
					stringMappings.Add(new StringCaseMapping(singleCharMapping));
					continue;
				}

				builder.AppendFormat(invariantCulture, "{0}case 0x{1:X4}:{2}", isNewLine ? "\t" : " ", singleCharMapping.CodePointValue, isLastCaseOnLine ? newLine : null);

				if (writeCaseBody)
				{
					if ((difference & 0x80000000) == 0)
					{
						if (difference == 0x28)
						{
							builder.AppendFormat(invariantCulture, "\t\t// The entries for code points 0xDC28 through 0xDC4F (inclusive) are a special case since they are{0}" +
								"\t\t// each the second half of a surrogate pair that maps to the second half of another surrogate pair.{0}" +
								"\t\t// Whether mapping these code points like this is actually correct or not is still to be determined...{0}", newLine);
						}
						builder.AppendFormat(invariantCulture, "\t\tx -= 0x{0:X4}; break;{1}", difference, newLine);
					}
					else
					{
						builder.AppendFormat(invariantCulture, "\t\tx += 0x{0:X4}; break;{1}", singleCharMapping.UppercaseMappingValue - singleCharMapping.CodePointValue, newLine);
					}
				}

				casesOnLine = isLastCaseOnLine ? 0 : casesOnLine + 1;
			}
			#endregion // singleCharMappings

			#region stringMappings
			// Sort the string mappings by the number of code point values in the upper case mapping, then by the source code point value
			stringMappings.Sort(delegate(StringCaseMapping x, StringCaseMapping y)
			{
				int diff = (x.UppercaseMappingValues != null ? x.UppercaseMappingValues.Length : 0).CompareTo(
					y.UppercaseMappingValues != null ? y.UppercaseMappingValues.Length : 0);
				return (diff != 0) ? diff : x.CodePointValue.CompareTo(y.CodePointValue);
			});

			int stringMappingsCount = stringMappings.Count;
			for (int i = 0; i < stringMappingsCount; i++)
			{
				StringCaseMapping stringMapping = stringMappings[i];

				builder.AppendFormat(invariantCulture, "\tcase 0x{0:X4}: return \"", stringMapping.CodePointValue);

				if (stringMapping.UppercaseMappingValues != null)
				{
					for (int j = 0; j < stringMapping.UppercaseMappingValues.Length; j++)
					{
						builder.AppendFormat(invariantCulture, @"\u{0:X4}", stringMapping.UppercaseMappingValues[j]);
					}
				}
				else
				{
					builder.AppendFormat(invariantCulture, @"\u{0:X4}", stringMapping.UppercaseMappingValue);
				}

				builder.Append("\";");
				builder.Append(newLine);
			}
			#endregion // stringMappings

			writer.Write(builder.ToString());
			Console.WriteLine(" Done!");
		}
	}
}
