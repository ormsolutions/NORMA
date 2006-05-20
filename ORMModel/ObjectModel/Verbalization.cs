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
				return instance.GetHashCode() ^ RotateRight(myOptions.GetHashCode(), 1);
			}
			return 0;
		}
		private static int RotateRight(int value, int places)
		{
			places = places & 0x1F;
			if (places == 0)
			{
				return value;
			}
			int mask = ~0x7FFFFFF >> (places - 1);
			return ((value >> places) & ~mask) | ((value << (32 - places)) & mask);
		}
		/// <summary>
		/// Typed Equals method
		/// </summary>
		public bool Equals(CustomChildVerbalizer obj)
		{
			return object.ReferenceEquals(myInstance, obj.myInstance) && myOptions == obj.myOptions;
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
		/// <param name="noForwardText">Match a reading with no forward text if possible</param>
		/// <param name="defaultRoleOrder">The default order to match</param>
		/// <param name="allowAnyOrder">If true, use the first reading order if there are no other matches</param>
		/// <returns>A matching reading order. Can return null if allowAnyOrder is false, or the readingOrders collection is empty.</returns>
		public static Reading GetMatchingReading(ReadingOrderMoveableCollection readingOrders, ReadingOrder ignoreReadingOrder, RoleBase matchLeadRole, IList matchAnyLeadRole, bool invertLeadRoles, bool noForwardText, RoleBaseMoveableCollection defaultRoleOrder, bool allowAnyOrder)
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
							RoleBase currentRole = defaultRoleOrder[i];
							if (currentRole != matchLeadRole)
							{
								if (GetMatchingReading(readingOrders, ignoreReadingOrderIndex, currentRole, defaultRoleOrder, ref retVal))
								{
									break;
								}
							}
						}
					}
					else
					{
						GetMatchingReading(readingOrders, ignoreReadingOrderIndex, matchLeadRole, defaultRoleOrder, ref retVal);
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
									if (GetMatchingReading(readingOrders, ignoreReadingOrderIndex, currentRole, defaultRoleOrder, ref retVal))
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
							if (GetMatchingReading(readingOrders, ignoreReadingOrderIndex, (RoleBase)matchAnyLeadRole[i], defaultRoleOrder, ref retVal))
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
						RoleBaseMoveableCollection testRoles = testOrder.RoleCollection;
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
			return (retVal != null) ? retVal.PrimaryReading : null;
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
		private static bool GetMatchingReading(ReadingOrderMoveableCollection readingOrders, int ignoreReadingOrderIndex, RoleBase matchLeadRole, RoleBaseMoveableCollection defaultRoleOrder, ref ReadingOrder matchingOrder)
		{
			int orderCount = readingOrders.Count;
			ReadingOrder testOrder;
			bool optimalMatch = false;
			RoleBaseMoveableCollection testRoles;
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
						if (testRolesCount != 0 && object.ReferenceEquals(testRoles[0], matchLeadRole))
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
			return optimalMatch;
		}
		/// <summary>
		/// Populate the predicate text with the supplied in the
		/// correct order.
		/// </summary>
		/// <param name="reading">The reading to populate.</param>
		/// <param name="defaultOrder">The default role order. Corresponds to the order of the role replacement fields</param>
		/// <param name="roleReplacements">The replacement fields. The length of the replacement array can be greater than
		/// the number of roles in the defaultOrder collection</param>
		/// <returns>The populated predicate text</returns>
		public static string PopulatePredicateText(Reading reading, RoleBaseMoveableCollection defaultOrder, string[] roleReplacements)
		{
			string retVal = null;
			if (reading != null)
			{
				string[] useReplacements = roleReplacements;
				int roleCount = defaultOrder.Count;
				RoleBaseMoveableCollection readingRoles = reading.ReadingOrder.RoleCollection;
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
					retVal = string.Format(CultureInfo.InvariantCulture, reading.Text, useReplacements);
				}
				catch (FormatException ex)
				{
					// UNDONE: Localize
					retVal = string.Format(CultureInfo.InvariantCulture, "{0} ({1})", reading.Text, ex.Message);
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
