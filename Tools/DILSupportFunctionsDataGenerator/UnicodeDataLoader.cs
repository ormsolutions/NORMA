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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;

namespace Neumont.Tools.DIL.Unicode
{
	public partial struct CodePoint
	{
		public static void LoadUnicodeData()
		{
			const string UnicodeDataFileName = "UnicodeData.txt";
			const string SpecialCasingFileName = "SpecialCasing.txt";

			GetFile(UnicodeDataFileName);

			GetFile(SpecialCasingFileName);

			SortedList<uint, CodePoint> codePointsByValue = CodePoint.codePointsByValue;
			Dictionary<uint, CodePoint[]> uppercaseMappings = CodePoint.uppercaseMappings;
			Dictionary<uint, CodePoint[]> lowercaseMappings = CodePoint.lowercaseMappings;
			Dictionary<uint, CodePoint[]> titlecaseMappings = CodePoint.titlecaseMappings;

			char[] spaceArray = new char[] { ' ' };
			char[] semicolonArray = new char[] { ';' };

			#region Process UnicodeData file
			{
				#region Lookup dictionaries
				Dictionary<string, GeneralCategory> generalCategoryLookup;
				{
					GeneralCategory[] generalCategoryValues = (GeneralCategory[])Enum.GetValues(typeof(GeneralCategory));
					generalCategoryLookup = new Dictionary<string, GeneralCategory>(generalCategoryValues.Length, StringComparer.Ordinal);
					for (int i = 0; i < generalCategoryValues.Length; i++)
					{
						GeneralCategory generalCategoryValue = generalCategoryValues[i];
						generalCategoryLookup[generalCategoryValue.ToString("G")] = generalCategoryValue;
					}
				}
				Dictionary<string, BidiClass> bidiClassLookup;
				{
					BidiClass[] bidiClassValues = (BidiClass[])Enum.GetValues(typeof(BidiClass));
					bidiClassLookup = new Dictionary<string, BidiClass>(bidiClassValues.Length, StringComparer.Ordinal);
					for (int i = 0; i < bidiClassValues.Length; i++)
					{
						BidiClass bidiClassValue = bidiClassValues[i];
						bidiClassLookup[bidiClassValue.ToString("G")] = bidiClassValue;
					}
				}
				#endregion // Lookup dictionaries

				string[] unicodeDataLines = File.ReadAllLines(UnicodeDataFileName, Encoding.UTF8);
				for (int i = 0; i < unicodeDataLines.Length; i++)
				{
					string unicodeDataLine = unicodeDataLines[i];
					if (!string.IsNullOrEmpty(unicodeDataLine) && unicodeDataLine[0] != '#')
					{
						string[] unicodeDataTokens = unicodeDataLine.Split(semicolonArray);
						Debug.Assert(unicodeDataTokens.Length >= 15);

						const int ValueIndex = 0;
						const int NameIndex = 1;
						const int GeneralCategoryIndex = 2;
						const int CanonicalCombiningClassIndex = 3;
						const int BidiClassIndex = 4;
						// 5, 6, 7, 8 omitted for the moment
						const int BidiMirroredIndex = 9;
						const int Unicode1NameIndex = 10;
						const int IsoCommentIndex = 11;
						const int SimpleUppercaseMappingIndex = 12;
						const int SimpleLowercaseMappingIndex = 13;
						const int SimpleTitlecaseMappingIndex = 14;

						uint value = uint.Parse(unicodeDataTokens[ValueIndex], NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo);

						string name = unicodeDataTokens[NameIndex];
						if (string.IsNullOrEmpty(name))
						{
							name = NameDefault;
						}

						GeneralCategory generalCategory;
						{
							string generalCategoryString = unicodeDataTokens[GeneralCategoryIndex];
							if (string.IsNullOrEmpty(generalCategoryString))
							{
								generalCategory = GeneralCategory.Cn;
							}
							else
							{
								generalCategory = generalCategoryLookup[generalCategoryString];
							}
						}

						byte canonicalCombiningClass;
						{
							string canonicalCombiningClassString = unicodeDataTokens[CanonicalCombiningClassIndex];
							if (string.IsNullOrEmpty(canonicalCombiningClassString))
							{
								canonicalCombiningClass = 0;
							}
							else
							{
								canonicalCombiningClass = byte.Parse(canonicalCombiningClassString, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
							}
						}

						BidiClass bidiClass;
						{
							string bidiClassString = unicodeDataTokens[BidiClassIndex];
							if (string.IsNullOrEmpty(bidiClassString))
							{
								bidiClass = BidiClass.Invalid;
							}
							else
							{
								bidiClass = bidiClassLookup[bidiClassString];
							}
						}

						// 5, 6, 7, 8 omitted for the moment

						bool bidiMirrored = (unicodeDataTokens[BidiMirroredIndex] == "Y");

						string unicode1Name = unicodeDataTokens[Unicode1NameIndex];
						if (string.IsNullOrEmpty(unicode1Name))
						{
							unicode1Name = null;
						}

						string isoComment = unicodeDataTokens[IsoCommentIndex];
						if (string.IsNullOrEmpty(isoComment))
						{
							isoComment = null;
						}

						uint? simpleUppercaseMapping;
						{
							string simpleUppercaseMappingString = unicodeDataTokens[SimpleUppercaseMappingIndex];
							if (string.IsNullOrEmpty(simpleUppercaseMappingString))
							{
								simpleUppercaseMapping = null;
							}
							else
							{
								simpleUppercaseMapping = uint.Parse(simpleUppercaseMappingString, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo);
								if (simpleUppercaseMapping.Value == value)
								{
									simpleUppercaseMapping = null;
								}
							}
						}

						uint? simpleLowercaseMapping;
						{
							string simpleLowercaseMappingString = unicodeDataTokens[SimpleLowercaseMappingIndex];
							if (string.IsNullOrEmpty(simpleLowercaseMappingString))
							{
								simpleLowercaseMapping = null;
							}
							else
							{
								simpleLowercaseMapping = uint.Parse(simpleLowercaseMappingString, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo);
								if (simpleLowercaseMapping.Value == value)
								{
									simpleLowercaseMapping = null;
								}
							}
						}

						uint? simpleTitlecaseMapping;
						{
							string simpleTitlecaseMappingString = unicodeDataTokens[SimpleTitlecaseMappingIndex];
							if (string.IsNullOrEmpty(simpleTitlecaseMappingString))
							{
								simpleTitlecaseMapping = null;
							}
							else
							{
								simpleTitlecaseMapping = uint.Parse(simpleTitlecaseMappingString, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo);
								if (simpleTitlecaseMapping.Value == value)
								{
									simpleTitlecaseMapping = null;
								}
							}
						}

						codePointsByValue[value] = new CodePoint(value, name, generalCategory, canonicalCombiningClass, bidiClass, bidiMirrored, unicode1Name, isoComment, simpleUppercaseMapping, simpleLowercaseMapping, simpleTitlecaseMapping);
					}
				}
			}
			#endregion // Process UnicodeData file

			#region Process SpecialCasing file
			{
				List<CodePoint> mappingCodePoints = new List<CodePoint>();

				string[] specialCasingLines = File.ReadAllLines(SpecialCasingFileName, Encoding.UTF8);
				for (int i = 0; i < specialCasingLines.Length; i++)
				{
					string specialCasingLine = specialCasingLines[i];
					if (!string.IsNullOrEmpty(specialCasingLine))
					{
						int commentStartIndex = specialCasingLine.IndexOf('#');
						if (commentStartIndex >= 0)
						{
							specialCasingLine = specialCasingLine.Remove(commentStartIndex).Trim(spaceArray);
							if (string.IsNullOrEmpty(specialCasingLine))
							{
								continue;
							}
						}

						string[] specialCasingTokens = specialCasingLine.Split(semicolonArray);
						Debug.Assert(specialCasingTokens.Length >= 4);

						const int ValueIndex = 0;
						const int LowerMappingIndex = 1;
						const int TitleMappingIndex = 2;
						const int UpperMappingIndex = 3;
						const int ConditionIndex = 4;

						uint value = uint.Parse(specialCasingTokens[ValueIndex], NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo);

						if (specialCasingTokens.Length >= 5 && !string.IsNullOrEmpty(specialCasingTokens[ConditionIndex].Trim(spaceArray)))
						{
							// We don't want any conditional mappings
							continue;
						}

						ProcessTokenStringForCodePoints(value, specialCasingTokens[LowerMappingIndex], spaceArray, mappingCodePoints, lowercaseMappings);
						ProcessTokenStringForCodePoints(value, specialCasingTokens[TitleMappingIndex], spaceArray, mappingCodePoints, titlecaseMappings);
						ProcessTokenStringForCodePoints(value, specialCasingTokens[UpperMappingIndex], spaceArray, mappingCodePoints, uppercaseMappings);
					}
				}
			}
			#endregion // Process SpecialCasing file
		}

		private static void ProcessTokenStringForCodePoints(uint value, string tokenString, char[] separatorArray, List<CodePoint> list, Dictionary<uint, CodePoint[]> mappings)
		{
			SortedList<uint, CodePoint> codePointsByValue = CodePoint.codePointsByValue;
			list.Clear();
			string[] tokens = tokenString.Split(separatorArray, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < tokens.Length; i++)
			{
				list.Add(codePointsByValue[uint.Parse(tokens[i], NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo)]);
			}
			int codePointCount = list.Count;
			if (codePointCount > 1 || (codePointCount == 1 && list[0]._value != value))
			{
				mappings[value] = list.ToArray();
			}
		}

		#region GetFile method
		private static void GetFile(string fileName)
		{
			if (File.Exists(fileName))
			{
				Console.WriteLine(@"Using existing local copy of ""{0}"".", fileName);
			}
			else
			{
				Console.WriteLine(@"No local copy exists for ""{0}"". Downloading... ", fileName);
				Console.Write("\t");
				System.Net.WebClient webClient = new System.Net.WebClient();
				webClient.Encoding = Encoding.UTF8;
				int previousProgressLength = 0;
				int previousProgressPercentage = -1;
				bool downloadCompleted = false;
				object progressChangedLockObject = new object();
				webClient.DownloadProgressChanged += delegate(object sender, DownloadProgressChangedEventArgs e)
				{
					Thread.MemoryBarrier();
					if (downloadCompleted)
					{
						return;
					}
					lock (progressChangedLockObject)
					{
						if (e.ProgressPercentage >= previousProgressPercentage)
						{
							string progress = string.Format("{0}% ({1} bytes / {2} bytes)", e.ProgressPercentage, e.BytesReceived, e.TotalBytesToReceive);
							if (previousProgressLength > 0)
							{
								Console.CursorLeft -= previousProgressLength;
							}
							previousProgressLength = progress.Length;
							Console.Write(progress);
						}
					}
				};
				using (ManualResetEvent downloadCompletedEvent = new ManualResetEvent(false))
				{
					webClient.DownloadFileCompleted += delegate(object sender, AsyncCompletedEventArgs e)
					{
						if (e.Cancelled || e.Error != null)
						{
							if (e.Error == null)
							{
								Console.WriteLine(" Cancelled!");
							}
							else
							{
								Console.WriteLine(" Error occurred:");
								foreach (string errorLine in e.Error.ToString().Split(new string[] { Environment.NewLine, "\r\n", "\r", "\n" }, StringSplitOptions.None))
								{
									Console.WriteLine("\t" + errorLine);
								}
							}
							Console.WriteLine("Terminating program...");
							Environment.Exit(1);
						}
						else
						{
							Console.WriteLine(" Done!");
							Console.Out.Flush();
							downloadCompleted = true;
							Thread.MemoryBarrier();
							downloadCompletedEvent.Set();
						}
					};
					webClient.DownloadFileAsync(new Uri("http://www.unicode.org/Public/UNIDATA/" + fileName), fileName);
					downloadCompletedEvent.WaitOne();
				}
				Debug.Assert(File.Exists(fileName));
				WebClient webClient1, webClient2;
				
using (ManualResetEvent download1CompletedEvent = new ManualResetEvent(false), download2CompletedEvent = new ManualResetEvent(false))
{
	webClient1.DownloadStringCompleted += delegate
	{
		download1CompletedEvent.Set();
	};
	webClient2.DownloadStringCompleted += delegate
	{
		download2CompletedEvent.Set();
	};
	webClient1.DownloadStringAsync(new Uri("whatever1"));
	webClient2.DownloadStringAsync(new Uri("whatever2"));
	WaitHandle.WaitAll(new WaitHandle[] { download1CompletedEvent, download2CompletedEvent });
}
			}
		}
		#endregion // GetFile method

	}
}
