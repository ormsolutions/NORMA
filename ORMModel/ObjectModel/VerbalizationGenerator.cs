using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
// Common Public License Copyright Notice
// /**************************************************************************\
// * Neumont Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © Neumont University. All rights reserved.                     *
// *                                                                          *
// * The use and distribution terms for this software are covered by the      *
// * Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
// * can be found in the file CPL.txt at the root of this distribution.       *
// * By using this software in any fashion, you are agreeing to be bound by   *
// * the terms of this license.                                               *
// *                                                                          *
// * You must not remove this notice, or any other, from this software.       *
// \**************************************************************************/
namespace Neumont.Tools.ORM.ObjectModel
{
	#region CoreVerbalizationTextSnippetType enum
	/// <summary>
	/// An enum with one value for each recognized snippet
	/// </summary>
	[CLSCompliant(true)]
	public enum CoreVerbalizationTextSnippetType
	{
		/// <summary>
		/// The 'AtMostOneQuantifier' format string snippet. Contains 1 replacement field.
		/// </summary>
		AtMostOneQuantifier,
		/// <summary>
		/// The 'CloseVerbalizationSentence' simple snippet value.
		/// </summary>
		CloseVerbalizationSentence,
		/// <summary>
		/// The 'CombinationIdentifier' format string snippet. Contains 1 replacement field.
		/// </summary>
		CombinationIdentifier,
		/// <summary>
		/// The 'CompactSimpleListClose' simple snippet value.
		/// </summary>
		CompactSimpleListClose,
		/// <summary>
		/// The 'CompactSimpleListFinalSeparator' simple snippet value.
		/// </summary>
		CompactSimpleListFinalSeparator,
		/// <summary>
		/// The 'CompactSimpleListOpen' simple snippet value.
		/// </summary>
		CompactSimpleListOpen,
		/// <summary>
		/// The 'CompactSimpleListPairSeparator' simple snippet value.
		/// </summary>
		CompactSimpleListPairSeparator,
		/// <summary>
		/// The 'CompactSimpleListSeparator' simple snippet value.
		/// </summary>
		CompactSimpleListSeparator,
		/// <summary>
		/// The 'CompoundListClose' simple snippet value.
		/// </summary>
		CompoundListClose,
		/// <summary>
		/// The 'CompoundListFinalSeparator' simple snippet value.
		/// </summary>
		CompoundListFinalSeparator,
		/// <summary>
		/// The 'CompoundListOpen' simple snippet value.
		/// </summary>
		CompoundListOpen,
		/// <summary>
		/// The 'CompoundListPairSeparator' simple snippet value.
		/// </summary>
		CompoundListPairSeparator,
		/// <summary>
		/// The 'CompoundListSeparator' simple snippet value.
		/// </summary>
		CompoundListSeparator,
		/// <summary>
		/// The 'ConstraintProvidesPreferredIdentifier' format string snippet. Contains 2 replacement fields.
		/// </summary>
		ConstraintProvidesPreferredIdentifier,
		/// <summary>
		/// The 'DefiniteArticle' format string snippet. Contains 1 replacement field.
		/// </summary>
		DefiniteArticle,
		/// <summary>
		/// The 'EachInstanceQuantifier' format string snippet. Contains 1 replacement field.
		/// </summary>
		EachInstanceQuantifier,
		/// <summary>
		/// The 'EntityTypeVerbalization' format string snippet. Contains 1 replacement field.
		/// </summary>
		EntityTypeVerbalization,
		/// <summary>
		/// The 'ExistentialQuantifier' format string snippet. Contains 1 replacement field.
		/// </summary>
		ExistentialQuantifier,
		/// <summary>
		/// The 'ForEachCompactQuantifier' format string snippet. Contains 2 replacement fields.
		/// </summary>
		ForEachCompactQuantifier,
		/// <summary>
		/// The 'ForEachQuantifier' format string snippet. Contains 2 replacement fields.
		/// </summary>
		ForEachQuantifier,
		/// <summary>
		/// The 'IdentityReferenceQuantifier' format string snippet. Contains 1 replacement field.
		/// </summary>
		IdentityReferenceQuantifier,
		/// <summary>
		/// The 'ImpersonalPronoun' format string snippet. Contains 1 replacement field.
		/// </summary>
		ImpersonalPronoun,
		/// <summary>
		/// The 'ImpliedModalNecessityOperator' format string snippet. Contains 1 replacement field.
		/// </summary>
		ImpliedModalNecessityOperator,
		/// <summary>
		/// The 'IndentedCompoundListClose' simple snippet value.
		/// </summary>
		IndentedCompoundListClose,
		/// <summary>
		/// The 'IndentedCompoundListFinalSeparator' simple snippet value.
		/// </summary>
		IndentedCompoundListFinalSeparator,
		/// <summary>
		/// The 'IndentedCompoundListOpen' simple snippet value.
		/// </summary>
		IndentedCompoundListOpen,
		/// <summary>
		/// The 'IndentedCompoundListPairSeparator' simple snippet value.
		/// </summary>
		IndentedCompoundListPairSeparator,
		/// <summary>
		/// The 'IndentedCompoundListSeparator' simple snippet value.
		/// </summary>
		IndentedCompoundListSeparator,
		/// <summary>
		/// The 'IndentedListClose' simple snippet value.
		/// </summary>
		IndentedListClose,
		/// <summary>
		/// The 'IndentedListFinalSeparator' simple snippet value.
		/// </summary>
		IndentedListFinalSeparator,
		/// <summary>
		/// The 'IndentedListOpen' simple snippet value.
		/// </summary>
		IndentedListOpen,
		/// <summary>
		/// The 'IndentedListPairSeparator' simple snippet value.
		/// </summary>
		IndentedListPairSeparator,
		/// <summary>
		/// The 'IndentedListSeparator' simple snippet value.
		/// </summary>
		IndentedListSeparator,
		/// <summary>
		/// The 'IndentedLogicalAndListClose' simple snippet value.
		/// </summary>
		IndentedLogicalAndListClose,
		/// <summary>
		/// The 'IndentedLogicalAndListFinalSeparator' simple snippet value.
		/// </summary>
		IndentedLogicalAndListFinalSeparator,
		/// <summary>
		/// The 'IndentedLogicalAndListOpen' simple snippet value.
		/// </summary>
		IndentedLogicalAndListOpen,
		/// <summary>
		/// The 'IndentedLogicalAndListPairSeparator' simple snippet value.
		/// </summary>
		IndentedLogicalAndListPairSeparator,
		/// <summary>
		/// The 'IndentedLogicalAndListSeparator' simple snippet value.
		/// </summary>
		IndentedLogicalAndListSeparator,
		/// <summary>
		/// The 'IndentedLogicalOrListClose' simple snippet value.
		/// </summary>
		IndentedLogicalOrListClose,
		/// <summary>
		/// The 'IndentedLogicalOrListFinalSeparator' simple snippet value.
		/// </summary>
		IndentedLogicalOrListFinalSeparator,
		/// <summary>
		/// The 'IndentedLogicalOrListOpen' simple snippet value.
		/// </summary>
		IndentedLogicalOrListOpen,
		/// <summary>
		/// The 'IndentedLogicalOrListPairSeparator' simple snippet value.
		/// </summary>
		IndentedLogicalOrListPairSeparator,
		/// <summary>
		/// The 'IndentedLogicalOrListSeparator' simple snippet value.
		/// </summary>
		IndentedLogicalOrListSeparator,
		/// <summary>
		/// The 'IndependentVerbalization' format string snippet. Contains 1 replacement field.
		/// </summary>
		IndependentVerbalization,
		/// <summary>
		/// The 'InSeparator' format string snippet. Contains 2 replacement fields.
		/// </summary>
		InSeparator,
		/// <summary>
		/// The 'MinClosedMaxClosed' format string snippet. Contains 2 replacement fields.
		/// </summary>
		MinClosedMaxClosed,
		/// <summary>
		/// The 'MinClosedMaxOpen' format string snippet. Contains 2 replacement fields.
		/// </summary>
		MinClosedMaxOpen,
		/// <summary>
		/// The 'MinClosedMaxUnbounded' format string snippet. Contains 1 replacement field.
		/// </summary>
		MinClosedMaxUnbounded,
		/// <summary>
		/// The 'MinOpenMaxClosed' format string snippet. Contains 2 replacement fields.
		/// </summary>
		MinOpenMaxClosed,
		/// <summary>
		/// The 'MinOpenMaxOpen' format string snippet. Contains 2 replacement fields.
		/// </summary>
		MinOpenMaxOpen,
		/// <summary>
		/// The 'MinOpenMaxUnbounded' format string snippet. Contains 1 replacement field.
		/// </summary>
		MinOpenMaxUnbounded,
		/// <summary>
		/// The 'MinUnboundedMaxClosed' format string snippet. Contains 1 replacement field.
		/// </summary>
		MinUnboundedMaxClosed,
		/// <summary>
		/// The 'MinUnboundedMaxOpen' format string snippet. Contains 1 replacement field.
		/// </summary>
		MinUnboundedMaxOpen,
		/// <summary>
		/// The 'ModalNecessityOperator' format string snippet. Contains 1 replacement field.
		/// </summary>
		ModalNecessityOperator,
		/// <summary>
		/// The 'ModalPossibilityOperator' format string snippet. Contains 1 replacement field.
		/// </summary>
		ModalPossibilityOperator,
		/// <summary>
		/// The 'MoreThanOneQuantifier' format string snippet. Contains 1 replacement field.
		/// </summary>
		MoreThanOneQuantifier,
		/// <summary>
		/// The 'MultiValueValueConstraint' format string snippet. Contains 2 replacement fields.
		/// </summary>
		MultiValueValueConstraint,
		/// <summary>
		/// The 'NameWithRefMode' format string snippet. Contains 2 replacement fields.
		/// </summary>
		NameWithRefMode,
		/// <summary>
		/// The 'NegativeReadingForUnaryOnlyDisjunctiveMandatory' format string snippet. Contains 2 replacement fields.
		/// </summary>
		NegativeReadingForUnaryOnlyDisjunctiveMandatory,
		/// <summary>
		/// The 'NotesVerbalization' format string snippet. Contains 1 replacement field.
		/// </summary>
		NotesVerbalization,
		/// <summary>
		/// The 'ObjectifiesFactTypeVerbalization' format string snippet. Contains 2 replacement fields.
		/// </summary>
		ObjectifiesFactTypeVerbalization,
		/// <summary>
		/// The 'ObjectType' format string snippet. Contains 1 replacement field.
		/// </summary>
		ObjectType,
		/// <summary>
		/// The 'ObjectTypeMissing' format string snippet. Contains 1 replacement field.
		/// </summary>
		ObjectTypeMissing,
		/// <summary>
		/// The 'ObjectTypeWithSubscript' format string snippet. Contains 2 replacement fields.
		/// </summary>
		ObjectTypeWithSubscript,
		/// <summary>
		/// The 'OccursInPopulation' format string snippet. Contains 2 replacement fields.
		/// </summary>
		OccursInPopulation,
		/// <summary>
		/// The 'OneQuantifier' format string snippet. Contains 1 replacement field.
		/// </summary>
		OneQuantifier,
		/// <summary>
		/// The 'PeriodDelimiter' format string snippet. Contains 2 replacement fields.
		/// </summary>
		PeriodDelimiter,
		/// <summary>
		/// The 'PersonalPronoun' format string snippet. Contains 1 replacement field.
		/// </summary>
		PersonalPronoun,
		/// <summary>
		/// The 'PortableDataTypeVerbalization' format string snippet. Contains 1 replacement field.
		/// </summary>
		PortableDataTypeVerbalization,
		/// <summary>
		/// The 'ReferenceModeVerbalization' format string snippet. Contains 1 replacement field.
		/// </summary>
		ReferenceModeVerbalization,
		/// <summary>
		/// The 'ReferenceSchemeVerbalization' format string snippet. Contains 1 replacement field.
		/// </summary>
		ReferenceSchemeVerbalization,
		/// <summary>
		/// The 'SelfReference' format string snippet. Contains 1 replacement field.
		/// </summary>
		SelfReference,
		/// <summary>
		/// The 'SimpleListClose' simple snippet value.
		/// </summary>
		SimpleListClose,
		/// <summary>
		/// The 'SimpleListFinalSeparator' simple snippet value.
		/// </summary>
		SimpleListFinalSeparator,
		/// <summary>
		/// The 'SimpleListOpen' simple snippet value.
		/// </summary>
		SimpleListOpen,
		/// <summary>
		/// The 'SimpleListPairSeparator' simple snippet value.
		/// </summary>
		SimpleListPairSeparator,
		/// <summary>
		/// The 'SimpleListSeparator' simple snippet value.
		/// </summary>
		SimpleListSeparator,
		/// <summary>
		/// The 'SingleValueValueConstraint' format string snippet. Contains 2 replacement fields.
		/// </summary>
		SingleValueValueConstraint,
		/// <summary>
		/// The 'TopLevelIndentedLogicalAndListClose' simple snippet value.
		/// </summary>
		TopLevelIndentedLogicalAndListClose,
		/// <summary>
		/// The 'TopLevelIndentedLogicalAndListFinalSeparator' simple snippet value.
		/// </summary>
		TopLevelIndentedLogicalAndListFinalSeparator,
		/// <summary>
		/// The 'TopLevelIndentedLogicalAndListOpen' simple snippet value.
		/// </summary>
		TopLevelIndentedLogicalAndListOpen,
		/// <summary>
		/// The 'TopLevelIndentedLogicalAndListPairSeparator' simple snippet value.
		/// </summary>
		TopLevelIndentedLogicalAndListPairSeparator,
		/// <summary>
		/// The 'TopLevelIndentedLogicalAndListSeparator' simple snippet value.
		/// </summary>
		TopLevelIndentedLogicalAndListSeparator,
		/// <summary>
		/// The 'TopLevelIndentedLogicalOrListClose' simple snippet value.
		/// </summary>
		TopLevelIndentedLogicalOrListClose,
		/// <summary>
		/// The 'TopLevelIndentedLogicalOrListFinalSeparator' simple snippet value.
		/// </summary>
		TopLevelIndentedLogicalOrListFinalSeparator,
		/// <summary>
		/// The 'TopLevelIndentedLogicalOrListOpen' simple snippet value.
		/// </summary>
		TopLevelIndentedLogicalOrListOpen,
		/// <summary>
		/// The 'TopLevelIndentedLogicalOrListPairSeparator' simple snippet value.
		/// </summary>
		TopLevelIndentedLogicalOrListPairSeparator,
		/// <summary>
		/// The 'TopLevelIndentedLogicalOrListSeparator' simple snippet value.
		/// </summary>
		TopLevelIndentedLogicalOrListSeparator,
		/// <summary>
		/// The 'UniversalQuantifier' format string snippet. Contains 1 replacement field.
		/// </summary>
		UniversalQuantifier,
		/// <summary>
		/// The 'ValueTypeVerbalization' format string snippet. Contains 1 replacement field.
		/// </summary>
		ValueTypeVerbalization,
		/// <summary>
		/// The 'VerbalizerCloseError' simple snippet value.
		/// </summary>
		VerbalizerCloseError,
		/// <summary>
		/// The 'VerbalizerCloseVerbalization' simple snippet value.
		/// </summary>
		VerbalizerCloseVerbalization,
		/// <summary>
		/// The 'VerbalizerDecreaseIndent' simple snippet value.
		/// </summary>
		VerbalizerDecreaseIndent,
		/// <summary>
		/// The 'VerbalizerDocumentFooter' simple snippet value.
		/// </summary>
		VerbalizerDocumentFooter,
		/// <summary>
		/// The 'VerbalizerDocumentHeader' format string snippet. Contains 16 replacement fields.
		/// </summary>
		VerbalizerDocumentHeader,
		/// <summary>
		/// The 'VerbalizerFontWeightBold' simple snippet value.
		/// </summary>
		VerbalizerFontWeightBold,
		/// <summary>
		/// The 'VerbalizerFontWeightNormal' simple snippet value.
		/// </summary>
		VerbalizerFontWeightNormal,
		/// <summary>
		/// The 'VerbalizerIncreaseIndent' simple snippet value.
		/// </summary>
		VerbalizerIncreaseIndent,
		/// <summary>
		/// The 'VerbalizerNewLine' simple snippet value.
		/// </summary>
		VerbalizerNewLine,
		/// <summary>
		/// The 'VerbalizerOpenError' simple snippet value.
		/// </summary>
		VerbalizerOpenError,
		/// <summary>
		/// The 'VerbalizerOpenVerbalization' simple snippet value.
		/// </summary>
		VerbalizerOpenVerbalization,
	}
	#endregion // CoreVerbalizationTextSnippetType enum
	#region IVerbalizationSets interface
	/// <summary>
	/// A base class for the generic VerbalizationSets class.
	/// </summary>
	public interface IVerbalizationSets
	{
	}
	#endregion // IVerbalizationSets interface
	#region Generic VerbalizationSets class
	/// <summary>
	/// A generic class containing one VerbalizationSet structure for each combination of {alethic,deontic} and {positive,negative} snippets.
	/// </summary>
	/// <typeparam name="EnumType">
	/// The enumeration type of snippet set
	/// </typeparam>
	public abstract class VerbalizationSets<EnumType> : IVerbalizationSets
		where EnumType : struct
	{
		#region VerbalizationSet class
		/// <summary>
		/// An abstract class holding an array of strings. Strings are retrieved with values from CoreVerbalizationTextSnippetType.
		/// </summary>
		protected abstract class VerbalizationSet
		{
			/// <summary>
			/// Retrieve a snippet value
			/// </summary>
			/// <param name="snippetType">
			/// A value from the CoreVerbalizationTextSnippetType enum representing the snippet string to retrieve.
			/// </param>
			/// <param name="owner">
			/// The VerbalizationSets object that is the owner of the snippet sets.
			/// </param>
			/// <returns>
			/// Snippet string
			/// </returns>
			public abstract string GetSnippet(EnumType snippetType, VerbalizationSets<EnumType> owner);
		}
		#endregion // VerbalizationSet class
		#region ArrayVerbalizationSet class
		/// <summary>
		/// A class holding an array of strings. Strings are retrieved with values from CoreVerbalizationTextSnippetType.
		/// </summary>
		protected class ArrayVerbalizationSet : VerbalizationSet
		{
			private string[] mySnippets;
			/// <summary>
			/// VerbalizationSet constructor.
			/// </summary>
			/// <param name="snippets">
			/// An array of strings with one string for each value in the CoreVerbalizationTextSnippetType enum.
			/// </param>
			public ArrayVerbalizationSet(string[] snippets)
			{
				this.mySnippets = snippets;
			}
			/// <summary>
			/// Retrieve a snippet value
			/// </summary>
			/// <param name="snippetType">
			/// A value from the CoreVerbalizationTextSnippetType enum representing the snippet string to retrieve.
			/// </param>
			/// <param name="owner">
			/// The VerbalizationSets object that is the owner of the snippet sets.
			/// </param>
			/// <returns>
			/// Snippet string
			/// </returns>
			public override string GetSnippet(EnumType snippetType, VerbalizationSets<EnumType> owner)
			{
				return this.mySnippets[owner.ValueToIndex(snippetType)];
			}
		}
		#endregion // ArrayVerbalizationSet class
		#region DictionaryVerbalizationSet class
		/// <summary>
		/// A class holding dictionary items that refer to values from the enumeration of CoreVerbalizationTextSnippetType.
		/// </summary>
		protected class DictionaryVerbalizationSet : VerbalizationSet
		{
			private Dictionary<EnumType, string> mySnippets;
			/// <summary>
			/// Retrieves all of the IDictionary snippets in the snippet set
			/// </summary>
			public IDictionary<EnumType, string> Dictionary
			{
				get
				{
					return mySnippets;
				}
			}
			/// <summary>
			/// VerbalizationSet constructor.
			/// </summary>
			public DictionaryVerbalizationSet()
			{
				this.mySnippets = new Dictionary<EnumType, string>();
			}
			/// <summary>
			/// Retrieve a snippet value
			/// </summary>
			/// <param name="snippetType">
			/// A value from the CoreVerbalizationTextSnippetType enum representing the snippet string to retrieve.
			/// </param>
			/// <param name="owner">
			/// The VerbalizationSets object that is the owner of the snippet sets.
			/// </param>
			/// <returns>
			/// Snippet string
			/// </returns>
			public override string GetSnippet(EnumType snippetType, VerbalizationSets<EnumType> owner)
			{
				string retVal = null;
				this.mySnippets.TryGetValue(snippetType, out retVal);
				return retVal;
			}
		}
		#endregion // DictionaryVerbalizationSet class
		private VerbalizationSet[] mySets;
		/// <summary>
		/// Retrieve a snippet for the specified type with default criteria.
		/// </summary>
		/// <param name="snippetType">
		/// A value from the CoreVerbalizationTextSnippetType enum representing the snippet string to retrieve.
		/// </param>
		/// <returns>
		/// Snippet string
		/// </returns>
		public string GetSnippet(EnumType snippetType)
		{
			return this.GetSnippet(snippetType, false, false);
		}
		/// <summary>
		/// Retrieve a snippet for the specified type and criteria.
		/// </summary>
		/// <param name="snippetType">
		/// A value from the CoreVerbalizationTextSnippetType enum representing the snippet string to retrieve.
		/// </param>
		/// <param name="isDeontic">
		/// Set to true to retrieve the snippet for a deontic verbalization, false for alethic.
		/// </param>
		/// <param name="isNegative">
		/// Set to true to retrieve the snippet for a negative reading, false for positive.
		/// </param>
		/// <returns>
		/// Snippet string
		/// </returns>
		public string GetSnippet(EnumType snippetType, bool isDeontic, bool isNegative)
		{
			VerbalizationSet set = this.mySets[VerbalizationSets<EnumType>.GetSetIndex(isDeontic, isNegative)];
			if (set != null)
			{
				return set.GetSnippet(snippetType, this);
			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// Get the snippet index of the deontic/negative VerbalizationSet
		/// </summary>
		/// <param name="isDeontic">
		/// Set to true to retrieve the snippet for a deontic verbalization, false for alethic.
		/// </param>
		/// <param name="isNegative">
		/// Set to true to retrieve the snippet for a negative reading, false for positive.
		/// </param>
		/// <returns>
		/// 0-based index
		/// </returns>
		protected static int GetSetIndex(bool isDeontic, bool isNegative)
		{
			int setIndex = 0;
			if (isDeontic)
			{
				setIndex = setIndex + 1;
			}
			if (isNegative)
			{
				setIndex = setIndex + 2;
			}
			return setIndex;
		}
		/// <summary>
		/// Method to populate verbalization sets of an abstract VerbalizationSets object.
		/// </summary>
		/// <param name="sets">
		/// The empty verbalization sets to be populated
		/// </param>
		protected abstract void PopulateVerbalizationSets(VerbalizationSet[] sets);
		/// <summary>
		/// Method to convert enum value to integer index value
		/// </summary>
		/// <param name="enumValue">
		/// The enum value to be converted
		/// </param>
		/// <returns>
		/// integer value of enum type
		/// </returns>
		protected abstract int ValueToIndex(EnumType enumValue);
		/// <summary>
		/// Creates an instance of the VerbalizationSets class calling the PopulateVerbalizationSets method.
		/// </summary>
		/// <typeparam name="DerivedType">
		/// Name of class to instantiate that derives from VerbalizationSets.
		/// </typeparam>
		/// <returns>
		/// Returns a generic {$VerbalizationSets} object with snippet sets
		/// </returns>
		public static VerbalizationSets<EnumType> Create<DerivedType>()
			where DerivedType : VerbalizationSets<EnumType>, new()
		{
			VerbalizationSets<EnumType> retVal = new DerivedType();
			VerbalizationSet[] newSets = new VerbalizationSet[4];
			retVal.PopulateVerbalizationSets(newSets);
			retVal.mySets = newSets;
			return retVal;
		}
	}
	#endregion // Generic VerbalizationSets class
	#region CoreVerbalizationSets class
	/// <summary>
	/// A class derving from VerbalizationSets.
	/// </summary>
	[CLSCompliant(true)]
	public class CoreVerbalizationSets : VerbalizationSets<CoreVerbalizationTextSnippetType>
	{
		/// <summary>
		/// The default verbalization snippet set. Contains english HTML snippets.
		/// </summary>
		public static readonly CoreVerbalizationSets Default = (CoreVerbalizationSets)VerbalizationSets<CoreVerbalizationTextSnippetType>.Create<CoreVerbalizationSets>();
		/// <summary>
		/// Populates the snippet sets of the CoreVerbalizationSets object.
		/// </summary>
		/// <param name="sets">
		/// The sets to be populated.
		/// </param>
		protected override void PopulateVerbalizationSets(VerbalizationSet[] sets)
		{
			sets[0] = new ArrayVerbalizationSet(new string[]{
				@"<span class=""quantifier"">at most one</span> {0}",
				@"<span class=""listSeparator"">.</span>",
				@"{0} <span class=""quantifier"">combination</span>",
				"",
				@"<span class=""listSeparator"">, </span>",
				"",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""listSeparator"">, </span>",
				"",
				"; ",
				"",
				"; ",
				"; ",
				@"<span class=""quantifier"">this association with</span> {0} <span class=""quantifier"">provides the preferred identification scheme for</span> {1}",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">each instance of</span> {0} <span class=""quantifier"">occurs only once</span>",
				@"{0} <span class=""quantifier"">is an entity type</span>",
				@"<span class=""quantifier"">some</span> {0}",
				@"<span class=""quantifier"">for each</span> {0}, {1}",
				@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">the same</span> {0}",
				@"<span class=""quantifier"">that</span> {0}",
				"{0}",
				"</span>",
				@"<span class=""listSeparator"">; </span>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">; </span>",
				@"<span class=""listSeparator"">; </span>",
				"</span>",
				@"<span class=""listSeparator""> and </span><br/>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator""> and </span><br/>",
				@"<span class=""listSeparator""> and </span><br/>",
				"</span>",
				@"<br/><span class=""logicalOperator"">and that </span>",
				@"<span class=""smallIndent"">",
				@"<br/><span class=""logicalOperator"">and that </span>",
				@"<br/><span class=""logicalOperator"">and that </span>",
				"</span>",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"<span class=""smallIndent"">",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"{0} <span class=""quantifier"">is independent (it may have instances that play no other roles)</span>",
				@"<span class=""objectType"">{0}</span> in {1}",
				@"<span class=""quantifier"">at least <span class=""objectType"">{0}</span> to at most <span class=""objectType"">{1}</span></span>",
				@"at least <span class=""objectType"">{0}</span> to below <span class=""objectType"">{1}</span>",
				@"at least <span class=""objectType"">{0}</span>",
				@"above <span class=""objectType"">{0}</span> to at most <span class=""objectType"">{1}</span>",
				@"above <span class=""objectType"">{0}</span> to below <span class=""objectType"">{1}</span>",
				@"above <span class=""objectType"">{0}</span>",
				@"at most <span class=""objectType"">{1}</span>",
				@"below <span class=""objectType"">{1}</span>",
				@"<span class=""quantifier"">it is necessary that</span> {0}",
				@"<span class=""quantifier"">it is possible that</span> {0}",
				@"<span class=""quantifier"">more than one</span> {0}",
				@"<span class=""quantifier"">the possible values of <span class=""objectType"">{0}</span> are {1}</span>",
				@"{0}(<span class=""objectType"">{1}</span>)",
				@"<span class=""quantifier"">some</span> {0} <span class=""quantifier"">participates in none of the following:</span>{1}",
				@"<span class=""quantifier"">Notes:</span> <span class=""note"">{0}</span>",
				@"{0} <span class=""quantifier"">objectifies</span> ""{1}""",
				@"<span class=""objectType"">{0}</span>",
				@"<span class=""objectTypeMissing"">Missing<sub>{0}</sub></span>",
				@"<span class=""objectType"">{0}<sub>{1}</sub></span>",
				@"{0} <span class=""quantifier"">occurs at most once in the population of</span> {1}",
				@"<span class=""quantifier"">at most one</span> {0}",
				@"{0}.<span class=""objectType"">{1}</span>",
				@"<span class=""quantifier"">who</span> {0}",
				@"<span class=""quantifier"">Portable data type:</span> {0}",
				@"<span class=""quantifier"">Reference Mode:</span> <span class=""referenceMode"">{0}</span>",
				@"<span class=""quantifier"">Reference Scheme:</span> {0}",
				"{0}",
				"",
				@"<span class=""listSeparator"">, and </span>",
				"",
				@"<span class=""listSeparator""> and </span>",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""quantifier"">the possible value of {0} is {1}</span>",
				"</span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">and that </span>",
				"<span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">and that </span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">and that </span>",
				"</span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">or </span>",
				"<span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">or </span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">or </span>",
				@"<span class=""quantifier"">each</span> {0}",
				@"{0} <span class=""quantifier"">is a value type</span>",
				"</span>",
				"</p>",
				"</span>",
				"</body></html>",
				@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
<html>
	<head>
		<title>ORM2 Verbalization</title>
		<style>
			body {{ font-family: {0}; font-size: {1}pt; padding: .1em; color: {2}; {3} }}
			.objectType {{ color: {4}; {5} }}
			.objectTypeMissing {{ color: {4}; {5} }}
			.referenceMode {{ color: {10}; {11} }}
			.predicateText {{ color: #0000ff; }}
			.quantifier {{ color: {6}; {7} }}
			.error {{ color: red; }}
			.verbalization {{ }}
			.indent {{ left: 20px; position: relative; }}
			.smallIndent {{ left: 8px; position: relative;}}
			.listSeparator {{ color: windowtext; font-weight: 200;}}
			.logicalOperator {{ color: {6}; {7}}}
			.note {{ color: {8}; font-style: italic; {9} }}
			.notAvailable {{ font-style: italic; }}
		</style>
	</head>
	<body>",
				"font-weight: bold;",
				"font-weight: normal;",
				@"<span class=""indent"">",
				"<br/>\n",
				@"<span class=""error"">",
				@"<p class=""verbalization"">"});
			sets[1] = new ArrayVerbalizationSet(new string[]{
				@"<span class=""quantifier"">at most one</span> {0}",
				@"<span class=""listSeparator"">.</span>",
				@"{0} <span class=""quantifier"">combination</span>",
				"",
				@"<span class=""listSeparator"">, </span>",
				"",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""listSeparator"">, </span>",
				"",
				"; ",
				"",
				"; ",
				"; ",
				@"<span class=""quantifier"">this association with</span> {0} <span class=""quantifier"">provides the preferred identification scheme for</span> {1}",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">each instance of</span> {0} <span class=""quantifier"">occurs only once</span>",
				@"{0} <span class=""quantifier"">is an entity type</span>",
				@"<span class=""quantifier"">some</span> {0}",
				@"<span class=""quantifier"">for each</span> {0}, {1}",
				@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">the same</span> {0}",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">it is obligatory that</span> {0}",
				"</span>",
				@"<span class=""listSeparator"">; </span>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">; </span>",
				@"<span class=""listSeparator"">; </span>",
				"</span>",
				@"<span class=""listSeparator""> and </span><br/>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator""> and </span><br/>",
				@"<span class=""listSeparator""> and </span><br/>",
				"</span>",
				@"<br/><span class=""logicalOperator"">and that </span>",
				@"<span class=""smallIndent"">",
				@"<br/><span class=""logicalOperator"">and that </span>",
				@"<br/><span class=""logicalOperator"">and that </span>",
				"</span>",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"<span class=""smallIndent"">",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"{0} <span class=""quantifier"">is independent (it may have instances that play no other roles)</span>",
				@"<span class=""objectType"">{0}</span> in {1}",
				@"<span class=""quantifier"">at least <span class=""objectType"">{0}</span> to at most <span class=""objectType"">{1}</span></span>",
				@"at least <span class=""objectType"">{0}</span> to below <span class=""objectType"">{1}</span>",
				@"at least <span class=""objectType"">{0}</span>",
				@"above <span class=""objectType"">{0}</span> to at most <span class=""objectType"">{1}</span>",
				@"above <span class=""objectType"">{0}</span> to below <span class=""objectType"">{1}</span>",
				@"above <span class=""objectType"">{0}</span>",
				@"at most <span class=""objectType"">{1}</span>",
				@"below <span class=""objectType"">{1}</span>",
				@"<span class=""quantifier"">it is obligatory that</span> {0}",
				@"<span class=""quantifier"">it is permitted that</span> {0}",
				@"<span class=""quantifier"">more than one</span> {0}",
				@"<span class=""quantifier"">the possible values of <span class=""objectType"">{0}</span> are {1}</span>",
				@"{0}(<span class=""objectType"">{1}</span>)",
				@"<span class=""quantifier"">some</span> {0} <span class=""quantifier"">participates in none of the following:</span>{1}",
				@"<span class=""quantifier"">Notes:</span> <span class=""note"">{0}</span>",
				@"{0} <span class=""quantifier"">objectifies</span> ""{1}""",
				@"<span class=""objectType"">{0}</span>",
				@"<span class=""objectTypeMissing"">Missing<sub>{0}</sub></span>",
				@"<span class=""objectType"">{0}<sub>{1}</sub></span>",
				@"{0} <span class=""quantifier"">occurs at most once in the population of</span> {1}",
				@"<span class=""quantifier"">at most one</span> {0}",
				@"{0}.<span class=""objectType"">{1}</span>",
				@"<span class=""quantifier"">who</span> {0}",
				@"<span class=""quantifier"">Portable data type:</span> {0}",
				@"<span class=""quantifier"">Reference Mode:</span> <span class=""referenceMode"">{0}</span>",
				@"<span class=""quantifier"">Reference Scheme:</span> {0}",
				"{0}",
				"",
				@"<span class=""listSeparator"">, and </span>",
				"",
				@"<span class=""listSeparator""> and </span>",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""quantifier"">the possible value of {0} is {1}</span>",
				"</span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">and that </span>",
				"<span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">and that </span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">and that </span>",
				"</span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">or </span>",
				"<span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">or </span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">or </span>",
				@"<span class=""quantifier"">each</span> {0}",
				@"{0} <span class=""quantifier"">is a value type</span>",
				"</span>",
				"</p>",
				"</span>",
				"</body></html>",
				@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
<html>
	<head>
		<title>ORM2 Verbalization</title>
		<style>
			body {{ font-family: {0}; font-size: {1}pt; padding: .1em; color: {2}; {3} }}
			.objectType {{ color: {4}; {5} }}
			.objectTypeMissing {{ color: {4}; {5} }}
			.referenceMode {{ color: {10}; {11} }}
			.predicateText {{ color: #0000ff; }}
			.quantifier {{ color: {6}; {7} }}
			.error {{ color: red; }}
			.verbalization {{ }}
			.indent {{ left: 20px; position: relative; }}
			.smallIndent {{ left: 8px; position: relative;}}
			.listSeparator {{ color: windowtext; font-weight: 200;}}
			.logicalOperator {{ color: {6}; {7}}}
			.note {{ color: {8}; font-style: italic; {9} }}
			.notAvailable {{ font-style: italic; }}
		</style>
	</head>
	<body>",
				"font-weight: bold;",
				"font-weight: normal;",
				@"<span class=""indent"">",
				"<br/>\n",
				@"<span class=""error"">",
				@"<p class=""verbalization"">"});
			sets[2] = new ArrayVerbalizationSet(new string[]{
				@"<span class=""quantifier"">at most one</span> {0}",
				@"<span class=""listSeparator"">.</span>",
				@"{0} <span class=""quantifier"">combination</span>",
				"",
				@"<span class=""listSeparator"">, </span>",
				"",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""listSeparator"">, </span>",
				"",
				"; ",
				"",
				"; ",
				"; ",
				@"<span class=""quantifier"">this association with</span> {0} <span class=""quantifier"">provides the preferred identification scheme for</span> {1}",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">each instance of</span> {0} <span class=""quantifier"">occurs only once</span>",
				@"{0} <span class=""quantifier"">is an entity type</span>",
				@"<span class=""quantifier"">some</span> {0}",
				@"<span class=""quantifier"">for each</span> {0}, {1}",
				@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">the same</span> {0}",
				@"<span class=""quantifier"">that</span> {0}",
				"{0}",
				"</span>",
				@"<span class=""listSeparator"">; </span>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">; </span>",
				@"<span class=""listSeparator"">; </span>",
				"</span>",
				@"<span class=""listSeparator""> and </span><br/>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator""> and </span><br/>",
				@"<span class=""listSeparator""> and </span><br/>",
				"</span>",
				@"<br/><span class=""logicalOperator"">and that </span>",
				@"<span class=""smallIndent"">",
				@"<br/><span class=""logicalOperator"">and that </span>",
				@"<br/><span class=""logicalOperator"">and that </span>",
				"</span>",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"<span class=""smallIndent"">",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"{0} <span class=""quantifier"">is independent (it may have instances that play no other roles)</span>",
				@"<span class=""objectType"">{0}</span> in {1}",
				@"<span class=""quantifier"">at least <span class=""objectType"">{0}</span> to at most <span class=""objectType"">{1}</span></span>",
				@"at least <span class=""objectType"">{0}</span> to below <span class=""objectType"">{1}</span>",
				@"at least <span class=""objectType"">{0}</span>",
				@"above <span class=""objectType"">{0}</span> to at most <span class=""objectType"">{1}</span>",
				@"above <span class=""objectType"">{0}</span> to below <span class=""objectType"">{1}</span>",
				@"above <span class=""objectType"">{0}</span>",
				@"at most <span class=""objectType"">{1}</span>",
				@"below <span class=""objectType"">{1}</span>",
				@"<span class=""quantifier"">it is necessary that</span> {0}",
				@"<span class=""quantifier"">it is impossible that</span> {0}",
				@"<span class=""quantifier"">more than one</span> {0}",
				@"<span class=""quantifier"">the possible values of <span class=""objectType"">{0}</span> are {1}</span>",
				@"{0}(<span class=""objectType"">{1}</span>)",
				@"<span class=""quantifier"">some</span> {0} <span class=""quantifier"">participates in none of the following:</span>{1}",
				@"<span class=""quantifier"">Notes:</span> <span class=""note"">{0}</span>",
				@"{0} <span class=""quantifier"">objectifies</span> ""{1}""",
				@"<span class=""objectType"">{0}</span>",
				@"<span class=""objectTypeMissing"">Missing<sub>{0}</sub></span>",
				@"<span class=""objectType"">{0}<sub>{1}</sub></span>",
				@"{0} <span class=""quantifier"">occurs more than once in the population of</span> {1}",
				@"<span class=""quantifier"">more than one</span> {0}",
				@"{0}.<span class=""objectType"">{1}</span>",
				@"<span class=""quantifier"">who</span> {0}",
				@"<span class=""quantifier"">Portable data type:</span> {0}",
				@"<span class=""quantifier"">Reference Mode:</span> <span class=""referenceMode"">{0}</span>",
				@"<span class=""quantifier"">Reference Scheme:</span> {0}",
				"{0}",
				"",
				@"<span class=""listSeparator"">, and </span>",
				"",
				@"<span class=""listSeparator""> and </span>",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""quantifier"">the possible value of {0} is {1}</span>",
				"</span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">and that </span>",
				"<span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">and that </span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">and that </span>",
				"</span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">or </span>",
				"<span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">or </span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">or </span>",
				@"<span class=""quantifier"">each</span> {0}",
				@"{0} <span class=""quantifier"">is a value type</span>",
				"</span>",
				"</p>",
				"</span>",
				"</body></html>",
				@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
<html>
	<head>
		<title>ORM2 Verbalization</title>
		<style>
			body {{ font-family: {0}; font-size: {1}pt; padding: .1em; color: {2}; {3} }}
			.objectType {{ color: {4}; {5} }}
			.objectTypeMissing {{ color: {4}; {5} }}
			.referenceMode {{ color: {10}; {11} }}
			.predicateText {{ color: #0000ff; }}
			.quantifier {{ color: {6}; {7} }}
			.error {{ color: red; }}
			.verbalization {{ }}
			.indent {{ left: 20px; position: relative; }}
			.smallIndent {{ left: 8px; position: relative;}}
			.listSeparator {{ color: windowtext; font-weight: 200;}}
			.logicalOperator {{ color: {6}; {7}}}
			.note {{ color: {8}; font-style: italic; {9} }}
			.notAvailable {{ font-style: italic; }}
		</style>
	</head>
	<body>",
				"font-weight: bold;",
				"font-weight: normal;",
				@"<span class=""indent"">",
				"<br/>\n",
				@"<span class=""error"">",
				@"<p class=""verbalization"">"});
			sets[3] = new ArrayVerbalizationSet(new string[]{
				@"<span class=""quantifier"">at most one</span> {0}",
				@"<span class=""listSeparator"">.</span>",
				@"{0} <span class=""quantifier"">combination</span>",
				"",
				@"<span class=""listSeparator"">, </span>",
				"",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""listSeparator"">, </span>",
				"",
				"; ",
				"",
				"; ",
				"; ",
				@"<span class=""quantifier"">this association with</span> {0} <span class=""quantifier"">provides the preferred identification scheme for</span> {1}",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">each instance of</span> {0} <span class=""quantifier"">occurs only once</span>",
				@"{0} <span class=""quantifier"">is an entity type</span>",
				@"<span class=""quantifier"">some</span> {0}",
				@"<span class=""quantifier"">for each</span> {0}, {1}",
				@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">the same</span> {0}",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">it is obligatory that</span> {0}",
				"</span>",
				@"<span class=""listSeparator"">; </span>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">; </span>",
				@"<span class=""listSeparator"">; </span>",
				"</span>",
				@"<span class=""listSeparator""> and </span><br/>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator""> and </span><br/>",
				@"<span class=""listSeparator""> and </span><br/>",
				"</span>",
				@"<br/><span class=""logicalOperator"">and that </span>",
				@"<span class=""smallIndent"">",
				@"<br/><span class=""logicalOperator"">and that </span>",
				@"<br/><span class=""logicalOperator"">and that </span>",
				"</span>",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"<span class=""smallIndent"">",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"{0} <span class=""quantifier"">is independent (it may have instances that play no other roles)</span>",
				@"<span class=""objectType"">{0}</span> in {1}",
				@"<span class=""quantifier"">at least <span class=""objectType"">{0}</span> to at most <span class=""objectType"">{1}</span></span>",
				@"at least <span class=""objectType"">{0}</span> to below <span class=""objectType"">{1}</span>",
				@"at least <span class=""objectType"">{0}</span>",
				@"above <span class=""objectType"">{0}</span> to at most <span class=""objectType"">{1}</span>",
				@"above <span class=""objectType"">{0}</span> to below <span class=""objectType"">{1}</span>",
				@"above <span class=""objectType"">{0}</span>",
				@"at most <span class=""objectType"">{1}</span>",
				@"below <span class=""objectType"">{1}</span>",
				@"<span class=""quantifier"">it is obligatory that</span> {0}",
				@"<span class=""quantifier"">it is forbidden that</span> {0}",
				@"<span class=""quantifier"">more than one</span> {0}",
				@"<span class=""quantifier"">the possible values of <span class=""objectType"">{0}</span> are {1}</span>",
				@"{0}(<span class=""objectType"">{1}</span>)",
				@"<span class=""quantifier"">some</span> {0} <span class=""quantifier"">participates in none of the following:</span>{1}",
				@"<span class=""quantifier"">Notes:</span> <span class=""note"">{0}</span>",
				@"{0} <span class=""quantifier"">objectifies</span> ""{1}""",
				@"<span class=""objectType"">{0}</span>",
				@"<span class=""objectTypeMissing"">Missing<sub>{0}</sub></span>",
				@"<span class=""objectType"">{0}<sub>{1}</sub></span>",
				@"{0} <span class=""quantifier"">occurs more than once in the population of</span> {1}",
				@"<span class=""quantifier"">more than one</span> {0}",
				@"{0}.<span class=""objectType"">{1}</span>",
				@"<span class=""quantifier"">who</span> {0}",
				@"<span class=""quantifier"">Portable data type:</span> {0}",
				@"<span class=""quantifier"">Reference Mode:</span> <span class=""referenceMode"">{0}</span>",
				@"<span class=""quantifier"">Reference Scheme:</span> {0}",
				"{0}",
				"",
				@"<span class=""listSeparator"">, and </span>",
				"",
				@"<span class=""listSeparator""> and </span>",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""quantifier"">the possible value of {0} is {1}</span>",
				"</span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">and that </span>",
				"<span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">and that </span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">and that </span>",
				"</span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">or </span>",
				"<span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">or </span>",
				@"</span><span class=""smallIndent""><br/><span class=""logicalOperator"">or </span>",
				@"<span class=""quantifier"">each</span> {0}",
				@"{0} <span class=""quantifier"">is a value type</span>",
				"</span>",
				"</p>",
				"</span>",
				"</body></html>",
				@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
<html>
	<head>
		<title>ORM2 Verbalization</title>
		<style>
			body {{ font-family: {0}; font-size: {1}pt; padding: .1em; color: {2}; {3} }}
			.objectType {{ color: {4}; {5} }}
			.objectTypeMissing {{ color: {4}; {5} }}
			.referenceMode {{ color: {10}; {11} }}
			.predicateText {{ color: #0000ff; }}
			.quantifier {{ color: {6}; {7} }}
			.error {{ color: red; }}
			.verbalization {{ }}
			.indent {{ left: 20px; position: relative; }}
			.smallIndent {{ left: 8px; position: relative;}}
			.listSeparator {{ color: windowtext; font-weight: 200;}}
			.logicalOperator {{ color: {6}; {7}}}
			.note {{ color: {8}; font-style: italic; {9} }}
			.notAvailable {{ font-style: italic; }}
		</style>
	</head>
	<body>",
				"font-weight: bold;",
				"font-weight: normal;",
				@"<span class=""indent"">",
				"<br/>\n",
				@"<span class=""error"">",
				@"<p class=""verbalization"">"});
		}
		/// <summary>
		/// Converts enum value of CoreVerbalizationTextSnippetType to an integer index value.
		/// </summary>
		protected override int ValueToIndex(CoreVerbalizationTextSnippetType enumValue)
		{
			return (int)enumValue;
		}
	}
	#endregion // CoreVerbalizationSets class
	#region FactType verbalization
	public partial class FactType : IVerbalize
	{
		/// <summary>
		/// IVerbalize.GetVerbalization implementation
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			if (errorOwner != null)
			{
				bool firstElement = true;
				foreach (ModelError error in errorOwner.ErrorCollection)
				{
					if (firstElement)
					{
						firstElement = false;
						beginVerbalization(VerbalizationContent.ErrorReport);
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(error.Name);
				}
				if (!(firstElement))
				{
					return false;
				}
			}
			VerbalizationSets<CoreVerbalizationTextSnippetType> snippets = (VerbalizationSets<CoreVerbalizationTextSnippetType>)snippetsDictionary[typeof(CoreVerbalizationTextSnippetType)];
			RoleMoveableCollection factRoles = this.RoleCollection;
			int factArity = factRoles.Count;
			ReadingOrderMoveableCollection allReadingOrders = this.ReadingOrderCollection;
			const bool isDeontic = false;
			Reading reading;
			string[] basicRoleReplacements = new string[factArity];
			for (int i = 0; i < factArity; ++i)
			{
				ObjectType rolePlayer = factRoles[i].RolePlayer;
				string basicReplacement;
				if (rolePlayer != null)
				{
					basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name);
				}
				else
				{
					basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
				}
				basicRoleReplacements[i] = basicReplacement;
			}
			beginVerbalization(VerbalizationContent.Normal);
			reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
			FactType.WriteVerbalizerSentence(writer, FactType.PopulatePredicateText(reading, factRoles, basicRoleReplacements), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
	}
	#endregion // FactType verbalization
	#region ObjectType verbalization
	public partial class ObjectType : IVerbalize
	{
		/// <summary>
		/// IVerbalize.GetVerbalization implementation
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			if (errorOwner != null)
			{
				bool firstElement = true;
				foreach (ModelError error in errorOwner.ErrorCollection)
				{
					if (firstElement)
					{
						firstElement = false;
						beginVerbalization(VerbalizationContent.ErrorReport);
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(error.Name);
				}
				if (!(firstElement))
				{
					return false;
				}
			}
			VerbalizationSets<CoreVerbalizationTextSnippetType> snippets = (VerbalizationSets<CoreVerbalizationTextSnippetType>)snippetsDictionary[typeof(CoreVerbalizationTextSnippetType)];
			StringBuilder sbTemp = null;
			const bool isDeontic = false;
			CoreVerbalizationTextSnippetType variableSnippetSnippetType1 = 0;
			if (!(this.IsValueType))
			{
				variableSnippetSnippetType1 = CoreVerbalizationTextSnippetType.EntityTypeVerbalization;
			}
			else
			{
				variableSnippetSnippetType1 = CoreVerbalizationTextSnippetType.ValueTypeVerbalization;
			}
			beginVerbalization(VerbalizationContent.Normal);
			string variableSnippetFormat1 = snippets.GetSnippet(variableSnippetSnippetType1, isDeontic, isNegative);
			string variableSnippet1Replace1;
			string variableSnippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectType, isDeontic, isNegative);
			string variableSnippet1Replace1Replace1;
			variableSnippet1Replace1Replace1 = this.Name;
			variableSnippet1Replace1 = string.Format(writer.FormatProvider, variableSnippet1ReplaceFormat1, variableSnippet1Replace1Replace1);
			FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, variableSnippetFormat1, variableSnippet1Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			if (this.NestedFactType != null)
			{
				RoleMoveableCollection factRoles = null;
				int factArity = 0;
				ReadingOrderMoveableCollection allReadingOrders = null;
				Reading reading = null;
				FactType nested = this.NestedFactType;
				factRoles = nested.RoleCollection;
				factArity = factRoles.Count;
				allReadingOrders = nested.ReadingOrderCollection;
				string[] basicRoleReplacements = new string[factArity];
				for (int i = 0; i < factArity; ++i)
				{
					ObjectType rolePlayer = factRoles[i].RolePlayer;
					string basicReplacement;
					if (rolePlayer != null)
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name);
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				writer.WriteLine();
				string snippetFormat2 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectifiesFactTypeVerbalization, isDeontic, isNegative);
				string snippet2Replace1;
				string snippet2ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectType, isDeontic, isNegative);
				string snippet2Replace1Replace1;
				snippet2Replace1Replace1 = this.Name;
				snippet2Replace1 = string.Format(writer.FormatProvider, snippet2ReplaceFormat1, snippet2Replace1Replace1);
				string snippet2Replace2;
				reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
				snippet2Replace2 = FactType.PopulatePredicateText(reading, factRoles, basicRoleReplacements);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat2, snippet2Replace1, snippet2Replace2), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			if (this.PreferredIdentifier != null)
			{
				writer.WriteLine();
				string snippetFormat3 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ReferenceSchemeVerbalization, isDeontic, isNegative);
				string snippet3Replace1;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				RoleMoveableCollection includedRoles = ((ConstraintRoleSequence)this.PreferredIdentifier).RoleCollection;
				int constraintRoleArity = includedRoles.Count;
				for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity; ++RoleIter1)
				{
					Role primaryRole = includedRoles[RoleIter1];
					FactType parentFact = primaryRole.FactType;
					RoleMoveableCollection factRoles = parentFact.RoleCollection;
					int factArity = factRoles.Count;
					ReadingOrderMoveableCollection allReadingOrders = parentFact.ReadingOrderCollection;
					Reading reading = null;
					string[] basicRoleReplacements = new string[factArity];
					for (int i = 0; i < factArity; ++i)
					{
						ObjectType rolePlayer = factRoles[i].RolePlayer;
						string basicReplacement;
						if (rolePlayer != null)
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name);
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
						}
						basicRoleReplacements[i] = basicReplacement;
					}
					CoreVerbalizationTextSnippetType listSnippet;
					if (RoleIter1 == 0)
					{
						listSnippet = CoreVerbalizationTextSnippetType.CompoundListOpen;
					}
					else if (RoleIter1 == (constraintRoleArity - 1))
					{
						if (RoleIter1 == 1)
						{
							listSnippet = CoreVerbalizationTextSnippetType.CompoundListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationTextSnippetType.CompoundListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationTextSnippetType.CompoundListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					snippet3Replace1 = null;
					reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
					snippet3Replace1 = FactType.PopulatePredicateText(reading, factRoles, basicRoleReplacements);
					sbTemp.Append(snippet3Replace1);
					if (RoleIter1 == (constraintRoleArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.CompoundListClose, isDeontic, isNegative));
					}
				}
				snippet3Replace1 = sbTemp.ToString();
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat3, snippet3Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			if (this.HasReferenceMode)
			{
				writer.WriteLine();
				string snippetFormat4 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ReferenceModeVerbalization, isDeontic, isNegative);
				string snippet4Replace1;
				snippet4Replace1 = this.ReferenceModeString;
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat4, snippet4Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			if (this.IsIndependent)
			{
				writer.WriteLine();
				string snippetFormat5 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.IndependentVerbalization, isDeontic, isNegative);
				string snippet5Replace1;
				string snippet5ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectType, isDeontic, isNegative);
				string snippet5Replace1Replace1;
				snippet5Replace1Replace1 = this.Name;
				snippet5Replace1 = string.Format(writer.FormatProvider, snippet5ReplaceFormat1, snippet5Replace1Replace1);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat5, snippet5Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			if (this.DataType != null)
			{
				writer.WriteLine();
				string snippetFormat6 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.PortableDataTypeVerbalization, isDeontic, isNegative);
				string snippet6Replace1;
				snippet6Replace1 = this.DataType.ToString();
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat6, snippet6Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
	}
	#endregion // ObjectType verbalization
	#region Note verbalization
	public partial class Note : IVerbalize
	{
		/// <summary>
		/// IVerbalize.GetVerbalization implementation
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			if (errorOwner != null)
			{
				bool firstElement = true;
				foreach (ModelError error in errorOwner.ErrorCollection)
				{
					if (firstElement)
					{
						firstElement = false;
						beginVerbalization(VerbalizationContent.ErrorReport);
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(error.Name);
				}
				if (!(firstElement))
				{
					return false;
				}
			}
			VerbalizationSets<CoreVerbalizationTextSnippetType> snippets = (VerbalizationSets<CoreVerbalizationTextSnippetType>)snippetsDictionary[typeof(CoreVerbalizationTextSnippetType)];
			const bool isDeontic = false;
			beginVerbalization(VerbalizationContent.Normal);
			string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.NotesVerbalization, isDeontic, isNegative);
			string snippet1Replace1;
			snippet1Replace1 = this.Text;
			FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
	}
	#endregion // Note verbalization
	#region InternalUniquenessConstraint verbalization
	public partial class InternalUniquenessConstraint : IVerbalize
	{
		/// <summary>
		/// IVerbalize.GetVerbalization implementation
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			if (errorOwner != null)
			{
				bool firstElement = true;
				foreach (ModelError error in errorOwner.ErrorCollection)
				{
					if (firstElement)
					{
						firstElement = false;
						beginVerbalization(VerbalizationContent.ErrorReport);
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(error.Name);
				}
				if (!(firstElement))
				{
					return false;
				}
			}
			VerbalizationSets<CoreVerbalizationTextSnippetType> snippets = (VerbalizationSets<CoreVerbalizationTextSnippetType>)snippetsDictionary[typeof(CoreVerbalizationTextSnippetType)];
			bool isDeontic = (this as IConstraint).Modality == ConstraintModality.Deontic;
			StringBuilder sbTemp = null;
			FactType parentFact = this.FactType;
			RoleMoveableCollection includedRoles = this.RoleCollection;
			RoleMoveableCollection factRoles = parentFact.RoleCollection;
			int factArity = factRoles.Count;
			ReadingOrderMoveableCollection allReadingOrders = parentFact.ReadingOrderCollection;
			int includedArity = includedRoles.Count;
			if ((allReadingOrders.Count == 0) || (includedArity == 0))
			{
				return false;
			}
			string[] basicRoleReplacements = new string[factArity];
			for (int i = 0; i < factArity; ++i)
			{
				ObjectType rolePlayer = factRoles[i].RolePlayer;
				string basicReplacement;
				if (rolePlayer != null)
				{
					int subscript = 0;
					bool useSubscript = false;
					if (true)
					{
						int j = 0;
						for (; j < i; ++j)
						{
							if (object.ReferenceEquals(rolePlayer, factRoles[j].RolePlayer))
							{
								useSubscript = true;
								subscript = subscript + 1;
							}
						}
						for (j = i + 1; !(useSubscript) && (j < factArity); ++j)
						{
							if (object.ReferenceEquals(rolePlayer, factRoles[j].RolePlayer))
							{
								useSubscript = true;
							}
						}
					}
					if (useSubscript)
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), rolePlayer.Name, subscript + 1);
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name);
					}
				}
				else
				{
					basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
				}
				basicRoleReplacements[i] = basicReplacement;
			}
			string[] roleReplacements = new string[factArity];
			Reading reading;
			if ((factArity == 1) && !(isNegative))
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.OccursInPopulation, isDeontic, isNegative);
				string snippet1Replace1Replace1;
				string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.UniversalQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1Replace1;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (int RoleIter1 = 0; RoleIter1 < includedArity; ++RoleIter1)
				{
					CoreVerbalizationTextSnippetType listSnippet;
					if (RoleIter1 == 0)
					{
						listSnippet = CoreVerbalizationTextSnippetType.SimpleListOpen;
					}
					else if (RoleIter1 == (includedArity - 1))
					{
						if (RoleIter1 == 1)
						{
							listSnippet = CoreVerbalizationTextSnippetType.SimpleListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationTextSnippetType.SimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationTextSnippetType.SimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[RoleIter1])]);
					if (RoleIter1 == (includedArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1Replace1Replace1 = sbTemp.ToString();
				snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1);
				string snippet1Replace1Replace2;
				reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
				snippet1Replace1Replace2 = FactType.PopulatePredicateText(reading, factRoles, basicRoleReplacements);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (factArity == 1)
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace1;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.OccursInPopulation, isDeontic, isNegative);
				string snippet1Replace1Replace1;
				string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1Replace1;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (int RoleIter1 = 0; RoleIter1 < includedArity; ++RoleIter1)
				{
					CoreVerbalizationTextSnippetType listSnippet;
					if (RoleIter1 == 0)
					{
						listSnippet = CoreVerbalizationTextSnippetType.SimpleListOpen;
					}
					else if (RoleIter1 == (includedArity - 1))
					{
						if (RoleIter1 == 1)
						{
							listSnippet = CoreVerbalizationTextSnippetType.SimpleListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationTextSnippetType.SimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationTextSnippetType.SimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[RoleIter1])]);
					if (RoleIter1 == (includedArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1Replace1Replace1 = sbTemp.ToString();
				snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1);
				string snippet1Replace1Replace2;
				reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
				snippet1Replace1Replace2 = FactType.PopulatePredicateText(reading, factRoles, basicRoleReplacements);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if ((factArity == includedArity) && !(isNegative))
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace1;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (int RoleIter1 = 0; RoleIter1 < factArity; ++RoleIter1)
				{
					Role primaryRole = factRoles[RoleIter1];
					CoreVerbalizationTextSnippetType listSnippet;
					if (RoleIter1 == 0)
					{
						listSnippet = CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalAndListOpen;
					}
					else if (RoleIter1 == (factArity - 1))
					{
						if (RoleIter1 == 1)
						{
							listSnippet = CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalAndListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalAndListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalAndListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					snippet1Replace1 = null;
					reading = FactType.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, factRoles, true);
					int snippet1ReplaceFactRoleIter1 = 0;
					for (; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
					{
						Role currentRole = factRoles[snippet1ReplaceFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = basicRoleReplacements[snippet1ReplaceFactRoleIter1];
						if (primaryRole == currentRole)
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.MoreThanOneQuantifier, isDeontic, isNegative), basicReplacement);
						}
						else
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative), basicReplacement);
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
					}
					snippet1Replace1 = FactType.PopulatePredicateText(reading, factRoles, roleReplacements);
					sbTemp.Append(snippet1Replace1);
					if (RoleIter1 == (factArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalAndListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1 = sbTemp.ToString();
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				writer.WriteLine();
				string snippetFormat2 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet2Replace1;
				string snippet2ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.OccursInPopulation, isDeontic, isNegative);
				string snippet2Replace1Replace1;
				string snippet2Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.CombinationIdentifier, isDeontic, isNegative);
				string snippet2Replace1Replace1Replace1;
				string snippet2Replace1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.UniversalQuantifier, isDeontic, isNegative);
				string snippet2Replace1Replace1Replace1Replace1;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (int RoleIter1 = 0; RoleIter1 < includedArity; ++RoleIter1)
				{
					CoreVerbalizationTextSnippetType listSnippet;
					if (RoleIter1 == 0)
					{
						listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListOpen;
					}
					else if (RoleIter1 == (includedArity - 1))
					{
						if (RoleIter1 == 1)
						{
							listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[RoleIter1])]);
					if (RoleIter1 == (includedArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.CompactSimpleListClose, isDeontic, isNegative));
					}
				}
				snippet2Replace1Replace1Replace1Replace1 = sbTemp.ToString();
				snippet2Replace1Replace1Replace1 = string.Format(writer.FormatProvider, snippet2Replace1Replace1ReplaceFormat1, snippet2Replace1Replace1Replace1Replace1);
				snippet2Replace1Replace1 = string.Format(writer.FormatProvider, snippet2Replace1ReplaceFormat1, snippet2Replace1Replace1Replace1);
				string snippet2Replace1Replace2;
				reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
				snippet2Replace1Replace2 = FactType.PopulatePredicateText(reading, factRoles, basicRoleReplacements);
				snippet2Replace1 = string.Format(writer.FormatProvider, snippet2ReplaceFormat1, snippet2Replace1Replace1, snippet2Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat2, snippet2Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (factArity == includedArity)
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace1;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.OccursInPopulation, isDeontic, isNegative);
				string snippet1Replace1Replace1;
				string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.CombinationIdentifier, isDeontic, isNegative);
				string snippet1Replace1Replace1Replace1;
				string snippet1Replace1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1Replace1Replace1;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (int RoleIter1 = 0; RoleIter1 < includedArity; ++RoleIter1)
				{
					CoreVerbalizationTextSnippetType listSnippet;
					if (RoleIter1 == 0)
					{
						listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListOpen;
					}
					else if (RoleIter1 == (includedArity - 1))
					{
						if (RoleIter1 == 1)
						{
							listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[RoleIter1])]);
					if (RoleIter1 == (includedArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.CompactSimpleListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1Replace1Replace1Replace1 = sbTemp.ToString();
				snippet1Replace1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1Replace1);
				snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1);
				string snippet1Replace1Replace2;
				reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
				snippet1Replace1Replace2 = FactType.PopulatePredicateText(reading, factRoles, basicRoleReplacements);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if ((factArity == 2) && !(isNegative))
			{
				reading = FactType.GetMatchingReading(allReadingOrders, null, null, includedRoles, false, false, factRoles, false);
				if (reading != null)
				{
					beginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1;
					int snippet1ReplaceFactRoleIter1 = 0;
					for (; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
					{
						Role currentRole = factRoles[snippet1ReplaceFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = basicRoleReplacements[snippet1ReplaceFactRoleIter1];
						if (includedRoles.Contains(currentRole))
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
						}
						else
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.OneQuantifier, isDeontic, isNegative), basicReplacement);
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
					}
					snippet1Replace1 = FactType.PopulatePredicateText(reading, factRoles, roleReplacements);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
					if (reading != null)
					{
						beginVerbalization(VerbalizationContent.Normal);
						string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace1;
						string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
						string snippet1Replace1Replace1;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						for (int RoleIter1 = 0; RoleIter1 < includedArity; ++RoleIter1)
						{
							CoreVerbalizationTextSnippetType listSnippet;
							if (RoleIter1 == 0)
							{
								listSnippet = CoreVerbalizationTextSnippetType.SimpleListOpen;
							}
							else if (RoleIter1 == (includedArity - 1))
							{
								if (RoleIter1 == 1)
								{
									listSnippet = CoreVerbalizationTextSnippetType.SimpleListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationTextSnippetType.SimpleListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationTextSnippetType.SimpleListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[RoleIter1])]);
							if (RoleIter1 == (includedArity - 1))
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
							}
						}
						snippet1Replace1Replace1 = sbTemp.ToString();
						string snippet1Replace1Replace2;
						reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
						int snippet1Replace1ReplaceFactRoleIter2 = 0;
						for (; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
						{
							Role currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
							string roleReplacement = null;
							string basicReplacement = basicRoleReplacements[snippet1Replace1ReplaceFactRoleIter2];
							if (includedRoles.Contains(currentRole))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.OneQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
						}
						snippet1Replace1Replace2 = FactType.PopulatePredicateText(reading, factRoles, roleReplacements);
						snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
				}
				if (this.IsPreferred)
				{
					writer.WriteLine();
					string snippetFormat2 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ConstraintProvidesPreferredIdentifier, isDeontic, isNegative);
					string snippet2Replace1;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int RoleIter1 = 0; RoleIter1 < includedArity; ++RoleIter1)
					{
						sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[RoleIter1])]);
					}
					snippet2Replace1 = sbTemp.ToString();
					string snippet2Replace2;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int RoleIter2 = 0; RoleIter2 < factArity; ++RoleIter2)
					{
						if (includedRoles.IndexOf(factRoles[RoleIter2]) == -1)
						{
							sbTemp.Append(basicRoleReplacements[RoleIter2]);
						}
					}
					snippet2Replace2 = sbTemp.ToString();
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat2, snippet2Replace1, snippet2Replace2), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
			}
			else if (factArity == 2)
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
				string snippet1Replace1;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (int RoleIter1 = 0; RoleIter1 < includedArity; ++RoleIter1)
				{
					CoreVerbalizationTextSnippetType listSnippet;
					if (RoleIter1 == 0)
					{
						listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListOpen;
					}
					else if (RoleIter1 == (includedArity - 1))
					{
						if (RoleIter1 == 1)
						{
							listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[RoleIter1])]);
					if (RoleIter1 == (includedArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.CompactSimpleListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1 = sbTemp.ToString();
				string snippet1Replace2;
				string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace2Replace1;
				reading = FactType.GetMatchingReading(allReadingOrders, null, null, includedRoles, false, false, factRoles, true);
				int snippet1Replace2ReplaceFactRoleIter1 = 0;
				for (; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
				{
					Role currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
					string roleReplacement = null;
					string basicReplacement = basicRoleReplacements[snippet1Replace2ReplaceFactRoleIter1];
					if (includedRoles.Contains(currentRole))
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
					}
					else if (!(includedRoles.Contains(currentRole)))
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.OneQuantifier, isDeontic, isNegative), basicReplacement);
					}
					if (roleReplacement == null)
					{
						roleReplacement = basicReplacement;
					}
					roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
				}
				snippet1Replace2Replace1 = FactType.PopulatePredicateText(reading, factRoles, roleReplacements);
				snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (isNegative)
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace1;
				reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
				int snippet1ReplaceFactRoleIter1 = 0;
				for (; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
				{
					Role currentRole = factRoles[snippet1ReplaceFactRoleIter1];
					string roleReplacement = null;
					string basicReplacement = basicRoleReplacements[snippet1ReplaceFactRoleIter1];
					if (includedRoles.Contains(currentRole))
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative), basicReplacement);
					}
					else
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.MoreThanOneQuantifier, isDeontic, isNegative), basicReplacement);
					}
					if (roleReplacement == null)
					{
						roleReplacement = basicReplacement;
					}
					roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
				}
				snippet1Replace1 = FactType.PopulatePredicateText(reading, factRoles, roleReplacements);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ForEachQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (int RoleIter1 = 0; RoleIter1 < includedArity; ++RoleIter1)
				{
					CoreVerbalizationTextSnippetType listSnippet;
					if (RoleIter1 == 0)
					{
						listSnippet = CoreVerbalizationTextSnippetType.SimpleListOpen;
					}
					else if (RoleIter1 == (includedArity - 1))
					{
						if (RoleIter1 == 1)
						{
							listSnippet = CoreVerbalizationTextSnippetType.SimpleListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationTextSnippetType.SimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationTextSnippetType.SimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[RoleIter1])]);
					if (RoleIter1 == (includedArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1Replace1 = sbTemp.ToString();
				string snippet1Replace1Replace2;
				reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
				int snippet1Replace1ReplaceFactRoleIter2 = 0;
				for (; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
				{
					Role currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
					string roleReplacement = null;
					string basicReplacement = basicRoleReplacements[snippet1Replace1ReplaceFactRoleIter2];
					if (includedRoles.Contains(currentRole))
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
					}
					else if (!(includedRoles.Contains(currentRole)))
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.OneQuantifier, isDeontic, isNegative), basicReplacement);
					}
					if (roleReplacement == null)
					{
						roleReplacement = basicReplacement;
					}
					roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
				}
				snippet1Replace1Replace2 = FactType.PopulatePredicateText(reading, factRoles, roleReplacements);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
	}
	#endregion // InternalUniquenessConstraint verbalization
	#region SimpleMandatoryConstraint verbalization
	public partial class SimpleMandatoryConstraint : IVerbalize
	{
		/// <summary>
		/// IVerbalize.GetVerbalization implementation
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			if (errorOwner != null)
			{
				bool firstElement = true;
				foreach (ModelError error in errorOwner.ErrorCollection)
				{
					if (firstElement)
					{
						firstElement = false;
						beginVerbalization(VerbalizationContent.ErrorReport);
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(error.Name);
				}
				if (!(firstElement))
				{
					return false;
				}
			}
			VerbalizationSets<CoreVerbalizationTextSnippetType> snippets = (VerbalizationSets<CoreVerbalizationTextSnippetType>)snippetsDictionary[typeof(CoreVerbalizationTextSnippetType)];
			bool isDeontic = (this as IConstraint).Modality == ConstraintModality.Deontic;
			StringBuilder sbTemp = null;
			FactType parentFact = this.FactType;
			RoleMoveableCollection includedRoles = this.RoleCollection;
			RoleMoveableCollection factRoles = parentFact.RoleCollection;
			int factArity = factRoles.Count;
			ReadingOrderMoveableCollection allReadingOrders = parentFact.ReadingOrderCollection;
			int includedArity = includedRoles.Count;
			if ((allReadingOrders.Count == 0) || (includedArity == 0))
			{
				return false;
			}
			string[] basicRoleReplacements = new string[factArity];
			for (int i = 0; i < factArity; ++i)
			{
				ObjectType rolePlayer = factRoles[i].RolePlayer;
				string basicReplacement;
				if (rolePlayer != null)
				{
					int subscript = 0;
					bool useSubscript = false;
					if (true)
					{
						int j = 0;
						for (; j < i; ++j)
						{
							if (object.ReferenceEquals(rolePlayer, factRoles[j].RolePlayer))
							{
								useSubscript = true;
								subscript = subscript + 1;
							}
						}
						for (j = i + 1; !(useSubscript) && (j < factArity); ++j)
						{
							if (object.ReferenceEquals(rolePlayer, factRoles[j].RolePlayer))
							{
								useSubscript = true;
							}
						}
					}
					if (useSubscript)
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), rolePlayer.Name, subscript + 1);
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name);
					}
				}
				else
				{
					basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
				}
				basicRoleReplacements[i] = basicReplacement;
			}
			string[] roleReplacements = new string[factArity];
			Reading reading;
			if (factArity == 1)
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.UniversalQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1;
				reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
				snippet1Replace1Replace1 = FactType.PopulatePredicateText(reading, factRoles, basicRoleReplacements);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (factArity == 2)
			{
				reading = FactType.GetMatchingReading(allReadingOrders, null, null, includedRoles, false, false, factRoles, false);
				if (reading != null)
				{
					beginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1;
					int snippet1ReplaceFactRoleIter1 = 0;
					for (; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
					{
						Role currentRole = factRoles[snippet1ReplaceFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = basicRoleReplacements[snippet1ReplaceFactRoleIter1];
						if (includedRoles.Contains(currentRole))
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
						}
						else
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
					}
					snippet1Replace1 = FactType.PopulatePredicateText(reading, factRoles, roleReplacements);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
					if (reading != null)
					{
						beginVerbalization(VerbalizationContent.Normal);
						string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace1;
						string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
						string snippet1Replace1Replace1;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						for (int RoleIter1 = 0; RoleIter1 < includedArity; ++RoleIter1)
						{
							CoreVerbalizationTextSnippetType listSnippet;
							if (RoleIter1 == 0)
							{
								listSnippet = CoreVerbalizationTextSnippetType.SimpleListOpen;
							}
							else if (RoleIter1 == (includedArity - 1))
							{
								if (RoleIter1 == 1)
								{
									listSnippet = CoreVerbalizationTextSnippetType.SimpleListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationTextSnippetType.SimpleListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationTextSnippetType.SimpleListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[RoleIter1])]);
							if (RoleIter1 == (includedArity - 1))
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
							}
						}
						snippet1Replace1Replace1 = sbTemp.ToString();
						string snippet1Replace1Replace2;
						reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
						int snippet1Replace1ReplaceFactRoleIter2 = 0;
						for (; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
						{
							Role currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
							string roleReplacement = null;
							string basicReplacement = basicRoleReplacements[snippet1Replace1ReplaceFactRoleIter2];
							if (includedRoles.Contains(currentRole))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
						}
						snippet1Replace1Replace2 = FactType.PopulatePredicateText(reading, factRoles, roleReplacements);
						snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
				}
			}
			else
			{
				reading = FactType.GetMatchingReading(allReadingOrders, null, null, includedRoles, false, false, factRoles, false);
				if (reading != null)
				{
					beginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1;
					int snippet1ReplaceFactRoleIter1 = 0;
					for (; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
					{
						Role currentRole = factRoles[snippet1ReplaceFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = basicRoleReplacements[snippet1ReplaceFactRoleIter1];
						if (includedRoles.Contains(currentRole))
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
						}
						else
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
					}
					snippet1Replace1 = FactType.PopulatePredicateText(reading, factRoles, roleReplacements);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
					if (reading != null)
					{
						beginVerbalization(VerbalizationContent.Normal);
						string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace1;
						string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ForEachQuantifier, isDeontic, isNegative);
						string snippet1Replace1Replace1;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						for (int RoleIter1 = 0; RoleIter1 < includedArity; ++RoleIter1)
						{
							CoreVerbalizationTextSnippetType listSnippet;
							if (RoleIter1 == 0)
							{
								listSnippet = CoreVerbalizationTextSnippetType.SimpleListOpen;
							}
							else if (RoleIter1 == (includedArity - 1))
							{
								if (RoleIter1 == 1)
								{
									listSnippet = CoreVerbalizationTextSnippetType.SimpleListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationTextSnippetType.SimpleListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationTextSnippetType.SimpleListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[RoleIter1])]);
							if (RoleIter1 == (includedArity - 1))
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
							}
						}
						snippet1Replace1Replace1 = sbTemp.ToString();
						string snippet1Replace1Replace2;
						reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
						int snippet1Replace1ReplaceFactRoleIter2 = 0;
						for (; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
						{
							Role currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
							string roleReplacement = null;
							string basicReplacement = basicRoleReplacements[snippet1Replace1ReplaceFactRoleIter2];
							if (includedRoles.Contains(currentRole))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
						}
						snippet1Replace1Replace2 = FactType.PopulatePredicateText(reading, factRoles, roleReplacements);
						snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
				}
			}
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
	}
	#endregion // SimpleMandatoryConstraint verbalization
	#region DisjunctiveMandatoryConstraint verbalization
	public partial class DisjunctiveMandatoryConstraint : IVerbalize
	{
		/// <summary>
		/// IVerbalize.GetVerbalization implementation
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			if (errorOwner != null)
			{
				bool firstElement = true;
				foreach (ModelError error in errorOwner.ErrorCollection)
				{
					if (firstElement)
					{
						firstElement = false;
						beginVerbalization(VerbalizationContent.ErrorReport);
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(error.Name);
				}
				if (!(firstElement))
				{
					return false;
				}
			}
			VerbalizationSets<CoreVerbalizationTextSnippetType> snippets = (VerbalizationSets<CoreVerbalizationTextSnippetType>)snippetsDictionary[typeof(CoreVerbalizationTextSnippetType)];
			bool isDeontic = false;
			StringBuilder sbTemp = null;
			FactType parentFact;
			RoleMoveableCollection factRoles;
			int factArity;
			ReadingOrderMoveableCollection allReadingOrders;
			RoleMoveableCollection allConstraintRoles = this.RoleCollection;
			FactTypeMoveableCollection allFacts = this.FactTypeCollection;
			int allFactsCount = allFacts.Count;
			if (allFactsCount == 0)
			{
				return false;
			}
			string[][] allBasicRoleReplacements = new string[allFactsCount][];
			int minFactArity = int.MaxValue;
			int maxFactArity = int.MinValue;
			int iFact = 0;
			for (; iFact < allFactsCount; ++iFact)
			{
				FactType currentFact = allFacts[iFact];
				if (currentFact.ReadingOrderCollection.Count == 0)
				{
					return false;
				}
				factRoles = currentFact.RoleCollection;
				factArity = factRoles.Count;
				if (factArity < minFactArity)
				{
					minFactArity = factArity;
				}
				if (factArity > maxFactArity)
				{
					maxFactArity = factArity;
				}
				string[] basicRoleReplacements = new string[factArity];
				for (int i = 0; i < factArity; ++i)
				{
					ObjectType rolePlayer = factRoles[i].RolePlayer;
					string basicReplacement;
					if (rolePlayer != null)
					{
						int subscript = 0;
						bool useSubscript = false;
						if (true)
						{
							int j = 0;
							for (; j < i; ++j)
							{
								if (object.ReferenceEquals(rolePlayer, factRoles[j].RolePlayer))
								{
									useSubscript = true;
									subscript = subscript + 1;
								}
							}
							for (j = i + 1; !(useSubscript) && (j < factArity); ++j)
							{
								if (object.ReferenceEquals(rolePlayer, factRoles[j].RolePlayer))
								{
									useSubscript = true;
								}
							}
						}
						if (useSubscript)
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), rolePlayer.Name, subscript + 1);
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name);
						}
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				allBasicRoleReplacements[iFact] = basicRoleReplacements;
			}
			int constraintRoleArity = allConstraintRoles.Count;
			Reading[] allConstraintRoleReadings = new Reading[constraintRoleArity];
			string[] roleReplacements = new string[maxFactArity];
			Reading reading;
			if (isNegative && (maxFactArity <= 1))
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace1;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.NegativeReadingForUnaryOnlyDisjunctiveMandatory, isDeontic, isNegative);
				string snippet1Replace1Replace1;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (int RoleIter1 = 0; RoleIter1 < 1; ++RoleIter1)
				{
					Role primaryRole = allConstraintRoles[RoleIter1];
					parentFact = primaryRole.FactType;
					factRoles = parentFact.RoleCollection;
					factArity = factRoles.Count;
					allReadingOrders = parentFact.ReadingOrderCollection;
					int currentFactIndex = allFacts.IndexOf(parentFact);
					string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
					CoreVerbalizationTextSnippetType listSnippet;
					listSnippet = CoreVerbalizationTextSnippetType.SimpleListOpen;
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(allConstraintRoles[RoleIter1])]);
					sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
				}
				snippet1Replace1Replace1 = sbTemp.ToString();
				string snippet1Replace1Replace2;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity; ++RoleIter2)
				{
					Role primaryRole = allConstraintRoles[RoleIter2];
					parentFact = primaryRole.FactType;
					factRoles = parentFact.RoleCollection;
					factArity = factRoles.Count;
					allReadingOrders = parentFact.ReadingOrderCollection;
					int currentFactIndex = allFacts.IndexOf(parentFact);
					string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
					CoreVerbalizationTextSnippetType listSnippet;
					if (RoleIter2 == 0)
					{
						listSnippet = CoreVerbalizationTextSnippetType.IndentedCompoundListOpen;
					}
					else if (RoleIter2 == (constraintRoleArity - 1))
					{
						if (RoleIter2 == 1)
						{
							listSnippet = CoreVerbalizationTextSnippetType.IndentedCompoundListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationTextSnippetType.IndentedCompoundListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationTextSnippetType.IndentedCompoundListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					snippet1Replace1Replace2 = null;
					reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
					snippet1Replace1Replace2 = FactType.PopulatePredicateText(reading, factRoles, basicRoleReplacements);
					sbTemp.Append(snippet1Replace1Replace2);
					if (RoleIter2 == (constraintRoleArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.IndentedCompoundListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1Replace2 = sbTemp.ToString();
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (!(isNegative) && (maxFactArity <= 1))
			{
				beginVerbalization(VerbalizationContent.Normal);
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				bool factTextIsFirstPass1 = true;
				for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity; ++RoleIter1)
				{
					Role primaryRole = allConstraintRoles[RoleIter1];
					parentFact = primaryRole.FactType;
					factRoles = parentFact.RoleCollection;
					factArity = factRoles.Count;
					allReadingOrders = parentFact.ReadingOrderCollection;
					int currentFactIndex = allFacts.IndexOf(parentFact);
					string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
					CoreVerbalizationTextSnippetType listSnippet;
					if (RoleIter1 == 0)
					{
						listSnippet = CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalOrListOpen;
					}
					else if (RoleIter1 == (constraintRoleArity - 1))
					{
						if (RoleIter1 == 1)
						{
							listSnippet = CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalOrListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalOrListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalOrListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					string factText1 = null;
					reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
					int factTextFactRoleIter1 = 0;
					for (; factTextFactRoleIter1 < factArity; ++factTextFactRoleIter1)
					{
						Role currentRole = factRoles[factTextFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = basicRoleReplacements[factTextFactRoleIter1];
						if ((primaryRole == currentRole) && factTextIsFirstPass1)
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
						}
						else if (primaryRole == currentRole)
						{
							roleReplacement = "";
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[factTextFactRoleIter1] = roleReplacement;
					}
					factText1 = FactType.PopulatePredicateText(reading, factRoles, roleReplacements);
					sbTemp.Append(factText1);
					if (RoleIter1 == (constraintRoleArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalOrListClose, isDeontic, isNegative));
					}
					factTextIsFirstPass1 = false;
				}
				FactType.WriteVerbalizerSentence(writer, sbTemp.ToString(), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (!(isNegative))
			{
				bool missingReading1 = false;
				int readingMatchIndex1 = 0;
				for (; !(missingReading1) && (readingMatchIndex1 < constraintRoleArity); ++readingMatchIndex1)
				{
					Role primaryRole = allConstraintRoles[readingMatchIndex1];
					parentFact = primaryRole.FactType;
					factRoles = parentFact.RoleCollection;
					allReadingOrders = parentFact.ReadingOrderCollection;
					reading = FactType.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, factRoles, false);
					if (reading == null)
					{
						missingReading1 = true;
					}
					else
					{
						allConstraintRoleReadings[readingMatchIndex1] = reading;
					}
				}
				if (!(missingReading1))
				{
					beginVerbalization(VerbalizationContent.Normal);
					int listCompositeCount1 = 0;
					int listCompositeIterator1;
					for (listCompositeIterator1 = 0; listCompositeIterator1 < constraintRoleArity; ++listCompositeIterator1)
					{
						Role primaryRole = allConstraintRoles[listCompositeIterator1];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						if (factArity >= 2)
						{
							listCompositeCount1 = listCompositeCount1 + 1;
						}
					}
					for (listCompositeIterator1 = 0; listCompositeIterator1 < constraintRoleArity; ++listCompositeIterator1)
					{
						Role primaryRole = allConstraintRoles[listCompositeIterator1];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						if (factArity == 1)
						{
							listCompositeCount1 = listCompositeCount1 + 1;
						}
					}
					listCompositeIterator1 = 0;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					string list1Item1 = null;
					bool list1ItemIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity; ++RoleIter1)
					{
						Role primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						int currentFactIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
						if (factArity >= 2)
						{
							CoreVerbalizationTextSnippetType listSnippet;
							if (listCompositeIterator1 == 0)
							{
								listSnippet = CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalOrListOpen;
							}
							else if (listCompositeIterator1 == (listCompositeCount1 - 1))
							{
								if (listCompositeIterator1 == 1)
								{
									listSnippet = CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalOrListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalOrListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalOrListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							list1Item1 = null;
							reading = allConstraintRoleReadings[RoleIter1];
							int list1ItemFactRoleIter1 = 0;
							for (; list1ItemFactRoleIter1 < factArity; ++list1ItemFactRoleIter1)
							{
								Role currentRole = factRoles[list1ItemFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = basicRoleReplacements[list1ItemFactRoleIter1];
								if ((primaryRole == currentRole) && list1ItemIsFirstPass1)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
								}
								else if (primaryRole == currentRole)
								{
									roleReplacement = "";
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								}
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[list1ItemFactRoleIter1] = roleReplacement;
							}
							list1Item1 = FactType.PopulatePredicateText(reading, factRoles, roleReplacements);
							sbTemp.Append(list1Item1);
							if (RoleIter1 == (listCompositeCount1 - 1))
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalOrListClose, isDeontic, isNegative));
							}
							listCompositeIterator1 = listCompositeIterator1 + 1;
							list1ItemIsFirstPass1 = false;
						}
					}
					string list1Item2 = null;
					for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity; ++RoleIter2)
					{
						Role primaryRole = allConstraintRoles[RoleIter2];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						int currentFactIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
						if (factArity == 1)
						{
							CoreVerbalizationTextSnippetType listSnippet;
							if (listCompositeIterator1 == 0)
							{
								listSnippet = CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalOrListOpen;
							}
							else if (listCompositeIterator1 == (listCompositeCount1 - 1))
							{
								if (listCompositeIterator1 == 1)
								{
									listSnippet = CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalOrListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalOrListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalOrListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							list1Item2 = null;
							reading = allConstraintRoleReadings[RoleIter2];
							int list1ItemFactRoleIter2 = 0;
							for (; list1ItemFactRoleIter2 < factArity; ++list1ItemFactRoleIter2)
							{
								Role currentRole = factRoles[list1ItemFactRoleIter2];
								string roleReplacement = null;
								string basicReplacement = basicRoleReplacements[list1ItemFactRoleIter2];
								roleReplacement = "";
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[list1ItemFactRoleIter2] = roleReplacement;
							}
							list1Item2 = FactType.PopulatePredicateText(reading, factRoles, roleReplacements);
							sbTemp.Append(list1Item2);
							if (RoleIter2 == (listCompositeCount1 - 1))
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.TopLevelIndentedLogicalOrListClose, isDeontic, isNegative));
							}
							listCompositeIterator1 = listCompositeIterator1 + 1;
						}
					}
					FactType.WriteVerbalizerSentence(writer, sbTemp.ToString(), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					beginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ForEachQuantifier, isDeontic, isNegative);
					string snippet1Replace1;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int RoleIter1 = 0; RoleIter1 < 1; ++RoleIter1)
					{
						Role primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						int currentFactIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
						CoreVerbalizationTextSnippetType listSnippet;
						listSnippet = CoreVerbalizationTextSnippetType.SimpleListOpen;
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(allConstraintRoles[RoleIter1])]);
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
					}
					snippet1Replace1 = sbTemp.ToString();
					string snippet1Replace2;
					int snippet1ReplaceCompositeCount2 = 0;
					int snippet1ReplaceCompositeIterator2;
					for (snippet1ReplaceCompositeIterator2 = 0; snippet1ReplaceCompositeIterator2 < constraintRoleArity; ++snippet1ReplaceCompositeIterator2)
					{
						Role primaryRole = allConstraintRoles[snippet1ReplaceCompositeIterator2];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						if (factArity >= 2)
						{
							snippet1ReplaceCompositeCount2 = snippet1ReplaceCompositeCount2 + 1;
						}
					}
					for (snippet1ReplaceCompositeIterator2 = 0; snippet1ReplaceCompositeIterator2 < constraintRoleArity; ++snippet1ReplaceCompositeIterator2)
					{
						Role primaryRole = allConstraintRoles[snippet1ReplaceCompositeIterator2];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						if (factArity == 1)
						{
							snippet1ReplaceCompositeCount2 = snippet1ReplaceCompositeCount2 + 1;
						}
					}
					snippet1ReplaceCompositeIterator2 = 0;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					string snippet1Replace2Item1 = null;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity; ++RoleIter1)
					{
						Role primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						int currentFactIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
						if (factArity >= 2)
						{
							CoreVerbalizationTextSnippetType listSnippet;
							if (snippet1ReplaceCompositeIterator2 == 0)
							{
								listSnippet = CoreVerbalizationTextSnippetType.IndentedLogicalOrListOpen;
							}
							else if (snippet1ReplaceCompositeIterator2 == (snippet1ReplaceCompositeCount2 - 1))
							{
								if (snippet1ReplaceCompositeIterator2 == 1)
								{
									listSnippet = CoreVerbalizationTextSnippetType.IndentedLogicalOrListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationTextSnippetType.IndentedLogicalOrListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationTextSnippetType.IndentedLogicalOrListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							snippet1Replace2Item1 = null;
							reading = FactType.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, true, factRoles, true);
							int snippet1Replace2ItemFactRoleIter1 = 0;
							for (; snippet1Replace2ItemFactRoleIter1 < factArity; ++snippet1Replace2ItemFactRoleIter1)
							{
								Role currentRole = factRoles[snippet1Replace2ItemFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = basicRoleReplacements[snippet1Replace2ItemFactRoleIter1];
								if (currentRole == primaryRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
								}
								else if (currentRole != primaryRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								}
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace2ItemFactRoleIter1] = roleReplacement;
							}
							snippet1Replace2Item1 = FactType.PopulatePredicateText(reading, factRoles, roleReplacements);
							sbTemp.Append(snippet1Replace2Item1);
							if (RoleIter1 == (snippet1ReplaceCompositeCount2 - 1))
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.IndentedLogicalOrListClose, isDeontic, isNegative));
							}
							snippet1ReplaceCompositeIterator2 = snippet1ReplaceCompositeIterator2 + 1;
						}
					}
					string snippet1Replace2Item2 = null;
					for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity; ++RoleIter2)
					{
						Role primaryRole = allConstraintRoles[RoleIter2];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						int currentFactIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
						if (factArity == 1)
						{
							CoreVerbalizationTextSnippetType listSnippet;
							if (snippet1ReplaceCompositeIterator2 == 0)
							{
								listSnippet = CoreVerbalizationTextSnippetType.IndentedLogicalOrListOpen;
							}
							else if (snippet1ReplaceCompositeIterator2 == (snippet1ReplaceCompositeCount2 - 1))
							{
								if (snippet1ReplaceCompositeIterator2 == 1)
								{
									listSnippet = CoreVerbalizationTextSnippetType.IndentedLogicalOrListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationTextSnippetType.IndentedLogicalOrListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationTextSnippetType.IndentedLogicalOrListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							snippet1Replace2Item2 = null;
							reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
							int snippet1Replace2ItemFactRoleIter2 = 0;
							for (; snippet1Replace2ItemFactRoleIter2 < factArity; ++snippet1Replace2ItemFactRoleIter2)
							{
								Role currentRole = factRoles[snippet1Replace2ItemFactRoleIter2];
								string roleReplacement = null;
								string basicReplacement = basicRoleReplacements[snippet1Replace2ItemFactRoleIter2];
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace2ItemFactRoleIter2] = roleReplacement;
							}
							snippet1Replace2Item2 = FactType.PopulatePredicateText(reading, factRoles, roleReplacements);
							sbTemp.Append(snippet1Replace2Item2);
							if (RoleIter2 == (snippet1ReplaceCompositeCount2 - 1))
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.IndentedLogicalOrListClose, isDeontic, isNegative));
							}
							snippet1ReplaceCompositeIterator2 = snippet1ReplaceCompositeIterator2 + 1;
						}
					}
					snippet1Replace2 = sbTemp.ToString();
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
			}
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
	}
	#endregion // DisjunctiveMandatoryConstraint verbalization
	#region ExternalUniquenessConstraint verbalization
	public partial class ExternalUniquenessConstraint : IVerbalize
	{
		/// <summary>
		/// IVerbalize.GetVerbalization implementation
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			if (errorOwner != null)
			{
				bool firstElement = true;
				foreach (ModelError error in errorOwner.ErrorCollection)
				{
					if (firstElement)
					{
						firstElement = false;
						beginVerbalization(VerbalizationContent.ErrorReport);
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(error.Name);
				}
				if (!(firstElement))
				{
					return false;
				}
			}
			VerbalizationSets<CoreVerbalizationTextSnippetType> snippets = (VerbalizationSets<CoreVerbalizationTextSnippetType>)snippetsDictionary[typeof(CoreVerbalizationTextSnippetType)];
			bool isDeontic = false;
			StringBuilder sbTemp = null;
			FactType parentFact;
			RoleMoveableCollection factRoles;
			int factArity;
			ReadingOrderMoveableCollection allReadingOrders;
			RoleMoveableCollection allConstraintRoles = this.RoleCollection;
			FactTypeMoveableCollection allFacts = this.FactTypeCollection;
			int allFactsCount = allFacts.Count;
			if (allFactsCount == 0)
			{
				return false;
			}
			string[][] allBasicRoleReplacements = new string[allFactsCount][];
			int minFactArity = int.MaxValue;
			int maxFactArity = int.MinValue;
			int iFact = 0;
			for (; iFact < allFactsCount; ++iFact)
			{
				FactType currentFact = allFacts[iFact];
				if (currentFact.ReadingOrderCollection.Count == 0)
				{
					return false;
				}
				factRoles = currentFact.RoleCollection;
				factArity = factRoles.Count;
				if (factArity < minFactArity)
				{
					minFactArity = factArity;
				}
				if (factArity > maxFactArity)
				{
					maxFactArity = factArity;
				}
				string[] basicRoleReplacements = new string[factArity];
				for (int i = 0; i < factArity; ++i)
				{
					ObjectType rolePlayer = factRoles[i].RolePlayer;
					string basicReplacement;
					if (rolePlayer != null)
					{
						int subscript = 0;
						bool useSubscript = false;
						if (true)
						{
							int j = 0;
							for (; j < i; ++j)
							{
								if (object.ReferenceEquals(rolePlayer, factRoles[j].RolePlayer))
								{
									useSubscript = true;
									subscript = subscript + 1;
								}
							}
							for (j = i + 1; !(useSubscript) && (j < factArity); ++j)
							{
								if (object.ReferenceEquals(rolePlayer, factRoles[j].RolePlayer))
								{
									useSubscript = true;
								}
							}
						}
						if (useSubscript)
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), rolePlayer.Name, subscript + 1);
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name);
						}
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				allBasicRoleReplacements[iFact] = basicRoleReplacements;
			}
			int constraintRoleArity = allConstraintRoles.Count;
			Reading[] allConstraintRoleReadings = new Reading[constraintRoleArity];
			string[] roleReplacements = new string[maxFactArity];
			Reading reading;
			if (!(isNegative) && ((minFactArity >= 2) && (maxFactArity <= 2)))
			{
				bool missingReading1 = false;
				int readingMatchIndex1 = 0;
				for (; !(missingReading1) && (readingMatchIndex1 < constraintRoleArity); ++readingMatchIndex1)
				{
					Role primaryRole = allConstraintRoles[readingMatchIndex1];
					parentFact = primaryRole.FactType;
					factRoles = parentFact.RoleCollection;
					allReadingOrders = parentFact.ReadingOrderCollection;
					reading = FactType.GetMatchingReading(allReadingOrders, null, primaryRole, null, true, true, factRoles, false);
					if (reading == null)
					{
						missingReading1 = true;
					}
					else
					{
						allConstraintRoleReadings[readingMatchIndex1] = reading;
					}
				}
				if (!(missingReading1))
				{
					beginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
					string snippet1Replace1;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity; ++RoleIter1)
					{
						Role primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						int currentFactIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
						CoreVerbalizationTextSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationTextSnippetType.SimpleListOpen;
						}
						else if (RoleIter1 == (constraintRoleArity - 1))
						{
							if (RoleIter1 == 1)
							{
								listSnippet = CoreVerbalizationTextSnippetType.SimpleListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationTextSnippetType.SimpleListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationTextSnippetType.SimpleListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(allConstraintRoles[RoleIter1])]);
						if (RoleIter1 == (constraintRoleArity - 1))
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace1 = sbTemp.ToString();
					string snippet1Replace2;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1ReplaceIsFirstPass2 = true;
					for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity; ++RoleIter2)
					{
						Role primaryRole = allConstraintRoles[RoleIter2];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						int currentFactIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
						CoreVerbalizationTextSnippetType listSnippet;
						if (RoleIter2 == 0)
						{
							listSnippet = CoreVerbalizationTextSnippetType.IndentedListOpen;
						}
						else if (RoleIter2 == (constraintRoleArity - 1))
						{
							if (RoleIter2 == 1)
							{
								listSnippet = CoreVerbalizationTextSnippetType.IndentedListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationTextSnippetType.IndentedListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationTextSnippetType.IndentedListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						snippet1Replace2 = null;
						reading = allConstraintRoleReadings[RoleIter2];
						int snippet1ReplaceFactRoleIter2 = 0;
						for (; snippet1ReplaceFactRoleIter2 < factArity; ++snippet1ReplaceFactRoleIter2)
						{
							Role currentRole = factRoles[snippet1ReplaceFactRoleIter2];
							string roleReplacement = null;
							string basicReplacement = basicRoleReplacements[snippet1ReplaceFactRoleIter2];
							if ((currentRole != primaryRole) && snippet1ReplaceIsFirstPass2)
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.AtMostOneQuantifier, isDeontic, isNegative), basicReplacement);
							}
							else if (currentRole != primaryRole)
							{
								roleReplacement = "";
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1ReplaceFactRoleIter2] = roleReplacement;
						}
						snippet1Replace2 = FactType.PopulatePredicateText(reading, factRoles, roleReplacements);
						sbTemp.Append(snippet1Replace2);
						if (RoleIter2 == (constraintRoleArity - 1))
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.IndentedListClose, isDeontic, isNegative));
						}
						snippet1ReplaceIsFirstPass2 = false;
					}
					snippet1Replace2 = sbTemp.ToString();
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
			}
			else if (isNegative && ((minFactArity >= 2) && (maxFactArity <= 2)))
			{
				bool missingReading1 = false;
				int readingMatchIndex1 = 0;
				for (; !(missingReading1) && (readingMatchIndex1 < constraintRoleArity); ++readingMatchIndex1)
				{
					Role primaryRole = allConstraintRoles[readingMatchIndex1];
					parentFact = primaryRole.FactType;
					factRoles = parentFact.RoleCollection;
					allReadingOrders = parentFact.ReadingOrderCollection;
					reading = FactType.GetMatchingReading(allReadingOrders, null, primaryRole, null, true, true, factRoles, false);
					if (reading == null)
					{
						missingReading1 = true;
					}
					else
					{
						allConstraintRoleReadings[readingMatchIndex1] = reading;
					}
				}
				if (!(missingReading1))
				{
					beginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
					string snippet1Replace1;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity; ++RoleIter1)
					{
						Role primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						int currentFactIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
						CoreVerbalizationTextSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationTextSnippetType.SimpleListOpen;
						}
						else if (RoleIter1 == (constraintRoleArity - 1))
						{
							if (RoleIter1 == 1)
							{
								listSnippet = CoreVerbalizationTextSnippetType.SimpleListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationTextSnippetType.SimpleListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationTextSnippetType.SimpleListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						snippet1Replace1 = null;
						reading = allConstraintRoleReadings[RoleIter1];
						int snippet1ReplaceFactRoleIter1 = 0;
						for (; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
						{
							Role currentRole = factRoles[snippet1ReplaceFactRoleIter1];
							string roleReplacement = null;
							string basicReplacement = basicRoleReplacements[snippet1ReplaceFactRoleIter1];
							if ((currentRole != primaryRole) && snippet1ReplaceIsFirstPass1)
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.MoreThanOneQuantifier, isDeontic, isNegative), basicReplacement);
							}
							else if (currentRole != primaryRole)
							{
								roleReplacement = "";
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
						}
						snippet1Replace1 = FactType.PopulatePredicateText(reading, factRoles, roleReplacements);
						sbTemp.Append(snippet1Replace1);
						if (RoleIter1 == (constraintRoleArity - 1))
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
						}
						snippet1ReplaceIsFirstPass1 = false;
					}
					snippet1Replace1 = sbTemp.ToString();
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
			}
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
	}
	#endregion // ExternalUniquenessConstraint verbalization
	#region RoleValueConstraint verbalization
	public partial class RoleValueConstraint : IVerbalize
	{
		/// <summary>
		/// IVerbalize.GetVerbalization implementation
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			if (errorOwner != null)
			{
				bool firstElement = true;
				foreach (ModelError error in errorOwner.ErrorCollection)
				{
					if (firstElement)
					{
						firstElement = false;
						beginVerbalization(VerbalizationContent.ErrorReport);
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(error.Name);
				}
				if (!(firstElement))
				{
					return false;
				}
			}
			VerbalizationSets<CoreVerbalizationTextSnippetType> snippets = (VerbalizationSets<CoreVerbalizationTextSnippetType>)snippetsDictionary[typeof(CoreVerbalizationTextSnippetType)];
			Role valueRole = this.Role;
			bool isDeontic = false;
			StringBuilder sbTemp = null;
			FactType parentFact = valueRole.FactType;
			IList<Role> includedRoles = new Role[]{
				valueRole};
			RoleMoveableCollection factRoles = parentFact.RoleCollection;
			int factArity = factRoles.Count;
			ReadingOrderMoveableCollection allReadingOrders = parentFact.ReadingOrderCollection;
			int includedArity = includedRoles.Count;
			if ((allReadingOrders.Count == 0) || (includedArity == 0))
			{
				return false;
			}
			string[] basicRoleReplacements = new string[factArity];
			for (int i = 0; i < factArity; ++i)
			{
				ObjectType rolePlayer = factRoles[i].RolePlayer;
				string basicReplacement;
				if (rolePlayer != null)
				{
					int subscript = 0;
					bool useSubscript = false;
					if (true)
					{
						int j = 0;
						for (; j < i; ++j)
						{
							if (object.ReferenceEquals(rolePlayer, factRoles[j].RolePlayer))
							{
								useSubscript = true;
								subscript = subscript + 1;
							}
						}
						for (j = i + 1; !(useSubscript) && (j < factArity); ++j)
						{
							if (object.ReferenceEquals(rolePlayer, factRoles[j].RolePlayer))
							{
								useSubscript = true;
							}
						}
					}
					if (useSubscript)
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), rolePlayer.Name, subscript + 1);
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name);
					}
				}
				else
				{
					basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
				}
				basicRoleReplacements[i] = basicReplacement;
			}
			string[] roleReplacements = new string[factArity];
			Reading reading;
			ValueRangeMoveableCollection ranges = this.ValueRangeCollection;
			bool isSingleValue = (ranges.Count == 1) && (ranges[0].MinValue == ranges[0].MaxValue);
			CoreVerbalizationTextSnippetType variableSnippetSnippetType1 = 0;
			if (isSingleValue)
			{
				variableSnippetSnippetType1 = CoreVerbalizationTextSnippetType.SingleValueValueConstraint;
			}
			else
			{
				variableSnippetSnippetType1 = CoreVerbalizationTextSnippetType.MultiValueValueConstraint;
			}
			beginVerbalization(VerbalizationContent.Normal);
			string variableSnippetFormat1 = snippets.GetSnippet(variableSnippetSnippetType1, isDeontic, isNegative);
			string variableSnippet1Replace1;
			if ((factArity == 2) && (valueRole.Name != ""))
			{
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (int RoleIter1 = 0; RoleIter1 < includedArity; ++RoleIter1)
				{
					Role primaryRole = includedRoles[RoleIter1];
					variableSnippet1Replace1 = null;
					string variableSnippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.PeriodDelimiter, isDeontic, isNegative);
					string variableSnippet1Replace1Replace1;
					variableSnippet1Replace1Replace1 = @"<span style=""font:smaller"">undone</span>";
					string variableSnippet1Replace1Replace2;
					if ((primaryRole.RolePlayer != null) && (0 != primaryRole.RolePlayer.ReferenceModeString.Length))
					{
						string variableSnippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.NameWithRefMode, isDeontic, isNegative);
						string variableSnippet1Replace1Replace2Replace1;
						variableSnippet1Replace1Replace2Replace1 = valueRole.Name;
						string variableSnippet1Replace1Replace2Replace2;
						variableSnippet1Replace1Replace2Replace2 = valueRole.RolePlayer.ReferenceModeString;
						variableSnippet1Replace1Replace2 = string.Format(writer.FormatProvider, variableSnippet1Replace1ReplaceFormat2, variableSnippet1Replace1Replace2Replace1, variableSnippet1Replace1Replace2Replace2);
					}
					else
					{
						string variableSnippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.SelfReference, isDeontic, isNegative);
						string variableSnippet1Replace1Replace2Replace1;
						variableSnippet1Replace1Replace2Replace1 = valueRole.Name;
						variableSnippet1Replace1Replace2 = string.Format(writer.FormatProvider, variableSnippet1Replace1ReplaceFormat2, variableSnippet1Replace1Replace2Replace1);
					}
					variableSnippet1Replace1 = string.Format(writer.FormatProvider, variableSnippet1ReplaceFormat1, variableSnippet1Replace1Replace1, variableSnippet1Replace1Replace2);
					sbTemp.Append(variableSnippet1Replace1);
				}
				variableSnippet1Replace1 = sbTemp.ToString();
			}
			else
			{
				string variableSnippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.InSeparator, isDeontic, isNegative);
				string variableSnippet1Replace1Replace1;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (int RoleIter1 = 0; RoleIter1 < includedArity; ++RoleIter1)
				{
					sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[RoleIter1])]);
				}
				variableSnippet1Replace1Replace1 = sbTemp.ToString();
				string variableSnippet1Replace1Replace2;
				reading = FactType.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
				variableSnippet1Replace1Replace2 = FactType.PopulatePredicateText(reading, factRoles, basicRoleReplacements);
				variableSnippet1Replace1 = string.Format(writer.FormatProvider, variableSnippet1ReplaceFormat1, variableSnippet1Replace1Replace1, variableSnippet1Replace1Replace2);
			}
			string variableSnippet1Replace2;
			if (sbTemp == null)
			{
				sbTemp = new StringBuilder();
			}
			else
			{
				sbTemp.Length = 0;
			}
			int rangeCount = ranges.Count;
			for (int i = 0; i < rangeCount; ++i)
			{
				string minValue = ranges[i].MinValue;
				string maxValue = ranges[i].MaxValue;
				RangeInclusion minInclusion = ranges[i].MinInclusion;
				RangeInclusion maxInclusion = ranges[i].MaxInclusion;
				CoreVerbalizationTextSnippetType listSnippet;
				if (i == 0)
				{
					listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListOpen;
				}
				else if (i == (rangeCount - 1))
				{
					if (i == 1)
					{
						listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListPairSeparator;
					}
					else
					{
						listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListFinalSeparator;
					}
				}
				else
				{
					listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListSeparator;
				}
				sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
				variableSnippet1Replace2 = null;
				CoreVerbalizationTextSnippetType variableSnippet1ReplaceSnippetType2 = 0;
				if (minValue == maxValue)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.SelfReference;
				}
				else if ((minInclusion != RangeInclusion.Open) && (maxValue.Length == 0))
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.MinClosedMaxUnbounded;
				}
				else if ((minInclusion == RangeInclusion.Open) && (maxValue.Length == 0))
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.MinOpenMaxUnbounded;
				}
				else if ((minValue.Length == 0) && (maxInclusion != RangeInclusion.Open))
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.MinUnboundedMaxClosed;
				}
				else if ((minValue.Length == 0) && (maxInclusion == RangeInclusion.Open))
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.MinUnboundedMaxOpen;
				}
				else if ((minInclusion != RangeInclusion.Open) && (maxInclusion != RangeInclusion.Open))
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.MinClosedMaxClosed;
				}
				else if ((minInclusion != RangeInclusion.Open) && (maxInclusion == RangeInclusion.Open))
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.MinClosedMaxOpen;
				}
				else if ((minInclusion == RangeInclusion.Open) && (maxInclusion != RangeInclusion.Open))
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.MinOpenMaxClosed;
				}
				else if ((minInclusion == RangeInclusion.Open) && (maxInclusion == RangeInclusion.Open))
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.MinOpenMaxOpen;
				}
				string variableSnippet1ReplaceFormat2 = snippets.GetSnippet(variableSnippet1ReplaceSnippetType2, isDeontic, isNegative);
				string variableSnippet1Replace2Replace1;
				variableSnippet1Replace2Replace1 = minValue;
				string variableSnippet1Replace2Replace2;
				variableSnippet1Replace2Replace2 = maxValue;
				variableSnippet1Replace2 = string.Format(writer.FormatProvider, variableSnippet1ReplaceFormat2, variableSnippet1Replace2Replace1, variableSnippet1Replace2Replace2);
				sbTemp.Append(variableSnippet1Replace2);
				if (i == (rangeCount - 1))
				{
					sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.CompactSimpleListClose, isDeontic, isNegative));
				}
			}
			variableSnippet1Replace2 = sbTemp.ToString();
			FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, variableSnippetFormat1, variableSnippet1Replace1, variableSnippet1Replace2), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
	}
	#endregion // RoleValueConstraint verbalization
	#region ValueTypeValueConstraint verbalization
	public partial class ValueTypeValueConstraint : IVerbalize
	{
		/// <summary>
		/// IVerbalize.GetVerbalization implementation
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			if (errorOwner != null)
			{
				bool firstElement = true;
				foreach (ModelError error in errorOwner.ErrorCollection)
				{
					if (firstElement)
					{
						firstElement = false;
						beginVerbalization(VerbalizationContent.ErrorReport);
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(error.Name);
				}
				if (!(firstElement))
				{
					return false;
				}
			}
			VerbalizationSets<CoreVerbalizationTextSnippetType> snippets = (VerbalizationSets<CoreVerbalizationTextSnippetType>)snippetsDictionary[typeof(CoreVerbalizationTextSnippetType)];
			const bool isDeontic = false;
			StringBuilder sbTemp = null;
			ValueRangeMoveableCollection ranges = this.ValueRangeCollection;
			bool isSingleValue = (ranges.Count == 1) && (ranges[0].MinValue == ranges[0].MaxValue);
			CoreVerbalizationTextSnippetType variableSnippetSnippetType1 = 0;
			if (isSingleValue)
			{
				variableSnippetSnippetType1 = CoreVerbalizationTextSnippetType.SingleValueValueConstraint;
			}
			else
			{
				variableSnippetSnippetType1 = CoreVerbalizationTextSnippetType.MultiValueValueConstraint;
			}
			beginVerbalization(VerbalizationContent.Normal);
			string variableSnippetFormat1 = snippets.GetSnippet(variableSnippetSnippetType1, isDeontic, isNegative);
			string variableSnippet1Replace1;
			string variableSnippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationTextSnippetType.ObjectType, isDeontic, isNegative);
			string variableSnippet1Replace1Replace1;
			variableSnippet1Replace1Replace1 = this.ValueType.Name;
			variableSnippet1Replace1 = string.Format(writer.FormatProvider, variableSnippet1ReplaceFormat1, variableSnippet1Replace1Replace1);
			string variableSnippet1Replace2;
			if (sbTemp == null)
			{
				sbTemp = new StringBuilder();
			}
			else
			{
				sbTemp.Length = 0;
			}
			int rangeCount = ranges.Count;
			for (int i = 0; i < rangeCount; ++i)
			{
				string minValue = ranges[i].MinValue;
				string maxValue = ranges[i].MaxValue;
				RangeInclusion minInclusion = ranges[i].MinInclusion;
				RangeInclusion maxInclusion = ranges[i].MaxInclusion;
				CoreVerbalizationTextSnippetType listSnippet;
				if (i == 0)
				{
					listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListOpen;
				}
				else if (i == (rangeCount - 1))
				{
					if (i == 1)
					{
						listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListPairSeparator;
					}
					else
					{
						listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListFinalSeparator;
					}
				}
				else
				{
					listSnippet = CoreVerbalizationTextSnippetType.CompactSimpleListSeparator;
				}
				sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
				variableSnippet1Replace2 = null;
				CoreVerbalizationTextSnippetType variableSnippet1ReplaceSnippetType2 = 0;
				if (minValue == maxValue)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.SelfReference;
				}
				else if ((minInclusion != RangeInclusion.Open) && (maxValue.Length == 0))
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.MinClosedMaxUnbounded;
				}
				else if ((minInclusion == RangeInclusion.Open) && (maxValue.Length == 0))
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.MinOpenMaxUnbounded;
				}
				else if ((minValue.Length == 0) && (maxInclusion != RangeInclusion.Open))
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.MinUnboundedMaxClosed;
				}
				else if ((minValue.Length == 0) && (maxInclusion == RangeInclusion.Open))
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.MinUnboundedMaxOpen;
				}
				else if ((minInclusion != RangeInclusion.Open) && (maxInclusion != RangeInclusion.Open))
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.MinClosedMaxClosed;
				}
				else if ((minInclusion != RangeInclusion.Open) && (maxInclusion == RangeInclusion.Open))
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.MinClosedMaxOpen;
				}
				else if ((minInclusion == RangeInclusion.Open) && (maxInclusion != RangeInclusion.Open))
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.MinOpenMaxClosed;
				}
				else if ((minInclusion == RangeInclusion.Open) && (maxInclusion == RangeInclusion.Open))
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationTextSnippetType.MinOpenMaxOpen;
				}
				string variableSnippet1ReplaceFormat2 = snippets.GetSnippet(variableSnippet1ReplaceSnippetType2, isDeontic, isNegative);
				string variableSnippet1Replace2Replace1;
				variableSnippet1Replace2Replace1 = minValue;
				string variableSnippet1Replace2Replace2;
				variableSnippet1Replace2Replace2 = maxValue;
				variableSnippet1Replace2 = string.Format(writer.FormatProvider, variableSnippet1ReplaceFormat2, variableSnippet1Replace2Replace1, variableSnippet1Replace2Replace2);
				sbTemp.Append(variableSnippet1Replace2);
				if (i == (rangeCount - 1))
				{
					sbTemp.Append(snippets.GetSnippet(CoreVerbalizationTextSnippetType.CompactSimpleListClose, isDeontic, isNegative));
				}
			}
			variableSnippet1Replace2 = sbTemp.ToString();
			FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, variableSnippetFormat1, variableSnippet1Replace1, variableSnippet1Replace2), snippets.GetSnippet(CoreVerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, beginVerbalization, isNegative);
		}
	}
	#endregion // ValueTypeValueConstraint verbalization
}
