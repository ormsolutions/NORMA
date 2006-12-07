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
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Modeling;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region IVerbalize interface
	/// <summary>
	/// An enum representing the type of verbalization
	/// for a given element
	/// </summary>
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
	public delegate void NotifyBeginVerbalization(VerbalizationContent content);
	/// <summary>
	/// Interface for verbalization
	/// </summary>
	public interface IVerbalize
	{
		/// <summary>
		/// Verbalize in the requested form
		/// </summary>
		/// <param name="writer">The output text writer</param>
		/// <param name="snippetsDictionary">The IVerbalizationSets to use</param>
		/// <param name="beginVerbalization">A callback function to notify when verbalization is starting</param>
		/// <param name="isNegative">true for a negative reading</param>
		/// <returns>true to continue with child verbalization, otherwise false</returns>
		bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative);
	}
	/// <summary>
	/// Interface to redirect verbalization. Called for top-level selected objects only
	/// </summary>
	public interface IRedirectVerbalization
	{
		/// <summary>
		/// Use the returned object as the verbalizer
		/// </summary>
		IVerbalize SurrogateVerbalizer { get;}
	}
	#endregion // IVerbalize interface
	#region IVerbalizeChildren interface
	/// <summary>
	/// Implement this interface to let the verbalization engine
	/// automatically verbalize child elements without implementing
	/// IVerbalize. IVerbalizeChildren is ignored for the top-level
	/// verbalization object if IRedirectVerbalization is specified.
	/// </summary>
	public interface IVerbalizeChildren { }
	#endregion // IVerbalizeChildren interface
	#region CustomChildVerbalizer struct
	/// <summary>
	/// Structure to hold return information from the IVerbalizeFilterChildren.FilterChildVerbalizer
	/// and IVerbalizeCustomChildren.GetCustomChildVerbalizations methods
	/// </summary>
	public struct CustomChildVerbalizer : IEquatable<CustomChildVerbalizer>
	{
		private readonly IVerbalize myInstance;
		private readonly bool myOptions;
		/// <summary>
		/// Any empty VerbalizationFilterResult structure
		/// </summary>
		public readonly static CustomChildVerbalizer Empty = default(CustomChildVerbalizer);
		/// <summary>
		/// Test if the structure is empty
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return myInstance == null;
			}
		}
		/// <summary>
		/// Create an VerbalizationFilterResult structure
		/// </summary>
		/// <param name="instance">The resulting IVerbalize</param>
		/// <param name="options">Options specifying how the instance is used. true indicates
		/// the returned element should be disposed.</param>
		public CustomChildVerbalizer(IVerbalize instance, bool options)
		{
			myInstance = instance;
			myOptions = options;
		}
		/// <summary>
		/// Create an VerbalizationFilterResult structure
		/// </summary>
		/// <param name="instance">The resulting IVerbalize</param>
		public CustomChildVerbalizer(IVerbalize instance) : this(instance, false) { }
		/// <summary>
		/// The instance.
		/// </summary>
		public IVerbalize Instance
		{
			get
			{
				return myInstance;
			}
		}
		/// <summary>
		/// Options for using the instance
		/// </summary>
		public bool Options
		{
			get
			{
				return myOptions;
			}
		}
		#region Equality and casting routines
		/// <summary>
		/// Standard Equals override
		/// </summary>
		public override bool Equals(object obj)
		{
			return (obj is CustomChildVerbalizer) && Equals((CustomChildVerbalizer)obj);
		}
		/// <summary>
		/// Standard GetHashCode override
		/// </summary>
		public override int GetHashCode()
		{
			IVerbalize instance = myInstance;
			if (instance != null)
			{
				return Neumont.Tools.Modeling.Utility.GetCombinedHashCode(instance.GetHashCode(), myOptions.GetHashCode());
			}
			return 0;
		}
		/// <summary>
		/// Typed Equals method
		/// </summary>
		public bool Equals(CustomChildVerbalizer obj)
		{
			return myInstance == obj.myInstance && myOptions == obj.myOptions;
		}
		/// <summary>
		/// Equality operator
		/// </summary>
		public static bool operator ==(CustomChildVerbalizer left, CustomChildVerbalizer right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Inequality operator
		/// </summary>
		public static bool operator !=(CustomChildVerbalizer left, CustomChildVerbalizer right)
		{
			return !left.Equals(right);
		}
		#endregion // Equality and casting routines
	}
	#endregion // CustomChildVerbalizer struct
	#region IVerbalizeCustomChildren interface
	/// <summary>
	/// Implement to verbalize children that are not naturally aggregated
	/// </summary>
	public interface IVerbalizeCustomChildren
	{
		/// <summary>
		/// Retrieve children to verbalize that are not part of the standard
		/// verbalization.
		/// </summary>
		/// <param name="isNegative">true if a negative verbalization is being requested</param>
		/// <returns>IEnumerable of CustomChildVerbalizer structures</returns>
		IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(bool isNegative);
	}
	#endregion // IVerbalizeCustomChildren interface
	#region IVerbalizeFilterChildren interface
	/// <summary>
	/// Implement to remove or provide an alternate verbalization for
	/// aggregated children that are naturally verbalized.
	/// </summary>
	public interface IVerbalizeFilterChildren
	{
		/// <summary>
		/// Provides an opportunity for a parent object to filter the
		/// verbalization of aggregated child verbalization implementations
		/// </summary>
		/// <param name="childVerbalizer"></param>
		/// <param name="isNegative">true if a negative verbalization is being requested</param>
		/// <returns>Return the provided childVerbalizer to verbalize normally, null to block verbalization, or an
		/// alternate IVerbalize. The value is returned with a boolean option. The element will be disposed with
		/// this is true.</returns>
		CustomChildVerbalizer FilterChildVerbalizer(IVerbalize childVerbalizer, bool isNegative);
	}
	#endregion // IVerbalizeFilterChildren interface
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
		/// <param name="matchAnyLeadRole">Same as matchAnyLeadRole, except with a set match. An IList of RoleBase elements.</param>
		/// <param name="invertLeadRoles">Invert the matchLeadRole and matchAnyLeadRole values</param>
		/// <param name="noFrontText">Match a reading with no front text if possible</param>
		/// <param name="defaultRoleOrder">The default order to match</param>
		/// <param name="allowAnyOrder">If true, use the first reading order if there are no other matches</param>
		/// <returns>A matching reading order. Can return null if allowAnyOrder is false, or the readingOrders collection is empty.</returns>
		public static Reading GetMatchingReading(LinkedElementCollection<ReadingOrder> readingOrders, ReadingOrder ignoreReadingOrder, RoleBase matchLeadRole, IList matchAnyLeadRole, bool invertLeadRoles, bool noFrontText, LinkedElementCollection<RoleBase> defaultRoleOrder, bool allowAnyOrder)
		{
			int orderCount = readingOrders.Count;
			Reading retVal = null;
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
							RoleBase currentRole = defaultRoleOrder[i];
							if (currentRole != matchLeadRole)
							{
								if (GetMatchingReading(readingOrders, ignoreReadingOrderIndex, currentRole, defaultRoleOrder, noFrontText, !allowAnyOrder, ref retVal))
								{
									break;
								}
							}
						}
					}
					else
					{
						GetMatchingReading(readingOrders, ignoreReadingOrderIndex, matchLeadRole, defaultRoleOrder, noFrontText, !allowAnyOrder, ref retVal);
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
								RoleBase currentRole = defaultRoleOrder[i];
								if (!matchAnyLeadRole.Contains(currentRole.Role))
								{
									if (GetMatchingReading(readingOrders, ignoreReadingOrderIndex, currentRole, defaultRoleOrder, noFrontText, !allowAnyOrder, ref retVal))
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
							if (GetMatchingReading(readingOrders, ignoreReadingOrderIndex, (RoleBase)matchAnyLeadRole[i], defaultRoleOrder, noFrontText, !allowAnyOrder, ref retVal))
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
						LinkedElementCollection<RoleBase> testRoles = testOrder.RoleCollection;
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
							retVal = testOrder.PrimaryReading;
							break;
						}
					}
				}

				if (retVal == null && allowAnyOrder)
				{
					retVal = readingOrders[(ignoreReadingOrderIndex == 0) ? 1 : 0].PrimaryReading;
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
		/// <param name="testNoFrontText">Test for no front text if true.</param>
		/// <param name="requireNoFrontText">Ignored if testNoFrontText is false. Otherwise, do not set matchingReading if frontText not satisfied</param>
		/// <param name="matchingReading">The matching reading. Can be non-null to start with</param>
		/// <returns>true if an optimal match was found. retVal will be false if a match is found but
		/// a more optimal match is possible</returns>
		private static bool GetMatchingReading(LinkedElementCollection<ReadingOrder> readingOrders, int ignoreReadingOrderIndex, RoleBase matchLeadRole, LinkedElementCollection<RoleBase> defaultRoleOrder, bool testNoFrontText, bool requireNoFrontText, ref Reading matchingReading)
		{
			ReadingOrder matchingOrder = null;
			int orderCount = readingOrders.Count;
			ReadingOrder testOrder;
			bool optimalMatch = false;
			LinkedElementCollection<RoleBase> testRoles;
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
						testRolesCount = testRoles.Count;
						if (testRolesCount != 0 && testRoles[0] == matchLeadRole)
						{
							if (defaultRoleOrder != null)
							{
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
			if (matchingOrder != null)
			{
				if (!testNoFrontText)
				{
					matchingReading = matchingOrder.PrimaryReading;
				}
				else
				{
					LinkedElementCollection<Reading> readings = matchingOrder.ReadingCollection;
					Reading noFrontTextReading = null;
					int readingCount = readings.Count;
					for (int i = 0; i < readingCount; ++i)
					{
						Reading testReading = readings[i];
						if (testReading.Text.StartsWith("{0}", StringComparison.Ordinal))
						{
							noFrontTextReading = testReading;
							break;
						}
					}
					if (noFrontTextReading != null)
					{
						matchingReading = noFrontTextReading;
					}
					else if (requireNoFrontText)
					{
						optimalMatch = false;
					}
					else
					{
						matchingReading = readings[0];
						optimalMatch = false;
					}
				}
			}
			return optimalMatch;
		}
		/// <summary>
		/// Helper function to reliably return the index of a role in a fact.
		/// </summary>
		/// <param name="factRoles"></param>
		/// <param name="role"></param>
		/// <returns></returns>
		/// <remarks>The role collection of a FactType is a RoleBase collection, but
		/// all constraint role collections are made up of Role. Without overriding
		/// the equality operator (Equals method, etc) (undesirable because we often
		/// do need to know the difference), this means that factRoles.IndexOf(role)
		/// will return a false negative, so we write our own helper function.</remarks>
		public static int IndexOfRole(LinkedElementCollection<RoleBase> factRoles, Role role)
		{
			int roleCount = factRoles.Count;
			for (int i = 0; i < roleCount; ++i)
			{
				if (factRoles[i].Role == role)
				{
					return i;
				}
			}
			return -1;
		}
		/// <summary>
		/// Populate the predicate text with the supplied replacement fields.
		/// </summary>
		/// <param name="reading">The reading to populate.</param>
		/// <param name="defaultOrder">The default role order. Corresponds to the order of the role replacement fields</param>
		/// <param name="roleReplacements">The replacement fields. The length of the replacement array can be greater than
		/// the number of roles in the defaultOrder collection</param>
		/// <returns>The populated predicate text</returns>
		public static string PopulatePredicateText(Reading reading, LinkedElementCollection<RoleBase> defaultOrder, string[] roleReplacements)
		{
			string retVal = null;
			if (reading != null)
			{
				string[] useReplacements = roleReplacements;
				int roleCount = defaultOrder.Count;
				LinkedElementCollection<RoleBase> readingRoles = reading.ReadingOrder.RoleCollection;
				Debug.Assert(readingRoles.Count >= roleCount);
				// First, see if anything is out of order
				int i;
				for (i = 0; i < roleCount; ++i)
				{
					RoleBase testRole = readingRoles[i];
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
				try
				{
					retVal = string.Format(CultureInfo.CurrentCulture, reading.Text, useReplacements);
				}
				catch (FormatException ex)
				{
					// UNDONE: Localize
					retVal = string.Format(CultureInfo.CurrentCulture, "{0} ({1})", reading.Text, ex.Message);
				}
			}
			return retVal;
		}
		/// <summary>
		/// Match the first non whitespace/html character
		/// </summary>
		private static readonly Regex FirstBodyCharacterPatternAny = new Regex(@"^(?:((<[^>]*?>)|\s)*?)(?<1>[^<\s])", RegexOptions.Compiled | RegexOptions.Singleline);
		/// <summary>
		/// Match the first non whitespace/html character, but only if it is lower case
		/// </summary>
		private static readonly Regex FirstBodyCharacterPatternLower = new Regex(@"^(?:((<[^>]*?>)|\s)*?)(?<1>\p{Ll})", RegexOptions.Compiled | RegexOptions.Singleline);
		/// <summary>
		/// Match the last non whitespace/html character
		/// </summary>
		private static readonly Regex LastBodyCharacterPattern = new Regex(@"(?<1>[^<\s])((<[^>]*?>)|\s)*?\z", RegexOptions.Compiled | RegexOptions.Singleline);
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
					writer.Write(char.ToUpper(body[charIndex], CultureInfo.CurrentCulture));
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
	#region VerbalizationHyphenBinder struct
	/// <summary>
	/// A helper structure to enable hyphen binding
	/// </summary>
	public struct VerbalizationHyphenBinder
	{
		#region Member Variables
		/// <summary>
		/// The reading text modified for verbalization. This will
		/// always be set if there are any hyphens in the reading's
		/// format text, and the replacement fields will always correspond
		/// to the default fact order.
		/// </summary>
		private string myModifiedReadingText;
		/// <summary>
		/// An array of format strings for individual roles
		/// </summary>
		private string[] myFormatReplacementFields;
		/// <summary>
		/// A regex pattern to 
		/// </summary>
		private static Regex myMainRegex;
		private static Regex myIndexMapRegex;
		#endregion // Member Variables
        #region Regex properties
        private static Regex MainRegex
        {
            get
            {
                #region Commented main regex pattern
                //            string mainPatternCommented = @"(?xn)
                //\G
                //# Test if there is a hyphen binding match before the next format replacement field
                //(?(.*?\S-\s.*?(?<!\{)\{\d+\}(?!\}))
                //	# If there is a hyphen bind before the next replacement field then use it
                //	((?<BeforeLeftHyphenWord>.*?\s??)(?<LeftHyphenWord>\S+?)-(?<AfterLeftHyphen>\s.*?))
                //	|
                //	# Otherwise, pick up all text before the next format replacement field
                //	((?<BeforeLeftHyphenWord>.*?))
                //)
                //# Get the format replacement field
                //((?<!\{)\{)(?<ReplaceIndex>\d+)(\}(?!\}))
                //# Get any trailing information if it exists prior to the next format field
                //(
                //	(?=
                //		# Positive lookahead to see if there is a next format string
                //		(?(.+(?<!\{)\{\d+\}(?!\}))
                //			# Check before if there is a next format string
                //			(((?!(?<!\{)\{\d+\}(?!\})).)*?\s-\S.*?(?<!\{)\{\d+\}(?!\}))
                //			|
                //			# Get any trailer if there is not a next format string
                //			([^\-]*?\s-\S.*?)
                //		)
                //	)
                //	# Get the before hyphen and right hyphen word if the look ahead succeeded
                //	(?<BeforeRightHyphen>.*?\s+?)-(?<RightHyphenWord>\S+)
                //)?";
                #endregion // Commented main regex pattern
                Regex regexMain = myMainRegex;
                if (regexMain == null)
                {
                    System.Threading.Interlocked.CompareExchange<Regex>(
                        ref myMainRegex,
                        new Regex(
                            @"(?n)\G(?(.*?\S-\s.*?(?<!\{)\{\d+\}(?!\}))((?<BeforeLeftHyphenWord>.*?\s??)(?<LeftHyphenWord>\S+?)-(?<AfterLeftHyphen>\s.*?))|((?<BeforeLeftHyphenWord>.*?)))((?<!\{)\{)(?<ReplaceIndex>\d+)(\}(?!\}))((?=(?(.+(?<!\{)\{\d+\}(?!\}))(((?!(?<!\{)\{\d+\}(?!\})).)*?\s-\S.*?(?<!\{)\{\d+\}(?!\}))|([^\-]*?\s-\S.*?)))(?<BeforeRightHyphen>.*?\s+?)-(?<RightHyphenWord>\S+))?",
                            RegexOptions.Compiled),
                        null);
                    regexMain = myMainRegex;
                }
                return regexMain;
            }
        }
        private static Regex IndexMapRegex
        {
            get
            {
                Regex regexIndexMap = myIndexMapRegex;
                if (regexIndexMap == null)
                {
                    System.Threading.Interlocked.CompareExchange<Regex>(
                        ref myIndexMapRegex,
                        new Regex(
                            @"(?n)((?<!\{)\{)(?<ReplaceIndex>\d+)(\}(?!\}))",
                            RegexOptions.Compiled),
                        null);
                    regexIndexMap = myIndexMapRegex;
                }
                return regexIndexMap;
            }
        }
        #endregion // Regex properties
        #region Constructor
        /// <summary>
		/// Initialize a structure to hyphen-bind the verbalization for a reading
		/// </summary>
		/// <param name="reading">The reading to test.</param>
		/// <param name="defaultOrder">The roles from the parent fact type. Provides the order of the expected replacement fields.</param>
		/// <param name="replacementFormatString">The string used to format replacement fields. The format string is used to build another
		/// format string with one replacement field. It must consist of a {{0}} representing the eventual replacement field, a {0} for the leading
		/// hyphen-bound text, and a {1} for the trailing hyphen-bound text.</param>
		public VerbalizationHyphenBinder(Reading reading, LinkedElementCollection<RoleBase> defaultOrder, string replacementFormatString)
		{
			string readingText;

			// First test if there is any hyphen to look for
			if (reading == null ||
				-1 == (readingText = reading.Text).IndexOf('-'))
			{
				myModifiedReadingText = null;
				myFormatReplacementFields = null;
				return;
			}
			
			// Now see the reading has the same order as the fact. If not,
			// create an indexMap array that maps the reading role order to
			// the fact role order.
			int roleCount = defaultOrder.Count;
			LinkedElementCollection<RoleBase> readingRoles = reading.ReadingOrder.RoleCollection;
			Debug.Assert(readingRoles.Count == roleCount);
			int[] indexMap = null;
			int firstIndexChange = -1;
			for (int i = 0; i < roleCount; ++i)
			{
				RoleBase readingRole = readingRoles[i];
				if (readingRole == defaultOrder[i])
				{
					if (indexMap != null)
					{
						indexMap[i] = i;
					}
					continue;
				}
				if (indexMap == null)
				{
					indexMap = new int[roleCount];
					// Catch up to where we are now
					for (int j = 0; j < i; ++j)
					{
						indexMap[j] = j;
					}
					firstIndexChange = i;
				}
				for (int j = firstIndexChange; j < roleCount; ++j)
				{
					if (readingRole == defaultOrder[j])
					{
						indexMap[i] = j;
						break;
					}
				}
			}

			// Make sure the regex objects are initialied
			Regex regexMain = MainRegex;
			Regex regexIndexMap = IndexMapRegex;

			// Build the new format string and do index mapping along the way
			IFormatProvider formatProvider = CultureInfo.CurrentCulture;
			string[] hyphenBoundFormatStrings = null;
			myModifiedReadingText = regexMain.Replace(
				readingText,
				delegate(Match match)
				{
					string retVal;
					GroupCollection groups = match.Groups;
					string stringReplaceIndex = groups["ReplaceIndex"].Value;
					int replaceIndex = int.Parse(stringReplaceIndex, formatProvider);
					string leftWord = groups["LeftHyphenWord"].Value;
					string rightWord = groups["RightHyphenWord"].Value;
					string leadText = groups["BeforeLeftHyphenWord"].Value;
					if (leftWord.Length != 0 || rightWord.Length != 0)
					{
						bool validIndex = replaceIndex < roleCount;
						if (validIndex)
						{
							string boundFormatter = string.Format(formatProvider, replacementFormatString, leftWord + groups["AfterLeftHyphen"].Value, groups["BeforeRightHyphen"].Value + rightWord);
							if (hyphenBoundFormatStrings == null)
							{
								hyphenBoundFormatStrings = new string[roleCount];
							}
							if (indexMap != null)
							{
								replaceIndex = indexMap[replaceIndex];
							}
							hyphenBoundFormatStrings[replaceIndex] = boundFormatter;
						}
						if (leadText.Length != 0 && indexMap != null)
						{
							leadText = regexIndexMap.Replace(
								leadText,
								delegate(Match innerMatch)
								{
									int innerReplaceIndex = int.Parse(innerMatch.Groups["ReplaceIndex"].Value, formatProvider);
									return (innerReplaceIndex < roleCount) ?
										string.Concat("{", indexMap[innerReplaceIndex].ToString(formatProvider), "}") :
										string.Concat("{{", innerReplaceIndex.ToString(formatProvider), "}}");
								});
						}
						retVal = string.Concat(
							leadText,
							validIndex ? "{" : "{{",
							(indexMap == null) ? stringReplaceIndex : replaceIndex.ToString(formatProvider),
							validIndex ? "}" : "}}");
					}
					else if (indexMap != null)
					{
						retVal = regexIndexMap.Replace(
							match.Value,
							delegate(Match innerMatch)
							{
								int innerReplaceIndex = int.Parse(innerMatch.Groups["ReplaceIndex"].Value, formatProvider);
								return (innerReplaceIndex < roleCount) ?
									string.Concat("{", indexMap[innerReplaceIndex].ToString(formatProvider), "}") :
									string.Concat("{{", innerReplaceIndex.ToString(formatProvider), "}}");
							});
					}
					else
					{
						retVal = match.Value;
					}
					return retVal;
				});
			myFormatReplacementFields = hyphenBoundFormatStrings;
		}
		#endregion // Constructor
		#region Member Functions
		/// <summary>
		/// Perform any necessary hyphen-binding on the provided role replacement field
		/// </summary>
		/// <param name="basicRoleReplacement">The basic replacement field. Generally consists of a formatted object name.</param>
		/// <param name="roleIndex">The index of the represented role in the fact order</param>
		/// <returns>A modified replacement</returns>
		public string HyphenBindRoleReplacement(string basicRoleReplacement, int roleIndex)
		{
			string[] formatFields = myFormatReplacementFields;
			string formatField;
			if (formatFields != null &&
				roleIndex < formatFields.Length &&
				null != (formatField = formatFields[roleIndex]))
			{
				return string.Format(CultureInfo.CurrentCulture, formatField, basicRoleReplacement);
			}
			return basicRoleReplacement;
		}
		/// <summary>
		/// Populate the predicate text with the supplied replacement fields. Defers to
		/// FactType.PopulatePredicateText if no hyphen-bind occurred
		/// </summary>
		/// <param name="reading">The reading to populate.</param>
		/// <param name="defaultOrder">The default role order. Corresponds to the order of the role replacement fields</param>
		/// <param name="roleReplacements">The replacement fields</param>
		/// <param name="unmodifiedRoleReplacements">The roleReplacements array have not been modified with the HyphenBindRoleReplacement method</param>
		/// <returns>The populated predicate text</returns>
		public string PopulatePredicateText(Reading reading, LinkedElementCollection<RoleBase> defaultOrder, string[] roleReplacements, bool unmodifiedRoleReplacements)
		{
			string formatText = myModifiedReadingText;
			if (formatText == null)
			{
				return FactType.PopulatePredicateText(reading, defaultOrder, roleReplacements);
			}
			else
			{
				string[] useRoleReplacements = roleReplacements;
				string[] formatFields;
				if (unmodifiedRoleReplacements &&
					(null != (formatFields = myFormatReplacementFields)))
				{
					IFormatProvider formatProvider = CultureInfo.CurrentCulture;
					int count = formatFields.Length;
					useRoleReplacements = new string[count];
					for (int i = 0; i < count; ++i)
					{
						string useFormat = formatFields[i];
						useRoleReplacements[i] = (useFormat == null) ? roleReplacements[i] : string.Format(formatProvider, useFormat, roleReplacements[i]);
					}
				}
				return string.Format(CultureInfo.CurrentCulture, formatText, useRoleReplacements);
			}
		}
		#endregion // Member Functions
        #region Static Functions
        /// <summary>
        /// Determines whether or not the given predicate is hyphen bound.
        /// </summary>
        /// <param name="reading">The reading to test.</param>
        /// <returns>True if the predicate is hyphen bound</returns>
        public static bool IsHyphenBound(Reading reading)
        {
            string readingText;

            // First test if there is any hyphen to look for
            if (reading == null ||
                -1 == (readingText = reading.Text).IndexOf('-'))
            {
                return false;
            }

            Match match = MainRegex.Match(readingText);
            while (match.Success)
            {
                GroupCollection groups = match.Groups;
                string leftWord = groups["LeftHyphenWord"].Value;
                string rightWord = groups["RightHyphenWord"].Value;
                if (leftWord.Length != 0 || rightWord.Length != 0)
                {
                    return true;
                }
                match = match.NextMatch();
            }
            return false;
        }
        #endregion
    }
	#endregion // VerbalizationHyphenBinder struct
}
