#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using System.Diagnostics;
using System.Security.Permissions;
using ORMSolutions.ORMArchitect.Framework.Design;
using System.Collections.ObjectModel;
using System.Drawing.Design;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using System.Reflection;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region NamePart struct
	/// <summary>
	/// Options used with the <see cref="NamePart"/> structure
	/// </summary>
	[Flags]
	public enum NamePartOptions
	{
		/// <summary>
		/// No special options
		/// </summary>
		None = 0,
		/// <summary>
		/// The element should not be cased
		/// </summary>
		ExplicitCasing = 1,
		/// <summary>
		/// Stop a name part that was added as a single-word expansion
		/// from being split again into multiple parts. Used to
		/// block recursive expansion of a phrase containing a single
		/// word it was expanded from.
		/// </summary>
		ReplacementOfSelf = 2,
	}
	/// <summary>
	/// A callback delegate for adding a <see cref="NamePart"/>
	/// </summary>
	/// <param name="part">The <see cref="NamePart"/> to add</param>
	/// <param name="insertIndex">The index to insert the name part at.</param>
	public delegate void AddNamePart(NamePart part, int? insertIndex);
	/// <summary>
	/// Represent a single string with options
	/// </summary>
	public struct NamePart
	{
		#region Fields and Construtors
		private string myString;
		private NamePartOptions myOptions;
		/// <summary>
		/// Create a new NamePart with default options
		/// </summary>
		/// <param name="value">The string value for this <see cref="NamePart"/></param>
		public NamePart(string value)
		{
			myString = value;
			myOptions = NamePartOptions.None;
		}
		/// <summary>
		/// Create a new NamePart with explicit options
		/// </summary>
		/// <param name="value">The string value for this <see cref="NamePart"/></param>
		/// <param name="options">Values from <see cref="NamePartOptions"/></param>
		public NamePart(string value, NamePartOptions options)
		{
			myString = value;
			myOptions = options;
		}
		#endregion // Fields and Constructors
		#region Accessor Properties
		/// <summary>
		/// Is the structure populated?
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return string.IsNullOrEmpty(myString);
			}
		}
		/// <summary>
		/// Return the <see cref="NamePartOptions"/> passed to the constructor
		/// </summary>
		public NamePartOptions Options
		{
			get
			{
				return myOptions;
			}
		}
		/// <summary>
		/// If <see langword="true"/>, then the casing of the string should not be changed
		/// </summary>
		public bool ExplicitCasing
		{
			get
			{
				return 0 != (myOptions & NamePartOptions.ExplicitCasing);
			}
		}
		#endregion // Accessor Properties
		#region Cast Operators
		/// <summary>
		/// Implicitly cast the <see cref="NamePart"/> to its string value
		/// </summary>
		public static implicit operator string(NamePart part)
		{
			return part.myString;
		}
		/// <summary>
		/// Implicitly cast the <see cref="NamePart"/> to its string value
		/// </summary>
		public static implicit operator NamePart(string value)
		{
			return new NamePart(value);
		}
		#endregion // Cast Operators
		#region NameCollection helpers
		private static readonly char[] NameDelimiterArray = new char[] { ' ', '-' };
		/// <summary>
		/// Add a part of a name to a collection of other partial names.
		/// The collection is only created if there are multiple names.
		/// </summary>
		/// <param name="singleName">A single name part, initialize as default(NamePart)</param>
		/// <param name="nameCollection">A list of name parts, initialize to null.</param>
		/// <param name="newNamePart">The name part to add.</param>
		public static void AddToNameCollection(ref NamePart singleName, ref List<NamePart> nameCollection, NamePart newNamePart)
		{
			AddToNameCollection(ref singleName, ref nameCollection, newNamePart, -1, true);
		}
		/// <summary>
		/// Add a part of a name to a collection of other partial names.
		/// The collection is only created if there are multiple names.
		/// </summary>
		/// <param name="singleName">A single name part, initialize as default(NamePart)</param>
		/// <param name="nameCollection">A list of name parts, initialize to null.</param>
		/// <param name="newNamePart">The name part to add.</param>
		/// <param name="index">The location to insert the name, or -1 for the end.</param>
		/// <param name="collapseAdjacentName">Collapse an adjacent repeated name.</param>
		public static void AddToNameCollection(ref NamePart singleName, ref List<NamePart> nameCollection, NamePart newNamePart, int index, bool collapseAdjacentName)
		{
			string newName = newNamePart;
			newName = newName.Trim();
			NamePartOptions options = newNamePart.Options;
			int startNameCount = GetNameCount(ref singleName, ref nameCollection);
			int endNameCount;
			// Test for space separated and pattern based multi-part names
			if (newName.IndexOfAny(NameDelimiterArray) != -1)
			{
				string[] individualEntries = newName.Split(NameDelimiterArray, StringSplitOptions.RemoveEmptyEntries);
				// We don't know at this point if the names are single or will split further with
				// the next call. Test how many items are added by tracking the count at each stage.
				for (int i = 0; i < individualEntries.Length; ++i)
				{
					// Add each space separated name individually
					AddToNameCollection(ref singleName, ref nameCollection, new NamePart(individualEntries[i], options), index == -1 ? -1 : index + (i == 0 ? 0 : GetNameCount(ref singleName, ref nameCollection) - startNameCount), false);
				}
				endNameCount = GetNameCount(ref singleName, ref nameCollection);
			}
			else if (0 == (options & NamePartOptions.ExplicitCasing) &&
				Utility.IsMultiPartName(newName))
			{
				Match match = Utility.MatchNameParts(newName);
				int matchIndex = 0;
				while (match.Success)
				{
					// Using the match index as an increment is sufficient
					// because we know the names will not split further and
					// adjacent names will not collapse.
					GroupCollection groups = match.Groups;
					AddToNameCollection(ref singleName, ref nameCollection, new NamePart(match.Value, groups["TrailingUpper"].Success || groups["Numeric"].Success ? NamePartOptions.ExplicitCasing : NamePartOptions.None), index == -1 ? -1 : index + matchIndex, false);
					++matchIndex;
					match = match.NextMatch();
				}
				endNameCount = startNameCount + matchIndex;
			}
			else if (singleName.IsEmpty)
			{
				// We only have one name so far, so just use the string
				singleName = new NamePart(newName, options);
				endNameCount = 1;
			}
			else
			{
				// We need to now use the collection
				if (null == nameCollection)
				{
					nameCollection = new List<NamePart>();
					// First add the previously added element
					nameCollection.Add(singleName);
				}
				if (index == -1)
				{
					nameCollection.Add(new NamePart(newName, options));
				}
				else
				{
					nameCollection.Insert(index, new NamePart(newName, options));
				}
				endNameCount = startNameCount + 1;
			}

			int newNameCount;
			if (collapseAdjacentName &&
				0 != (newNameCount = (endNameCount - startNameCount))) // A name was added
			{
				// Remove duplicate names, treating the multiple parts as a split
				// name as a single name.
				if (index == -1)
				{
					index = startNameCount;
				}

				NamePart firstPart;
				NamePart secondPart;
				if (newNameCount <= startNameCount) // There are sufficient adjacent names to collapse a single or multi-part name
				{
					// Check for preceding name matches on all parts of the name
					while (index >= newNameCount)
					{
						int i = 0;
						for (; i < newNameCount; ++i)
						{
							firstPart = nameCollection[index + i];
							secondPart = nameCollection[index - newNameCount + i];
							if (!((string)firstPart).Equals((string)secondPart, firstPart.ExplicitCasing || secondPart.ExplicitCasing ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase))
							{
								break;
							}
						}
						if (i < newNameCount)
						{
							break;
						}
						nameCollection.RemoveRange(index, newNameCount);
						index -= newNameCount;
						endNameCount -= newNameCount;
					}

					// Check for following name matches on all parts of the name
					while ((endNameCount - (index + newNameCount)) >= newNameCount)
					{
						int i = 0;
						for (; i < newNameCount; ++i)
						{
							firstPart = nameCollection[index + i];
							secondPart = nameCollection[index + newNameCount + i];
							if (!((string)firstPart).Equals((string)secondPart, firstPart.ExplicitCasing || secondPart.ExplicitCasing ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase))
							{
								break;
							}
						}
						if (i < newNameCount)
						{
							break;
						}
						nameCollection.RemoveRange(index + newNameCount, newNameCount);
						index -= newNameCount;
						endNameCount -= newNameCount;
					}
				}

				if (newNameCount != 1)
				{
					// Enhance the multi-part collapse semantics by checking the
					// leading and trailing name parts.

					// Compare the parts preceding the first word
					while (index > 0 && ((string)(firstPart = nameCollection[index])).Equals((string)(secondPart = nameCollection[index - 1]), firstPart.ExplicitCasing || secondPart.ExplicitCasing ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase))
					{
						nameCollection.RemoveAt(index);
						--index;
						--endNameCount;
					}

					// Compare the parts following the last word
					while ((index + newNameCount) < endNameCount &&
						((string)(firstPart = nameCollection[index + newNameCount - 1])).Equals((string)(secondPart = nameCollection[index + newNameCount]), firstPart.ExplicitCasing || secondPart.ExplicitCasing ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase))
					{
						nameCollection.RemoveAt(index + newNameCount - 1);
						--endNameCount;
					}
				}
			}
		}
		private static int GetNameCount(ref NamePart singleName, ref List<NamePart> nameCollection)
		{
			if (singleName.IsEmpty)
			{
				return 0;
			}
			else if (nameCollection == null)
			{
				return 1;
			}
			return nameCollection.Count;
		}
		/// <summary>
		/// Combine name parts into a final name based on a <see cref="NameGenerator"/>.
		/// Recognized phrases from the context model are also supplied.
		/// </summary>
		/// <param name="singleName">A single name, use if there is no collection.</param>
		/// <param name="nameCollection">Pass in more than one name.</param>
		/// <param name="generator">The <see cref="NameGenerator"/> to apply.</param>
		/// <param name="model">The context <see cref="ORMModel"/> to manage phrase replacement.</param>
		/// <returns></returns>
		public static string GetFinalName(NamePart singleName, List<NamePart> nameCollection, NameGenerator generator, ORMModel model)
		{
			ResolveRecognizedPhrases(ref singleName, ref nameCollection, generator, model);
			string finalName;
			if (null == nameCollection)
			{
				if (singleName.IsEmpty)
				{
					return "";
				}

				NameGeneratorCasingOption casing = generator.CasingOption;
				if (casing == NameGeneratorCasingOption.None)
				{
					finalName = singleName;
				}
				else
				{
					finalName = DoFirstWordCasing(singleName, casing, CultureInfo.CurrentCulture.TextInfo);
				}
			}
			else
			{
				TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

				string name;
				NameGeneratorCasingOption casing = generator.CasingOption;
				if (casing == NameGeneratorCasingOption.None)
				{
					name = nameCollection[0];
				}
				else
				{
					name = DoFirstWordCasing(nameCollection[0], casing, textInfo);
				}

				//we already know there are at least two name entries, so use a string builder
				StringBuilder builder = new StringBuilder(name);

				//we already have the first entry, so mark camel as pascal
				NameGeneratorCasingOption tempCasing = casing;
				if (tempCasing == NameGeneratorCasingOption.Camel)
				{
					tempCasing = NameGeneratorCasingOption.Pascal;
				}

				//add each entry with proper spaces and casing
				int count = nameCollection.Count;
				string space = GetSpacingReplacement(generator);
				for (int i = 1; i < count; ++i)
				{
					builder.Append(space);
					if (casing == NameGeneratorCasingOption.None)
					{
						name = nameCollection[i];
					}
					else
					{
						name = DoFirstWordCasing(nameCollection[i], tempCasing, textInfo);
					}
					builder.Append(name);
				}
				finalName = builder.ToString();
			}

			return finalName;
		}
		private static string GetSpacingReplacement(NameGenerator generator)
		{
			string retVal;
			switch (generator.SpacingFormat)
			{
				case NameGeneratorSpacingFormat.ReplaceWith:
					retVal = generator.SpacingReplacement;
					break;
				case NameGeneratorSpacingFormat.Retain:
					retVal = " ";
					break;
				// case NameGeneratorSpacingFormat.Remove:
				default:
					retVal = "";
					break;
			}
			return retVal;
		}
		private struct RecognizedPhraseData
		{
			private static readonly char[] SpaceCharArray = new char[] { ' ' };
			private string[] myOriginalNames;
			private string myUnparsedReplacement;
			public static bool Populate(NameAlias alias, int remainingParts, out RecognizedPhraseData phraseData)
			{
				phraseData = new RecognizedPhraseData();
				string matchPhrase = ((RecognizedPhrase)alias.Element).Name;
				string replacePhrase = alias.Name;
				if (0 == string.Compare(matchPhrase, replacePhrase, StringComparison.CurrentCulture))
				{
					// Sanity check, don't process these
					return false;
				}
				if (matchPhrase.IndexOf(' ') != -1)
				{
					if (replacePhrase.IndexOf(matchPhrase, StringComparison.CurrentCultureIgnoreCase) != -1)
					{
						// UNDONE: We handle expanding single words to a phrase containing the word, but not
						// multi-word phrases doing the same thing. However, make sure we don't recurse in this
						// situation.
						return false;
					}
					string[] parts = matchPhrase.Split(SpaceCharArray, StringSplitOptions.RemoveEmptyEntries);
					if (parts.Length > remainingParts)
					{
						return false;
					}
					phraseData.myOriginalNames = parts;
				}
				else
				{
					phraseData.myOriginalNames = new string[] { matchPhrase };
				}
				phraseData.myUnparsedReplacement = replacePhrase;
				return true;
			}
			public bool IsEmpty
			{
				get
				{
					return myOriginalNames == null;
				}
			}
			public string[] OriginalNames
			{
				get
				{
					return myOriginalNames;
				}
			}
			/// <summary>
			/// Get the replacement names. The assumption is that this is rarely called,
			/// and the results are not cached.
			/// </summary>
			public string[] ReplacementNames
			{
				get
				{
					string name = myUnparsedReplacement;
					return string.IsNullOrEmpty(name) ?
						new string[0] :
						(name.IndexOf(' ') != -1) ? name.Split(SpaceCharArray, StringSplitOptions.RemoveEmptyEntries) : new string[] { name };
				}
			}
		}
		private static void ResolveRecognizedPhrases(ref NamePart singleName, ref List<NamePart> nameCollection, NameGenerator generator, ORMModel model)
		{
			if (model != null)
			{
				if (nameCollection != null)
				{
					int nameCount = nameCollection.Count;
					int remainingParts = nameCount;
					for (int i = 0; i < nameCount; ++i, --remainingParts)
					{
						// For each part, collection possible replacement phrases beginning with that name
						NamePart currentPart = nameCollection[i];
						RecognizedPhraseData singlePhrase = new RecognizedPhraseData();
						List<RecognizedPhraseData> phraseList = null;
						bool possibleReplacement = false;
						foreach (NameAlias alias in model.GetRecognizedPhrasesStartingWith(currentPart, generator))
						{
							RecognizedPhraseData phraseData;
							if (RecognizedPhraseData.Populate(alias, remainingParts, out phraseData))
							{
								if (phraseList == null)
								{
									possibleReplacement = true;
									if (singlePhrase.IsEmpty)
									{
										singlePhrase = phraseData;
									}
									else
									{
										phraseList = new List<RecognizedPhraseData>();
										phraseList.Add(singlePhrase);
										phraseList.Add(phraseData);
										singlePhrase = new RecognizedPhraseData();
									}
								}
								else
								{
									phraseList.Add(phraseData);
								}
							}
						}
						// If we have possible replacements, then look farther to see
						// if the multi-part phrases match. Start by searching the longest
						// match possible.
						if (possibleReplacement)
						{
							if (phraseList != null)
							{
								phraseList.Sort(delegate (RecognizedPhraseData left, RecognizedPhraseData right)
								{
									return right.OriginalNames.Length.CompareTo(left.OriginalNames.Length);
								});
								int phraseCount = phraseList.Count;
								for (int j = 0; j < phraseCount; ++j)
								{
									if (TestResolvePhraseDataForCollection(phraseList[j], ref singleName, ref nameCollection, i, generator, model))
									{
										return;
									}
								}
							}
							else
							{
								if (TestResolvePhraseDataForCollection(singlePhrase, ref singleName, ref nameCollection, i, generator, model))
								{
									return;
								}
							}
						}
					}
				}
				else if (!singleName.IsEmpty)
				{
					LocatedElement element = model.RecognizedPhrasesDictionary.GetElement(singleName);
					RecognizedPhrase phrase;
					NameAlias alias;
					if (null != (phrase = element.SingleElement as RecognizedPhrase) &&
						null != (alias = generator.FindMatchingAlias(phrase.AbbreviationCollection)))
					{
						RecognizedPhraseData phraseData;
						if (RecognizedPhraseData.Populate(alias, 1, out phraseData))
						{
							string[] replacements = phraseData.ReplacementNames;
							int replacementLength = replacements.Length;
							NamePart startingPart = singleName;
							singleName = new NamePart();
							if (replacementLength == 0)
							{
								// Highly unusual, but possible with collapsing phrases and omitted readings
								singleName = new NamePart();
							}
							else
							{
								string testForEqual = singleName;
								bool caseIfEqual = 0 != (singleName.Options & NamePartOptions.ExplicitCasing);
								singleName = new NamePart();
								if (replacementLength == 1)
								{
									string replacement = replacements[0];
									NamePartOptions options = NamePartOptions.None;
									if ((caseIfEqual && 0 == string.Compare(testForEqual, replacement, StringComparison.CurrentCulture)) ||
										(0 == string.Compare(testForEqual, replacement, StringComparison.CurrentCultureIgnoreCase)))
									{
										// Single replacement for same string
										return;
									}
									AddToNameCollection(ref singleName, ref nameCollection, new NamePart(replacement, options));
								}
								else
								{
									for (int i = 0; i < replacementLength; ++i)
									{
										string replacement = replacements[i];
										NamePartOptions options = NamePartOptions.None;
										if (caseIfEqual && 0 == string.Compare(testForEqual, replacement, StringComparison.CurrentCulture))
										{
											options |= NamePartOptions.ExplicitCasing | NamePartOptions.ReplacementOfSelf;
										}
										else if (0 == string.Compare(testForEqual, replacement, StringComparison.CurrentCultureIgnoreCase))
										{
											options |= NamePartOptions.ReplacementOfSelf;
										}
										AddToNameCollection(ref singleName, ref nameCollection, new NamePart(replacement, options));
									}
								}
								ResolveRecognizedPhrases(ref singleName, ref nameCollection, generator, model);
							}
							return;
						}
					}
				}
			}
		}
		/// <summary>
		/// Helper for ResolveRecognizedPhrases. Returns true is parent processing is complete.
		/// </summary>
		private static bool TestResolvePhraseDataForCollection(RecognizedPhraseData phraseData, ref NamePart singleName, ref List<NamePart> nameCollection, int collectionIndex, NameGenerator generator, ORMModel model)
		{
			Debug.Assert(nameCollection != null);
			string[] matchNames = phraseData.OriginalNames;
			int matchLength = matchNames.Length;
			int i = 0;
			int firstExplicitPart = -1;
			int explicitPartCount = 0;
			for (; i < matchLength; ++i) // Note the bound on this is already verified by RecognizedPhraseData.Populate
			{
				NamePart testPart = nameCollection[collectionIndex + i];
				bool currentPartExplicit = 0 != (testPart.Options & NamePartOptions.ExplicitCasing);
				if (0 != string.Compare(testPart, matchNames[i], currentPartExplicit ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase) ||
					(matchLength == 1 && 0 != (testPart.Options & NamePartOptions.ReplacementOfSelf)))
				{
					break;
				}
				if (currentPartExplicit && firstExplicitPart == -1)
				{
					++explicitPartCount;
					firstExplicitPart = i;
				}
			}
			if (i == matchLength)
			{
				// We have a valid replacement, apply it and recurse
				string[] explicitlyCasedNames = null;
				string singleMatchName = (matchLength == 1) ? matchNames[0] : null;
				if (explicitPartCount != 0)
				{
					explicitlyCasedNames = new string[explicitPartCount];
					int nextExplicitName = 0;
					for (int j = collectionIndex + firstExplicitPart; ; ++j)
					{
						NamePart testPart = nameCollection[j];
						if (0 != (testPart.Options & NamePartOptions.ExplicitCasing))
						{
							explicitlyCasedNames[nextExplicitName] = testPart;
							if (++nextExplicitName == explicitPartCount)
							{
								break;
							}
						}
					}
					if (explicitPartCount > 1)
					{
						Array.Sort<string>(explicitlyCasedNames, StringComparer.CurrentCultureIgnoreCase);
					}
				}
				nameCollection.RemoveRange(collectionIndex, matchLength);
				int startingCollectionSize = nameCollection.Count;
				string[] replacements = phraseData.ReplacementNames;
				for (i = 0; i < replacements.Length; ++i)
				{
					// Recognized phrases do not record casing priority and phrases are
					// generally treated as case insensitive. However, if any replacement
					// word exactly matches an explicitly cased word in the original names
					// then case the replacement as well.
					NamePartOptions options = NamePartOptions.None;
					string replacement = replacements[i];
					int matchIndex;
					if (explicitlyCasedNames != null && 0 <= (matchIndex = Array.BinarySearch<string>(explicitlyCasedNames, replacement, StringComparer.CurrentCultureIgnoreCase)))
					{
						if (0 == string.Compare(explicitlyCasedNames[matchIndex], replacement, StringComparison.CurrentCulture))
						{
							options |= NamePartOptions.ExplicitCasing;
						}
						if (matchLength == 1)
						{
							options |= NamePartOptions.ReplacementOfSelf;
						}
					}
					else if (singleMatchName != null && 0 == string.Compare(singleMatchName, replacement, StringComparison.CurrentCultureIgnoreCase))
					{
						options |= NamePartOptions.ReplacementOfSelf;
					}
					AddToNameCollection(ref singleName, ref nameCollection, new NamePart(replacement, options), collectionIndex + nameCollection.Count - startingCollectionSize, true);
				}
				ResolveRecognizedPhrases(ref singleName, ref nameCollection, generator, model);
				return true;
			}
			return false;
		}
		#endregion // NameCollection helpers
		#region Casing helpers
		private static string DoFirstWordCasing(NamePart name, NameGeneratorCasingOption casing, TextInfo textInfo)
		{
			if (name.ExplicitCasing) return name;
			switch (casing)
			{
				case NameGeneratorCasingOption.Camel:
					return TestHasAdjacentUpperCase(name) ? (string)name : DoFirstLetterCase(name, false, textInfo);
				case NameGeneratorCasingOption.Pascal:
					return TestHasAdjacentUpperCase(name) ? (string)name : DoFirstLetterCase(name, true, textInfo);
				case NameGeneratorCasingOption.Lower:
					return TestHasAdjacentUpperCase(name) ? (string)name : textInfo.ToLower(name);
				case NameGeneratorCasingOption.Upper:
					return textInfo.ToUpper(name);
			}

			return null;
		}
		private static bool TestHasAdjacentUpperCase(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				int length = name.Length;
				bool previousCharUpper = false;
				for (int i = 0; i < length; ++i)
				{
					if (Char.IsUpper(name, i))
					{
						if (previousCharUpper)
						{
							return true;
						}
						previousCharUpper = true;
					}
					else
					{
						previousCharUpper = false;
					}
				}
			}
			return false;
		}
		private static string DoFirstLetterCase(NamePart name, bool upper, TextInfo textInfo)
		{
			string nameValue = name;
			if (string.IsNullOrEmpty(nameValue))
			{
				return nameValue;
			}
			char c = nameValue[0];
			if (upper)
			{
				c = textInfo.ToUpper(c);
			}
			else
			{
				c = textInfo.ToLower(c);
			}
			if (nameValue.Length > 1)
			{
				nameValue = c.ToString() + nameValue.Substring(1);
			}
			else
			{
				nameValue = c.ToString();
			}
			return nameValue;
		}
		#endregion // Casing helpers
	}
	#endregion // NamePart struct
}
