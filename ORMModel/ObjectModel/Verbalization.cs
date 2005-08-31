using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region Hand code
	/// <summary>
	/// Interface for verbalization
	/// </summary>
	public interface IVerbalize
	{
		/// <summary>
		/// Verbalize in the requested form
		/// </summary>
		/// <param name="isNegative">true for a negative reading</param>
		/// <returns>string verbalization</returns>
		string GetVerbalization(bool isNegative);
	}
	public partial class FactType
	{
		/// <summary>
		/// Helper function to get a matching reading order. The match priority is
		/// specified by the parameter order
		/// </summary>
		/// <param name="readingOrders">The ReadingOrder collection to search</param>
		/// <param name="matchLeadRole">Choose any order that begins with this role. If defaultRoleOrder is also
		/// set and starts with this role and the order is defined, then use it.</param>
		/// <param name="matchAnyLeadRole">Same as matchAnyLeadRole, except with a set match</param>
		/// <param name="defaultRoleOrder">The default order to match</param>
		/// <param name="allowAnyOrder">If true, use the first reading order if there are no other matches</param>
		/// <returns>A matching reading order. Can return null if allowAnyOrder is false, or the readingOrders collection is empty.</returns>
		public static ReadingOrder GetMatchingReadingOrder(ReadingOrderMoveableCollection readingOrders, Role matchLeadRole, RoleMoveableCollection matchAnyLeadRole, RoleMoveableCollection defaultRoleOrder, bool allowAnyOrder)
		{
			int orderCount = readingOrders.Count;
			ReadingOrder retVal = null;
			bool blockTestDefault = false; // If we have specific lead role requirements, then default is only used to enforce them, or as the default for any allowed order
			if (orderCount != 0)
			{
				// Match a single lead role, prefer the default order
				if (matchLeadRole != null)
				{
					GetMatchingReadingOrder(readingOrders, matchLeadRole, defaultRoleOrder, ref retVal);
					if (retVal == null && matchAnyLeadRole == null)
					{
						blockTestDefault = !allowAnyOrder;
					}
				}

				if (retVal == null && matchAnyLeadRole != null)
				{
					int matchAnyCount = matchAnyLeadRole.Count;
					for (int i = 0; i < matchAnyCount; ++i)
					{
						if (GetMatchingReadingOrder(readingOrders, matchAnyLeadRole[i], defaultRoleOrder, ref retVal))
						{
							break;
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
					retVal = readingOrders[0];
				}
			}
			return retVal;
		}
		/// <summary>
		/// Helper function for public method of the same name
		/// </summary>
		/// <param name="readingOrders">The ReadingOrder collection to search</param>
		/// <param name="matchLeadRole">The role to match as a lead</param>
		/// <param name="defaultRoleOrder">The default role order. If not specified, any match will be considered optimal</param>
		/// <param name="matchingOrder">The matching order. Can be non-null to start with</param>
		/// <returns>true if an optimal match was found. retVal will be false if a match is found but
		/// a more optimal match is possible</returns>
		private static bool GetMatchingReadingOrder(ReadingOrderMoveableCollection readingOrders, Role matchLeadRole, RoleMoveableCollection defaultRoleOrder, ref ReadingOrder matchingOrder)
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
		/// <param name="roleReplacements">The replacement fields.</param>
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
				Debug.Assert(readingRoles.Count == roleCount);
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
	}
	#endregion // Hand code
	#region Generatable code
#if ALREADYGENERATED
	/// <summary>
	/// The type of text snippet being requested
	/// </summary>
	public enum VerbalizationTextSnippetType
	{
		/// <summary/>
		UniversalQuantifier,
		/// <summary/>
		ImpersonalPronoun,
		/// <summary/>
		AtMostOneQuantifier,
		/// <summary/>
		EachInstanceQuantifier,
		/// <summary/>
		GivenAnyQuantifier,
		/// <summary/>
		ExistentialQuantifier,
		/// <summary/>
		IdentifyReferenceQuantifier,
		/// <summary/>
		MoreThanOneQuantifier,
		/// <summary/>
		ModalPossibilityOperator,
		/// <summary/>
		ModalNecessityOperator,
	}
	/// <summary>
	/// A single verbalization set, contains one snippet for each snippet type
	/// </summary>
	public struct VerbalizationSet
	{
		private string[] mySnippets;
		/// <summary>
		/// Construct a verbalization set with the following snippets
		/// </summary>
		/// <param name="snippets">An array of strings corresponding to VerbalizationTextSnippetType</param>
		public VerbalizationSet(string[] snippets)
		{
			mySnippets = snippets;
		}
		/// <summary>
		/// Get a snippet for the specified snippet type
		/// </summary>
		/// <param name="snippetType">The type of the requested snippet</param>
		/// <returns>The corresponding string</returns>
		public string GetSnippet(VerbalizationTextSnippetType snippetType)
		{
			return mySnippets[(int)snippetType];
		}
	}
	/// <summary>
	/// A group of verbalization sets, contains one verbalization set for each
	/// sign/modality combination.
	/// </summary>
	public class VerbalizationSets
	{
		/// <summary>
		/// Very temporary. Snippets will be loaded from disk dynamically
		/// </summary>
		public static VerbalizationSets Default = new VerbalizationSets();
		// Not used
		//private enum VerbalizationSetStyle
		//{
		//    PositiveAlethic = 0,
		//    PositiveDeontic = 1,
		//    NegativeAlethic = 2,
		//    NegativeDeontic = 3,
		//}
		private VerbalizationSet[] mySets;
		/// <summary>
		/// Retrieve the text snippet for the given snippet type, modality, and sign
		/// </summary>
		/// <param name="snippetType">The required snippet type</param>
		/// <param name="isDeontic">true for a deontic modality</param>
		/// <param name="isNegative">true for negative verbalization</param>
		/// <returns>A snippet suitable for use as a format string</returns>
		public string GetSnippet(VerbalizationTextSnippetType snippetType, bool isDeontic, bool isNegative)
		{
			int setIndex = (isDeontic ? 1 : 0) + (isNegative ? 2 : 0);
			if (mySets == null)
			{
				mySets = new VerbalizationSet[]{
					new VerbalizationSet(new string[]{"each {0}","that {0}","at most one {0}","each instance of {0} occurs only once","given any {0} {1}","some {0}","the same {0}","more than one {0}","it is possible that {0}","it is necessary that {0}"}),
					new VerbalizationSet(new string[]{"each {0}","that {0}","at most one {0}","each instance of {0} occurs only once","given any {0} {1}","some {0}","the same {0}","more than one {0}","it is permitted that {0}","it is obligatory that {0}"}),
					new VerbalizationSet(new string[]{"each {0}","that {0}","at most one {0}","each instance of {0} occurs only once","given any {0} {1}","some {0}","the same {0}","more than one {0}","it is impossible that {0}","it is necessary that {0}"}),
					new VerbalizationSet(new string[]{"each {0}","that {0}","at most one {0}","each instance of {0} occurs only once","given any {0} {1}","some {0}","the same {0}","more than one {0}","it is forbidden that {0}","it is obligatory that {0}"}),
				};
			}
			return mySets[setIndex].GetSnippet(snippetType);
		}
	}
	public partial class InternalUniquenessConstraint : IVerbalize
	{
		#region IVerbalize implementation
		/// <summary>
		/// Implements IVerbalize.GetVerbalization
		/// </summary>
		protected string GetVerbalization(bool isNegative)
		{
			// First, see if we have errors. If there are errors
			// then the verbalization is a list of the error text.
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			StringBuilder sbMain = null;
			StringBuilder sbTemp = null;
			if (errorOwner != null)
			{
				foreach (ModelError error in errorOwner.ErrorCollection)
				{
					if (sbMain == null)
					{
						sbMain = new StringBuilder();
					}
					else
					{
						sbMain.Append(Environment.NewLine);
					}
					sbMain.Append(error.Name);
				}
				if (sbMain != null)
				{
					return sbMain.ToString();
				}
			}

			VerbalizationSets snippets = VerbalizationSets.Default;
			bool isDeontic = false;
			FactType parentFact = FactType;
			RoleMoveableCollection includedRoles = RoleCollection;
			RoleMoveableCollection allRoles = parentFact.RoleCollection;
			ReadingOrderMoveableCollection allReadingOrders = parentFact.ReadingOrderCollection;
			if (allReadingOrders.Count == 0)
			{
				return "";
			}
			int includedArity = includedRoles.Count;
			int fullArity = allRoles.Count;
			string[] basicRoleReplacements = new string[fullArity];
			for (int i = 0; i < fullArity; ++i)
			{
				ObjectType rolePlayer = allRoles[i].RolePlayer;
				// UNDONE: Ring situations
				// UNDONE: Localize or pull the role name from the snippet set
				string basicReplacement = (rolePlayer != null) ? rolePlayer.Name : "Role" + (i + 1).ToString(CultureInfo.InvariantCulture);
				basicRoleReplacements[i] = basicReplacement;
			}
			sbMain = new StringBuilder();
			ReadingOrder readingOrder;
			if (includedArity == fullArity && fullArity == 2)
			{
				string[] roleReplacements = new string[fullArity];
				string snippet0 = snippets.GetSnippet(VerbalizationTextSnippetType.EachInstanceQuantifier, isDeontic, isNegative);
				readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, allRoles[0], null, allRoles, true);
				string replace0_0 = FactType.PopulatePredicateText(readingOrder, allRoles, basicRoleReplacements);
				sbMain.AppendFormat(CultureInfo.CurrentUICulture, snippet0, replace0_0);
				sbMain.AppendLine();
				string snippet1 = snippets.GetSnippet(VerbalizationTextSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (int i = 0; i < fullArity; ++i)
				{
					// Append a line and indent because of the combinationStyle on the IterateRoles tag
					if (i == 0)
					{
						sbTemp.AppendLine();
					}
					else
					{
						sbTemp.AppendLine(" and");// UNDONE: the and should be part of the snippet set
					}
					sbTemp.Append('\t'); // UNDONE: The indent should be part of the snippet set
					Role primaryRole = allRoles[i];
					readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, primaryRole, null, allRoles, true);
					for (int j = 0; j < fullArity; ++j)
					{
						Role currentRole = allRoles[j];
						string roleReplacement = null;
						string basicReplacement = basicRoleReplacements[j];
						if (primaryRole == currentRole)
						{
							roleReplacement = string.Format(CultureInfo.CurrentUICulture, snippets.GetSnippet(VerbalizationTextSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative), basicReplacement);
						}
						else // We're walking the full set here, so else is sufficient. With a match="included", we'd only do this for roles in the included list
						{
							roleReplacement = string.Format(CultureInfo.CurrentUICulture, snippets.GetSnippet(VerbalizationTextSnippetType.MoreThanOneQuantifier, isDeontic, isNegative), basicReplacement);
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[j] = roleReplacement;
					}
					sbTemp.Append(FactType.PopulatePredicateText(readingOrder, allRoles, roleReplacements));
				}
				string replace1_0 = sbTemp.ToString();
				sbMain.AppendFormat(CultureInfo.CurrentUICulture, snippet1, replace1_0);
			}
			else if (includedArity == fullArity)
			{
				string snippet0 = snippets.GetSnippet(VerbalizationTextSnippetType.EachInstanceQuantifier, isDeontic, isNegative);
				readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, allRoles[0], null, allRoles, true);
				string replace0_0 = FactType.PopulatePredicateText(readingOrder, allRoles, basicRoleReplacements);
				sbMain.AppendFormat(CultureInfo.CurrentUICulture, snippet0, replace0_0);
			}
			else
			{
				readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, includedRoles, allRoles, false);
				if (readingOrder != null)
				{
				}
				else
				{
					readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, null, allRoles, true);
				}
			}
			return sbMain.ToString();
		}
		string IVerbalize.GetVerbalization(bool isNegative)
		{
			return GetVerbalization(isNegative);
		}
		#endregion // IVerbalize implementation
	}
#endif // ALREADYGENERATED
	#endregion // Generatable code
}