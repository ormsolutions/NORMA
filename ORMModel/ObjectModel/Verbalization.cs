using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using System.Text.RegularExpressions;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region IVerbalize interface
	/// <summary>
	/// An enum representing the type of verbalization
	/// for a given element
	/// </summary>
	[CLSCompliant(true)]
	public enum VerbalizationContent
	{
		/// <summary>
		/// Normal content
		/// </summary>
		Normal = 0,
		/// <summary>
		/// An error report
		/// </summary>
		ErrorReport = 1,
	}
	/// <summary>
	/// A callback delegate enabling a verbalizer to tell
	/// the hosting window that it is about to begin verbalizing.
	/// This enables the host window to delay writing content outer
	/// content until it knows that text is about to be written by
	/// the verbalizer to the writer
	/// </summary>
	/// <param name="content">The style of verbalization content</param>
	[CLSCompliant(true)]
	public delegate void NotifyBeginVerbalization(VerbalizationContent content);
	/// <summary>
	/// Interface for verbalization
	/// </summary>
	[CLSCompliant(true)]
	public interface IVerbalize
	{
		/// <summary>
		/// Verbalize in the requested form
		/// </summary>
		/// <param name="writer">The output text writer</param>
		/// <param name="beginVerbalization">A callback function to notify when verbalization is starting</param>
		/// <param name="isNegative">true for a negative reading</param>
		/// <returns>true to continue with child verbalization, otherwise false</returns>
		bool GetVerbalization(TextWriter writer, NotifyBeginVerbalization beginVerbalization, bool isNegative);
	}
	/// <summary>
	/// Interface to redirect verbalization. Called for top-level selected objects only
	/// </summary>
	[CLSCompliant(true)]
	public interface IRedirectVerbalization
	{
		/// <summary>
		/// Use the returned object as the verbalizer
		/// </summary>
		IVerbalize SurrogateVerbalizer { get;}
	}
	#endregion // IVerbalize interface
	#region Static verbalization helpers on FactType class
	public partial class FactType
	{
		/// <summary>
		/// Helper function to get a matching reading order. The match priority is
		/// specified by the parameter order
		/// </summary>
		/// <param name="readingOrders">The ReadingOrder collection to search</param>
		/// <param name="ignoreReadingOrder">Ignore this reading order in the readingOrders collection</param>
		/// <param name="matchLeadRole">Choose any order that begins with this role. If defaultRoleOrder is also
		/// set and starts with this role and the order is defined, then use it.</param>
		/// <param name="matchAnyLeadRole">Same as matchAnyLeadRole, except with a set match</param>
		/// <param name="invertLeadRoles">Invert the matchLeadRole and matchAnyLeadRole values</param>
		/// <param name="noForwardText">Match a reading with no forward text if possible</param>
		/// <param name="defaultRoleOrder">The default order to match</param>
		/// <param name="allowAnyOrder">If true, use the first reading order if there are no other matches</param>
		/// <returns>A matching reading order. Can return null if allowAnyOrder is false, or the readingOrders collection is empty.</returns>
		public static ReadingOrder GetMatchingReadingOrder(ReadingOrderMoveableCollection readingOrders, ReadingOrder ignoreReadingOrder, Role matchLeadRole, RoleMoveableCollection matchAnyLeadRole, bool invertLeadRoles, bool noForwardText, RoleMoveableCollection defaultRoleOrder, bool allowAnyOrder)
		{
			// UNDONE: Implement noForwardText verification
			int orderCount = readingOrders.Count;
			ReadingOrder retVal = null;
			bool blockTestDefault = false; // If we have specific lead role requirements, then default is only used to enforce them, or as the default for any allowed order
			if (orderCount != 0)
			{
				int ignoreReadingOrderIndex = (ignoreReadingOrder == null) ? -1 : readingOrders.IndexOf(ignoreReadingOrder);
				if (ignoreReadingOrderIndex != -1 && orderCount == 1)
				{
					return null;
				}

				// Match a single lead role, prefer the default order
				if (matchLeadRole != null)
				{
					if (invertLeadRoles)
					{
						int matchAllCount = defaultRoleOrder.Count;
						for (int i = 0; i < matchAllCount; ++i)
						{
							Role currentRole = defaultRoleOrder[i];
							if (currentRole != matchLeadRole)
							{
								if (GetMatchingReadingOrder(readingOrders, ignoreReadingOrderIndex, currentRole, defaultRoleOrder, ref retVal))
								{
									break;
								}
							}
						}
					}
					else
					{
						GetMatchingReadingOrder(readingOrders, ignoreReadingOrderIndex, matchLeadRole, defaultRoleOrder, ref retVal);
					}
					if (retVal == null && matchAnyLeadRole == null)
					{
						blockTestDefault = !allowAnyOrder;
					}
				}

				if (retVal == null && matchAnyLeadRole != null)
				{
					int matchAnyCount = matchAnyLeadRole.Count;
					if (invertLeadRoles)
					{
						int matchAllCount = defaultRoleOrder.Count;
						if (matchAllCount > matchAnyCount)
						{
							for (int i = 0; i < matchAllCount; ++i)
							{
								Role currentRole = defaultRoleOrder[i];
								if (!matchAnyLeadRole.Contains(currentRole))
								{
									if (GetMatchingReadingOrder(readingOrders, ignoreReadingOrderIndex, currentRole, defaultRoleOrder, ref retVal))
									{
										break;
									}
								}
							}
						}
					}
					else
					{
						for (int i = 0; i < matchAnyCount; ++i)
						{
							if (GetMatchingReadingOrder(readingOrders, ignoreReadingOrderIndex, matchAnyLeadRole[i], defaultRoleOrder, ref retVal))
							{
								break;
							}
						}
					}
					if (retVal == null)
					{
						blockTestDefault = !allowAnyOrder;
					}
				}

				if (retVal == null && defaultRoleOrder != null && !blockTestDefault)
				{
					for (int i = 0; i < orderCount; ++i)
					{
						if (i == ignoreReadingOrderIndex)
						{
							continue;
						}
						ReadingOrder testOrder = readingOrders[i];
						RoleMoveableCollection testRoles = testOrder.RoleCollection;
						int testRolesCount = testRoles.Count;
						int j;
						for (j = 0; j < testRolesCount; ++j)
						{
							if (testRoles[j] != defaultRoleOrder[j])
							{
								break;
							}
						}
						if (j == testRolesCount)
						{
							retVal = testOrder;
							break;
						}
					}
				}

				if (retVal == null && allowAnyOrder)
				{
					retVal = readingOrders[(ignoreReadingOrderIndex == 0) ? 1 : 0];
				}
			}
			return retVal;
		}
		/// <summary>
		/// Helper function for public method of the same name
		/// </summary>
		/// <param name="readingOrders">The ReadingOrder collection to search</param>
		/// <param name="ignoreReadingOrderIndex">Ignore the reading order at this index</param>
		/// <param name="matchLeadRole">The role to match as a lead</param>
		/// <param name="defaultRoleOrder">The default role order. If not specified, any match will be considered optimal</param>
		/// <param name="matchingOrder">The matching order. Can be non-null to start with</param>
		/// <returns>true if an optimal match was found. retVal will be false if a match is found but
		/// a more optimal match is possible</returns>
		private static bool GetMatchingReadingOrder(ReadingOrderMoveableCollection readingOrders, int ignoreReadingOrderIndex, Role matchLeadRole, RoleMoveableCollection defaultRoleOrder, ref ReadingOrder matchingOrder)
		{
			int orderCount = readingOrders.Count;
			ReadingOrder testOrder;
			bool optimalMatch = false;
			RoleMoveableCollection testRoles;
			int testRolesCount;
			if (orderCount != 0)
			{
				if (matchLeadRole != null)
				{
					for (int i = 0; i < orderCount; ++i)
					{
						if (i == ignoreReadingOrderIndex)
						{
							continue;
						}
						testOrder = readingOrders[i];
						testRoles = testOrder.RoleCollection;
						if (testRoles[0] == matchLeadRole)
						{
							if (defaultRoleOrder != null)
							{
								int j;
								testRolesCount = testRoles.Count;
								for (j = 0; j < testRolesCount; ++j)
								{
									if (testRoles[j] != defaultRoleOrder[j])
									{
										break;
									}
								}
								if (j == testRolesCount)
								{
									matchingOrder = testOrder;
									optimalMatch = true;
									break;
								}
								if (matchingOrder == null)
								{
									matchingOrder = testOrder; // Remember the first one
								}
							}
							else
							{
								matchingOrder = testOrder;
								optimalMatch = true;
								break;
							}
						}
					}
				}
			}
			return optimalMatch;
		}
		/// <summary>
		/// Populate the predicate text with the supplied in the
		/// correct order.
		/// </summary>
		/// <param name="readingOrder">The order to populate. The predicate text is pulled from the primary reading.</param>
		/// <param name="defaultOrder">The default role order. Corresponds to the order of the role replacement fields</param>
		/// <param name="roleReplacements">The replacement fields. The length of the replacement array can be greater than
		/// the number of roles in the defaultOrder collection</param>
		/// <returns>The populated predicate text</returns>
		public static string PopulatePredicateText(ReadingOrder readingOrder, RoleMoveableCollection defaultOrder, string[] roleReplacements)
		{
			Reading reading = readingOrder.PrimaryReading;
			string retVal = null;
			if (reading != null)
			{
				string[] useReplacements = roleReplacements;
				int roleCount = defaultOrder.Count;
				RoleMoveableCollection readingRoles = readingOrder.RoleCollection;
				Debug.Assert(readingRoles.Count >= roleCount);
				// First, see if anything is out of order
				int i;
				for (i = 0; i < roleCount; ++i)
				{
					Role testRole = readingRoles[i];
					if (testRole != defaultOrder[i])
					{
						// Now we need a copy
						useReplacements = new string[roleCount];
						for (int j = 0; j < i; ++j)
						{
							useReplacements[j] = roleReplacements[j];
						}
						for (int j = i; j < roleCount; ++j)
						{
							useReplacements[j] = roleReplacements[defaultOrder.IndexOf(readingRoles[j])];
						}
						break;
					}
				}
				retVal = string.Format(CultureInfo.CurrentUICulture, reading.Text, useReplacements);
			}
			return retVal;
		}
		/// <summary>
		/// Match the first non whitespace/html character
		/// </summary>
		private static Regex FirstBodyCharacterPatternAny = new Regex(@"^(?:((<[^>]*?>)|\s)*?)(?<1>[^<\s])", RegexOptions.Compiled | RegexOptions.Singleline);
		/// <summary>
		/// Match the first non whitespace/html character, but only if it is lower case
		/// </summary>
		private static Regex FirstBodyCharacterPatternLower = new Regex(@"^(?:((<[^>]*?>)|\s)*?)(?<1>\p{Ll})", RegexOptions.Compiled | RegexOptions.Singleline);
		/// <summary>
		/// Match the last non whitespace/html character
		/// </summary>
		private static Regex LastBodyCharacterPattern = new Regex(@"(?<1>[^<\s])((<[^>]*?>)|\s)*?\z", RegexOptions.Compiled | RegexOptions.Singleline);
		/// <summary>
		/// Helper function for turning verbalizations into true sentences. Handles html and plain text
		/// body text.
		/// </summary>
		public static void WriteVerbalizerSentence(TextWriter writer, string body, string closeSentenceWith)
		{
			Match match = FirstBodyCharacterPatternLower.Match(body);
			if (match.Success)
			{
				Group group = match.Groups[1];
				if (group.Success)
				{
					int charIndex = group.Index;
					if (charIndex != 0)
					{
						writer.Write(body.Substring(0, charIndex));
					}
					writer.Write(char.ToUpper(body[charIndex], CultureInfo.CurrentUICulture));
					string trailingText = body.Substring(charIndex + 1);
					if (closeSentenceWith.Length != 0 && CloseSentence(writer, trailingText, closeSentenceWith))
					{
						return;
					}
					writer.Write(trailingText);
					return;
				}
			}
			else if (closeSentenceWith.Length != 0 && CloseSentence(writer, body, closeSentenceWith))
			{
				return;
			}
			writer.Write(body);
		}
		private static bool CloseSentence(TextWriter writer, string body, string closeSentenceWith)
		{
			// Note that the closeSentenceWith value must go inside any html tags with the last text
			// because these contain indentation styles which may be cleared, causing the sentence closure
			// to write in the wrong location
			Match match = LastBodyCharacterPattern.Match(body);
			if (match.Success)
			{
				string modifiedClose = closeSentenceWith;
				// We need to strip any html tags from around the sentence closure and compare the
				// contents, not the whole string
				int replaceLength = closeSentenceWith.Length;
				int modifiedReplaceLength = replaceLength;
				if (replaceLength > 1)
				{
					// UNDONE: Cache the last closure string, we'll be getting the same query every time
					Match closeStartMatch = FirstBodyCharacterPatternAny.Match(closeSentenceWith);
					if (closeStartMatch.Success)
					{
						Group closeStartGroup = closeStartMatch.Groups[1];
						if (closeStartGroup.Success)
						{
							Match closeLastMatch = LastBodyCharacterPattern.Match(closeSentenceWith);
							if (closeLastMatch.Success)
							{
								int startIndex = closeStartGroup.Index;
								int endIndex = closeLastMatch.Index;
								if (startIndex <= endIndex)
								{
									modifiedClose = closeSentenceWith.Substring(startIndex, endIndex - startIndex + 1);
									modifiedReplaceLength = modifiedClose.Length;
								}
							}
						}
					}
				}
				int charIndex = match.Index;
				if ((modifiedReplaceLength > charIndex) || (modifiedClose != body.Substring(charIndex - modifiedReplaceLength + 1, modifiedReplaceLength)))
				{
					if (charIndex != 0)
					{
						writer.Write(body.Substring(0, charIndex + 1));
					}
					writer.Write(closeSentenceWith);
					writer.Write(body.Substring(charIndex + 1));
					return true;
				}
			}
			return false;
		}
	}
	#endregion // Static verbalization helpers on FactType class
}