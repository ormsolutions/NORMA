using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;

// Common Public License Copyright Notice
// /**************************************************************************\
// * Neumont Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © Neumont University. All rights reserved.                     *
// * Copyright © Matthew Curland. All rights reserved.                        *
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
	#region CoreVerbalizationSnippetType enum
	/// <summary>An enum with one value for each recognized snippet</summary>
	public enum CoreVerbalizationSnippetType
	{
		/// <summary>The 'AtMostOneQuantifier' format string snippet. Contains 1 replacement field.</summary>
		AtMostOneQuantifier,
		/// <summary>The 'CloseVerbalizationSentence' simple snippet value.</summary>
		CloseVerbalizationSentence,
		/// <summary>The 'CombinationAssociation' format string snippet. Contains 2 replacement fields.</summary>
		CombinationAssociation,
		/// <summary>The 'CombinationIdentifier' format string snippet. Contains 1 replacement field.</summary>
		CombinationIdentifier,
		/// <summary>The 'CombinedObjectAndInstance' format string snippet. Contains 2 replacement fields.</summary>
		CombinedObjectAndInstance,
		/// <summary>The 'CombinedObjectAndInstanceTypeMissing' format string snippet. Contains 1 replacement field.</summary>
		CombinedObjectAndInstanceTypeMissing,
		/// <summary>The 'CompactSimpleListClose' simple snippet value.</summary>
		CompactSimpleListClose,
		/// <summary>The 'CompactSimpleListFinalSeparator' simple snippet value.</summary>
		CompactSimpleListFinalSeparator,
		/// <summary>The 'CompactSimpleListOpen' simple snippet value.</summary>
		CompactSimpleListOpen,
		/// <summary>The 'CompactSimpleListPairSeparator' simple snippet value.</summary>
		CompactSimpleListPairSeparator,
		/// <summary>The 'CompactSimpleListSeparator' simple snippet value.</summary>
		CompactSimpleListSeparator,
		/// <summary>The 'CompoundListClose' simple snippet value.</summary>
		CompoundListClose,
		/// <summary>The 'CompoundListFinalSeparator' simple snippet value.</summary>
		CompoundListFinalSeparator,
		/// <summary>The 'CompoundListOpen' simple snippet value.</summary>
		CompoundListOpen,
		/// <summary>The 'CompoundListPairSeparator' simple snippet value.</summary>
		CompoundListPairSeparator,
		/// <summary>The 'CompoundListSeparator' simple snippet value.</summary>
		CompoundListSeparator,
		/// <summary>The 'Conditional' format string snippet. Contains 2 replacement fields.</summary>
		Conditional,
		/// <summary>The 'ConstraintProvidesPreferredIdentifier' format string snippet. Contains 2 replacement fields.</summary>
		ConstraintProvidesPreferredIdentifier,
		/// <summary>The 'ContextScope' format string snippet. Contains 1 replacement field.</summary>
		ContextScope,
		/// <summary>The 'ContextScopeReference' format string snippet. Contains 1 replacement field.</summary>
		ContextScopeReference,
		/// <summary>The 'DefiniteArticle' format string snippet. Contains 1 replacement field.</summary>
		DefiniteArticle,
		/// <summary>The 'DefinitionVerbalization' format string snippet. Contains 1 replacement field.</summary>
		DefinitionVerbalization,
		/// <summary>The 'EachInstanceQuantifier' format string snippet. Contains 1 replacement field.</summary>
		EachInstanceQuantifier,
		/// <summary>The 'EntityTypeVerbalization' format string snippet. Contains 1 replacement field.</summary>
		EntityTypeVerbalization,
		/// <summary>The 'Equality' format string snippet. Contains 2 replacement fields.</summary>
		Equality,
		/// <summary>The 'ErrorClosePrimaryReport' simple snippet value.</summary>
		ErrorClosePrimaryReport,
		/// <summary>The 'ErrorCloseSecondaryReport' simple snippet value.</summary>
		ErrorCloseSecondaryReport,
		/// <summary>The 'ErrorOpenPrimaryReport' simple snippet value.</summary>
		ErrorOpenPrimaryReport,
		/// <summary>The 'ErrorOpenSecondaryReport' simple snippet value.</summary>
		ErrorOpenSecondaryReport,
		/// <summary>The 'ErrorPrimary' format string snippet. Contains 1 replacement field.</summary>
		ErrorPrimary,
		/// <summary>The 'ErrorSecondary' format string snippet. Contains 1 replacement field.</summary>
		ErrorSecondary,
		/// <summary>The 'ExactlyOneQuantifier' format string snippet. Contains 1 replacement field.</summary>
		ExactlyOneQuantifier,
		/// <summary>The 'ExclusionBinaryLeadReading' format string snippet. Contains 2 replacement fields.</summary>
		ExclusionBinaryLeadReading,
		/// <summary>The 'ExclusionCombined' format string snippet. Contains 2 replacement fields.</summary>
		ExclusionCombined,
		/// <summary>The 'ExistentialQuantifier' format string snippet. Contains 1 replacement field.</summary>
		ExistentialQuantifier,
		/// <summary>The 'FactTypeInstanceBlockEnd' simple snippet value.</summary>
		FactTypeInstanceBlockEnd,
		/// <summary>The 'FactTypeInstanceBlockStart' simple snippet value.</summary>
		FactTypeInstanceBlockStart,
		/// <summary>The 'FactTypeInstanceIdentifier' format string snippet. Contains 1 replacement field.</summary>
		FactTypeInstanceIdentifier,
		/// <summary>The 'ForEachCompactQuantifier' format string snippet. Contains 2 replacement fields.</summary>
		ForEachCompactQuantifier,
		/// <summary>The 'ForEachIndentedQuantifier' format string snippet. Contains 2 replacement fields.</summary>
		ForEachIndentedQuantifier,
		/// <summary>The 'FrequencyPopulation' format string snippet. Contains 3 replacement fields.</summary>
		FrequencyPopulation,
		/// <summary>The 'FrequencyRangeExact' format string snippet. Contains 1 replacement field.</summary>
		FrequencyRangeExact,
		/// <summary>The 'FrequencyRangeMaxUnbounded' format string snippet. Contains 1 replacement field.</summary>
		FrequencyRangeMaxUnbounded,
		/// <summary>The 'FrequencyRangeMinAndMax' format string snippet. Contains 2 replacement fields.</summary>
		FrequencyRangeMinAndMax,
		/// <summary>The 'FrequencyRangeMinUnbounded' format string snippet. Contains 1 replacement field.</summary>
		FrequencyRangeMinUnbounded,
		/// <summary>The 'FrequencyRangeMinUnboundedOptimized' format string snippet. Contains 2 replacement fields.</summary>
		FrequencyRangeMinUnboundedOptimized,
		/// <summary>The 'GroupEquality' format string snippet. Contains 1 replacement field.</summary>
		GroupEquality,
		/// <summary>The 'GroupExclusion' format string snippet. Contains 1 replacement field.</summary>
		GroupExclusion,
		/// <summary>The 'GroupExclusiveOr' format string snippet. Contains 1 replacement field.</summary>
		GroupExclusiveOr,
		/// <summary>The 'HyphenBoundPredicatePart' format string snippet. Contains 2 replacement fields.</summary>
		HyphenBoundPredicatePart,
		/// <summary>The 'IdentityEqualityListClose' simple snippet value.</summary>
		IdentityEqualityListClose,
		/// <summary>The 'IdentityEqualityListFinalSeparator' simple snippet value.</summary>
		IdentityEqualityListFinalSeparator,
		/// <summary>The 'IdentityEqualityListOpen' simple snippet value.</summary>
		IdentityEqualityListOpen,
		/// <summary>The 'IdentityEqualityListPairSeparator' simple snippet value.</summary>
		IdentityEqualityListPairSeparator,
		/// <summary>The 'IdentityEqualityListSeparator' simple snippet value.</summary>
		IdentityEqualityListSeparator,
		/// <summary>The 'IdentityReferenceQuantifier' format string snippet. Contains 1 replacement field.</summary>
		IdentityReferenceQuantifier,
		/// <summary>The 'ImpersonalPronoun' format string snippet. Contains 1 replacement field.</summary>
		ImpersonalPronoun,
		/// <summary>The 'ImpliedModalNecessityOperator' format string snippet. Contains 1 replacement field.</summary>
		ImpliedModalNecessityOperator,
		/// <summary>The 'IndentedCompoundListClose' simple snippet value.</summary>
		IndentedCompoundListClose,
		/// <summary>The 'IndentedCompoundListFinalSeparator' simple snippet value.</summary>
		IndentedCompoundListFinalSeparator,
		/// <summary>The 'IndentedCompoundListOpen' simple snippet value.</summary>
		IndentedCompoundListOpen,
		/// <summary>The 'IndentedCompoundListPairSeparator' simple snippet value.</summary>
		IndentedCompoundListPairSeparator,
		/// <summary>The 'IndentedCompoundListSeparator' simple snippet value.</summary>
		IndentedCompoundListSeparator,
		/// <summary>The 'IndentedListClose' simple snippet value.</summary>
		IndentedListClose,
		/// <summary>The 'IndentedListFinalSeparator' simple snippet value.</summary>
		IndentedListFinalSeparator,
		/// <summary>The 'IndentedListOpen' simple snippet value.</summary>
		IndentedListOpen,
		/// <summary>The 'IndentedListPairSeparator' simple snippet value.</summary>
		IndentedListPairSeparator,
		/// <summary>The 'IndentedListSeparator' simple snippet value.</summary>
		IndentedListSeparator,
		/// <summary>The 'IndentedLogicalAndListClose' simple snippet value.</summary>
		IndentedLogicalAndListClose,
		/// <summary>The 'IndentedLogicalAndListFinalSeparator' simple snippet value.</summary>
		IndentedLogicalAndListFinalSeparator,
		/// <summary>The 'IndentedLogicalAndListOpen' simple snippet value.</summary>
		IndentedLogicalAndListOpen,
		/// <summary>The 'IndentedLogicalAndListPairSeparator' simple snippet value.</summary>
		IndentedLogicalAndListPairSeparator,
		/// <summary>The 'IndentedLogicalAndListSeparator' simple snippet value.</summary>
		IndentedLogicalAndListSeparator,
		/// <summary>The 'IndentedLogicalOrListClose' simple snippet value.</summary>
		IndentedLogicalOrListClose,
		/// <summary>The 'IndentedLogicalOrListFinalSeparator' simple snippet value.</summary>
		IndentedLogicalOrListFinalSeparator,
		/// <summary>The 'IndentedLogicalOrListOpen' simple snippet value.</summary>
		IndentedLogicalOrListOpen,
		/// <summary>The 'IndentedLogicalOrListPairSeparator' simple snippet value.</summary>
		IndentedLogicalOrListPairSeparator,
		/// <summary>The 'IndentedLogicalOrListSeparator' simple snippet value.</summary>
		IndentedLogicalOrListSeparator,
		/// <summary>The 'IndependentVerbalization' format string snippet. Contains 1 replacement field.</summary>
		IndependentVerbalization,
		/// <summary>The 'InQuantifier' format string snippet. Contains 2 replacement fields.</summary>
		InQuantifier,
		/// <summary>The 'MinClosedMaxClosed' format string snippet. Contains 2 replacement fields.</summary>
		MinClosedMaxClosed,
		/// <summary>The 'MinClosedMaxOpen' format string snippet. Contains 2 replacement fields.</summary>
		MinClosedMaxOpen,
		/// <summary>The 'MinClosedMaxUnbounded' format string snippet. Contains 1 replacement field.</summary>
		MinClosedMaxUnbounded,
		/// <summary>The 'MinOpenMaxClosed' format string snippet. Contains 2 replacement fields.</summary>
		MinOpenMaxClosed,
		/// <summary>The 'MinOpenMaxOpen' format string snippet. Contains 2 replacement fields.</summary>
		MinOpenMaxOpen,
		/// <summary>The 'MinOpenMaxUnbounded' format string snippet. Contains 1 replacement field.</summary>
		MinOpenMaxUnbounded,
		/// <summary>The 'MinUnboundedMaxClosed' format string snippet. Contains 1 replacement field.</summary>
		MinUnboundedMaxClosed,
		/// <summary>The 'MinUnboundedMaxOpen' format string snippet. Contains 1 replacement field.</summary>
		MinUnboundedMaxOpen,
		/// <summary>The 'ModalNecessityOperator' format string snippet. Contains 1 replacement field.</summary>
		ModalNecessityOperator,
		/// <summary>The 'ModalPossibilityOperator' format string snippet. Contains 1 replacement field.</summary>
		ModalPossibilityOperator,
		/// <summary>The 'MoreThanOneQuantifier' format string snippet. Contains 1 replacement field.</summary>
		MoreThanOneQuantifier,
		/// <summary>The 'MultilineIndentedCompoundListClose' simple snippet value.</summary>
		MultilineIndentedCompoundListClose,
		/// <summary>The 'MultilineIndentedCompoundListFinalSeparator' simple snippet value.</summary>
		MultilineIndentedCompoundListFinalSeparator,
		/// <summary>The 'MultilineIndentedCompoundListOpen' simple snippet value.</summary>
		MultilineIndentedCompoundListOpen,
		/// <summary>The 'MultilineIndentedCompoundListPairSeparator' simple snippet value.</summary>
		MultilineIndentedCompoundListPairSeparator,
		/// <summary>The 'MultilineIndentedCompoundListSeparator' simple snippet value.</summary>
		MultilineIndentedCompoundListSeparator,
		/// <summary>The 'MultiValueValueConstraint' format string snippet. Contains 2 replacement fields.</summary>
		MultiValueValueConstraint,
		/// <summary>The 'NegativeReadingForUnaryOnlyDisjunctiveMandatory' format string snippet. Contains 2 replacement fields.</summary>
		NegativeReadingForUnaryOnlyDisjunctiveMandatory,
		/// <summary>The 'NonTextInstanceValue' format string snippet. Contains 1 replacement field.</summary>
		NonTextInstanceValue,
		/// <summary>The 'NotesVerbalization' format string snippet. Contains 1 replacement field.</summary>
		NotesVerbalization,
		/// <summary>The 'ObjectifiesFactTypeVerbalization' format string snippet. Contains 2 replacement fields.</summary>
		ObjectifiesFactTypeVerbalization,
		/// <summary>The 'ObjectType' format string snippet. Contains 2 replacement fields.</summary>
		ObjectType,
		/// <summary>The 'ObjectTypeInstanceListClose' simple snippet value.</summary>
		ObjectTypeInstanceListClose,
		/// <summary>The 'ObjectTypeInstanceListFinalSeparator' simple snippet value.</summary>
		ObjectTypeInstanceListFinalSeparator,
		/// <summary>The 'ObjectTypeInstanceListOpen' simple snippet value.</summary>
		ObjectTypeInstanceListOpen,
		/// <summary>The 'ObjectTypeInstanceListPairSeparator' simple snippet value.</summary>
		ObjectTypeInstanceListPairSeparator,
		/// <summary>The 'ObjectTypeInstanceListSeparator' simple snippet value.</summary>
		ObjectTypeInstanceListSeparator,
		/// <summary>The 'ObjectTypeMissing' format string snippet. Contains 1 replacement field.</summary>
		ObjectTypeMissing,
		/// <summary>The 'ObjectTypeWithSubscript' format string snippet. Contains 3 replacement fields.</summary>
		ObjectTypeWithSubscript,
		/// <summary>The 'OccursInPopulation' format string snippet. Contains 2 replacement fields.</summary>
		OccursInPopulation,
		/// <summary>The 'OneQuantifier' format string snippet. Contains 1 replacement field.</summary>
		OneQuantifier,
		/// <summary>The 'PeriodSeparator' format string snippet. Contains 2 replacement fields.</summary>
		PeriodSeparator,
		/// <summary>The 'PersonalPronoun' format string snippet. Contains 1 replacement field.</summary>
		PersonalPronoun,
		/// <summary>The 'PortableDataTypeVerbalization' format string snippet. Contains 1 replacement field.</summary>
		PortableDataTypeVerbalization,
		/// <summary>The 'PredicatePart' format string snippet. Contains 1 replacement field.</summary>
		PredicatePart,
		/// <summary>The 'ReferenceModeVerbalization' format string snippet. Contains 1 replacement field.</summary>
		ReferenceModeVerbalization,
		/// <summary>The 'ReferenceScheme' format string snippet. Contains 2 replacement fields.</summary>
		ReferenceScheme,
		/// <summary>The 'ReferenceSchemeVerbalization' format string snippet. Contains 1 replacement field.</summary>
		ReferenceSchemeVerbalization,
		/// <summary>The 'SelfReference' format string snippet. Contains 1 replacement field.</summary>
		SelfReference,
		/// <summary>The 'SimpleListClose' simple snippet value.</summary>
		SimpleListClose,
		/// <summary>The 'SimpleListFinalSeparator' simple snippet value.</summary>
		SimpleListFinalSeparator,
		/// <summary>The 'SimpleListOpen' simple snippet value.</summary>
		SimpleListOpen,
		/// <summary>The 'SimpleListPairSeparator' simple snippet value.</summary>
		SimpleListPairSeparator,
		/// <summary>The 'SimpleListSeparator' simple snippet value.</summary>
		SimpleListSeparator,
		/// <summary>The 'SimpleLogicalAndListClose' simple snippet value.</summary>
		SimpleLogicalAndListClose,
		/// <summary>The 'SimpleLogicalAndListFinalSeparator' simple snippet value.</summary>
		SimpleLogicalAndListFinalSeparator,
		/// <summary>The 'SimpleLogicalAndListOpen' simple snippet value.</summary>
		SimpleLogicalAndListOpen,
		/// <summary>The 'SimpleLogicalAndListPairSeparator' simple snippet value.</summary>
		SimpleLogicalAndListPairSeparator,
		/// <summary>The 'SimpleLogicalAndListSeparator' simple snippet value.</summary>
		SimpleLogicalAndListSeparator,
		/// <summary>The 'SimpleLogicalOrListClose' simple snippet value.</summary>
		SimpleLogicalOrListClose,
		/// <summary>The 'SimpleLogicalOrListFinalSeparator' simple snippet value.</summary>
		SimpleLogicalOrListFinalSeparator,
		/// <summary>The 'SimpleLogicalOrListOpen' simple snippet value.</summary>
		SimpleLogicalOrListOpen,
		/// <summary>The 'SimpleLogicalOrListPairSeparator' simple snippet value.</summary>
		SimpleLogicalOrListPairSeparator,
		/// <summary>The 'SimpleLogicalOrListSeparator' simple snippet value.</summary>
		SimpleLogicalOrListSeparator,
		/// <summary>The 'SingleValueValueConstraint' format string snippet. Contains 2 replacement fields.</summary>
		SingleValueValueConstraint,
		/// <summary>The 'SubtypeMetaReading' format string snippet. Contains 2 replacement fields.</summary>
		SubtypeMetaReading,
		/// <summary>The 'TextInstanceValue' format string snippet. Contains 1 replacement field.</summary>
		TextInstanceValue,
		/// <summary>The 'TopLevelIndentedLogicalAndListClose' simple snippet value.</summary>
		TopLevelIndentedLogicalAndListClose,
		/// <summary>The 'TopLevelIndentedLogicalAndListFinalSeparator' simple snippet value.</summary>
		TopLevelIndentedLogicalAndListFinalSeparator,
		/// <summary>The 'TopLevelIndentedLogicalAndListOpen' simple snippet value.</summary>
		TopLevelIndentedLogicalAndListOpen,
		/// <summary>The 'TopLevelIndentedLogicalAndListPairSeparator' simple snippet value.</summary>
		TopLevelIndentedLogicalAndListPairSeparator,
		/// <summary>The 'TopLevelIndentedLogicalAndListSeparator' simple snippet value.</summary>
		TopLevelIndentedLogicalAndListSeparator,
		/// <summary>The 'TopLevelIndentedLogicalOrListClose' simple snippet value.</summary>
		TopLevelIndentedLogicalOrListClose,
		/// <summary>The 'TopLevelIndentedLogicalOrListFinalSeparator' simple snippet value.</summary>
		TopLevelIndentedLogicalOrListFinalSeparator,
		/// <summary>The 'TopLevelIndentedLogicalOrListOpen' simple snippet value.</summary>
		TopLevelIndentedLogicalOrListOpen,
		/// <summary>The 'TopLevelIndentedLogicalOrListPairSeparator' simple snippet value.</summary>
		TopLevelIndentedLogicalOrListPairSeparator,
		/// <summary>The 'TopLevelIndentedLogicalOrListSeparator' simple snippet value.</summary>
		TopLevelIndentedLogicalOrListSeparator,
		/// <summary>The 'UniversalQuantifier' format string snippet. Contains 1 replacement field.</summary>
		UniversalQuantifier,
		/// <summary>The 'ValueTypeVerbalization' format string snippet. Contains 1 replacement field.</summary>
		ValueTypeVerbalization,
		/// <summary>The 'VerbalizerCloseVerbalization' simple snippet value.</summary>
		VerbalizerCloseVerbalization,
		/// <summary>The 'VerbalizerDecreaseIndent' simple snippet value.</summary>
		VerbalizerDecreaseIndent,
		/// <summary>The 'VerbalizerDocumentFooter' simple snippet value.</summary>
		VerbalizerDocumentFooter,
		/// <summary>The 'VerbalizerDocumentHeader' format string snippet. Contains 14 replacement fields.</summary>
		VerbalizerDocumentHeader,
		/// <summary>The 'VerbalizerFontWeightBold' simple snippet value.</summary>
		VerbalizerFontWeightBold,
		/// <summary>The 'VerbalizerFontWeightNormal' simple snippet value.</summary>
		VerbalizerFontWeightNormal,
		/// <summary>The 'VerbalizerIncreaseIndent' simple snippet value.</summary>
		VerbalizerIncreaseIndent,
		/// <summary>The 'VerbalizerNewLine' simple snippet value.</summary>
		VerbalizerNewLine,
		/// <summary>The 'VerbalizerOpenVerbalization' simple snippet value.</summary>
		VerbalizerOpenVerbalization,
	}
	#endregion // CoreVerbalizationSnippetType enum
	#region CoreVerbalizationSets class
	/// <summary>A class deriving from VerbalizationSets.</summary>
	public class CoreVerbalizationSets : VerbalizationSets<CoreVerbalizationSnippetType>
	{
		/// <summary>The default verbalization snippet set. Contains english HTML snippets.</summary>
		public static readonly CoreVerbalizationSets Default = (CoreVerbalizationSets)VerbalizationSets<CoreVerbalizationSnippetType>.Create<CoreVerbalizationSets>(null);
		/// <summary>Populates the snippet sets of the CoreVerbalizationSets object.</summary>
		/// <param name="sets">The sets to be populated.</param>
		/// <param name="userData">User-defined data passed to the Create method</param>
		protected override void PopulateVerbalizationSets(VerbalizationSet[] sets, object userData)
		{
			sets[0] = new ArrayVerbalizationSet(new string[]{
				@"<span class=""quantifier"">at most one</span> {0}",
				@"<span class=""listSeparator"">.</span>",
				@"{0} <span class=""quantifier"">combination is associated with</span> {1}",
				@"{0} <span class=""quantifier"">combination</span>",
				@"{0} <span class=""instance"">{1}</span>",
				"Missing {0}",
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
				@"<span class=""quantifier"">if </span>{0}<span class=""quantifier""> then </span>{1}",
				@"<span class=""smallIndent""><span class=""quantifier"">this association with</span> {0} <span class=""quantifier"">provides the preferred identification scheme for</span> {1}</span>",
				@"<span class=""quantifier"">context: </span>{0}",
				@"<span class=""quantifier"">in this context,</span> {0}",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">Definition:</span> <span class=""definition"">{0}</span>",
				@"<span class=""quantifier"">each instance of</span> {0} <span class=""quantifier"">occurs only once</span>",
				@"{0} <span class=""quantifier"">is an entity type</span>",
				@"{0}<span class=""quantifier""> if and only if </span>{1}",
				"</span>",
				"</span>",
				@"<span class=""primaryErrorReport"">",
				@"<span class=""secondaryErrorReport"">",
				"Model Error: {0}",
				"Model Error: {0}",
				@"<span class=""quantifier"">exactly one</span> {0}",
				@"<span class=""quantifier"">no</span> {0}<span class=""quantifier""> the same </span>{1}",
				"{0} {1}",
				@"<span class=""quantifier"">some</span> {0}",
				"</span>",
				@"<br/><span class=""quantifier"">Examples: </span><span class=""smallIndent"">",
				@"<span class=""smallIndent""><span class=""quantifier"">Identifier: <span class=""instance"">{0}</span></span></span>",
				@"<span class=""quantifier"">for each</span> {0}, {1}",
				@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">each {0} in the population of “{1}” occurs there {2}</span>",
				@"<span class=""quantifier"">exactly <span class=""instance"">{0}</span> times</span>",
				@"<span class=""quantifier"">at least <span class=""instance"">{0}</span> times</span>",
				@"<span class=""quantifier"">at least <span class=""instance"">{0}</span> and at most <span class=""instance"">{1}</span> times</span>",
				@"<span class=""quantifier"">at most <span class=""instance"">{1}</span> times</span>",
				@"<span class=""quantifier"">at most <span class=""instance"">{1}</span> {0} instances</span>",
				@"<span class=""quantifier"">all or none of the following hold:</span> {0}",
				@"<span class=""quantifier"">at most one of the following holds:</span> {0}",
				@"<span class=""quantifier"">exactly one of the following holds:</span> {0}",
				"{0}{{0}}{1}",
				"",
				@"<span class=""logicalOperator""> that is a </span>",
				"",
				@"<span class=""logicalOperator""> that is a </span>",
				@"<span class=""logicalOperator""> that is a </span>",
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
				@"<br/><span class=""smallIndent"">",
				@"<br/><span class=""logicalOperator"">and that </span>",
				@"<br/><span class=""logicalOperator"">and that </span>",
				"</span>",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"<br/><span class=""smallIndent"">",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"{0} <span class=""quantifier"">is independent (it may have instances that play no other roles)</span>",
				@"{0} <span class=""quantifier"">in</span> {1}",
				@"<span class=""quantifier"">at least {0} to at most {1}</span>",
				@"<span class=""quantifier"">at least {0} to below {1}</span>",
				@"<span class=""quantifier"">at least {0}</span>",
				@"<span class=""quantifier"">above {0} to at most {1}</span>",
				@"<span class=""quantifier"">above {0} to below {1}</span>",
				@"<span class=""quantifier"">above {0}</span>",
				@"<span class=""quantifier"">at most {1}</span>",
				@"<span class=""quantifier"">below {1}</span>",
				@"<span class=""quantifier"">it is necessary that</span> {0}",
				@"<span class=""quantifier"">it is possible that</span> {0}",
				@"<span class=""quantifier"">more than one</span> {0}",
				"</span>",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<span class=""quantifier"">the possible values of</span> {0} <span class=""quantifier"">are</span> {1}",
				@"<span class=""quantifier"">some</span> {0} <span class=""quantifier"">participates in none of the following:</span>{1}",
				@"<span class=""instance"">{0}</span>",
				@"<span class=""quantifier"">Notes:</span> <span class=""note"">{0}</span>",
				@"{0} <span class=""quantifier"">objectifies</span> ""{1}""",
				@"<a class=""objectType"" href=""elementid:{1}"">{0}</a>",
				"",
				@"<span class=""listSeparator"">, </span>",
				@"<br/><span class=""quantifier"">Examples:</span> ",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""objectTypeMissing"">Missing<sub>{0}</sub></span>",
				@"<a class=""objectType"" href=""elementid:{1}"">{0}<sub>{2}</sub></a>",
				@"{0} <span class=""quantifier"">occurs at most once in the population of</span> {1}",
				@"<span class=""quantifier"">at most one</span> {0}",
				@"{0}<span class=""listSeparator"">.</span>{1}",
				@"<span class=""quantifier"">who</span> {0}",
				@"<span class=""quantifier"">Portable data type:</span> {0}",
				@"<a class=""predicateText"" href=""elementid:{1}"">{{0}}</a>",
				@"<span class=""quantifier"">Reference Mode:</span> <span class=""referenceMode"">{0}</span>",
				@"{0}<span class=""listSeparator"">(</span><span class=""referenceMode"">{1}</span><span class=""listSeparator"">)</span>",
				@"<span class=""quantifier"">Reference Scheme:</span> {0}",
				"{0}",
				"",
				@"<span class=""listSeparator"">, and </span>",
				"",
				@"<span class=""listSeparator""> and </span>",
				@"<span class=""listSeparator"">, </span>",
				"",
				@"<span class=""logicalOperator""> and </span>",
				"",
				@"<span class=""logicalOperator""> and </span>",
				@"<span class=""logicalOperator""> and </span>",
				"",
				@"<span class=""logicalOperator""> or </span>",
				"",
				@"<span class=""logicalOperator""> or </span>",
				@"<span class=""logicalOperator""> or </span>",
				@"<span class=""quantifier"">the possible value of</span> {0} <span class=""quantifier"">is</span> {1}",
				@"<span class=""quantifier"">Each </span>{0} <span class=""quantifier"">is an instance of</span> {1}",
				@"<span class=""instance"">'{0}'</span>",
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
				"</p>",
				"</span>",
				"</body></html>",
				@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
	<title>ORM2 Verbalization</title>
	<style type=""text/css"">
		body {{ font-family: {0}; font-size: {1}pt; padding: .1em; color: {2}; {3} }}
		a {{text-decoration:none; }}
		a:hover {{background-color:infobackground; }}
		.objectType {{ color: {4}; {5} }}
		.objectTypeMissing {{ color: {4}; {5} }}
		.referenceMode {{ color: {10}; {11} }}
		.predicateText {{ color: {2}; {3} }}
		.quantifier {{ color: {6}; {7} }}
		.primaryErrorReport {{ color: red; font-weight: bolder; }}
		.secondaryErrorReport {{ color: red; }}
		.verbalization {{ }}
		.indent {{ left: 20px; position: relative; }}
		.smallIndent {{ left: 8px; position: relative;}}
		.listSeparator {{ color: windowtext; font-weight: 200;}}
		.logicalOperator {{ color: {6}; {7}}}
		.note {{ color: {8}; font-style: italic; {9} }}
		.definition {{ color: {8}; font-style: italic; {9} }}
		.notAvailable {{ font-style: italic; }}
		.instance {{ color: {12}; {13} }}
	</style>
</head>
<body>",
				"font-weight: bold;",
				"font-weight: normal;",
				@"<span class=""indent"">",
				"<br/>",
				@"<p class=""verbalization"">"});
			sets[1] = new ArrayVerbalizationSet(new string[]{
				@"<span class=""quantifier"">at most one</span> {0}",
				@"<span class=""listSeparator"">.</span>",
				@"{0} <span class=""quantifier"">combination is associated with</span> {1}",
				@"{0} <span class=""quantifier"">combination</span>",
				@"{0} <span class=""instance"">{1}</span>",
				"Missing {0}",
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
				@"<span class=""quantifier"">if </span>{0}<span class=""quantifier""> then </span>{1}",
				@"<span class=""smallIndent""><span class=""quantifier"">this association with</span> {0} <span class=""quantifier"">provides the preferred identification scheme for</span> {1}</span>",
				@"<span class=""quantifier"">context: </span>{0}",
				@"<span class=""quantifier"">in this context,</span> {0}",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">Definition:</span> <span class=""definition"">{0}</span>",
				@"<span class=""quantifier"">each instance of</span> {0} <span class=""quantifier"">occurs only once</span>",
				@"{0} <span class=""quantifier"">is an entity type</span>",
				@"{0}<span class=""quantifier""> if and only if </span>{1}",
				"</span>",
				"</span>",
				@"<span class=""primaryErrorReport"">",
				@"<span class=""secondaryErrorReport"">",
				"Model Error: {0}",
				"Model Error: {0}",
				@"<span class=""quantifier"">exactly one</span> {0}",
				@"<span class=""quantifier"">no</span> {0}<span class=""quantifier""> the same </span>{1}",
				"{0} {1}",
				@"<span class=""quantifier"">some</span> {0}",
				"</span>",
				@"<br/><span class=""quantifier"">Examples: </span><span class=""smallIndent"">",
				@"<span class=""smallIndent""><span class=""quantifier"">Identifier: <span class=""instance"">{0}</span></span></span>",
				@"<span class=""quantifier"">for each</span> {0}, {1}",
				@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">each {0} in the population of “{1}” occurs there {2}</span>",
				@"<span class=""quantifier"">exactly <span class=""instance"">{0}</span> times</span>",
				@"<span class=""quantifier"">at least <span class=""instance"">{0}</span> times</span>",
				@"<span class=""quantifier"">at least <span class=""instance"">{0}</span> and at most <span class=""instance"">{1}</span> times</span>",
				@"<span class=""quantifier"">at most <span class=""instance"">{1}</span> times</span>",
				@"<span class=""quantifier"">at most <span class=""instance"">{1}</span> {0} instances</span>",
				@"<span class=""quantifier"">all or none of the following hold:</span> {0}",
				@"<span class=""quantifier"">at most one of the following holds:</span> {0}",
				@"<span class=""quantifier"">exactly one of the following holds:</span> {0}",
				"{0}{{0}}{1}",
				"",
				@"<span class=""logicalOperator""> that is a </span>",
				"",
				@"<span class=""logicalOperator""> that is a </span>",
				@"<span class=""logicalOperator""> that is a </span>",
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
				@"<br/><span class=""smallIndent"">",
				@"<br/><span class=""logicalOperator"">and that </span>",
				@"<br/><span class=""logicalOperator"">and that </span>",
				"</span>",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"<br/><span class=""smallIndent"">",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"{0} <span class=""quantifier"">is independent (it may have instances that play no other roles)</span>",
				@"{0} <span class=""quantifier"">in</span> {1}",
				@"<span class=""quantifier"">at least {0} to at most {1}</span>",
				@"<span class=""quantifier"">at least {0} to below {1}</span>",
				@"<span class=""quantifier"">at least {0}</span>",
				@"<span class=""quantifier"">above {0} to at most {1}</span>",
				@"<span class=""quantifier"">above {0} to below {1}</span>",
				@"<span class=""quantifier"">above {0}</span>",
				@"<span class=""quantifier"">at most {1}</span>",
				@"<span class=""quantifier"">below {1}</span>",
				@"<span class=""quantifier"">it is obligatory that</span> {0}",
				@"<span class=""quantifier"">it is permitted that</span> {0}",
				@"<span class=""quantifier"">more than one</span> {0}",
				"</span>",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<span class=""quantifier"">the possible values of</span> {0} <span class=""quantifier"">are</span> {1}",
				@"<span class=""quantifier"">some</span> {0} <span class=""quantifier"">participates in none of the following:</span>{1}",
				@"<span class=""instance"">{0}</span>",
				@"<span class=""quantifier"">Notes:</span> <span class=""note"">{0}</span>",
				@"{0} <span class=""quantifier"">objectifies</span> ""{1}""",
				@"<a class=""objectType"" href=""elementid:{1}"">{0}</a>",
				"",
				@"<span class=""listSeparator"">, </span>",
				@"<br/><span class=""quantifier"">Examples:</span> ",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""objectTypeMissing"">Missing<sub>{0}</sub></span>",
				@"<a class=""objectType"" href=""elementid:{1}"">{0}<sub>{2}</sub></a>",
				@"{0} <span class=""quantifier"">occurs at most once in the population of</span> {1}",
				@"<span class=""quantifier"">at most one</span> {0}",
				@"{0}<span class=""listSeparator"">.</span>{1}",
				@"<span class=""quantifier"">who</span> {0}",
				@"<span class=""quantifier"">Portable data type:</span> {0}",
				@"<a class=""predicateText"" href=""elementid:{1}"">{{0}}</a>",
				@"<span class=""quantifier"">Reference Mode:</span> <span class=""referenceMode"">{0}</span>",
				@"{0}<span class=""listSeparator"">(</span><span class=""referenceMode"">{1}</span><span class=""listSeparator"">)</span>",
				@"<span class=""quantifier"">Reference Scheme:</span> {0}",
				"{0}",
				"",
				@"<span class=""listSeparator"">, and </span>",
				"",
				@"<span class=""listSeparator""> and </span>",
				@"<span class=""listSeparator"">, </span>",
				"",
				@"<span class=""logicalOperator""> and </span>",
				"",
				@"<span class=""logicalOperator""> and </span>",
				@"<span class=""logicalOperator""> and </span>",
				"",
				@"<span class=""logicalOperator""> or </span>",
				"",
				@"<span class=""logicalOperator""> or </span>",
				@"<span class=""logicalOperator""> or </span>",
				@"<span class=""quantifier"">the possible value of</span> {0} <span class=""quantifier"">is</span> {1}",
				@"<span class=""quantifier"">Each </span>{0} <span class=""quantifier"">is an instance of</span> {1}",
				@"<span class=""instance"">'{0}'</span>",
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
				"</p>",
				"</span>",
				"</body></html>",
				@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
	<title>ORM2 Verbalization</title>
	<style type=""text/css"">
		body {{ font-family: {0}; font-size: {1}pt; padding: .1em; color: {2}; {3} }}
		a {{text-decoration:none; }}
		a:hover {{background-color:infobackground; }}
		.objectType {{ color: {4}; {5} }}
		.objectTypeMissing {{ color: {4}; {5} }}
		.referenceMode {{ color: {10}; {11} }}
		.predicateText {{ color: {2}; {3} }}
		.quantifier {{ color: {6}; {7} }}
		.primaryErrorReport {{ color: red; font-weight: bolder; }}
		.secondaryErrorReport {{ color: red; }}
		.verbalization {{ }}
		.indent {{ left: 20px; position: relative; }}
		.smallIndent {{ left: 8px; position: relative;}}
		.listSeparator {{ color: windowtext; font-weight: 200;}}
		.logicalOperator {{ color: {6}; {7}}}
		.note {{ color: {8}; font-style: italic; {9} }}
		.definition {{ color: {8}; font-style: italic; {9} }}
		.notAvailable {{ font-style: italic; }}
		.instance {{ color: {12}; {13} }}
	</style>
</head>
<body>",
				"font-weight: bold;",
				"font-weight: normal;",
				@"<span class=""indent"">",
				"<br/>",
				@"<p class=""verbalization"">"});
			sets[2] = new ArrayVerbalizationSet(new string[]{
				@"<span class=""quantifier"">at most one</span> {0}",
				@"<span class=""listSeparator"">.</span>",
				@"{0} <span class=""quantifier"">combination is associated with</span> {1}",
				@"{0} <span class=""quantifier"">combination</span>",
				@"{0} <span class=""instance"">{1}</span>",
				"Missing {0}",
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
				@"<span class=""quantifier"">if </span>{0}<span class=""quantifier""> then </span>{1}",
				@"<span class=""smallIndent""><span class=""quantifier"">this association with</span> {0} <span class=""quantifier"">provides the preferred identification scheme for</span> {1}</span>",
				@"<span class=""quantifier"">context: </span>{0}",
				@"<span class=""quantifier"">in this context,</span> {0}",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">Definition:</span> <span class=""definition"">{0}</span>",
				@"<span class=""quantifier"">each instance of</span> {0} <span class=""quantifier"">occurs only once</span>",
				@"{0} <span class=""quantifier"">is an entity type</span>",
				@"{0}<span class=""quantifier""> if and only if </span>{1}",
				"</span>",
				"</span>",
				@"<span class=""primaryErrorReport"">",
				@"<span class=""secondaryErrorReport"">",
				"Model Error: {0}",
				"Model Error: {0}",
				@"<span class=""quantifier"">exactly one</span> {0}",
				@"<span class=""quantifier"">no</span> {0}<span class=""quantifier""> the same </span>{1}",
				"{0} {1}",
				@"<span class=""quantifier"">no</span> {0}",
				"</span>",
				@"<br/><span class=""quantifier"">Examples: </span><span class=""smallIndent"">",
				@"<span class=""smallIndent""><span class=""quantifier"">Identifier: <span class=""instance"">{0}</span></span></span>",
				@"<span class=""quantifier"">for each</span> {0}, {1}",
				@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">each {0} in the population of “{1}” occurs there {2}</span>",
				@"<span class=""quantifier"">exactly <span class=""instance"">{0}</span> times</span>",
				@"<span class=""quantifier"">at least <span class=""instance"">{0}</span> times</span>",
				@"<span class=""quantifier"">at least <span class=""instance"">{0}</span> and at most <span class=""instance"">{1}</span> times</span>",
				@"<span class=""quantifier"">at most <span class=""instance"">{1}</span> times</span>",
				@"<span class=""quantifier"">at most <span class=""instance"">{1}</span> {0} instances</span>",
				@"<span class=""quantifier"">all or none of the following hold:</span> {0}",
				@"<span class=""quantifier"">at most one of the following holds:</span> {0}",
				@"<span class=""quantifier"">exactly one of the following holds:</span> {0}",
				"{0}{{0}}{1}",
				"",
				@"<span class=""logicalOperator""> that is a </span>",
				"",
				@"<span class=""logicalOperator""> that is a </span>",
				@"<span class=""logicalOperator""> that is a </span>",
				@"<span class=""quantifier"">the same</span> {0}",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">it is impossible that</span> {0}",
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
				@"<br/><span class=""smallIndent"">",
				@"<br/><span class=""logicalOperator"">and that </span>",
				@"<br/><span class=""logicalOperator"">and that </span>",
				"</span>",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"<br/><span class=""smallIndent"">",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"{0} <span class=""quantifier"">is independent (it may have instances that play no other roles)</span>",
				@"{0} <span class=""quantifier"">in</span> {1}",
				@"<span class=""quantifier"">at least {0} to at most {1}</span>",
				@"<span class=""quantifier"">at least {0} to below {1}</span>",
				@"<span class=""quantifier"">at least {0}</span>",
				@"<span class=""quantifier"">above {0} to at most {1}</span>",
				@"<span class=""quantifier"">above {0} to below {1}</span>",
				@"<span class=""quantifier"">above {0}</span>",
				@"<span class=""quantifier"">at most {1}</span>",
				@"<span class=""quantifier"">below {1}</span>",
				@"<span class=""quantifier"">it is necessary that</span> {0}",
				@"<span class=""quantifier"">it is impossible that</span> {0}",
				@"<span class=""quantifier"">more than one</span> {0}",
				"</span>",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<span class=""quantifier"">the possible values of</span> {0} <span class=""quantifier"">are</span> {1}",
				@"<span class=""quantifier"">some</span> {0} <span class=""quantifier"">participates in none of the following:</span>{1}",
				@"<span class=""instance"">{0}</span>",
				@"<span class=""quantifier"">Notes:</span> <span class=""note"">{0}</span>",
				@"{0} <span class=""quantifier"">objectifies</span> ""{1}""",
				@"<a class=""objectType"" href=""elementid:{1}"">{0}</a>",
				"",
				@"<span class=""listSeparator"">, </span>",
				@"<br/><span class=""quantifier"">Examples:</span> ",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""objectTypeMissing"">Missing<sub>{0}</sub></span>",
				@"<a class=""objectType"" href=""elementid:{1}"">{0}<sub>{2}</sub></a>",
				@"{0} <span class=""quantifier"">occurs more than once in the population of</span> {1}",
				@"<span class=""quantifier"">more than one</span> {0}",
				@"{0}<span class=""listSeparator"">.</span>{1}",
				@"<span class=""quantifier"">who</span> {0}",
				@"<span class=""quantifier"">Portable data type:</span> {0}",
				@"<a class=""predicateText"" href=""elementid:{1}"">{{0}}</a>",
				@"<span class=""quantifier"">Reference Mode:</span> <span class=""referenceMode"">{0}</span>",
				@"{0}<span class=""listSeparator"">(</span><span class=""referenceMode"">{1}</span><span class=""listSeparator"">)</span>",
				@"<span class=""quantifier"">Reference Scheme:</span> {0}",
				"{0}",
				"",
				@"<span class=""listSeparator"">, and </span>",
				"",
				@"<span class=""listSeparator""> and </span>",
				@"<span class=""listSeparator"">, </span>",
				"",
				@"<span class=""logicalOperator""> and </span>",
				"",
				@"<span class=""logicalOperator""> and </span>",
				@"<span class=""logicalOperator""> and </span>",
				"",
				@"<span class=""logicalOperator""> or </span>",
				"",
				@"<span class=""logicalOperator""> or </span>",
				@"<span class=""logicalOperator""> or </span>",
				@"<span class=""quantifier"">the possible value of</span> {0} <span class=""quantifier"">is</span> {1}",
				@"<span class=""quantifier"">Each </span>{0} <span class=""quantifier"">is an instance of</span> {1}",
				@"<span class=""instance"">'{0}'</span>",
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
				@"<span class=""quantifier"">any</span> {0}",
				@"{0} <span class=""quantifier"">is a value type</span>",
				"</p>",
				"</span>",
				"</body></html>",
				@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
	<title>ORM2 Verbalization</title>
	<style type=""text/css"">
		body {{ font-family: {0}; font-size: {1}pt; padding: .1em; color: {2}; {3} }}
		a {{text-decoration:none; }}
		a:hover {{background-color:infobackground; }}
		.objectType {{ color: {4}; {5} }}
		.objectTypeMissing {{ color: {4}; {5} }}
		.referenceMode {{ color: {10}; {11} }}
		.predicateText {{ color: {2}; {3} }}
		.quantifier {{ color: {6}; {7} }}
		.primaryErrorReport {{ color: red; font-weight: bolder; }}
		.secondaryErrorReport {{ color: red; }}
		.verbalization {{ }}
		.indent {{ left: 20px; position: relative; }}
		.smallIndent {{ left: 8px; position: relative;}}
		.listSeparator {{ color: windowtext; font-weight: 200;}}
		.logicalOperator {{ color: {6}; {7}}}
		.note {{ color: {8}; font-style: italic; {9} }}
		.definition {{ color: {8}; font-style: italic; {9} }}
		.notAvailable {{ font-style: italic; }}
		.instance {{ color: {12}; {13} }}
	</style>
</head>
<body>",
				"font-weight: bold;",
				"font-weight: normal;",
				@"<span class=""indent"">",
				"<br/>",
				@"<p class=""verbalization"">"});
			sets[3] = new ArrayVerbalizationSet(new string[]{
				@"<span class=""quantifier"">at most one</span> {0}",
				@"<span class=""listSeparator"">.</span>",
				@"{0} <span class=""quantifier"">combination is associated with</span> {1}",
				@"{0} <span class=""quantifier"">combination</span>",
				@"{0} <span class=""instance"">{1}</span>",
				"Missing {0}",
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
				@"<span class=""quantifier"">if </span>{0}<span class=""quantifier""> then </span>{1}",
				@"<span class=""smallIndent""><span class=""quantifier"">this association with</span> {0} <span class=""quantifier"">provides the preferred identification scheme for</span> {1}</span>",
				@"<span class=""quantifier"">context: </span>{0}",
				@"<span class=""quantifier"">in this context,</span> {0}",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">Definition:</span> <span class=""definition"">{0}</span>",
				@"<span class=""quantifier"">each instance of</span> {0} <span class=""quantifier"">occurs only once</span>",
				@"{0} <span class=""quantifier"">is an entity type</span>",
				@"{0}<span class=""quantifier""> if and only if </span>{1}",
				"</span>",
				"</span>",
				@"<span class=""primaryErrorReport"">",
				@"<span class=""secondaryErrorReport"">",
				"Model Error: {0}",
				"Model Error: {0}",
				@"<span class=""quantifier"">exactly one</span> {0}",
				@"<span class=""quantifier"">no</span> {0}<span class=""quantifier""> the same </span>{1}",
				"{0} {1}",
				@"<span class=""quantifier"">no</span> {0}",
				"</span>",
				@"<br/><span class=""quantifier"">Examples: </span><span class=""smallIndent"">",
				@"<span class=""smallIndent""><span class=""quantifier"">Identifier: <span class=""instance"">{0}</span></span></span>",
				@"<span class=""quantifier"">for each</span> {0}, {1}",
				@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">each {0} in the population of “{1}” occurs there {2}</span>",
				@"<span class=""quantifier"">exactly <span class=""instance"">{0}</span> times</span>",
				@"<span class=""quantifier"">at least <span class=""instance"">{0}</span> times</span>",
				@"<span class=""quantifier"">at least <span class=""instance"">{0}</span> and at most <span class=""instance"">{1}</span> times</span>",
				@"<span class=""quantifier"">at most <span class=""instance"">{1}</span> times</span>",
				@"<span class=""quantifier"">at most <span class=""instance"">{1}</span> {0} instances</span>",
				@"<span class=""quantifier"">all or none of the following hold:</span> {0}",
				@"<span class=""quantifier"">at most one of the following holds:</span> {0}",
				@"<span class=""quantifier"">exactly one of the following holds:</span> {0}",
				"{0}{{0}}{1}",
				"",
				@"<span class=""logicalOperator""> that is a </span>",
				"",
				@"<span class=""logicalOperator""> that is a </span>",
				@"<span class=""logicalOperator""> that is a </span>",
				@"<span class=""quantifier"">the same</span> {0}",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">it is forbidden that</span> {0}",
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
				@"<br/><span class=""smallIndent"">",
				@"<br/><span class=""logicalOperator"">and that </span>",
				@"<br/><span class=""logicalOperator"">and that </span>",
				"</span>",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"<br/><span class=""smallIndent"">",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"<br/><span class=""logicalOperator"">or </span>",
				@"{0} <span class=""quantifier"">is independent (it may have instances that play no other roles)</span>",
				@"{0} <span class=""quantifier"">in</span> {1}",
				@"<span class=""quantifier"">at least {0} to at most {1}</span>",
				@"<span class=""quantifier"">at least {0} to below {1}</span>",
				@"<span class=""quantifier"">at least {0}</span>",
				@"<span class=""quantifier"">above {0} to at most {1}</span>",
				@"<span class=""quantifier"">above {0} to below {1}</span>",
				@"<span class=""quantifier"">above {0}</span>",
				@"<span class=""quantifier"">at most {1}</span>",
				@"<span class=""quantifier"">below {1}</span>",
				@"<span class=""quantifier"">it is obligatory that</span> {0}",
				@"<span class=""quantifier"">it is forbidden that</span> {0}",
				@"<span class=""quantifier"">more than one</span> {0}",
				"</span>",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<span class=""quantifier"">the possible values of</span> {0} <span class=""quantifier"">are</span> {1}",
				@"<span class=""quantifier"">some</span> {0} <span class=""quantifier"">participates in none of the following:</span>{1}",
				@"<span class=""instance"">{0}</span>",
				@"<span class=""quantifier"">Notes:</span> <span class=""note"">{0}</span>",
				@"{0} <span class=""quantifier"">objectifies</span> ""{1}""",
				@"<a class=""objectType"" href=""elementid:{1}"">{0}</a>",
				"",
				@"<span class=""listSeparator"">, </span>",
				@"<br/><span class=""quantifier"">Examples:</span> ",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""objectTypeMissing"">Missing<sub>{0}</sub></span>",
				@"<a class=""objectType"" href=""elementid:{1}"">{0}<sub>{2}</sub></a>",
				@"{0} <span class=""quantifier"">occurs more than once in the population of</span> {1}",
				@"<span class=""quantifier"">more than one</span> {0}",
				@"{0}<span class=""listSeparator"">.</span>{1}",
				@"<span class=""quantifier"">who</span> {0}",
				@"<span class=""quantifier"">Portable data type:</span> {0}",
				@"<a class=""predicateText"" href=""elementid:{1}"">{{0}}</a>",
				@"<span class=""quantifier"">Reference Mode:</span> <span class=""referenceMode"">{0}</span>",
				@"{0}<span class=""listSeparator"">(</span><span class=""referenceMode"">{1}</span><span class=""listSeparator"">)</span>",
				@"<span class=""quantifier"">Reference Scheme:</span> {0}",
				"{0}",
				"",
				@"<span class=""listSeparator"">, and </span>",
				"",
				@"<span class=""listSeparator""> and </span>",
				@"<span class=""listSeparator"">, </span>",
				"",
				@"<span class=""logicalOperator""> and </span>",
				"",
				@"<span class=""logicalOperator""> and </span>",
				@"<span class=""logicalOperator""> and </span>",
				"",
				@"<span class=""logicalOperator""> or </span>",
				"",
				@"<span class=""logicalOperator""> or </span>",
				@"<span class=""logicalOperator""> or </span>",
				@"<span class=""quantifier"">the possible value of</span> {0} <span class=""quantifier"">is</span> {1}",
				@"<span class=""quantifier"">Each </span>{0} <span class=""quantifier"">is an instance of</span> {1}",
				@"<span class=""instance"">'{0}'</span>",
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
				@"<span class=""quantifier"">any</span> {0}",
				@"{0} <span class=""quantifier"">is a value type</span>",
				"</p>",
				"</span>",
				"</body></html>",
				@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
	<title>ORM2 Verbalization</title>
	<style type=""text/css"">
		body {{ font-family: {0}; font-size: {1}pt; padding: .1em; color: {2}; {3} }}
		a {{text-decoration:none; }}
		a:hover {{background-color:infobackground; }}
		.objectType {{ color: {4}; {5} }}
		.objectTypeMissing {{ color: {4}; {5} }}
		.referenceMode {{ color: {10}; {11} }}
		.predicateText {{ color: {2}; {3} }}
		.quantifier {{ color: {6}; {7} }}
		.primaryErrorReport {{ color: red; font-weight: bolder; }}
		.secondaryErrorReport {{ color: red; }}
		.verbalization {{ }}
		.indent {{ left: 20px; position: relative; }}
		.smallIndent {{ left: 8px; position: relative;}}
		.listSeparator {{ color: windowtext; font-weight: 200;}}
		.logicalOperator {{ color: {6}; {7}}}
		.note {{ color: {8}; font-style: italic; {9} }}
		.definition {{ color: {8}; font-style: italic; {9} }}
		.notAvailable {{ font-style: italic; }}
		.instance {{ color: {12}; {13} }}
	</style>
</head>
<body>",
				"font-weight: bold;",
				"font-weight: normal;",
				@"<span class=""indent"">",
				"<br/>",
				@"<p class=""verbalization"">"});
		}
		/// <summary>Converts enum value of CoreVerbalizationSnippetType to an integer index value.</summary>
		protected override int ValueToIndex(CoreVerbalizationSnippetType enumValue)
		{
			return (int)enumValue;
		}
	}
	#endregion // CoreVerbalizationSets class
	#region FactType verbalization
	public partial class FactType : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					if (firstErrorPending)
					{
						firstErrorPending = false;
						verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenPrimaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							writer.WriteLine();
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			LinkedElementCollection<ReadingOrder> allReadingOrders = this.ReadingOrderCollection;
			IList<RoleBase> factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : this.RoleCollection;
			Nullable<int> unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
			int factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
			int unaryRoleOffset = unaryRoleIndex.HasValue ? unaryRoleIndex.Value : 0;
			FactType parentFact = this;
			const bool isDeontic = false;
			string predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
			IReading reading;
			VerbalizationHyphenBinder hyphenBinder;
			#endregion // Preliminary
			#region Pattern Matches
			string[] basicRoleReplacements = new string[factArity];
			for (int i = 0; i < factArity; ++i)
			{
				Role factRole = factRoles[i + unaryRoleOffset].Role;
				ObjectType rolePlayer = factRole.RolePlayer;
				string basicReplacement;
				if (rolePlayer != null)
				{
					basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"));
				}
				else
				{
					basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
				}
				basicRoleReplacements[i] = basicReplacement;
			}
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
			hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
			FactType.WriteVerbalizerSentence(writer, hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, basicRoleReplacements, true), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
					{
						continue;
					}
					if (firstErrorPending)
					{
						firstErrorPending = false;
						writer.WriteLine();
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
	}
	#endregion // FactType verbalization
	#region SubtypeFact verbalization
	public partial class SubtypeFact : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected new bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					if (firstErrorPending)
					{
						firstErrorPending = false;
						verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenPrimaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							writer.WriteLine();
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			const bool isDeontic = false;
			#endregion // Preliminary
			#region Pattern Matches
			if (!isNegative)
			{
				ObjectType supertype = this.Supertype;
				ObjectType subtype = this.Subtype;
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.SubtypeMetaReading, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				snippet1Replace1Replace1 = subtype.Name;
				string snippet1Replace1Replace2 = null;
				snippet1Replace1Replace2 = subtype.Id.ToString("D");
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				string snippet1Replace2 = null;
				string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative);
				string snippet1Replace2Replace1 = null;
				snippet1Replace2Replace1 = supertype.Name;
				string snippet1Replace2Replace2 = null;
				snippet1Replace2Replace2 = supertype.Id.ToString("D");
				snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1, snippet1Replace2Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
					{
						continue;
					}
					if (firstErrorPending)
					{
						firstErrorPending = false;
						writer.WriteLine();
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
	}
	#endregion // SubtypeFact verbalization
	#region ObjectType verbalization
	public partial class ObjectType : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					if (firstErrorPending)
					{
						firstErrorPending = false;
						verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenPrimaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							writer.WriteLine();
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			const bool isDeontic = false;
			StringBuilder sbTemp = null;
			UniquenessConstraint preferredIdentifier = this.ResolvedPreferredIdentifier;
			ObjectType identifyingObjectType = preferredIdentifier != null ? preferredIdentifier.PreferredIdentifierFor : null;
			LinkedElementCollection<Role> preferredIdentifierRoles = preferredIdentifier != null ? preferredIdentifier.RoleCollection : null;
			#endregion // Preliminary
			#region Pattern Matches
			CoreVerbalizationSnippetType variableSnippetSnippetType1 = 0;
			if (!this.IsValueType)
			{
				variableSnippetSnippetType1 = CoreVerbalizationSnippetType.EntityTypeVerbalization;
			}
			else
			{
				variableSnippetSnippetType1 = CoreVerbalizationSnippetType.ValueTypeVerbalization;
			}
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			string variableSnippetFormat1 = snippets.GetSnippet(variableSnippetSnippetType1, isDeontic, isNegative);
			string variableSnippet1Replace1 = null;
			string variableSnippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative);
			string variableSnippet1Replace1Replace1 = null;
			variableSnippet1Replace1Replace1 = this.Name;
			string variableSnippet1Replace1Replace2 = null;
			variableSnippet1Replace1Replace2 = this.Id.ToString("D");
			variableSnippet1Replace1 = string.Format(writer.FormatProvider, variableSnippet1ReplaceFormat1, variableSnippet1Replace1Replace1, variableSnippet1Replace1Replace2);
			FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, variableSnippetFormat1, variableSnippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			if (this.NestedFactType != null)
			{
				IList<RoleBase> factRoles = null;
				Nullable<int> unaryRoleIndex = null;
				int factArity = 0;
				int unaryRoleOffset = 0;
				LinkedElementCollection<ReadingOrder> allReadingOrders = null;
				IReading reading = null;
				VerbalizationHyphenBinder hyphenBinder;
				FactType parentFact = this.NestedFactType;
				string predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				allReadingOrders = parentFact.ReadingOrderCollection;
				factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
				unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
				factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
				unaryRoleOffset = unaryRoleIndex.HasValue ? unaryRoleIndex.Value : 0;
				string[] basicRoleReplacements = new string[factArity];
				for (int i = 0; i < factArity; ++i)
				{
					Role factRole = factRoles[i + unaryRoleOffset].Role;
					ObjectType rolePlayer = factRole.RolePlayer;
					string basicReplacement;
					if (rolePlayer != null)
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"));
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				writer.WriteLine();
				string snippetFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectifiesFactTypeVerbalization, isDeontic, isNegative);
				string snippet2Replace1 = null;
				string snippet2ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative);
				string snippet2Replace1Replace1 = null;
				snippet2Replace1Replace1 = this.Name;
				string snippet2Replace1Replace2 = null;
				snippet2Replace1Replace2 = this.Id.ToString("D");
				snippet2Replace1 = string.Format(writer.FormatProvider, snippet2ReplaceFormat1, snippet2Replace1Replace1, snippet2Replace1Replace2);
				string snippet2Replace2 = null;
				reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				snippet2Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, basicRoleReplacements, true);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat2, snippet2Replace1, snippet2Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			if (preferredIdentifier != null && !(preferredIdentifier.IsInternal && preferredIdentifier.FactTypeCollection[0].NestingType != null))
			{
				writer.WriteLine();
				string snippetFormat3 = snippets.GetSnippet(CoreVerbalizationSnippetType.ReferenceSchemeVerbalization, isDeontic, isNegative);
				string snippet3Replace1 = null;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				LinkedElementCollection<Role> includedRoles = preferredIdentifierRoles;
				int constraintRoleArity = includedRoles.Count;
				for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity; ++RoleIter1)
				{
					RoleBase primaryRole = includedRoles[RoleIter1];
					FactType parentFact = primaryRole.FactType;
					string predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					LinkedElementCollection<ReadingOrder> allReadingOrders = parentFact.ReadingOrderCollection;
					IList<RoleBase> factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
					Nullable<int> unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
					int factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
					int unaryRoleOffset = unaryRoleIndex.HasValue ? unaryRoleIndex.Value : 0;
					IReading reading = null;
					VerbalizationHyphenBinder hyphenBinder;
					string[] basicRoleReplacements = new string[factArity];
					for (int i = 0; i < factArity; ++i)
					{
						Role factRole = factRoles[i + unaryRoleOffset].Role;
						ObjectType rolePlayer = factRole.RolePlayer;
						string basicReplacement;
						if (rolePlayer != null)
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"));
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
						}
						basicRoleReplacements[i] = basicReplacement;
					}
					CoreVerbalizationSnippetType listSnippet;
					if (RoleIter1 == 0)
					{
						listSnippet = CoreVerbalizationSnippetType.CompoundListOpen;
					}
					else if (RoleIter1 == constraintRoleArity - 1)
					{
						if (RoleIter1 == 1)
						{
							listSnippet = CoreVerbalizationSnippetType.CompoundListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.CompoundListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationSnippetType.CompoundListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					snippet3Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, basicRoleReplacements, true);
					sbTemp.Append(snippet3Replace1);
					if (RoleIter1 == constraintRoleArity - 1)
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
					}
				}
				snippet3Replace1 = sbTemp.ToString();
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat3, snippet3Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			if (identifyingObjectType != null && identifyingObjectType.HasReferenceMode)
			{
				writer.WriteLine();
				string snippetFormat4 = snippets.GetSnippet(CoreVerbalizationSnippetType.ReferenceModeVerbalization, isDeontic, isNegative);
				string snippet4Replace1 = null;
				snippet4Replace1 = identifyingObjectType.ReferenceModeDecoratedString;
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat4, snippet4Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			if (this.IsIndependent)
			{
				writer.WriteLine();
				string snippetFormat5 = snippets.GetSnippet(CoreVerbalizationSnippetType.IndependentVerbalization, isDeontic, isNegative);
				string snippet5Replace1 = null;
				string snippet5ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative);
				string snippet5Replace1Replace1 = null;
				snippet5Replace1Replace1 = this.Name;
				string snippet5Replace1Replace2 = null;
				snippet5Replace1Replace2 = this.Id.ToString("D");
				snippet5Replace1 = string.Format(writer.FormatProvider, snippet5ReplaceFormat1, snippet5Replace1Replace1, snippet5Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat5, snippet5Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			if (this.DataType != null)
			{
				writer.WriteLine();
				string snippetFormat6 = snippets.GetSnippet(CoreVerbalizationSnippetType.PortableDataTypeVerbalization, isDeontic, isNegative);
				string snippet6Replace1 = null;
				snippet6Replace1 = this.DataType.ToString();
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat6, snippet6Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
					{
						continue;
					}
					if (firstErrorPending)
					{
						firstErrorPending = false;
						writer.WriteLine();
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
	}
	#endregion // ObjectType verbalization
	#region Definition verbalization
	public partial class Definition : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					if (firstErrorPending)
					{
						firstErrorPending = false;
						verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenPrimaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							writer.WriteLine();
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			const bool isDeontic = false;
			#endregion // Preliminary
			#region Pattern Matches
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.DefinitionVerbalization, isDeontic, isNegative);
			string snippet1Replace1 = null;
			snippet1Replace1 = this.Text;
			FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
					{
						continue;
					}
					if (firstErrorPending)
					{
						firstErrorPending = false;
						writer.WriteLine();
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
	}
	#endregion // Definition verbalization
	#region Note verbalization
	public partial class Note : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					if (firstErrorPending)
					{
						firstErrorPending = false;
						verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenPrimaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							writer.WriteLine();
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			const bool isDeontic = false;
			#endregion // Preliminary
			#region Pattern Matches
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.NotesVerbalization, isDeontic, isNegative);
			string snippet1Replace1 = null;
			snippet1Replace1 = this.Text;
			FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
					{
						continue;
					}
					if (firstErrorPending)
					{
						firstErrorPending = false;
						writer.WriteLine();
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
	}
	#endregion // Note verbalization
	#region SubsetConstraint verbalization
	public partial class SubsetConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					if (firstErrorPending)
					{
						firstErrorPending = false;
						verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenPrimaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							writer.WriteLine();
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			bool isDeontic = (this as IConstraint).Modality == ConstraintModality.Deontic;
			StringBuilder sbTemp = null;
			FactType parentFact;
			string predicatePartFormatString;
			LinkedElementCollection<ReadingOrder> allReadingOrders;
			IList<RoleBase> factRoles = null;
			Nullable<int> unaryRoleIndex = null;
			int factArity = 0;
			int unaryRoleOffset = 0;
			LinkedElementCollection<SetComparisonConstraintRoleSequence> constraintSequences = this.RoleSequenceCollection;
			int constraintRoleArity = constraintSequences.Count;
			IList<Role>[] allConstraintSequences = new IList<Role>[constraintRoleArity];
			for (int i = 0; i < constraintRoleArity; ++i)
			{
				allConstraintSequences[i] = constraintSequences[i].RoleCollection;
			}
			int columnArity = allConstraintSequences[0].Count;
			LinkedElementCollection<FactType> allFacts = this.FactTypeCollection;
			int allFactsCount = allFacts.Count;
			if (allFactsCount == 0)
			{
				#region Error report
				if (errorOwner != null)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
				}
				#endregion // Error report
				return false;
			}
			string[][] allBasicRoleReplacements = new string[allFactsCount][];
			bool[] unaryReplacements = new bool[allFactsCount];
			int contextBasicReplacementIndex;
			int minFactArity = int.MaxValue;
			int maxFactArity = int.MinValue;
			for (int iFact = 0; iFact < allFactsCount; ++iFact)
			{
				FactType currentFact = allFacts[iFact];
				if (currentFact.ReadingRequiredError != null)
				{
					#region Error report
					if (errorOwner != null)
					{
						firstErrorPending = true;
						foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
						{
							ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
							if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
							{
								continue;
							}
							if (firstErrorPending)
							{
								firstErrorPending = false;
								verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
								writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
							}
							else
							{
								writer.WriteLine();
							}
							writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
						}
						if (!firstErrorPending)
						{
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
						}
					}
					#endregion // Error report
					return false;
				}
				allReadingOrders = currentFact.ReadingOrderCollection;
				factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : currentFact.RoleCollection;
				unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
				factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
				unaryRoleOffset = unaryRoleIndex.HasValue ? unaryRoleIndex.Value : 0;
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
					Role factRole = factRoles[i + unaryRoleOffset].Role;
					ObjectType rolePlayer = factRole.RolePlayer;
					string basicReplacement;
					if (rolePlayer != null)
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"));
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				allBasicRoleReplacements[iFact] = basicRoleReplacements;
				unaryReplacements[iFact] = unaryRoleIndex.HasValue;
			}
			IReading[] allConstraintRoleReadings = new IReading[constraintRoleArity];
			string[] roleReplacements = new string[maxFactArity];
			IReading reading;
			VerbalizationHyphenBinder hyphenBinder;
			#endregion // Preliminary
			#region Pattern Matches
			if (columnArity == 1 && !isNegative)
			{
				bool missingReading1 = false;
				for (int readingMatchIndex1 = 0; !missingReading1 && readingMatchIndex1 < constraintRoleArity; ++readingMatchIndex1)
				{
					RoleBase primaryRole = allConstraintSequences[readingMatchIndex1][0];
					parentFact = primaryRole.FactType;
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, false, factRoles, false);
					if (reading == null)
					{
						missingReading1 = true;
					}
					else
					{
						allConstraintRoleReadings[readingMatchIndex1] = reading;
					}
				}
				if (!missingReading1)
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.Conditional, isDeontic, isNegative);
					string snippet1Replace1Replace1 = null;
					int snippet1Replace1ReplaceSequenceIter1;
					for (snippet1Replace1ReplaceSequenceIter1 = 0; snippet1Replace1ReplaceSequenceIter1 < 1; ++snippet1Replace1ReplaceSequenceIter1)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1ReplaceSequenceIter1];
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
						{
							RoleBase primaryRole = includedSequenceRoles[RoleIter1];
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							factArity = factRoles.Count;
							reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, false, factRoles, true);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter1 = 0; snippet1Replace1ReplaceFactRoleIter1 < factArity; ++snippet1Replace1ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[allFacts.IndexOf(includedSequenceRoles[RoleIter1].FactType)][snippet1Replace1ReplaceFactRoleIter1], snippet1Replace1ReplaceFactRoleIter1);
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace1);
						}
						snippet1Replace1Replace1 = sbTemp.ToString();
					}
					string snippet1Replace1Replace2 = null;
					int snippet1Replace1ReplaceSequenceIter2;
					for (snippet1Replace1ReplaceSequenceIter2 = 1; snippet1Replace1ReplaceSequenceIter2 < constraintRoleArity; ++snippet1Replace1ReplaceSequenceIter2)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1ReplaceSequenceIter2];
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						for (int RoleIter2 = 0; RoleIter2 < columnArity; ++RoleIter2)
						{
							RoleBase primaryRole = includedSequenceRoles[RoleIter2];
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							factArity = factRoles.Count;
							reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, false, factRoles, true);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[allFacts.IndexOf(includedSequenceRoles[RoleIter2].FactType)][snippet1Replace1ReplaceFactRoleIter2], snippet1Replace1ReplaceFactRoleIter2);
								if (includedSequenceRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								}
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace2);
						}
						snippet1Replace1Replace2 = sbTemp.ToString();
					}
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachIndentedQuantifier, isDeontic, isNegative);
					string snippet1Replace1Replace1 = null;
					int snippet1Replace1ReplaceSequenceIter1;
					for (snippet1Replace1ReplaceSequenceIter1 = 0; snippet1Replace1ReplaceSequenceIter1 < 1; ++snippet1Replace1ReplaceSequenceIter1)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1ReplaceSequenceIter1];
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
							RoleBase primaryRole = includedSequenceRoles[RoleIter1];
							parentFact = primaryRole.FactType;
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
							sbTemp.Append(basicRoleReplacements[unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
						}
						snippet1Replace1Replace1 = sbTemp.ToString();
					}
					string snippet1Replace1Replace2 = null;
					string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.Conditional, isDeontic, isNegative);
					string snippet1Replace1Replace2Replace1 = null;
					int snippet1Replace1Replace2ReplaceSequenceIter1;
					for (snippet1Replace1Replace2ReplaceSequenceIter1 = 0; snippet1Replace1Replace2ReplaceSequenceIter1 < 1; ++snippet1Replace1Replace2ReplaceSequenceIter1)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
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
							RoleBase primaryRole = includedSequenceRoles[RoleIter1];
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							factArity = factRoles.Count;
							reading = parentFact.GetMatchingReading(allReadingOrders, null, null, (System.Collections.IList)factRoles, false, false, false, factRoles, true);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace1Replace2ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace1Replace2ReplaceFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[allFacts.IndexOf(includedSequenceRoles[RoleIter1].FactType)][snippet1Replace1Replace2ReplaceFactRoleIter1], snippet1Replace1Replace2ReplaceFactRoleIter1);
								if (includedSequenceRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								}
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace1Replace2ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace2Replace1);
						}
						snippet1Replace1Replace2Replace1 = sbTemp.ToString();
					}
					string snippet1Replace1Replace2Replace2 = null;
					int snippet1Replace1Replace2ReplaceSequenceIter2;
					for (snippet1Replace1Replace2ReplaceSequenceIter2 = 1; snippet1Replace1Replace2ReplaceSequenceIter2 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceSequenceIter2)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace2ReplaceSequenceIter2];
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						for (int RoleIter2 = 0; RoleIter2 < 1; ++RoleIter2)
						{
							RoleBase primaryRole = includedSequenceRoles[RoleIter2];
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							factArity = factRoles.Count;
							reading = parentFact.GetMatchingReading(allReadingOrders, null, null, (System.Collections.IList)factRoles, false, false, false, factRoles, true);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1Replace2ReplaceFactRoleIter2 = 0; snippet1Replace1Replace2ReplaceFactRoleIter2 < factArity; ++snippet1Replace1Replace2ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1Replace2ReplaceFactRoleIter2];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[allFacts.IndexOf(includedSequenceRoles[RoleIter2].FactType)][snippet1Replace1Replace2ReplaceFactRoleIter2], snippet1Replace1Replace2ReplaceFactRoleIter2);
								if (includedSequenceRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								}
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace1Replace2ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace2Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace2Replace2);
						}
						snippet1Replace1Replace2Replace2 = sbTemp.ToString();
					}
					snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1, snippet1Replace1Replace2Replace2);
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
			}
			else if (!isNegative)
			{
				bool missingReading1 = false;
				for (int readingMatchIndex1 = 0; !missingReading1 && readingMatchIndex1 < constraintRoleArity; ++readingMatchIndex1)
				{
					RoleBase primaryRole = allConstraintSequences[readingMatchIndex1][0];
					parentFact = primaryRole.FactType;
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, false, factRoles, false);
					if (reading == null)
					{
						missingReading1 = true;
					}
					else
					{
						allConstraintRoleReadings[readingMatchIndex1] = reading;
					}
				}
				if (!missingReading1)
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.Conditional, isDeontic, isNegative);
					string snippet1Replace1Replace1 = null;
					int snippet1Replace1ReplaceSequenceIter1;
					for (snippet1Replace1ReplaceSequenceIter1 = 0; snippet1Replace1ReplaceSequenceIter1 < 1; ++snippet1Replace1ReplaceSequenceIter1)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1ReplaceSequenceIter1];
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						FactType[] snippet1Replace1ReplaceUniqueFactTypes1 = new FactType[columnArity];
						FactType snippet1Replace1ReplaceTestUniqueFactType1;
						int snippet1Replace1ReplaceFilteredIter1;
						int snippet1Replace1ReplaceFilteredCount1 = 0;
						for (snippet1Replace1ReplaceFilteredIter1 = 0; snippet1Replace1ReplaceFilteredIter1 < columnArity; ++snippet1Replace1ReplaceFilteredIter1)
						{
							RoleBase primaryRole = includedSequenceRoles[snippet1Replace1ReplaceFilteredIter1];
							if (Array.IndexOf(snippet1Replace1ReplaceUniqueFactTypes1, snippet1Replace1ReplaceTestUniqueFactType1 = primaryRole.FactType) == -1)
							{
								snippet1Replace1ReplaceUniqueFactTypes1[snippet1Replace1ReplaceFilteredIter1] = snippet1Replace1ReplaceTestUniqueFactType1;
								++snippet1Replace1ReplaceFilteredCount1;
							}
						}
						Array.Clear(snippet1Replace1ReplaceUniqueFactTypes1, 0, snippet1Replace1ReplaceUniqueFactTypes1.Length);
						snippet1Replace1ReplaceFilteredIter1 = 0;
						for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
						{
							RoleBase primaryRole = includedSequenceRoles[RoleIter1];
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							factArity = factRoles.Count;
							if (Array.IndexOf(snippet1Replace1ReplaceUniqueFactTypes1, snippet1Replace1ReplaceTestUniqueFactType1 = primaryRole.FactType) == -1)
							{
								snippet1Replace1ReplaceUniqueFactTypes1[RoleIter1] = snippet1Replace1ReplaceTestUniqueFactType1;
								CoreVerbalizationSnippetType listSnippet;
								if (snippet1Replace1ReplaceFilteredIter1 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.CompoundListOpen;
								}
								else if (snippet1Replace1ReplaceFilteredIter1 == snippet1Replace1ReplaceFilteredCount1 - 1)
								{
									if (snippet1Replace1ReplaceFilteredIter1 == 1)
									{
										listSnippet = CoreVerbalizationSnippetType.CompoundListPairSeparator;
									}
									else
									{
										listSnippet = CoreVerbalizationSnippetType.CompoundListFinalSeparator;
									}
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.CompoundListSeparator;
								}
								sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
								reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, false, factRoles, true);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								for (int snippet1Replace1ReplaceFactRoleIter1 = 0; snippet1Replace1ReplaceFactRoleIter1 < factArity; ++snippet1Replace1ReplaceFactRoleIter1)
								{
									RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter1];
									string roleReplacement = null;
									string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[allFacts.IndexOf(includedSequenceRoles[RoleIter1].FactType)][snippet1Replace1ReplaceFactRoleIter1], snippet1Replace1ReplaceFactRoleIter1);
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
									if (roleReplacement == null)
									{
										roleReplacement = basicReplacement;
									}
									roleReplacements[snippet1Replace1ReplaceFactRoleIter1] = roleReplacement;
								}
								snippet1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
								sbTemp.Append(snippet1Replace1Replace1);
								if (snippet1Replace1ReplaceFilteredIter1 == snippet1Replace1ReplaceFilteredCount1 - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
								}
								++snippet1Replace1ReplaceFilteredIter1;
							}
						}
						snippet1Replace1Replace1 = sbTemp.ToString();
					}
					string snippet1Replace1Replace2 = null;
					int snippet1Replace1ReplaceSequenceIter2;
					for (snippet1Replace1ReplaceSequenceIter2 = 0; snippet1Replace1ReplaceSequenceIter2 < constraintRoleArity; ++snippet1Replace1ReplaceSequenceIter2)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1ReplaceSequenceIter2];
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						FactType[] snippet1Replace1ReplaceUniqueFactTypes2 = new FactType[columnArity];
						FactType snippet1Replace1ReplaceTestUniqueFactType2;
						int snippet1Replace1ReplaceFilteredIter2;
						int snippet1Replace1ReplaceFilteredCount2 = 0;
						for (snippet1Replace1ReplaceFilteredIter2 = 0; snippet1Replace1ReplaceFilteredIter2 < columnArity; ++snippet1Replace1ReplaceFilteredIter2)
						{
							RoleBase primaryRole = includedSequenceRoles[snippet1Replace1ReplaceFilteredIter2];
							if (Array.IndexOf(snippet1Replace1ReplaceUniqueFactTypes2, snippet1Replace1ReplaceTestUniqueFactType2 = primaryRole.FactType) == -1)
							{
								snippet1Replace1ReplaceUniqueFactTypes2[snippet1Replace1ReplaceFilteredIter2] = snippet1Replace1ReplaceTestUniqueFactType2;
								++snippet1Replace1ReplaceFilteredCount2;
							}
						}
						Array.Clear(snippet1Replace1ReplaceUniqueFactTypes2, 0, snippet1Replace1ReplaceUniqueFactTypes2.Length);
						snippet1Replace1ReplaceFilteredIter2 = 0;
						for (int RoleIter2 = 0; RoleIter2 < columnArity; ++RoleIter2)
						{
							RoleBase primaryRole = includedSequenceRoles[RoleIter2];
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							factArity = factRoles.Count;
							if (Array.IndexOf(snippet1Replace1ReplaceUniqueFactTypes2, snippet1Replace1ReplaceTestUniqueFactType2 = primaryRole.FactType) == -1)
							{
								snippet1Replace1ReplaceUniqueFactTypes2[RoleIter2] = snippet1Replace1ReplaceTestUniqueFactType2;
								CoreVerbalizationSnippetType listSnippet;
								if (snippet1Replace1ReplaceFilteredIter2 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.CompoundListOpen;
								}
								else if (snippet1Replace1ReplaceFilteredIter2 == snippet1Replace1ReplaceFilteredCount2 - 1)
								{
									if (snippet1Replace1ReplaceFilteredIter2 == 1)
									{
										listSnippet = CoreVerbalizationSnippetType.CompoundListPairSeparator;
									}
									else
									{
										listSnippet = CoreVerbalizationSnippetType.CompoundListFinalSeparator;
									}
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.CompoundListSeparator;
								}
								sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
								reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, false, factRoles, true);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
								{
									RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
									string roleReplacement = null;
									string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[allFacts.IndexOf(includedSequenceRoles[RoleIter2].FactType)][snippet1Replace1ReplaceFactRoleIter2], snippet1Replace1ReplaceFactRoleIter2);
									if (includedSequenceRoles.Contains(currentRole.Role))
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
									}
									else
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
									}
									if (roleReplacement == null)
									{
										roleReplacement = basicReplacement;
									}
									roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
								}
								snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
								sbTemp.Append(snippet1Replace1Replace2);
								if (snippet1Replace1ReplaceFilteredIter2 == snippet1Replace1ReplaceFilteredCount2 - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
								}
								++snippet1Replace1ReplaceFilteredIter2;
							}
						}
						snippet1Replace1Replace2 = sbTemp.ToString();
					}
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.Conditional, isDeontic, isNegative);
					string snippet1Replace1Replace1 = null;
					int snippet1Replace1ReplaceSequenceIter1;
					for (snippet1Replace1ReplaceSequenceIter1 = 0; snippet1Replace1ReplaceSequenceIter1 < 1; ++snippet1Replace1ReplaceSequenceIter1)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1ReplaceSequenceIter1];
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						FactType[] snippet1Replace1ReplaceUniqueFactTypes1 = new FactType[columnArity];
						FactType snippet1Replace1ReplaceTestUniqueFactType1;
						int snippet1Replace1ReplaceFilteredIter1;
						int snippet1Replace1ReplaceFilteredCount1 = 0;
						for (snippet1Replace1ReplaceFilteredIter1 = 0; snippet1Replace1ReplaceFilteredIter1 < columnArity; ++snippet1Replace1ReplaceFilteredIter1)
						{
							RoleBase primaryRole = includedSequenceRoles[snippet1Replace1ReplaceFilteredIter1];
							if (Array.IndexOf(snippet1Replace1ReplaceUniqueFactTypes1, snippet1Replace1ReplaceTestUniqueFactType1 = primaryRole.FactType) == -1)
							{
								snippet1Replace1ReplaceUniqueFactTypes1[snippet1Replace1ReplaceFilteredIter1] = snippet1Replace1ReplaceTestUniqueFactType1;
								++snippet1Replace1ReplaceFilteredCount1;
							}
						}
						Array.Clear(snippet1Replace1ReplaceUniqueFactTypes1, 0, snippet1Replace1ReplaceUniqueFactTypes1.Length);
						snippet1Replace1ReplaceFilteredIter1 = 0;
						for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
						{
							RoleBase primaryRole = includedSequenceRoles[RoleIter1];
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							factArity = factRoles.Count;
							if (Array.IndexOf(snippet1Replace1ReplaceUniqueFactTypes1, snippet1Replace1ReplaceTestUniqueFactType1 = primaryRole.FactType) == -1)
							{
								snippet1Replace1ReplaceUniqueFactTypes1[RoleIter1] = snippet1Replace1ReplaceTestUniqueFactType1;
								CoreVerbalizationSnippetType listSnippet;
								if (snippet1Replace1ReplaceFilteredIter1 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.CompoundListOpen;
								}
								else if (snippet1Replace1ReplaceFilteredIter1 == snippet1Replace1ReplaceFilteredCount1 - 1)
								{
									if (snippet1Replace1ReplaceFilteredIter1 == 1)
									{
										listSnippet = CoreVerbalizationSnippetType.CompoundListPairSeparator;
									}
									else
									{
										listSnippet = CoreVerbalizationSnippetType.CompoundListFinalSeparator;
									}
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.CompoundListSeparator;
								}
								sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
								reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, false, factRoles, true);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								for (int snippet1Replace1ReplaceFactRoleIter1 = 0; snippet1Replace1ReplaceFactRoleIter1 < factArity; ++snippet1Replace1ReplaceFactRoleIter1)
								{
									RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter1];
									string roleReplacement = null;
									string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[allFacts.IndexOf(includedSequenceRoles[RoleIter1].FactType)][snippet1Replace1ReplaceFactRoleIter1], snippet1Replace1ReplaceFactRoleIter1);
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
									if (roleReplacement == null)
									{
										roleReplacement = basicReplacement;
									}
									roleReplacements[snippet1Replace1ReplaceFactRoleIter1] = roleReplacement;
								}
								snippet1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
								sbTemp.Append(snippet1Replace1Replace1);
								if (snippet1Replace1ReplaceFilteredIter1 == snippet1Replace1ReplaceFilteredCount1 - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
								}
								++snippet1Replace1ReplaceFilteredIter1;
							}
						}
						snippet1Replace1Replace1 = sbTemp.ToString();
					}
					string snippet1Replace1Replace2 = null;
					int snippet1Replace1ReplaceSequenceIter2;
					for (snippet1Replace1ReplaceSequenceIter2 = 0; snippet1Replace1ReplaceSequenceIter2 < constraintRoleArity; ++snippet1Replace1ReplaceSequenceIter2)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1ReplaceSequenceIter2];
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						FactType[] snippet1Replace1ReplaceUniqueFactTypes2 = new FactType[columnArity];
						FactType snippet1Replace1ReplaceTestUniqueFactType2;
						int snippet1Replace1ReplaceFilteredIter2;
						int snippet1Replace1ReplaceFilteredCount2 = 0;
						for (snippet1Replace1ReplaceFilteredIter2 = 0; snippet1Replace1ReplaceFilteredIter2 < columnArity; ++snippet1Replace1ReplaceFilteredIter2)
						{
							RoleBase primaryRole = includedSequenceRoles[snippet1Replace1ReplaceFilteredIter2];
							if (Array.IndexOf(snippet1Replace1ReplaceUniqueFactTypes2, snippet1Replace1ReplaceTestUniqueFactType2 = primaryRole.FactType) == -1)
							{
								snippet1Replace1ReplaceUniqueFactTypes2[snippet1Replace1ReplaceFilteredIter2] = snippet1Replace1ReplaceTestUniqueFactType2;
								++snippet1Replace1ReplaceFilteredCount2;
							}
						}
						Array.Clear(snippet1Replace1ReplaceUniqueFactTypes2, 0, snippet1Replace1ReplaceUniqueFactTypes2.Length);
						snippet1Replace1ReplaceFilteredIter2 = 0;
						for (int RoleIter2 = 0; RoleIter2 < columnArity; ++RoleIter2)
						{
							RoleBase primaryRole = includedSequenceRoles[RoleIter2];
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							factArity = factRoles.Count;
							if (Array.IndexOf(snippet1Replace1ReplaceUniqueFactTypes2, snippet1Replace1ReplaceTestUniqueFactType2 = primaryRole.FactType) == -1)
							{
								snippet1Replace1ReplaceUniqueFactTypes2[RoleIter2] = snippet1Replace1ReplaceTestUniqueFactType2;
								CoreVerbalizationSnippetType listSnippet;
								if (snippet1Replace1ReplaceFilteredIter2 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.CompoundListOpen;
								}
								else if (snippet1Replace1ReplaceFilteredIter2 == snippet1Replace1ReplaceFilteredCount2 - 1)
								{
									if (snippet1Replace1ReplaceFilteredIter2 == 1)
									{
										listSnippet = CoreVerbalizationSnippetType.CompoundListPairSeparator;
									}
									else
									{
										listSnippet = CoreVerbalizationSnippetType.CompoundListFinalSeparator;
									}
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.CompoundListSeparator;
								}
								sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
								reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, false, factRoles, true);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
								{
									RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
									string roleReplacement = null;
									string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[allFacts.IndexOf(includedSequenceRoles[RoleIter2].FactType)][snippet1Replace1ReplaceFactRoleIter2], snippet1Replace1ReplaceFactRoleIter2);
									if (includedSequenceRoles.Contains(currentRole.Role))
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
									}
									else
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
									}
									if (roleReplacement == null)
									{
										roleReplacement = basicReplacement;
									}
									roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
								}
								snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
								sbTemp.Append(snippet1Replace1Replace2);
								if (snippet1Replace1ReplaceFilteredIter2 == snippet1Replace1ReplaceFilteredCount2 - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
								}
								++snippet1Replace1ReplaceFilteredIter2;
							}
						}
						snippet1Replace1Replace2 = sbTemp.ToString();
					}
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
			}
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
					{
						continue;
					}
					if (firstErrorPending)
					{
						firstErrorPending = false;
						writer.WriteLine();
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
	}
	#endregion // SubsetConstraint verbalization
	#region FactType.ImpliedUniqueVerbalizer verbalization
	public partial class FactType
	{
		private partial class ImpliedUniqueVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static ImpliedUniqueVerbalizer myCache;
			public static ImpliedUniqueVerbalizer GetVerbalizer()
			{
				ImpliedUniqueVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<ImpliedUniqueVerbalizer>(ref myCache, null as ImpliedUniqueVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new ImpliedUniqueVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<ImpliedUniqueVerbalizer>(ref myCache, this, null as ImpliedUniqueVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
				StringBuilder sbTemp = null;
				FactType parentFact = this.FactType;
				string predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				LinkedElementCollection<Role> includedRoles = this.RoleCollection;
				LinkedElementCollection<ReadingOrder> allReadingOrders = parentFact.ReadingOrderCollection;
				IList<RoleBase> factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
				Nullable<int> unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
				int factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
				int unaryRoleOffset = unaryRoleIndex.HasValue ? unaryRoleIndex.Value : 0;
				int includedArity = includedRoles.Count;
				if (allReadingOrders.Count == 0 || includedArity == 0)
				{
					return false;
				}
				string[] basicRoleReplacements = new string[factArity];
				for (int i = 0; i < factArity; ++i)
				{
					Role factRole = factRoles[i + unaryRoleOffset].Role;
					ObjectType rolePlayer = factRole.RolePlayer;
					string basicReplacement;
					if (rolePlayer != null)
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"));
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				string[] roleReplacements = new string[factArity];
				IReading reading;
				VerbalizationHyphenBinder hyphenBinder;
				#endregion // Preliminary
				#region Pattern Matches
				if (includedArity == 1 && factArity == 2 && !isNegative)
				{
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, includedRoles, false, false, false, factRoles, false);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					if (reading != null)
					{
						verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
						string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace1 = null;
						for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
						{
							RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
							string roleReplacement = null;
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1ReplaceFactRoleIter1], snippet1ReplaceFactRoleIter1);
							if (includedRoles.Contains(currentRole.Role))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.OneQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
						}
						snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
					else
					{
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						if (reading != null)
						{
							verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
							string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
							string snippet1Replace1 = null;
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
								CoreVerbalizationSnippetType listSnippet;
								if (RoleIter1 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
								}
								else if (RoleIter1 == includedArity - 1)
								{
									if (RoleIter1 == 1)
									{
										listSnippet = CoreVerbalizationSnippetType.SimpleListPairSeparator;
									}
									else
									{
										listSnippet = CoreVerbalizationSnippetType.SimpleListFinalSeparator;
									}
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleListSeparator;
								}
								sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
								int ResolvedRoleIndex1 = FactType.IndexOfRole(factRoles, includedRoles[RoleIter1]);
								sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[ResolvedRoleIndex1], ResolvedRoleIndex1));
								if (RoleIter1 == includedArity - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
								}
							}
							snippet1Replace1 = sbTemp.ToString();
							string snippet1Replace2 = null;
							string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
							string snippet1Replace2Replace1 = null;
							reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1Replace2ReplaceFactRoleIter1], snippet1Replace2ReplaceFactRoleIter1);
								if (includedRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.OneQuantifier, isDeontic, isNegative), basicReplacement);
								}
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
							FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
						}
					}
					if (this.IsPreferred)
					{
						writer.WriteLine();
						string snippetFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConstraintProvidesPreferredIdentifier, isDeontic, isNegative);
						string snippet2Replace1 = null;
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
							sbTemp.Append(basicRoleReplacements[FactType.IndexOfRole(factRoles, includedRoles[RoleIter1])]);
						}
						snippet2Replace1 = sbTemp.ToString();
						string snippet2Replace2 = null;
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
							if (includedRoles.IndexOf(factRoles[RoleIter2].Role) == -1)
							{
								sbTemp.Append(basicRoleReplacements[RoleIter2]);
							}
						}
						snippet2Replace2 = sbTemp.ToString();
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat2, snippet2Replace1, snippet2Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
				}
				else if (includedArity == 1 && factArity == 2)
				{
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, includedRoles, false, false, false, factRoles, true);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
					string snippet1Replace1 = null;
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
						CoreVerbalizationSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListOpen;
						}
						else if (RoleIter1 == includedArity - 1)
						{
							if (RoleIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.CompactSimpleListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.CompactSimpleListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						int ResolvedRoleIndex1 = FactType.IndexOfRole(factRoles, includedRoles[RoleIter1]);
						sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[ResolvedRoleIndex1], ResolvedRoleIndex1));
						if (RoleIter1 == includedArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompactSimpleListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace1 = sbTemp.ToString();
					string snippet1Replace2 = null;
					string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
					string snippet1Replace2Replace1 = null;
					for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
					{
						RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1Replace2ReplaceFactRoleIter1], snippet1Replace2ReplaceFactRoleIter1);
						if (includedRoles.Contains(currentRole.Role))
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
						}
						else if (!includedRoles.Contains(currentRole.Role))
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.OneQuantifier, isDeontic, isNegative), basicReplacement);
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
					}
					snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
					snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
			}
		}
	}
	#endregion // FactType.ImpliedUniqueVerbalizer verbalization
	#region FactType.ImpliedMandatoryVerbalizer verbalization
	public partial class FactType
	{
		private partial class ImpliedMandatoryVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static ImpliedMandatoryVerbalizer myCache;
			public static ImpliedMandatoryVerbalizer GetVerbalizer()
			{
				ImpliedMandatoryVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<ImpliedMandatoryVerbalizer>(ref myCache, null as ImpliedMandatoryVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new ImpliedMandatoryVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<ImpliedMandatoryVerbalizer>(ref myCache, this, null as ImpliedMandatoryVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
				StringBuilder sbTemp = null;
				FactType parentFact = this.FactType;
				string predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				LinkedElementCollection<Role> includedRoles = this.RoleCollection;
				LinkedElementCollection<ReadingOrder> allReadingOrders = parentFact.ReadingOrderCollection;
				IList<RoleBase> factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
				Nullable<int> unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
				int factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
				int unaryRoleOffset = unaryRoleIndex.HasValue ? unaryRoleIndex.Value : 0;
				int includedArity = includedRoles.Count;
				if (allReadingOrders.Count == 0 || includedArity == 0)
				{
					return false;
				}
				string[] basicRoleReplacements = new string[factArity];
				for (int i = 0; i < factArity; ++i)
				{
					Role factRole = factRoles[i + unaryRoleOffset].Role;
					ObjectType rolePlayer = factRole.RolePlayer;
					string basicReplacement;
					if (rolePlayer != null)
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"));
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				string[] roleReplacements = new string[factArity];
				IReading reading;
				VerbalizationHyphenBinder hyphenBinder;
				#endregion // Preliminary
				#region Pattern Matches
				if (factArity == 2)
				{
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, includedRoles, false, false, false, factRoles, false);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					if (reading != null)
					{
						verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
						string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace1 = null;
						for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
						{
							RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
							string roleReplacement = null;
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1ReplaceFactRoleIter1], snippet1ReplaceFactRoleIter1);
							if (includedRoles.Contains(currentRole.Role))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
						}
						snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
					else
					{
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						if (reading != null)
						{
							verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
							string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
							string snippet1Replace1 = null;
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
								CoreVerbalizationSnippetType listSnippet;
								if (RoleIter1 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
								}
								else if (RoleIter1 == includedArity - 1)
								{
									if (RoleIter1 == 1)
									{
										listSnippet = CoreVerbalizationSnippetType.SimpleListPairSeparator;
									}
									else
									{
										listSnippet = CoreVerbalizationSnippetType.SimpleListFinalSeparator;
									}
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleListSeparator;
								}
								sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
								int ResolvedRoleIndex1 = FactType.IndexOfRole(factRoles, includedRoles[RoleIter1]);
								sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[ResolvedRoleIndex1], ResolvedRoleIndex1));
								if (RoleIter1 == includedArity - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
								}
							}
							snippet1Replace1 = sbTemp.ToString();
							string snippet1Replace2 = null;
							string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
							string snippet1Replace2Replace1 = null;
							reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1Replace2ReplaceFactRoleIter1], snippet1Replace2ReplaceFactRoleIter1);
								if (includedRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								}
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
							FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
						}
					}
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
			}
		}
	}
	#endregion // FactType.ImpliedMandatoryVerbalizer verbalization
	#region FactType.DefaultBinaryMissingUniquenessVerbalizer verbalization
	public partial class FactType
	{
		private partial class DefaultBinaryMissingUniquenessVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static DefaultBinaryMissingUniquenessVerbalizer myCache;
			public static DefaultBinaryMissingUniquenessVerbalizer GetVerbalizer()
			{
				DefaultBinaryMissingUniquenessVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<DefaultBinaryMissingUniquenessVerbalizer>(ref myCache, null as DefaultBinaryMissingUniquenessVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new DefaultBinaryMissingUniquenessVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<DefaultBinaryMissingUniquenessVerbalizer>(ref myCache, this, null as DefaultBinaryMissingUniquenessVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
				FactType parentFact = this.FactType;
				string predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				LinkedElementCollection<Role> includedRoles = this.RoleCollection;
				LinkedElementCollection<ReadingOrder> allReadingOrders = parentFact.ReadingOrderCollection;
				IList<RoleBase> factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
				Nullable<int> unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
				int factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
				int unaryRoleOffset = unaryRoleIndex.HasValue ? unaryRoleIndex.Value : 0;
				int includedArity = includedRoles.Count;
				if (allReadingOrders.Count == 0 || includedArity == 0)
				{
					return false;
				}
				string[] basicRoleReplacements = new string[factArity];
				for (int i = 0; i < factArity; ++i)
				{
					Role factRole = factRoles[i + unaryRoleOffset].Role;
					ObjectType rolePlayer = factRole.RolePlayer;
					string basicReplacement;
					if (rolePlayer != null)
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"));
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				string[] roleReplacements = new string[factArity];
				IReading reading;
				VerbalizationHyphenBinder hyphenBinder;
				#endregion // Preliminary
				#region Pattern Matches
				if (factArity == 2 && !isNegative)
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, includedRoles, true, false, false, factRoles, true);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
					{
						RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1ReplaceFactRoleIter1], snippet1ReplaceFactRoleIter1);
						if (includedRoles.Contains(currentRole.Role))
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.MoreThanOneQuantifier, isDeontic, isNegative), basicReplacement);
						}
						else
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative), basicReplacement);
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
					}
					snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
			}
		}
	}
	#endregion // FactType.DefaultBinaryMissingUniquenessVerbalizer verbalization
	#region FactType.CombinedMandatoryUniqueVerbalizer verbalization
	public partial class FactType
	{
		private partial class CombinedMandatoryUniqueVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static CombinedMandatoryUniqueVerbalizer myCache;
			public static CombinedMandatoryUniqueVerbalizer GetVerbalizer()
			{
				CombinedMandatoryUniqueVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<CombinedMandatoryUniqueVerbalizer>(ref myCache, null as CombinedMandatoryUniqueVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new CombinedMandatoryUniqueVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<CombinedMandatoryUniqueVerbalizer>(ref myCache, this, null as CombinedMandatoryUniqueVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
				StringBuilder sbTemp = null;
				FactType parentFact = this.FactType;
				string predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				LinkedElementCollection<Role> includedRoles = this.RoleCollection;
				LinkedElementCollection<ReadingOrder> allReadingOrders = parentFact.ReadingOrderCollection;
				IList<RoleBase> factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
				Nullable<int> unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
				int factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
				int unaryRoleOffset = unaryRoleIndex.HasValue ? unaryRoleIndex.Value : 0;
				int includedArity = includedRoles.Count;
				if (allReadingOrders.Count == 0 || includedArity == 0)
				{
					return false;
				}
				string[] basicRoleReplacements = new string[factArity];
				for (int i = 0; i < factArity; ++i)
				{
					Role factRole = factRoles[i + unaryRoleOffset].Role;
					ObjectType rolePlayer = factRole.RolePlayer;
					string basicReplacement;
					if (rolePlayer != null)
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"));
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				string[] roleReplacements = new string[factArity];
				IReading reading;
				VerbalizationHyphenBinder hyphenBinder;
				#endregion // Preliminary
				#region Pattern Matches
				if (factArity == 2 && !isNegative)
				{
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, includedRoles, false, false, false, factRoles, false);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					if (reading != null)
					{
						verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
						string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace1 = null;
						for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
						{
							RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
							string roleReplacement = null;
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1ReplaceFactRoleIter1], snippet1ReplaceFactRoleIter1);
							if (includedRoles.Contains(currentRole.Role))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExactlyOneQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
						}
						snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
					else
					{
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						if (reading != null)
						{
							verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
							string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
							string snippet1Replace1 = null;
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
								CoreVerbalizationSnippetType listSnippet;
								if (RoleIter1 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
								}
								else if (RoleIter1 == includedArity - 1)
								{
									if (RoleIter1 == 1)
									{
										listSnippet = CoreVerbalizationSnippetType.SimpleListPairSeparator;
									}
									else
									{
										listSnippet = CoreVerbalizationSnippetType.SimpleListFinalSeparator;
									}
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleListSeparator;
								}
								sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
								int ResolvedRoleIndex1 = FactType.IndexOfRole(factRoles, includedRoles[RoleIter1]);
								sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[ResolvedRoleIndex1], ResolvedRoleIndex1));
								if (RoleIter1 == includedArity - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
								}
							}
							snippet1Replace1 = sbTemp.ToString();
							string snippet1Replace2 = null;
							string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
							string snippet1Replace2Replace1 = null;
							reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1Replace2ReplaceFactRoleIter1], snippet1Replace2ReplaceFactRoleIter1);
								if (includedRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExactlyOneQuantifier, isDeontic, isNegative), basicReplacement);
								}
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
							FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
						}
					}
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
			}
		}
	}
	#endregion // FactType.CombinedMandatoryUniqueVerbalizer verbalization
	#region MandatoryConstraint verbalization
	public partial class MandatoryConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					if (firstErrorPending)
					{
						firstErrorPending = false;
						verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenPrimaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							writer.WriteLine();
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			bool isDeontic = (this as IConstraint).Modality == ConstraintModality.Deontic;
			StringBuilder sbTemp = null;
			FactType parentFact;
			string predicatePartFormatString;
			LinkedElementCollection<ReadingOrder> allReadingOrders;
			IList<RoleBase> factRoles = null;
			Nullable<int> unaryRoleIndex = null;
			int factArity = 0;
			int unaryRoleOffset = 0;
			LinkedElementCollection<Role> allConstraintRoles = this.RoleCollection;
			LinkedElementCollection<FactType> allFacts = this.FactTypeCollection;
			int allFactsCount = allFacts.Count;
			if (allFactsCount == 0)
			{
				#region Error report
				if (errorOwner != null)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
				}
				#endregion // Error report
				return false;
			}
			string[][] allBasicRoleReplacements = new string[allFactsCount][];
			bool[] unaryReplacements = new bool[allFactsCount];
			int contextBasicReplacementIndex;
			int minFactArity = int.MaxValue;
			int maxFactArity = int.MinValue;
			for (int iFact = 0; iFact < allFactsCount; ++iFact)
			{
				FactType currentFact = allFacts[iFact];
				if (currentFact.ReadingRequiredError != null)
				{
					#region Error report
					if (errorOwner != null)
					{
						firstErrorPending = true;
						foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
						{
							ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
							if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
							{
								continue;
							}
							if (firstErrorPending)
							{
								firstErrorPending = false;
								verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
								writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
							}
							else
							{
								writer.WriteLine();
							}
							writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
						}
						if (!firstErrorPending)
						{
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
						}
					}
					#endregion // Error report
					return false;
				}
				allReadingOrders = currentFact.ReadingOrderCollection;
				factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : currentFact.RoleCollection;
				unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
				factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
				unaryRoleOffset = unaryRoleIndex.HasValue ? unaryRoleIndex.Value : 0;
				if (factArity < minFactArity)
				{
					minFactArity = factArity;
				}
				if (factArity > maxFactArity)
				{
					maxFactArity = factArity;
				}
				string[] basicRoleReplacements = new string[factArity];
				ObjectType[] compatibleTypes = ObjectType.GetNearestCompatibleTypes(allConstraintRoles);
				for (int i = 0; i < factArity; ++i)
				{
					Role factRole = factRoles[i + unaryRoleOffset].Role;
					ObjectType rolePlayer = factRole.RolePlayer;
					string basicReplacement;
					if (rolePlayer != null)
					{
						if (allConstraintRoles.Contains(factRole))
						{
							int compatibleTypesCount = compatibleTypes.Length;
							if (compatibleTypesCount == 1)
							{
								basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), compatibleTypes[0].Name, compatibleTypes[0].Id.ToString("D"));
							}
							else
							{
								if (sbTemp == null)
								{
									sbTemp = new StringBuilder();
								}
								else
								{
									sbTemp.Length = 0;
								}
								for (int k = 0; k < compatibleTypesCount; ++k)
								{
									CoreVerbalizationSnippetType listSnippet;
									if (k == 0)
									{
										listSnippet = CoreVerbalizationSnippetType.IdentityEqualityListOpen;
									}
									else if (k == compatibleTypesCount - 1)
									{
										if (k == 1)
										{
											listSnippet = CoreVerbalizationSnippetType.IdentityEqualityListPairSeparator;
										}
										else
										{
											listSnippet = CoreVerbalizationSnippetType.IdentityEqualityListFinalSeparator;
										}
									}
									else
									{
										listSnippet = CoreVerbalizationSnippetType.IdentityEqualityListSeparator;
									}
									sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
									sbTemp.Append(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), compatibleTypes[k].Name, compatibleTypes[k].Id.ToString("D")));
									if (k == compatibleTypesCount - 1)
									{
										sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.IdentityEqualityListClose, isDeontic, isNegative));
									}
								}
								basicReplacement = sbTemp.ToString();
							}
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"));
						}
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				allBasicRoleReplacements[iFact] = basicRoleReplacements;
				unaryReplacements[iFact] = unaryRoleIndex.HasValue;
			}
			int constraintRoleArity = allConstraintRoles.Count;
			IReading[] allConstraintRoleReadings = new IReading[constraintRoleArity];
			string[] roleReplacements = new string[maxFactArity];
			IReading reading;
			VerbalizationHyphenBinder hyphenBinder;
			#endregion // Preliminary
			#region Pattern Matches
			if (constraintRoleArity == 1 && factArity == 1 && !isNegative)
			{
				parentFact = allFacts[0];
				predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				allReadingOrders = parentFact.ReadingOrderCollection;
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.UniversalQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				snippet1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, allBasicRoleReplacements[0], true);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (constraintRoleArity == 1 && factArity == 2 && maxFactArity <= 2)
			{
				parentFact = allFacts[0];
				predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				allReadingOrders = parentFact.ReadingOrderCollection;
				reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, false, false, false, factRoles, false);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				if (reading != null)
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
					{
						RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1], snippet1ReplaceFactRoleIter1);
						if (allConstraintRoles.Contains(currentRole.Role))
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
						}
						else
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
					}
					snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					if (reading != null)
					{
						verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
						string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
						string snippet1Replace1 = null;
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
							CoreVerbalizationSnippetType listSnippet;
							if (RoleIter1 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
							}
							else if (RoleIter1 == constraintRoleArity - 1)
							{
								if (RoleIter1 == 1)
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							int ResolvedRoleIndex1 = unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1]);
							sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][ResolvedRoleIndex1], ResolvedRoleIndex1));
							if (RoleIter1 == constraintRoleArity - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
							}
						}
						snippet1Replace1 = sbTemp.ToString();
						string snippet1Replace2 = null;
						string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace2Replace1 = null;
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
						{
							RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
							string roleReplacement = null;
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace2ReplaceFactRoleIter1], snippet1Replace2ReplaceFactRoleIter1);
							if (allConstraintRoles.Contains(currentRole.Role))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
						}
						snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
						snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
				}
			}
			else if (constraintRoleArity == 1 && minFactArity >= 3)
			{
				parentFact = allFacts[0];
				predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				allReadingOrders = parentFact.ReadingOrderCollection;
				reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, false, false, false, factRoles, false);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				if (reading != null)
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
					{
						RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1], snippet1ReplaceFactRoleIter1);
						if (allConstraintRoles.Contains(currentRole.Role))
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
						}
						else
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
					}
					snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					if (reading != null)
					{
						verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
						string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachIndentedQuantifier, isDeontic, isNegative);
						string snippet1Replace1 = null;
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
							CoreVerbalizationSnippetType listSnippet;
							if (RoleIter1 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
							}
							else if (RoleIter1 == constraintRoleArity - 1)
							{
								if (RoleIter1 == 1)
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							int ResolvedRoleIndex1 = unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1]);
							sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][ResolvedRoleIndex1], ResolvedRoleIndex1));
							if (RoleIter1 == constraintRoleArity - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
							}
						}
						snippet1Replace1 = sbTemp.ToString();
						string snippet1Replace2 = null;
						string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace2Replace1 = null;
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
						{
							RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
							string roleReplacement = null;
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace2ReplaceFactRoleIter1], snippet1Replace2ReplaceFactRoleIter1);
							if (allConstraintRoles.Contains(currentRole.Role))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
						}
						snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
						snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
				}
			}
			else if (isNegative && maxFactArity <= 1)
			{
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.NegativeReadingForUnaryOnlyDisjunctiveMandatory, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
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
					RoleBase primaryRole = allConstraintRoles[RoleIter1];
					parentFact = primaryRole.FactType;
					allReadingOrders = parentFact.ReadingOrderCollection;
					factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
					string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
					sbTemp.Append(basicRoleReplacements[unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
				}
				snippet1Replace1Replace1 = sbTemp.ToString();
				string snippet1Replace1Replace2 = null;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (int FactIter2 = 0; FactIter2 < allFactsCount; ++FactIter2)
				{
					parentFact = allFacts[FactIter2];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
					unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
					string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = FactIter2];
					CoreVerbalizationSnippetType listSnippet;
					if (FactIter2 == 0)
					{
						listSnippet = CoreVerbalizationSnippetType.IndentedCompoundListOpen;
					}
					else if (FactIter2 == allFactsCount - 1)
					{
						if (FactIter2 == 1)
						{
							listSnippet = CoreVerbalizationSnippetType.IndentedCompoundListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.IndentedCompoundListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationSnippetType.IndentedCompoundListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					snippet1Replace1Replace2 = null;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, basicRoleReplacements, true);
					sbTemp.Append(snippet1Replace1Replace2);
					if (FactIter2 == allFactsCount - 1)
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.IndentedCompoundListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1Replace2 = sbTemp.ToString();
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (!isNegative && maxFactArity <= 1)
			{
				bool missingReading1 = false;
				for (int readingMatchIndex1 = 0; !missingReading1 && readingMatchIndex1 < constraintRoleArity; ++readingMatchIndex1)
				{
					RoleBase primaryRole = allConstraintRoles[readingMatchIndex1];
					parentFact = primaryRole.FactType;
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, true, false, factRoles, false);
					if (reading == null)
					{
						missingReading1 = true;
					}
					else
					{
						allConstraintRoleReadings[readingMatchIndex1] = reading;
					}
				}
				if (!missingReading1)
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
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
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						CoreVerbalizationSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.TopLevelIndentedLogicalOrListOpen;
						}
						else if (RoleIter1 == constraintRoleArity - 1)
						{
							if (RoleIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.TopLevelIndentedLogicalOrListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.TopLevelIndentedLogicalOrListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.TopLevelIndentedLogicalOrListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						reading = allConstraintRoleReadings[RoleIter1];
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
						{
							RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
							string roleReplacement = null;
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1ReplaceFactRoleIter1], snippet1ReplaceFactRoleIter1);
							if (primaryRole == currentRole && snippet1ReplaceIsFirstPass1)
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
							}
							else if (primaryRole == currentRole)
							{
								roleReplacement = "";
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
						}
						snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
						sbTemp.Append(snippet1Replace1);
						if (RoleIter1 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.TopLevelIndentedLogicalOrListClose, isDeontic, isNegative));
						}
						snippet1ReplaceIsFirstPass1 = false;
					}
					snippet1Replace1 = sbTemp.ToString();
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
					string snippet1Replace1 = null;
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
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						sbTemp.Append(basicRoleReplacements[unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
					}
					snippet1Replace1 = sbTemp.ToString();
					string snippet1Replace2 = null;
					string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace2Replace1 = null;
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
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						CoreVerbalizationSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleLogicalOrListOpen;
						}
						else if (RoleIter1 == constraintRoleArity - 1)
						{
							if (RoleIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleLogicalOrListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleLogicalOrListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleLogicalOrListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, true, false, factRoles, true);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
						{
							RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
							string roleReplacement = null;
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1Replace2ReplaceFactRoleIter1], snippet1Replace2ReplaceFactRoleIter1);
							if (currentRole == primaryRole)
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
						}
						snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
						sbTemp.Append(snippet1Replace2Replace1);
						if (RoleIter1 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleLogicalOrListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace2Replace1 = sbTemp.ToString();
					snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
			}
			else if (!isNegative)
			{
				bool missingReading1 = false;
				for (int readingMatchIndex1 = 0; !missingReading1 && readingMatchIndex1 < constraintRoleArity; ++readingMatchIndex1)
				{
					RoleBase primaryRole = allConstraintRoles[readingMatchIndex1];
					parentFact = primaryRole.FactType;
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, true, false, factRoles, false);
					if (reading == null)
					{
						missingReading1 = true;
					}
					else
					{
						allConstraintRoleReadings[readingMatchIndex1] = reading;
					}
				}
				if (!missingReading1)
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					int snippet1ReplaceCompositeCount1 = 0;
					int snippet1ReplaceCompositeIterator1;
					for (snippet1ReplaceCompositeIterator1 = 0; snippet1ReplaceCompositeIterator1 < constraintRoleArity; ++snippet1ReplaceCompositeIterator1)
					{
						RoleBase primaryRole = allConstraintRoles[snippet1ReplaceCompositeIterator1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
						if (factArity >= 2)
						{
							++snippet1ReplaceCompositeCount1;
						}
					}
					for (snippet1ReplaceCompositeIterator1 = 0; snippet1ReplaceCompositeIterator1 < constraintRoleArity; ++snippet1ReplaceCompositeIterator1)
					{
						RoleBase primaryRole = allConstraintRoles[snippet1ReplaceCompositeIterator1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
						if (factArity == 1)
						{
							++snippet1ReplaceCompositeCount1;
						}
					}
					snippet1ReplaceCompositeIterator1 = 0;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					string snippet1Replace1Item1 = null;
					bool snippet1Replace1ItemIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						if (factArity >= 2)
						{
							CoreVerbalizationSnippetType listSnippet;
							if (snippet1ReplaceCompositeIterator1 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.TopLevelIndentedLogicalOrListOpen;
							}
							else if (snippet1ReplaceCompositeIterator1 == snippet1ReplaceCompositeCount1 - 1)
							{
								if (snippet1ReplaceCompositeIterator1 == 1)
								{
									listSnippet = CoreVerbalizationSnippetType.TopLevelIndentedLogicalOrListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.TopLevelIndentedLogicalOrListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.TopLevelIndentedLogicalOrListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							reading = allConstraintRoleReadings[RoleIter1];
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ItemFactRoleIter1 = 0; snippet1Replace1ItemFactRoleIter1 < factArity; ++snippet1Replace1ItemFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ItemFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1Replace1ItemFactRoleIter1], snippet1Replace1ItemFactRoleIter1);
								if (primaryRole == currentRole && snippet1Replace1ItemIsFirstPass1)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
								}
								else if (primaryRole == currentRole)
								{
									roleReplacement = "";
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								}
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace1ItemFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1Item1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Item1);
							if (snippet1ReplaceCompositeIterator1 == snippet1ReplaceCompositeCount1 - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.TopLevelIndentedLogicalOrListClose, isDeontic, isNegative));
							}
							++snippet1ReplaceCompositeIterator1;
							snippet1Replace1ItemIsFirstPass1 = false;
						}
					}
					string snippet1Replace1Item2 = null;
					for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity; ++RoleIter2)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter2];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
						if (factArity == 1)
						{
							CoreVerbalizationSnippetType listSnippet;
							if (snippet1ReplaceCompositeIterator1 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.TopLevelIndentedLogicalOrListOpen;
							}
							else if (snippet1ReplaceCompositeIterator1 == snippet1ReplaceCompositeCount1 - 1)
							{
								if (snippet1ReplaceCompositeIterator1 == 1)
								{
									listSnippet = CoreVerbalizationSnippetType.TopLevelIndentedLogicalOrListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.TopLevelIndentedLogicalOrListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.TopLevelIndentedLogicalOrListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							reading = allConstraintRoleReadings[RoleIter2];
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ItemFactRoleIter2 = 0; snippet1Replace1ItemFactRoleIter2 < factArity; ++snippet1Replace1ItemFactRoleIter2)
							{
								roleReplacements[snippet1Replace1ItemFactRoleIter2] = "";
							}
							snippet1Replace1Item2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Item2);
							if (snippet1ReplaceCompositeIterator1 == snippet1ReplaceCompositeCount1 - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.TopLevelIndentedLogicalOrListClose, isDeontic, isNegative));
							}
							++snippet1ReplaceCompositeIterator1;
						}
					}
					snippet1Replace1 = sbTemp.ToString();
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
					string snippet1Replace1 = null;
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
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						CoreVerbalizationSnippetType listSnippet;
						listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						sbTemp.Append(basicRoleReplacements[unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
					}
					snippet1Replace1 = sbTemp.ToString();
					string snippet1Replace2 = null;
					string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace2Replace1 = null;
					int snippet1Replace2ReplaceCompositeCount1 = 0;
					int snippet1Replace2ReplaceCompositeIterator1;
					for (snippet1Replace2ReplaceCompositeIterator1 = 0; snippet1Replace2ReplaceCompositeIterator1 < constraintRoleArity; ++snippet1Replace2ReplaceCompositeIterator1)
					{
						RoleBase primaryRole = allConstraintRoles[snippet1Replace2ReplaceCompositeIterator1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
						if (factArity >= 2)
						{
							++snippet1Replace2ReplaceCompositeCount1;
						}
					}
					for (snippet1Replace2ReplaceCompositeIterator1 = 0; snippet1Replace2ReplaceCompositeIterator1 < constraintRoleArity; ++snippet1Replace2ReplaceCompositeIterator1)
					{
						RoleBase primaryRole = allConstraintRoles[snippet1Replace2ReplaceCompositeIterator1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
						if (factArity == 1)
						{
							++snippet1Replace2ReplaceCompositeCount1;
						}
					}
					snippet1Replace2ReplaceCompositeIterator1 = 0;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					string snippet1Replace2Replace1Item1 = null;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						if (factArity >= 2)
						{
							CoreVerbalizationSnippetType listSnippet;
							if (snippet1Replace2ReplaceCompositeIterator1 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.IndentedLogicalOrListOpen;
							}
							else if (snippet1Replace2ReplaceCompositeIterator1 == snippet1Replace2ReplaceCompositeCount1 - 1)
							{
								if (snippet1Replace2ReplaceCompositeIterator1 == 1)
								{
									listSnippet = CoreVerbalizationSnippetType.IndentedLogicalOrListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.IndentedLogicalOrListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.IndentedLogicalOrListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, true, false, factRoles, true);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace2Replace1ItemFactRoleIter1 = 0; snippet1Replace2Replace1ItemFactRoleIter1 < factArity; ++snippet1Replace2Replace1ItemFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace2Replace1ItemFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1Replace2Replace1ItemFactRoleIter1], snippet1Replace2Replace1ItemFactRoleIter1);
								if (currentRole == primaryRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
								}
								else if (currentRole != primaryRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								}
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace2Replace1ItemFactRoleIter1] = roleReplacement;
							}
							snippet1Replace2Replace1Item1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace2Replace1Item1);
							if (snippet1Replace2ReplaceCompositeIterator1 == snippet1Replace2ReplaceCompositeCount1 - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.IndentedLogicalOrListClose, isDeontic, isNegative));
							}
							++snippet1Replace2ReplaceCompositeIterator1;
						}
					}
					string snippet1Replace2Replace1Item2 = null;
					for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity; ++RoleIter2)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter2];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						if (factArity == 1)
						{
							CoreVerbalizationSnippetType listSnippet;
							if (snippet1Replace2ReplaceCompositeIterator1 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.IndentedLogicalOrListOpen;
							}
							else if (snippet1Replace2ReplaceCompositeIterator1 == snippet1Replace2ReplaceCompositeCount1 - 1)
							{
								if (snippet1Replace2ReplaceCompositeIterator1 == 1)
								{
									listSnippet = CoreVerbalizationSnippetType.IndentedLogicalOrListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.IndentedLogicalOrListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.IndentedLogicalOrListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace2Replace1ItemFactRoleIter2 = 0; snippet1Replace2Replace1ItemFactRoleIter2 < factArity; ++snippet1Replace2Replace1ItemFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace2Replace1ItemFactRoleIter2];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1Replace2Replace1ItemFactRoleIter2], snippet1Replace2Replace1ItemFactRoleIter2);
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace2Replace1ItemFactRoleIter2] = roleReplacement;
							}
							snippet1Replace2Replace1Item2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace2Replace1Item2);
							if (snippet1Replace2ReplaceCompositeIterator1 == snippet1Replace2ReplaceCompositeCount1 - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.IndentedLogicalOrListClose, isDeontic, isNegative));
							}
							++snippet1Replace2ReplaceCompositeIterator1;
						}
					}
					snippet1Replace2Replace1 = sbTemp.ToString();
					snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
			}
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
					{
						continue;
					}
					if (firstErrorPending)
					{
						firstErrorPending = false;
						writer.WriteLine();
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
	}
	#endregion // MandatoryConstraint verbalization
	#region UniquenessConstraint verbalization
	public partial class UniquenessConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					if (firstErrorPending)
					{
						firstErrorPending = false;
						verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenPrimaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							writer.WriteLine();
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			bool isDeontic = (this as IConstraint).Modality == ConstraintModality.Deontic;
			StringBuilder sbTemp = null;
			FactType parentFact;
			string predicatePartFormatString;
			LinkedElementCollection<ReadingOrder> allReadingOrders;
			IList<RoleBase> factRoles = null;
			Nullable<int> unaryRoleIndex = null;
			int factArity = 0;
			int unaryRoleOffset = 0;
			LinkedElementCollection<Role> allConstraintRoles = this.RoleCollection;
			LinkedElementCollection<FactType> allFacts = this.FactTypeCollection;
			int allFactsCount = allFacts.Count;
			if (allFactsCount == 0)
			{
				#region Error report
				if (errorOwner != null)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
				}
				#endregion // Error report
				return false;
			}
			string[][] allBasicRoleReplacements = new string[allFactsCount][];
			bool[] unaryReplacements = new bool[allFactsCount];
			int contextBasicReplacementIndex;
			int minFactArity = int.MaxValue;
			int maxFactArity = int.MinValue;
			for (int iFact = 0; iFact < allFactsCount; ++iFact)
			{
				FactType currentFact = allFacts[iFact];
				if (currentFact.ReadingRequiredError != null)
				{
					#region Error report
					if (errorOwner != null)
					{
						firstErrorPending = true;
						foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
						{
							ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
							if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
							{
								continue;
							}
							if (firstErrorPending)
							{
								firstErrorPending = false;
								verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
								writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
							}
							else
							{
								writer.WriteLine();
							}
							writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
						}
						if (!firstErrorPending)
						{
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
						}
					}
					#endregion // Error report
					return false;
				}
				allReadingOrders = currentFact.ReadingOrderCollection;
				factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : currentFact.RoleCollection;
				unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
				factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
				unaryRoleOffset = unaryRoleIndex.HasValue ? unaryRoleIndex.Value : 0;
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
					Role factRole = factRoles[i + unaryRoleOffset].Role;
					ObjectType rolePlayer = factRole.RolePlayer;
					string basicReplacement;
					if (rolePlayer != null)
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"));
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				allBasicRoleReplacements[iFact] = basicRoleReplacements;
				unaryReplacements[iFact] = unaryRoleIndex.HasValue;
			}
			int constraintRoleArity = allConstraintRoles.Count;
			IReading[] allConstraintRoleReadings = new IReading[constraintRoleArity];
			string[] roleReplacements = new string[maxFactArity];
			IReading reading;
			VerbalizationHyphenBinder hyphenBinder;
			#endregion // Preliminary
			#region Pattern Matches
			if (allFactsCount == 1 && factArity == 1 && !isNegative)
			{
				parentFact = allFacts[0];
				predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				allReadingOrders = parentFact.ReadingOrderCollection;
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.OccursInPopulation, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.UniversalQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1Replace1 = null;
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
					CoreVerbalizationSnippetType listSnippet;
					if (RoleIter1 == 0)
					{
						listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
					}
					else if (RoleIter1 == constraintRoleArity - 1)
					{
						if (RoleIter1 == 1)
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationSnippetType.SimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					sbTemp.Append(allBasicRoleReplacements[0][unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1])]);
					if (RoleIter1 == constraintRoleArity - 1)
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1Replace1Replace1 = sbTemp.ToString();
				snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1);
				string snippet1Replace1Replace2 = null;
				reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, allBasicRoleReplacements[0], true);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				if (this.IsPreferred)
				{
					ObjectType preferredFor = this.PreferredIdentifierFor;
					writer.WriteLine();
					string snippetFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConstraintProvidesPreferredIdentifier, isDeontic, isNegative);
					string snippet2Replace1 = null;
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
						sbTemp.Append(allBasicRoleReplacements[0][unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1])]);
					}
					snippet2Replace1 = sbTemp.ToString();
					string snippet2Replace2 = null;
					string snippet2ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative);
					string snippet2Replace2Replace1 = null;
					snippet2Replace2Replace1 = preferredFor.Name;
					string snippet2Replace2Replace2 = null;
					snippet2Replace2Replace2 = preferredFor.Id.ToString("D");
					snippet2Replace2 = string.Format(writer.FormatProvider, snippet2ReplaceFormat2, snippet2Replace2Replace1, snippet2Replace2Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat2, snippet2Replace1, snippet2Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
			}
			else if (allFactsCount == 1 && factArity == 1)
			{
				parentFact = allFacts[0];
				predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				allReadingOrders = parentFact.ReadingOrderCollection;
				reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.OccursInPopulation, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1Replace1 = null;
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
					CoreVerbalizationSnippetType listSnippet;
					if (RoleIter1 == 0)
					{
						listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
					}
					else if (RoleIter1 == constraintRoleArity - 1)
					{
						if (RoleIter1 == 1)
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationSnippetType.SimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					int ResolvedRoleIndex1 = unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1]);
					sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][ResolvedRoleIndex1], ResolvedRoleIndex1));
					if (RoleIter1 == constraintRoleArity - 1)
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1Replace1Replace1 = sbTemp.ToString();
				snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1);
				string snippet1Replace1Replace2 = null;
				snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, allBasicRoleReplacements[0], true);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (allFactsCount == 1 && factArity == constraintRoleArity && !isNegative)
			{
				parentFact = allFacts[0];
				predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				allReadingOrders = parentFact.ReadingOrderCollection;
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
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
					RoleBase primaryRole = factRoles[RoleIter1];
					CoreVerbalizationSnippetType listSnippet;
					if (RoleIter1 == 0)
					{
						listSnippet = CoreVerbalizationSnippetType.TopLevelIndentedLogicalAndListOpen;
					}
					else if (RoleIter1 == factArity - 1)
					{
						if (RoleIter1 == 1)
						{
							listSnippet = CoreVerbalizationSnippetType.TopLevelIndentedLogicalAndListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.TopLevelIndentedLogicalAndListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationSnippetType.TopLevelIndentedLogicalAndListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, false, factRoles, true);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
					{
						RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1], snippet1ReplaceFactRoleIter1);
						if (primaryRole == currentRole)
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.MoreThanOneQuantifier, isDeontic, isNegative), basicReplacement);
						}
						else
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative), basicReplacement);
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
					}
					snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
					sbTemp.Append(snippet1Replace1);
					if (RoleIter1 == factArity - 1)
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.TopLevelIndentedLogicalAndListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1 = sbTemp.ToString();
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				writer.WriteLine();
				string snippetFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet2Replace1 = null;
				string snippet2ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.OccursInPopulation, isDeontic, isNegative);
				string snippet2Replace1Replace1 = null;
				string snippet2Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.CombinationIdentifier, isDeontic, isNegative);
				string snippet2Replace1Replace1Replace1 = null;
				string snippet2Replace1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.UniversalQuantifier, isDeontic, isNegative);
				string snippet2Replace1Replace1Replace1Replace1 = null;
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
					CoreVerbalizationSnippetType listSnippet;
					if (RoleIter1 == 0)
					{
						listSnippet = CoreVerbalizationSnippetType.CompactSimpleListOpen;
					}
					else if (RoleIter1 == constraintRoleArity - 1)
					{
						if (RoleIter1 == 1)
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationSnippetType.CompactSimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					sbTemp.Append(allBasicRoleReplacements[0][unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1])]);
					if (RoleIter1 == constraintRoleArity - 1)
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompactSimpleListClose, isDeontic, isNegative));
					}
				}
				snippet2Replace1Replace1Replace1Replace1 = sbTemp.ToString();
				snippet2Replace1Replace1Replace1 = string.Format(writer.FormatProvider, snippet2Replace1Replace1ReplaceFormat1, snippet2Replace1Replace1Replace1Replace1);
				snippet2Replace1Replace1 = string.Format(writer.FormatProvider, snippet2Replace1ReplaceFormat1, snippet2Replace1Replace1Replace1);
				string snippet2Replace1Replace2 = null;
				reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				snippet2Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, allBasicRoleReplacements[0], true);
				snippet2Replace1 = string.Format(writer.FormatProvider, snippet2ReplaceFormat1, snippet2Replace1Replace1, snippet2Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat2, snippet2Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				if (this.IsPreferred)
				{
					ObjectType preferredFor = this.PreferredIdentifierFor;
					writer.WriteLine();
					string snippetFormat3 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConstraintProvidesPreferredIdentifier, isDeontic, isNegative);
					string snippet3Replace1 = null;
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
						CoreVerbalizationSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListOpen;
						}
						else if (RoleIter1 == constraintRoleArity - 1)
						{
							if (RoleIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.CompactSimpleListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.CompactSimpleListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						sbTemp.Append(allBasicRoleReplacements[0][unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1])]);
						if (RoleIter1 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompactSimpleListClose, isDeontic, isNegative));
						}
					}
					snippet3Replace1 = sbTemp.ToString();
					string snippet3Replace2 = null;
					string snippet3ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative);
					string snippet3Replace2Replace1 = null;
					snippet3Replace2Replace1 = preferredFor.Name;
					string snippet3Replace2Replace2 = null;
					snippet3Replace2Replace2 = preferredFor.Id.ToString("D");
					snippet3Replace2 = string.Format(writer.FormatProvider, snippet3ReplaceFormat2, snippet3Replace2Replace1, snippet3Replace2Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat3, snippet3Replace1, snippet3Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
			}
			else if (allFactsCount == 1 && factArity == constraintRoleArity)
			{
				parentFact = allFacts[0];
				predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				allReadingOrders = parentFact.ReadingOrderCollection;
				reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.OccursInPopulation, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.CombinationIdentifier, isDeontic, isNegative);
				string snippet1Replace1Replace1Replace1 = null;
				string snippet1Replace1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1Replace1Replace1 = null;
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
					CoreVerbalizationSnippetType listSnippet;
					if (RoleIter1 == 0)
					{
						listSnippet = CoreVerbalizationSnippetType.CompactSimpleListOpen;
					}
					else if (RoleIter1 == constraintRoleArity - 1)
					{
						if (RoleIter1 == 1)
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationSnippetType.CompactSimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					int ResolvedRoleIndex1 = unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1]);
					sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][ResolvedRoleIndex1], ResolvedRoleIndex1));
					if (RoleIter1 == constraintRoleArity - 1)
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompactSimpleListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1Replace1Replace1Replace1 = sbTemp.ToString();
				snippet1Replace1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1Replace1);
				snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1);
				string snippet1Replace1Replace2 = null;
				snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, allBasicRoleReplacements[0], true);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (allFactsCount == 1 && factArity == 2 && !isNegative)
			{
				parentFact = allFacts[0];
				predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				allReadingOrders = parentFact.ReadingOrderCollection;
				reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, false, false, false, factRoles, false);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				if (reading != null)
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
					{
						RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1], snippet1ReplaceFactRoleIter1);
						if (allConstraintRoles.Contains(currentRole.Role))
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
						}
						else
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.OneQuantifier, isDeontic, isNegative), basicReplacement);
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
					}
					snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					if (reading != null)
					{
						verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
						string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
						string snippet1Replace1 = null;
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
							CoreVerbalizationSnippetType listSnippet;
							if (RoleIter1 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
							}
							else if (RoleIter1 == constraintRoleArity - 1)
							{
								if (RoleIter1 == 1)
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							int ResolvedRoleIndex1 = unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1]);
							sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][ResolvedRoleIndex1], ResolvedRoleIndex1));
							if (RoleIter1 == constraintRoleArity - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
							}
						}
						snippet1Replace1 = sbTemp.ToString();
						string snippet1Replace2 = null;
						string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace2Replace1 = null;
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
						{
							RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
							string roleReplacement = null;
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace2ReplaceFactRoleIter1], snippet1Replace2ReplaceFactRoleIter1);
							if (allConstraintRoles.Contains(currentRole.Role))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.OneQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
						}
						snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
						snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
				}
				if (this.IsPreferred)
				{
					ObjectType preferredFor = this.PreferredIdentifierFor;
					writer.WriteLine();
					string snippetFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConstraintProvidesPreferredIdentifier, isDeontic, isNegative);
					string snippet2Replace1 = null;
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
						CoreVerbalizationSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListOpen;
						}
						else if (RoleIter1 == constraintRoleArity - 1)
						{
							if (RoleIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.CompactSimpleListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.CompactSimpleListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						sbTemp.Append(allBasicRoleReplacements[0][unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1])]);
						if (RoleIter1 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompactSimpleListClose, isDeontic, isNegative));
						}
					}
					snippet2Replace1 = sbTemp.ToString();
					string snippet2Replace2 = null;
					string snippet2ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative);
					string snippet2Replace2Replace1 = null;
					snippet2Replace2Replace1 = preferredFor.Name;
					string snippet2Replace2Replace2 = null;
					snippet2Replace2Replace2 = preferredFor.Id.ToString("D");
					snippet2Replace2 = string.Format(writer.FormatProvider, snippet2ReplaceFormat2, snippet2Replace2Replace1, snippet2Replace2Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat2, snippet2Replace1, snippet2Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
			}
			else if (allFactsCount == 1 && factArity == 2)
			{
				parentFact = allFacts[0];
				predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				allReadingOrders = parentFact.ReadingOrderCollection;
				reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, false, false, false, factRoles, true);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
				string snippet1Replace1 = null;
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
					CoreVerbalizationSnippetType listSnippet;
					if (RoleIter1 == 0)
					{
						listSnippet = CoreVerbalizationSnippetType.CompactSimpleListOpen;
					}
					else if (RoleIter1 == constraintRoleArity - 1)
					{
						if (RoleIter1 == 1)
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationSnippetType.CompactSimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					int ResolvedRoleIndex1 = unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1]);
					sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][ResolvedRoleIndex1], ResolvedRoleIndex1));
					if (RoleIter1 == constraintRoleArity - 1)
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompactSimpleListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1 = sbTemp.ToString();
				string snippet1Replace2 = null;
				string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace2Replace1 = null;
				for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
				{
					RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
					string roleReplacement = null;
					string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace2ReplaceFactRoleIter1], snippet1Replace2ReplaceFactRoleIter1);
					if (allConstraintRoles.Contains(currentRole.Role))
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
					}
					else if (!allConstraintRoles.Contains(currentRole.Role))
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.OneQuantifier, isDeontic, isNegative), basicReplacement);
					}
					if (roleReplacement == null)
					{
						roleReplacement = basicReplacement;
					}
					roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
				}
				snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
				snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (allFactsCount == 1 && isNegative)
			{
				parentFact = allFacts[0];
				predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				allReadingOrders = parentFact.ReadingOrderCollection;
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
				{
					RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
					string roleReplacement = null;
					string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1], snippet1ReplaceFactRoleIter1);
					if (allConstraintRoles.Contains(currentRole.Role))
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative), basicReplacement);
					}
					else
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.MoreThanOneQuantifier, isDeontic, isNegative), basicReplacement);
					}
					if (roleReplacement == null)
					{
						roleReplacement = basicReplacement;
					}
					roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
				}
				snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (allFactsCount == 1)
			{
				parentFact = allFacts[0];
				predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				allReadingOrders = parentFact.ReadingOrderCollection;
				reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachIndentedQuantifier, isDeontic, isNegative);
				string snippet1Replace1 = null;
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
					CoreVerbalizationSnippetType listSnippet;
					if (RoleIter1 == 0)
					{
						listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
					}
					else if (RoleIter1 == constraintRoleArity - 1)
					{
						if (RoleIter1 == 1)
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationSnippetType.SimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					int ResolvedRoleIndex1 = unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1]);
					sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][ResolvedRoleIndex1], ResolvedRoleIndex1));
					if (RoleIter1 == constraintRoleArity - 1)
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1 = sbTemp.ToString();
				string snippet1Replace2 = null;
				string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace2Replace1 = null;
				for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
				{
					RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
					string roleReplacement = null;
					string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace2ReplaceFactRoleIter1], snippet1Replace2ReplaceFactRoleIter1);
					if (allConstraintRoles.Contains(currentRole.Role))
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
					}
					else if (!allConstraintRoles.Contains(currentRole.Role))
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.OneQuantifier, isDeontic, isNegative), basicReplacement);
					}
					if (roleReplacement == null)
					{
						roleReplacement = basicReplacement;
					}
					roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
				}
				snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
				snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (!isNegative && minFactArity >= 2 && maxFactArity <= 2)
			{
				bool missingReading1 = false;
				for (int readingMatchIndex1 = 0; !missingReading1 && readingMatchIndex1 < constraintRoleArity; ++readingMatchIndex1)
				{
					RoleBase primaryRole = allConstraintRoles[readingMatchIndex1];
					parentFact = primaryRole.FactType;
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, true, true, false, factRoles, false);
					if (reading == null)
					{
						missingReading1 = true;
					}
					else
					{
						allConstraintRoleReadings[readingMatchIndex1] = reading;
					}
				}
				if (!missingReading1)
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
					string snippet1Replace1 = null;
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
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						CoreVerbalizationSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
						}
						else if (RoleIter1 == constraintRoleArity - 1)
						{
							if (RoleIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						int ResolvedRoleIndex1 = unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole);
						sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[ResolvedRoleIndex1], ResolvedRoleIndex1));
						if (RoleIter1 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace1 = sbTemp.ToString();
					string snippet1Replace2 = null;
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
						RoleBase primaryRole = allConstraintRoles[RoleIter2];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						CoreVerbalizationSnippetType listSnippet;
						if (RoleIter2 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.IndentedListOpen;
						}
						else if (RoleIter2 == constraintRoleArity - 1)
						{
							if (RoleIter2 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.IndentedListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.IndentedListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.IndentedListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						reading = allConstraintRoleReadings[RoleIter2];
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						for (int snippet1ReplaceFactRoleIter2 = 0; snippet1ReplaceFactRoleIter2 < factArity; ++snippet1ReplaceFactRoleIter2)
						{
							RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter2];
							string roleReplacement = null;
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1ReplaceFactRoleIter2], snippet1ReplaceFactRoleIter2);
							if (currentRole != primaryRole && snippet1ReplaceIsFirstPass2)
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.AtMostOneQuantifier, isDeontic, isNegative), basicReplacement);
							}
							else if (currentRole != primaryRole)
							{
								roleReplacement = "";
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1ReplaceFactRoleIter2] = roleReplacement;
						}
						snippet1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
						sbTemp.Append(snippet1Replace2);
						if (RoleIter2 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.IndentedListClose, isDeontic, isNegative));
						}
						snippet1ReplaceIsFirstPass2 = false;
					}
					snippet1Replace2 = sbTemp.ToString();
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ContextScope, isDeontic, isNegative);
					string snippet1Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int FactIter1 = 0; FactIter1 < allFactsCount; ++FactIter1)
					{
						parentFact = allFacts[FactIter1];
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = FactIter1];
						CoreVerbalizationSnippetType listSnippet;
						if (FactIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.CompoundListOpen;
						}
						else if (FactIter1 == allFactsCount - 1)
						{
							if (FactIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.CompoundListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.CompoundListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.CompoundListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						snippet1Replace1 = null;
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, basicRoleReplacements, true);
						sbTemp.Append(snippet1Replace1);
						if (FactIter1 == allFactsCount - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace1 = sbTemp.ToString();
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					writer.WriteLine();
					string snippetFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ContextScopeReference, isDeontic, isNegative);
					string snippet2Replace1 = null;
					string snippet2ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet2Replace1Replace1 = null;
					string snippet2Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.UniversalQuantifier, isDeontic, isNegative);
					string snippet2Replace1Replace1Replace1 = null;
					string snippet2Replace1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.CombinationAssociation, isDeontic, isNegative);
					string snippet2Replace1Replace1Replace1Replace1 = null;
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
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						CoreVerbalizationSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListOpen;
						}
						else if (RoleIter1 == constraintRoleArity - 1)
						{
							if (RoleIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.CompactSimpleListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.CompactSimpleListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						sbTemp.Append(basicRoleReplacements[unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
						if (RoleIter1 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompactSimpleListClose, isDeontic, isNegative));
						}
					}
					snippet2Replace1Replace1Replace1Replace1 = sbTemp.ToString();
					string snippet2Replace1Replace1Replace1Replace2 = null;
					string snippet2Replace1Replace1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.OneQuantifier, isDeontic, isNegative);
					string snippet2Replace1Replace1Replace1Replace2Replace1 = null;
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
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						RoleBase contextPrimaryRole = primaryRole;
						int contextTempStringBuildLength = sbTemp.Length;
						for (int ContextRoleIter1 = 0; ContextRoleIter1 < factArity; ++ContextRoleIter1)
						{
							primaryRole = factRoles[ContextRoleIter1];
							if (primaryRole != contextPrimaryRole)
							{
								sbTemp.Append(basicRoleReplacements[ContextRoleIter1]);
							}
						}
						snippet2Replace1Replace1Replace1Replace2Replace1 = sbTemp.ToString(contextTempStringBuildLength, sbTemp.Length - contextTempStringBuildLength);
						primaryRole = contextPrimaryRole;
						sbTemp.Length = contextTempStringBuildLength;
						sbTemp.Append(snippet2Replace1Replace1Replace1Replace2Replace1);
					}
					snippet2Replace1Replace1Replace1Replace2Replace1 = sbTemp.ToString();
					snippet2Replace1Replace1Replace1Replace2 = string.Format(writer.FormatProvider, snippet2Replace1Replace1Replace1ReplaceFormat2, snippet2Replace1Replace1Replace1Replace2Replace1);
					snippet2Replace1Replace1Replace1 = string.Format(writer.FormatProvider, snippet2Replace1Replace1ReplaceFormat1, snippet2Replace1Replace1Replace1Replace1, snippet2Replace1Replace1Replace1Replace2);
					snippet2Replace1Replace1 = string.Format(writer.FormatProvider, snippet2Replace1ReplaceFormat1, snippet2Replace1Replace1Replace1);
					snippet2Replace1 = string.Format(writer.FormatProvider, snippet2ReplaceFormat1, snippet2Replace1Replace1);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat2, snippet2Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				if (this.IsPreferred)
				{
					ObjectType preferredFor = this.PreferredIdentifierFor;
					writer.WriteLine();
					string snippetFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConstraintProvidesPreferredIdentifier, isDeontic, isNegative);
					string snippet2Replace1 = null;
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
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						CoreVerbalizationSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListOpen;
						}
						else if (RoleIter1 == constraintRoleArity - 1)
						{
							if (RoleIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.CompactSimpleListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.CompactSimpleListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						sbTemp.Append(basicRoleReplacements[unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
						if (RoleIter1 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompactSimpleListClose, isDeontic, isNegative));
						}
					}
					snippet2Replace1 = sbTemp.ToString();
					string snippet2Replace2 = null;
					string snippet2ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative);
					string snippet2Replace2Replace1 = null;
					snippet2Replace2Replace1 = preferredFor.Name;
					string snippet2Replace2Replace2 = null;
					snippet2Replace2Replace2 = preferredFor.Id.ToString("D");
					snippet2Replace2 = string.Format(writer.FormatProvider, snippet2ReplaceFormat2, snippet2Replace2Replace1, snippet2Replace2Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat2, snippet2Replace1, snippet2Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
			}
			else if (isNegative && minFactArity >= 2 && maxFactArity <= 2)
			{
				bool missingReading1 = false;
				for (int readingMatchIndex1 = 0; !missingReading1 && readingMatchIndex1 < constraintRoleArity; ++readingMatchIndex1)
				{
					RoleBase primaryRole = allConstraintRoles[readingMatchIndex1];
					parentFact = primaryRole.FactType;
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, true, true, false, factRoles, false);
					if (reading == null)
					{
						missingReading1 = true;
					}
					else
					{
						allConstraintRoleReadings[readingMatchIndex1] = reading;
					}
				}
				if (!missingReading1)
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
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
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						CoreVerbalizationSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
						}
						else if (RoleIter1 == constraintRoleArity - 1)
						{
							if (RoleIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						reading = allConstraintRoleReadings[RoleIter1];
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
						{
							RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
							string roleReplacement = null;
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1ReplaceFactRoleIter1], snippet1ReplaceFactRoleIter1);
							if (currentRole != primaryRole && snippet1ReplaceIsFirstPass1)
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.MoreThanOneQuantifier, isDeontic, isNegative), basicReplacement);
							}
							else if (currentRole != primaryRole)
							{
								roleReplacement = "";
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
						}
						snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
						sbTemp.Append(snippet1Replace1);
						if (RoleIter1 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
						}
						snippet1ReplaceIsFirstPass1 = false;
					}
					snippet1Replace1 = sbTemp.ToString();
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ContextScope, isDeontic, isNegative);
					string snippet1Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int FactIter1 = 0; FactIter1 < allFactsCount; ++FactIter1)
					{
						parentFact = allFacts[FactIter1];
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = FactIter1];
						CoreVerbalizationSnippetType listSnippet;
						if (FactIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.CompoundListOpen;
						}
						else if (FactIter1 == allFactsCount - 1)
						{
							if (FactIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.CompoundListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.CompoundListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.CompoundListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						snippet1Replace1 = null;
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, basicRoleReplacements, true);
						sbTemp.Append(snippet1Replace1);
						if (FactIter1 == allFactsCount - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace1 = sbTemp.ToString();
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					writer.WriteLine();
					string snippetFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ContextScopeReference, isDeontic, isNegative);
					string snippet2Replace1 = null;
					string snippet2ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
					string snippet2Replace1Replace1 = null;
					string snippet2Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.CombinationAssociation, isDeontic, isNegative);
					string snippet2Replace1Replace1Replace1 = null;
					string snippet2Replace1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative);
					string snippet2Replace1Replace1Replace1Replace1 = null;
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
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						CoreVerbalizationSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListOpen;
						}
						else if (RoleIter1 == constraintRoleArity - 1)
						{
							if (RoleIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.CompactSimpleListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.CompactSimpleListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						sbTemp.Append(basicRoleReplacements[unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
						if (RoleIter1 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompactSimpleListClose, isDeontic, isNegative));
						}
					}
					snippet2Replace1Replace1Replace1Replace1 = sbTemp.ToString();
					snippet2Replace1Replace1Replace1 = string.Format(writer.FormatProvider, snippet2Replace1Replace1ReplaceFormat1, snippet2Replace1Replace1Replace1Replace1);
					string snippet2Replace1Replace1Replace2 = null;
					string snippet2Replace1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.MoreThanOneQuantifier, isDeontic, isNegative);
					string snippet2Replace1Replace1Replace2Replace1 = null;
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
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						RoleBase contextPrimaryRole = primaryRole;
						int contextTempStringBuildLength = sbTemp.Length;
						for (int ContextRoleIter1 = 0; ContextRoleIter1 < factArity; ++ContextRoleIter1)
						{
							primaryRole = factRoles[ContextRoleIter1];
							if (primaryRole != contextPrimaryRole)
							{
								sbTemp.Append(basicRoleReplacements[ContextRoleIter1]);
							}
						}
						snippet2Replace1Replace1Replace2Replace1 = sbTemp.ToString(contextTempStringBuildLength, sbTemp.Length - contextTempStringBuildLength);
						primaryRole = contextPrimaryRole;
						sbTemp.Length = contextTempStringBuildLength;
						sbTemp.Append(snippet2Replace1Replace1Replace2Replace1);
					}
					snippet2Replace1Replace1Replace2Replace1 = sbTemp.ToString();
					snippet2Replace1Replace1Replace2 = string.Format(writer.FormatProvider, snippet2Replace1Replace1ReplaceFormat2, snippet2Replace1Replace1Replace2Replace1);
					snippet2Replace1Replace1 = string.Format(writer.FormatProvider, snippet2Replace1ReplaceFormat1, snippet2Replace1Replace1Replace1, snippet2Replace1Replace1Replace2);
					snippet2Replace1 = string.Format(writer.FormatProvider, snippet2ReplaceFormat1, snippet2Replace1Replace1);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat2, snippet2Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
			}
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
					{
						continue;
					}
					if (firstErrorPending)
					{
						firstErrorPending = false;
						writer.WriteLine();
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
	}
	#endregion // UniquenessConstraint verbalization
	#region FrequencyConstraint verbalization
	public partial class FrequencyConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					if (firstErrorPending)
					{
						firstErrorPending = false;
						verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenPrimaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							writer.WriteLine();
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			bool isDeontic = (this as IConstraint).Modality == ConstraintModality.Deontic;
			StringBuilder sbTemp = null;
			FactType parentFact;
			string predicatePartFormatString;
			LinkedElementCollection<ReadingOrder> allReadingOrders;
			IList<RoleBase> factRoles = null;
			Nullable<int> unaryRoleIndex = null;
			int factArity = 0;
			int unaryRoleOffset = 0;
			LinkedElementCollection<Role> allConstraintRoles = this.RoleCollection;
			LinkedElementCollection<FactType> allFacts = this.FactTypeCollection;
			int allFactsCount = allFacts.Count;
			if (allFactsCount == 0)
			{
				#region Error report
				if (errorOwner != null)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
				}
				#endregion // Error report
				return false;
			}
			string[][] allBasicRoleReplacements = new string[allFactsCount][];
			bool[] unaryReplacements = new bool[allFactsCount];
			int minFactArity = int.MaxValue;
			int maxFactArity = int.MinValue;
			for (int iFact = 0; iFact < allFactsCount; ++iFact)
			{
				FactType currentFact = allFacts[iFact];
				if (currentFact.ReadingRequiredError != null)
				{
					#region Error report
					if (errorOwner != null)
					{
						firstErrorPending = true;
						foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
						{
							ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
							if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
							{
								continue;
							}
							if (firstErrorPending)
							{
								firstErrorPending = false;
								verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
								writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
							}
							else
							{
								writer.WriteLine();
							}
							writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
						}
						if (!firstErrorPending)
						{
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
						}
					}
					#endregion // Error report
					return false;
				}
				allReadingOrders = currentFact.ReadingOrderCollection;
				factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : currentFact.RoleCollection;
				unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
				factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
				unaryRoleOffset = unaryRoleIndex.HasValue ? unaryRoleIndex.Value : 0;
				if (factArity < minFactArity)
				{
					minFactArity = factArity;
				}
				if (factArity > maxFactArity)
				{
					maxFactArity = factArity;
				}
				string[] basicRoleReplacements = new string[factArity];
				bool generateSubscripts = factArity != 2 || this.MinFrequency != 1 || this.MaxFrequency <= 1 || allConstraintRoles.Count != 1 || null == allConstraintRoles[0].FactType.FindMatchingReadingOrder(new RoleBase[]{
					allConstraintRoles[0]});
				for (int i = 0; i < factArity; ++i)
				{
					Role factRole = factRoles[i + unaryRoleOffset].Role;
					ObjectType rolePlayer = factRole.RolePlayer;
					string basicReplacement;
					if (rolePlayer != null)
					{
						int subscript = 0;
						bool useSubscript = false;
						if (generateSubscripts)
						{
							int j = 0;
							for (; j < i; ++j)
							{
								if (rolePlayer == factRoles[j].Role.RolePlayer)
								{
									useSubscript = true;
									++subscript;
								}
							}
							for (j = i + 1; !useSubscript && j < factArity; ++j)
							{
								if (rolePlayer == factRoles[j].Role.RolePlayer)
								{
									useSubscript = true;
								}
							}
						}
						if (useSubscript)
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"), subscript + 1);
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"));
						}
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				allBasicRoleReplacements[iFact] = basicRoleReplacements;
				unaryReplacements[iFact] = unaryRoleIndex.HasValue;
			}
			int constraintRoleArity = allConstraintRoles.Count;
			IReading[] allConstraintRoleReadings = new IReading[constraintRoleArity];
			string[] roleReplacements = new string[maxFactArity];
			IReading reading;
			VerbalizationHyphenBinder hyphenBinder;
			#endregion // Preliminary
			#region Pattern Matches
			if (allFactsCount == 1 && factArity == 2 && constraintRoleArity == 1 && !isNegative && this.MinFrequency == 1)
			{
				parentFact = allFacts[0];
				predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				allReadingOrders = parentFact.ReadingOrderCollection;
				reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, false, false, false, factRoles, false);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				if (reading != null)
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
					{
						RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1], snippet1ReplaceFactRoleIter1);
						if (allConstraintRoles.Contains(currentRole.Role))
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
						}
						else
						{
							string snippet1Replace1Predicate1;
							snippet1Replace1Predicate1 = this.MaxFrequency.ToString(CultureInfo.CurrentCulture);
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.FrequencyRangeMinUnboundedOptimized, isDeontic, isNegative), basicReplacement, snippet1Replace1Predicate1);
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
					}
					snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					if (reading != null)
					{
						verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
						string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace1 = null;
						string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.FrequencyPopulation, isDeontic, isNegative);
						string snippet1Replace1Replace1 = null;
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
							RoleBase primaryRole = allConstraintRoles[RoleIter1];
							parentFact = primaryRole.FactType;
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							sbTemp.Append(allBasicRoleReplacements[0][unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
						}
						snippet1Replace1Replace1 = sbTemp.ToString();
						string snippet1Replace1Replace2 = null;
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, allBasicRoleReplacements[0], true);
						string snippet1Replace1Replace3 = null;
						CoreVerbalizationSnippetType snippet1Replace1ReplaceSnippetType3 = 0;
						if (this.MinFrequency == this.MaxFrequency)
						{
							snippet1Replace1ReplaceSnippetType3 = CoreVerbalizationSnippetType.FrequencyRangeExact;
						}
						else if (this.MaxFrequency == 0)
						{
							snippet1Replace1ReplaceSnippetType3 = CoreVerbalizationSnippetType.FrequencyRangeMaxUnbounded;
						}
						else if (this.MinFrequency == 1)
						{
							snippet1Replace1ReplaceSnippetType3 = CoreVerbalizationSnippetType.FrequencyRangeMinUnbounded;
						}
						else
						{
							snippet1Replace1ReplaceSnippetType3 = CoreVerbalizationSnippetType.FrequencyRangeMinAndMax;
						}
						string snippet1Replace1ReplaceFormat3 = snippets.GetSnippet(snippet1Replace1ReplaceSnippetType3, isDeontic, isNegative);
						string snippet1Replace1Replace3Replace1 = null;
						snippet1Replace1Replace3Replace1 = this.MinFrequency.ToString(CultureInfo.CurrentCulture);
						string snippet1Replace1Replace3Replace2 = null;
						snippet1Replace1Replace3Replace2 = this.MaxFrequency.ToString(CultureInfo.CurrentCulture);
						snippet1Replace1Replace3 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat3, snippet1Replace1Replace3Replace1, snippet1Replace1Replace3Replace2);
						snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2, snippet1Replace1Replace3);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
				}
			}
			else if (allFactsCount == 1 && minFactArity >= 2 && !isNegative)
			{
				parentFact = allFacts[0];
				predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				allReadingOrders = parentFact.ReadingOrderCollection;
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.FrequencyPopulation, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				if (constraintRoleArity == 1)
				{
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
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						sbTemp.Append(allBasicRoleReplacements[0][unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
					}
					snippet1Replace1Replace1 = sbTemp.ToString();
				}
				else
				{
					string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.CombinationIdentifier, isDeontic, isNegative);
					string snippet1Replace1Replace1Replace1 = null;
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
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						CoreVerbalizationSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListOpen;
						}
						else if (RoleIter1 == constraintRoleArity - 1)
						{
							if (RoleIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.CompactSimpleListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.CompactSimpleListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						sbTemp.Append(allBasicRoleReplacements[0][unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
						if (RoleIter1 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompactSimpleListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace1Replace1Replace1 = sbTemp.ToString();
					snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1);
				}
				string snippet1Replace1Replace2 = null;
				reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, allBasicRoleReplacements[0], true);
				string snippet1Replace1Replace3 = null;
				CoreVerbalizationSnippetType snippet1Replace1ReplaceSnippetType3 = 0;
				if (this.MinFrequency == this.MaxFrequency)
				{
					snippet1Replace1ReplaceSnippetType3 = CoreVerbalizationSnippetType.FrequencyRangeExact;
				}
				else if (this.MaxFrequency == 0)
				{
					snippet1Replace1ReplaceSnippetType3 = CoreVerbalizationSnippetType.FrequencyRangeMaxUnbounded;
				}
				else if (this.MinFrequency == 1)
				{
					snippet1Replace1ReplaceSnippetType3 = CoreVerbalizationSnippetType.FrequencyRangeMinUnbounded;
				}
				else
				{
					snippet1Replace1ReplaceSnippetType3 = CoreVerbalizationSnippetType.FrequencyRangeMinAndMax;
				}
				string snippet1Replace1ReplaceFormat3 = snippets.GetSnippet(snippet1Replace1ReplaceSnippetType3, isDeontic, isNegative);
				string snippet1Replace1Replace3Replace1 = null;
				snippet1Replace1Replace3Replace1 = this.MinFrequency.ToString(CultureInfo.CurrentCulture);
				string snippet1Replace1Replace3Replace2 = null;
				snippet1Replace1Replace3Replace2 = this.MaxFrequency.ToString(CultureInfo.CurrentCulture);
				snippet1Replace1Replace3 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat3, snippet1Replace1Replace3Replace1, snippet1Replace1Replace3Replace2);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2, snippet1Replace1Replace3);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
					{
						continue;
					}
					if (firstErrorPending)
					{
						firstErrorPending = false;
						writer.WriteLine();
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
	}
	#endregion // FrequencyConstraint verbalization
	#region RoleValueConstraint verbalization
	public partial class RoleValueConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					if (firstErrorPending)
					{
						firstErrorPending = false;
						verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenPrimaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							writer.WriteLine();
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			Role valueRole = this.Role;
			bool isDeontic = false;
			StringBuilder sbTemp = null;
			FactType parentFact = valueRole.FactType;
			string predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
			IList<Role> includedRoles = new Role[]{
				valueRole};
			LinkedElementCollection<ReadingOrder> allReadingOrders = parentFact.ReadingOrderCollection;
			IList<RoleBase> factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
			Nullable<int> unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
			int factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
			int unaryRoleOffset = unaryRoleIndex.HasValue ? unaryRoleIndex.Value : 0;
			int includedArity = includedRoles.Count;
			if (allReadingOrders.Count == 0 || includedArity == 0)
			{
				#region Error report
				if (errorOwner != null)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
				}
				#endregion // Error report
				return false;
			}
			string[] basicRoleReplacements = new string[factArity];
			for (int i = 0; i < factArity; ++i)
			{
				Role factRole = factRoles[i + unaryRoleOffset].Role;
				ObjectType rolePlayer = factRole.RolePlayer;
				string basicReplacement;
				if (rolePlayer != null)
				{
					basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"));
				}
				else
				{
					basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
				}
				basicRoleReplacements[i] = basicReplacement;
			}
			string[] roleReplacements = new string[factArity];
			IReading reading;
			VerbalizationHyphenBinder hyphenBinder;
			LinkedElementCollection<ValueRange> ranges = this.ValueRangeCollection;
			bool isSingleValue = ranges.Count == 1 && ranges[0].MinValue == ranges[0].MaxValue;
			bool isText = this.IsText;
			#endregion // Preliminary
			#region Pattern Matches
			CoreVerbalizationSnippetType variableSnippetSnippetType1 = 0;
			if (isSingleValue)
			{
				variableSnippetSnippetType1 = CoreVerbalizationSnippetType.SingleValueValueConstraint;
			}
			else
			{
				variableSnippetSnippetType1 = CoreVerbalizationSnippetType.MultiValueValueConstraint;
			}
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			string variableSnippetFormat1 = snippets.GetSnippet(variableSnippetSnippetType1, isDeontic, isNegative);
			string variableSnippet1Replace1 = null;
			if (factArity == 2 && valueRole.Name.Length != 0)
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
					RoleBase primaryRole = includedRoles[RoleIter1];
					string variableSnippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.PeriodSeparator, isDeontic, isNegative);
					string variableSnippet1Replace1Replace1 = null;
					RoleBase contextPrimaryRole = primaryRole;
					int contextTempStringBuildLength = sbTemp.Length;
					for (int ContextRoleIter1 = 0; ContextRoleIter1 < factArity; ++ContextRoleIter1)
					{
						primaryRole = factRoles[ContextRoleIter1];
						if (!includedRoles.Contains(primaryRole.Role))
						{
							sbTemp.Append(basicRoleReplacements[ContextRoleIter1]);
						}
					}
					variableSnippet1Replace1Replace1 = sbTemp.ToString(contextTempStringBuildLength, sbTemp.Length - contextTempStringBuildLength);
					primaryRole = contextPrimaryRole;
					sbTemp.Length = contextTempStringBuildLength;
					string variableSnippet1Replace1Replace2 = null;
					if (primaryRole.Role.RolePlayer != null && 0 != primaryRole.Role.RolePlayer.ReferenceModeString.Length)
					{
						string variableSnippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ReferenceScheme, isDeontic, isNegative);
						string variableSnippet1Replace1Replace2Replace1 = null;
						variableSnippet1Replace1Replace2Replace1 = primaryRole.Role.Name;
						string variableSnippet1Replace1Replace2Replace2 = null;
						variableSnippet1Replace1Replace2Replace2 = primaryRole.Role.RolePlayer.ReferenceModeDecoratedString;
						variableSnippet1Replace1Replace2 = string.Format(writer.FormatProvider, variableSnippet1Replace1ReplaceFormat2, variableSnippet1Replace1Replace2Replace1, variableSnippet1Replace1Replace2Replace2);
					}
					else
					{
						variableSnippet1Replace1Replace2 = primaryRole.Role.Name;
					}
					variableSnippet1Replace1 = string.Format(writer.FormatProvider, variableSnippet1ReplaceFormat1, variableSnippet1Replace1Replace1, variableSnippet1Replace1Replace2);
					sbTemp.Append(variableSnippet1Replace1);
				}
				variableSnippet1Replace1 = sbTemp.ToString();
			}
			else
			{
				reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				string variableSnippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.InQuantifier, isDeontic, isNegative);
				string variableSnippet1Replace1Replace1 = null;
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
					int ResolvedRoleIndex1 = FactType.IndexOfRole(factRoles, includedRoles[RoleIter1]);
					sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[ResolvedRoleIndex1], ResolvedRoleIndex1));
				}
				variableSnippet1Replace1Replace1 = sbTemp.ToString();
				string variableSnippet1Replace1Replace2 = null;
				variableSnippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, basicRoleReplacements, true);
				variableSnippet1Replace1 = string.Format(writer.FormatProvider, variableSnippet1ReplaceFormat1, variableSnippet1Replace1Replace1, variableSnippet1Replace1Replace2);
			}
			string variableSnippet1Replace2 = null;
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
				CoreVerbalizationSnippetType listSnippet;
				if (i == 0)
				{
					listSnippet = CoreVerbalizationSnippetType.CompactSimpleListOpen;
				}
				else if (i == rangeCount - 1)
				{
					if (i == 1)
					{
						listSnippet = CoreVerbalizationSnippetType.CompactSimpleListPairSeparator;
					}
					else
					{
						listSnippet = CoreVerbalizationSnippetType.CompactSimpleListFinalSeparator;
					}
				}
				else
				{
					listSnippet = CoreVerbalizationSnippetType.CompactSimpleListSeparator;
				}
				sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
				CoreVerbalizationSnippetType variableSnippet1ReplaceSnippetType2 = 0;
				if (minValue == maxValue)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.SelfReference;
				}
				else if (minInclusion != RangeInclusion.Open && maxValue.Length == 0)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinClosedMaxUnbounded;
				}
				else if (minInclusion == RangeInclusion.Open && maxValue.Length == 0)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinOpenMaxUnbounded;
				}
				else if (minValue.Length == 0 && maxInclusion != RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinUnboundedMaxClosed;
				}
				else if (minValue.Length == 0 && maxInclusion == RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinUnboundedMaxOpen;
				}
				else if (minInclusion != RangeInclusion.Open && maxInclusion != RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinClosedMaxClosed;
				}
				else if (minInclusion != RangeInclusion.Open && maxInclusion == RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinClosedMaxOpen;
				}
				else if (minInclusion == RangeInclusion.Open && maxInclusion != RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinOpenMaxClosed;
				}
				else if (minInclusion == RangeInclusion.Open && maxInclusion == RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinOpenMaxOpen;
				}
				string variableSnippet1ReplaceFormat2 = snippets.GetSnippet(variableSnippet1ReplaceSnippetType2, isDeontic, isNegative);
				string variableSnippet1Replace2Replace1 = null;
				CoreVerbalizationSnippetType variableSnippet1Replace2ReplaceSnippetType1 = 0;
				if (isText)
				{
					variableSnippet1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.TextInstanceValue;
				}
				else
				{
					variableSnippet1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.NonTextInstanceValue;
				}
				string variableSnippet1Replace2ReplaceFormat1 = snippets.GetSnippet(variableSnippet1Replace2ReplaceSnippetType1, isDeontic, isNegative);
				string variableSnippet1Replace2Replace1Replace1 = null;
				variableSnippet1Replace2Replace1Replace1 = minValue;
				variableSnippet1Replace2Replace1 = string.Format(writer.FormatProvider, variableSnippet1Replace2ReplaceFormat1, variableSnippet1Replace2Replace1Replace1);
				string variableSnippet1Replace2Replace2 = null;
				CoreVerbalizationSnippetType variableSnippet1Replace2ReplaceSnippetType2 = 0;
				if (isText)
				{
					variableSnippet1Replace2ReplaceSnippetType2 = CoreVerbalizationSnippetType.TextInstanceValue;
				}
				else
				{
					variableSnippet1Replace2ReplaceSnippetType2 = CoreVerbalizationSnippetType.NonTextInstanceValue;
				}
				string variableSnippet1Replace2ReplaceFormat2 = snippets.GetSnippet(variableSnippet1Replace2ReplaceSnippetType2, isDeontic, isNegative);
				string variableSnippet1Replace2Replace2Replace1 = null;
				variableSnippet1Replace2Replace2Replace1 = maxValue;
				variableSnippet1Replace2Replace2 = string.Format(writer.FormatProvider, variableSnippet1Replace2ReplaceFormat2, variableSnippet1Replace2Replace2Replace1);
				variableSnippet1Replace2 = string.Format(writer.FormatProvider, variableSnippet1ReplaceFormat2, variableSnippet1Replace2Replace1, variableSnippet1Replace2Replace2);
				sbTemp.Append(variableSnippet1Replace2);
				if (i == rangeCount - 1)
				{
					sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompactSimpleListClose, isDeontic, isNegative));
				}
			}
			variableSnippet1Replace2 = sbTemp.ToString();
			FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, variableSnippetFormat1, variableSnippet1Replace1, variableSnippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
					{
						continue;
					}
					if (firstErrorPending)
					{
						firstErrorPending = false;
						writer.WriteLine();
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
	}
	#endregion // RoleValueConstraint verbalization
	#region ValueTypeValueConstraint verbalization
	public partial class ValueTypeValueConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					if (firstErrorPending)
					{
						firstErrorPending = false;
						verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenPrimaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							writer.WriteLine();
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			const bool isDeontic = false;
			StringBuilder sbTemp = null;
			LinkedElementCollection<ValueRange> ranges = this.ValueRangeCollection;
			bool isSingleValue = ranges.Count == 1 && ranges[0].MinValue == ranges[0].MaxValue;
			bool isText = this.IsText;
			#endregion // Preliminary
			#region Pattern Matches
			CoreVerbalizationSnippetType variableSnippetSnippetType1 = 0;
			if (isSingleValue)
			{
				variableSnippetSnippetType1 = CoreVerbalizationSnippetType.SingleValueValueConstraint;
			}
			else
			{
				variableSnippetSnippetType1 = CoreVerbalizationSnippetType.MultiValueValueConstraint;
			}
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			string variableSnippetFormat1 = snippets.GetSnippet(variableSnippetSnippetType1, isDeontic, isNegative);
			string variableSnippet1Replace1 = null;
			string variableSnippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative);
			string variableSnippet1Replace1Replace1 = null;
			variableSnippet1Replace1Replace1 = this.ValueType.Name;
			string variableSnippet1Replace1Replace2 = null;
			variableSnippet1Replace1Replace2 = this.ValueType.Id.ToString("D");
			variableSnippet1Replace1 = string.Format(writer.FormatProvider, variableSnippet1ReplaceFormat1, variableSnippet1Replace1Replace1, variableSnippet1Replace1Replace2);
			string variableSnippet1Replace2 = null;
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
				CoreVerbalizationSnippetType listSnippet;
				if (i == 0)
				{
					listSnippet = CoreVerbalizationSnippetType.CompactSimpleListOpen;
				}
				else if (i == rangeCount - 1)
				{
					if (i == 1)
					{
						listSnippet = CoreVerbalizationSnippetType.CompactSimpleListPairSeparator;
					}
					else
					{
						listSnippet = CoreVerbalizationSnippetType.CompactSimpleListFinalSeparator;
					}
				}
				else
				{
					listSnippet = CoreVerbalizationSnippetType.CompactSimpleListSeparator;
				}
				sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
				CoreVerbalizationSnippetType variableSnippet1ReplaceSnippetType2 = 0;
				if (minValue == maxValue)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.SelfReference;
				}
				else if (minInclusion != RangeInclusion.Open && maxValue.Length == 0)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinClosedMaxUnbounded;
				}
				else if (minInclusion == RangeInclusion.Open && maxValue.Length == 0)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinOpenMaxUnbounded;
				}
				else if (minValue.Length == 0 && maxInclusion != RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinUnboundedMaxClosed;
				}
				else if (minValue.Length == 0 && maxInclusion == RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinUnboundedMaxOpen;
				}
				else if (minInclusion != RangeInclusion.Open && maxInclusion != RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinClosedMaxClosed;
				}
				else if (minInclusion != RangeInclusion.Open && maxInclusion == RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinClosedMaxOpen;
				}
				else if (minInclusion == RangeInclusion.Open && maxInclusion != RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinOpenMaxClosed;
				}
				else if (minInclusion == RangeInclusion.Open && maxInclusion == RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinOpenMaxOpen;
				}
				string variableSnippet1ReplaceFormat2 = snippets.GetSnippet(variableSnippet1ReplaceSnippetType2, isDeontic, isNegative);
				string variableSnippet1Replace2Replace1 = null;
				CoreVerbalizationSnippetType variableSnippet1Replace2ReplaceSnippetType1 = 0;
				if (isText)
				{
					variableSnippet1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.TextInstanceValue;
				}
				else
				{
					variableSnippet1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.NonTextInstanceValue;
				}
				string variableSnippet1Replace2ReplaceFormat1 = snippets.GetSnippet(variableSnippet1Replace2ReplaceSnippetType1, isDeontic, isNegative);
				string variableSnippet1Replace2Replace1Replace1 = null;
				variableSnippet1Replace2Replace1Replace1 = minValue;
				variableSnippet1Replace2Replace1 = string.Format(writer.FormatProvider, variableSnippet1Replace2ReplaceFormat1, variableSnippet1Replace2Replace1Replace1);
				string variableSnippet1Replace2Replace2 = null;
				CoreVerbalizationSnippetType variableSnippet1Replace2ReplaceSnippetType2 = 0;
				if (isText)
				{
					variableSnippet1Replace2ReplaceSnippetType2 = CoreVerbalizationSnippetType.TextInstanceValue;
				}
				else
				{
					variableSnippet1Replace2ReplaceSnippetType2 = CoreVerbalizationSnippetType.NonTextInstanceValue;
				}
				string variableSnippet1Replace2ReplaceFormat2 = snippets.GetSnippet(variableSnippet1Replace2ReplaceSnippetType2, isDeontic, isNegative);
				string variableSnippet1Replace2Replace2Replace1 = null;
				variableSnippet1Replace2Replace2Replace1 = maxValue;
				variableSnippet1Replace2Replace2 = string.Format(writer.FormatProvider, variableSnippet1Replace2ReplaceFormat2, variableSnippet1Replace2Replace2Replace1);
				variableSnippet1Replace2 = string.Format(writer.FormatProvider, variableSnippet1ReplaceFormat2, variableSnippet1Replace2Replace1, variableSnippet1Replace2Replace2);
				sbTemp.Append(variableSnippet1Replace2);
				if (i == rangeCount - 1)
				{
					sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompactSimpleListClose, isDeontic, isNegative));
				}
			}
			variableSnippet1Replace2 = sbTemp.ToString();
			FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, variableSnippetFormat1, variableSnippet1Replace1, variableSnippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
					{
						continue;
					}
					if (firstErrorPending)
					{
						firstErrorPending = false;
						writer.WriteLine();
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
	}
	#endregion // ValueTypeValueConstraint verbalization
	#region ObjectType.NearestValueConstraintVerbalizer verbalization
	public partial class ObjectType
	{
		private partial class NearestValueConstraintVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static NearestValueConstraintVerbalizer myCache;
			public static NearestValueConstraintVerbalizer GetVerbalizer()
			{
				NearestValueConstraintVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<NearestValueConstraintVerbalizer>(ref myCache, null as NearestValueConstraintVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new NearestValueConstraintVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<NearestValueConstraintVerbalizer>(ref myCache, this, null as NearestValueConstraintVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isDeontic = false;
				StringBuilder sbTemp = null;
				LinkedElementCollection<ValueRange> ranges = this.ValueRangeCollection;
				bool isSingleValue = ranges.Count == 1 && ranges[0].MinValue == ranges[0].MaxValue;
				bool isText = this.IsText;
				#endregion // Preliminary
				#region Pattern Matches
				CoreVerbalizationSnippetType variableSnippetSnippetType1 = 0;
				if (isSingleValue)
				{
					variableSnippetSnippetType1 = CoreVerbalizationSnippetType.SingleValueValueConstraint;
				}
				else
				{
					variableSnippetSnippetType1 = CoreVerbalizationSnippetType.MultiValueValueConstraint;
				}
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string variableSnippetFormat1 = snippets.GetSnippet(variableSnippetSnippetType1, isDeontic, isNegative);
				string variableSnippet1Replace1 = null;
				string variableSnippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative);
				string variableSnippet1Replace1Replace1 = null;
				variableSnippet1Replace1Replace1 = this.Name;
				string variableSnippet1Replace1Replace2 = null;
				variableSnippet1Replace1Replace2 = this.Id.ToString("D");
				variableSnippet1Replace1 = string.Format(writer.FormatProvider, variableSnippet1ReplaceFormat1, variableSnippet1Replace1Replace1, variableSnippet1Replace1Replace2);
				string variableSnippet1Replace2 = null;
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
					CoreVerbalizationSnippetType listSnippet;
					if (i == 0)
					{
						listSnippet = CoreVerbalizationSnippetType.CompactSimpleListOpen;
					}
					else if (i == rangeCount - 1)
					{
						if (i == 1)
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.CompactSimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationSnippetType.CompactSimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					CoreVerbalizationSnippetType variableSnippet1ReplaceSnippetType2 = 0;
					if (minValue == maxValue)
					{
						variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.SelfReference;
					}
					else if (minInclusion != RangeInclusion.Open && maxValue.Length == 0)
					{
						variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinClosedMaxUnbounded;
					}
					else if (minInclusion == RangeInclusion.Open && maxValue.Length == 0)
					{
						variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinOpenMaxUnbounded;
					}
					else if (minValue.Length == 0 && maxInclusion != RangeInclusion.Open)
					{
						variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinUnboundedMaxClosed;
					}
					else if (minValue.Length == 0 && maxInclusion == RangeInclusion.Open)
					{
						variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinUnboundedMaxOpen;
					}
					else if (minInclusion != RangeInclusion.Open && maxInclusion != RangeInclusion.Open)
					{
						variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinClosedMaxClosed;
					}
					else if (minInclusion != RangeInclusion.Open && maxInclusion == RangeInclusion.Open)
					{
						variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinClosedMaxOpen;
					}
					else if (minInclusion == RangeInclusion.Open && maxInclusion != RangeInclusion.Open)
					{
						variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinOpenMaxClosed;
					}
					else if (minInclusion == RangeInclusion.Open && maxInclusion == RangeInclusion.Open)
					{
						variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinOpenMaxOpen;
					}
					string variableSnippet1ReplaceFormat2 = snippets.GetSnippet(variableSnippet1ReplaceSnippetType2, isDeontic, isNegative);
					string variableSnippet1Replace2Replace1 = null;
					CoreVerbalizationSnippetType variableSnippet1Replace2ReplaceSnippetType1 = 0;
					if (isText)
					{
						variableSnippet1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.TextInstanceValue;
					}
					else
					{
						variableSnippet1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.NonTextInstanceValue;
					}
					string variableSnippet1Replace2ReplaceFormat1 = snippets.GetSnippet(variableSnippet1Replace2ReplaceSnippetType1, isDeontic, isNegative);
					string variableSnippet1Replace2Replace1Replace1 = null;
					variableSnippet1Replace2Replace1Replace1 = minValue;
					variableSnippet1Replace2Replace1 = string.Format(writer.FormatProvider, variableSnippet1Replace2ReplaceFormat1, variableSnippet1Replace2Replace1Replace1);
					string variableSnippet1Replace2Replace2 = null;
					CoreVerbalizationSnippetType variableSnippet1Replace2ReplaceSnippetType2 = 0;
					if (isText)
					{
						variableSnippet1Replace2ReplaceSnippetType2 = CoreVerbalizationSnippetType.TextInstanceValue;
					}
					else
					{
						variableSnippet1Replace2ReplaceSnippetType2 = CoreVerbalizationSnippetType.NonTextInstanceValue;
					}
					string variableSnippet1Replace2ReplaceFormat2 = snippets.GetSnippet(variableSnippet1Replace2ReplaceSnippetType2, isDeontic, isNegative);
					string variableSnippet1Replace2Replace2Replace1 = null;
					variableSnippet1Replace2Replace2Replace1 = maxValue;
					variableSnippet1Replace2Replace2 = string.Format(writer.FormatProvider, variableSnippet1Replace2ReplaceFormat2, variableSnippet1Replace2Replace2Replace1);
					variableSnippet1Replace2 = string.Format(writer.FormatProvider, variableSnippet1ReplaceFormat2, variableSnippet1Replace2Replace1, variableSnippet1Replace2Replace2);
					sbTemp.Append(variableSnippet1Replace2);
					if (i == rangeCount - 1)
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompactSimpleListClose, isDeontic, isNegative));
					}
				}
				variableSnippet1Replace2 = sbTemp.ToString();
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, variableSnippetFormat1, variableSnippet1Replace1, variableSnippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
			}
		}
	}
	#endregion // ObjectType.NearestValueConstraintVerbalizer verbalization
	#region RingConstraint verbalization
	public partial class RingConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					if (firstErrorPending)
					{
						firstErrorPending = false;
						verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenPrimaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							writer.WriteLine();
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			bool isDeontic = (this as IConstraint).Modality == ConstraintModality.Deontic;
			StringBuilder sbTemp = null;
			FactType parentFact;
			string predicatePartFormatString;
			LinkedElementCollection<ReadingOrder> allReadingOrders;
			IList<RoleBase> factRoles = null;
			Nullable<int> unaryRoleIndex = null;
			int factArity = 0;
			int unaryRoleOffset = 0;
			LinkedElementCollection<Role> allConstraintRoles = this.RoleCollection;
			LinkedElementCollection<FactType> allFacts = this.FactTypeCollection;
			int allFactsCount = allFacts.Count;
			if (allFactsCount == 0)
			{
				#region Error report
				if (errorOwner != null)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
				}
				#endregion // Error report
				return false;
			}
			string[][] allBasicRoleReplacements = new string[allFactsCount][];
			bool[] unaryReplacements = new bool[allFactsCount];
			int minFactArity = int.MaxValue;
			int maxFactArity = int.MinValue;
			for (int iFact = 0; iFact < allFactsCount; ++iFact)
			{
				FactType currentFact = allFacts[iFact];
				if (currentFact.ReadingRequiredError != null)
				{
					#region Error report
					if (errorOwner != null)
					{
						firstErrorPending = true;
						foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
						{
							ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
							if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
							{
								continue;
							}
							if (firstErrorPending)
							{
								firstErrorPending = false;
								verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
								writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
							}
							else
							{
								writer.WriteLine();
							}
							writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
						}
						if (!firstErrorPending)
						{
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
						}
					}
					#endregion // Error report
					return false;
				}
				allReadingOrders = currentFact.ReadingOrderCollection;
				factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : currentFact.RoleCollection;
				unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
				factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
				unaryRoleOffset = unaryRoleIndex.HasValue ? unaryRoleIndex.Value : 0;
				if (factArity < minFactArity)
				{
					minFactArity = factArity;
				}
				if (factArity > maxFactArity)
				{
					maxFactArity = factArity;
				}
				string[] basicRoleReplacements = new string[factArity];
				bool generateSubscripts = factArity >= 3;
				for (int i = 0; i < factArity; ++i)
				{
					Role factRole = factRoles[i + unaryRoleOffset].Role;
					ObjectType rolePlayer = factRole.RolePlayer;
					string basicReplacement;
					if (rolePlayer != null)
					{
						int subscript = 0;
						bool useSubscript = false;
						if (generateSubscripts)
						{
							int j = 0;
							for (; j < i; ++j)
							{
								if (rolePlayer == factRoles[j].Role.RolePlayer)
								{
									useSubscript = true;
									++subscript;
								}
							}
							for (j = i + 1; !useSubscript && j < factArity; ++j)
							{
								if (rolePlayer == factRoles[j].Role.RolePlayer)
								{
									useSubscript = true;
								}
							}
						}
						if (useSubscript)
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"), subscript + 1);
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"));
						}
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				allBasicRoleReplacements[iFact] = basicRoleReplacements;
				unaryReplacements[iFact] = unaryRoleIndex.HasValue;
			}
			int constraintRoleArity = allConstraintRoles.Count;
			IReading[] allConstraintRoleReadings = new IReading[constraintRoleArity];
			string[] roleReplacements = new string[maxFactArity];
			IReading reading;
			VerbalizationHyphenBinder hyphenBinder;
			#endregion // Preliminary
			#region Pattern Matches
			if (constraintRoleArity == 2 && allFactsCount == 1 && this.RingType == RingConstraintType.Irreflexive)
			{
				parentFact = allFacts[0];
				predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				allReadingOrders = parentFact.ReadingOrderCollection;
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
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
					RoleBase primaryRole = allConstraintRoles[RoleIter1];
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, false, false, false, factRoles, true);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
					{
						RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1], snippet1ReplaceFactRoleIter1);
						if (primaryRole == currentRole)
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, !isNegative), basicReplacement);
						}
						else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative), basicReplacement);
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
					}
					snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
					sbTemp.Append(snippet1Replace1);
				}
				snippet1Replace1 = sbTemp.ToString();
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
					{
						continue;
					}
					if (firstErrorPending)
					{
						firstErrorPending = false;
						writer.WriteLine();
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
	}
	#endregion // RingConstraint verbalization
	#region EqualityConstraint verbalization
	public partial class EqualityConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					if (firstErrorPending)
					{
						firstErrorPending = false;
						verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenPrimaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							writer.WriteLine();
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			bool isDeontic = (this as IConstraint).Modality == ConstraintModality.Deontic;
			StringBuilder sbTemp = null;
			FactType parentFact;
			string predicatePartFormatString;
			LinkedElementCollection<ReadingOrder> allReadingOrders;
			IList<RoleBase> factRoles = null;
			Nullable<int> unaryRoleIndex = null;
			int factArity = 0;
			int unaryRoleOffset = 0;
			LinkedElementCollection<SetComparisonConstraintRoleSequence> constraintSequences = this.RoleSequenceCollection;
			int constraintRoleArity = constraintSequences.Count;
			IList<Role>[] allConstraintSequences = new IList<Role>[constraintRoleArity];
			for (int i = 0; i < constraintRoleArity; ++i)
			{
				allConstraintSequences[i] = constraintSequences[i].RoleCollection;
			}
			int columnArity = allConstraintSequences[0].Count;
			LinkedElementCollection<FactType> allFacts = this.FactTypeCollection;
			int allFactsCount = allFacts.Count;
			if (allFactsCount == 0)
			{
				#region Error report
				if (errorOwner != null)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
				}
				#endregion // Error report
				return false;
			}
			string[][] allBasicRoleReplacements = new string[allFactsCount][];
			bool[] unaryReplacements = new bool[allFactsCount];
			int contextBasicReplacementIndex;
			int minFactArity = int.MaxValue;
			int maxFactArity = int.MinValue;
			for (int iFact = 0; iFact < allFactsCount; ++iFact)
			{
				FactType currentFact = allFacts[iFact];
				if (currentFact.ReadingRequiredError != null)
				{
					#region Error report
					if (errorOwner != null)
					{
						firstErrorPending = true;
						foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
						{
							ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
							if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
							{
								continue;
							}
							if (firstErrorPending)
							{
								firstErrorPending = false;
								verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
								writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
							}
							else
							{
								writer.WriteLine();
							}
							writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
						}
						if (!firstErrorPending)
						{
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
						}
					}
					#endregion // Error report
					return false;
				}
				allReadingOrders = currentFact.ReadingOrderCollection;
				factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : currentFact.RoleCollection;
				unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
				factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
				unaryRoleOffset = unaryRoleIndex.HasValue ? unaryRoleIndex.Value : 0;
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
					Role factRole = factRoles[i + unaryRoleOffset].Role;
					ObjectType rolePlayer = factRole.RolePlayer;
					string basicReplacement;
					if (rolePlayer != null)
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"));
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				allBasicRoleReplacements[iFact] = basicRoleReplacements;
				unaryReplacements[iFact] = unaryRoleIndex.HasValue;
			}
			IReading[] allConstraintRoleReadings = new IReading[constraintRoleArity];
			string[] roleReplacements = new string[maxFactArity];
			IReading reading;
			VerbalizationHyphenBinder hyphenBinder;
			#endregion // Preliminary
			#region Pattern Matches
			if (columnArity == 1 && constraintRoleArity == 2 && !isNegative)
			{
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachIndentedQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				int snippet1Replace1ReplaceSequenceIter1;
				for (snippet1Replace1ReplaceSequenceIter1 = 0; snippet1Replace1ReplaceSequenceIter1 < 1; ++snippet1Replace1ReplaceSequenceIter1)
				{
					IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1ReplaceSequenceIter1];
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
						RoleBase primaryRole = includedSequenceRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						sbTemp.Append(basicRoleReplacements[unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
					}
					snippet1Replace1Replace1 = sbTemp.ToString();
				}
				string snippet1Replace1Replace2 = null;
				string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.Equality, isDeontic, isNegative);
				string snippet1Replace1Replace2Replace1 = null;
				int snippet1Replace1Replace2ReplaceSequenceIter1;
				for (snippet1Replace1Replace2ReplaceSequenceIter1 = 0; snippet1Replace1Replace2ReplaceSequenceIter1 < 1; ++snippet1Replace1Replace2ReplaceSequenceIter1)
				{
					IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
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
						RoleBase primaryRole = includedSequenceRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						factArity = factRoles.Count;
						reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, false, factRoles, true);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						for (int snippet1Replace1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace1Replace2ReplaceFactRoleIter1)
						{
							RoleBase currentRole = factRoles[snippet1Replace1Replace2ReplaceFactRoleIter1];
							string roleReplacement = null;
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[allFacts.IndexOf(includedSequenceRoles[RoleIter1].FactType)][snippet1Replace1Replace2ReplaceFactRoleIter1], snippet1Replace1Replace2ReplaceFactRoleIter1);
							if (includedSequenceRoles.Contains(currentRole.Role))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1Replace1Replace2ReplaceFactRoleIter1] = roleReplacement;
						}
						snippet1Replace1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
						sbTemp.Append(snippet1Replace1Replace2Replace1);
					}
					snippet1Replace1Replace2Replace1 = sbTemp.ToString();
				}
				string snippet1Replace1Replace2Replace2 = null;
				int snippet1Replace1Replace2ReplaceSequenceIter2;
				for (snippet1Replace1Replace2ReplaceSequenceIter2 = 1; snippet1Replace1Replace2ReplaceSequenceIter2 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceSequenceIter2)
				{
					IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace2ReplaceSequenceIter2];
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int RoleIter2 = 0; RoleIter2 < 1; ++RoleIter2)
					{
						RoleBase primaryRole = includedSequenceRoles[RoleIter2];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						factArity = factRoles.Count;
						reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, false, factRoles, true);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						for (int snippet1Replace1Replace2ReplaceFactRoleIter2 = 0; snippet1Replace1Replace2ReplaceFactRoleIter2 < factArity; ++snippet1Replace1Replace2ReplaceFactRoleIter2)
						{
							RoleBase currentRole = factRoles[snippet1Replace1Replace2ReplaceFactRoleIter2];
							string roleReplacement = null;
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[allFacts.IndexOf(includedSequenceRoles[RoleIter2].FactType)][snippet1Replace1Replace2ReplaceFactRoleIter2], snippet1Replace1Replace2ReplaceFactRoleIter2);
							if (includedSequenceRoles.Contains(currentRole.Role))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1Replace1Replace2ReplaceFactRoleIter2] = roleReplacement;
						}
						snippet1Replace1Replace2Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
						sbTemp.Append(snippet1Replace1Replace2Replace2);
					}
					snippet1Replace1Replace2Replace2 = sbTemp.ToString();
				}
				snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1, snippet1Replace1Replace2Replace2);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (constraintRoleArity == 2 && !isNegative)
			{
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachIndentedQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				int snippet1Replace1ReplaceSequenceIter1;
				for (snippet1Replace1ReplaceSequenceIter1 = 0; snippet1Replace1ReplaceSequenceIter1 < 1; ++snippet1Replace1ReplaceSequenceIter1)
				{
					IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1ReplaceSequenceIter1];
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
					{
						RoleBase primaryRole = includedSequenceRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						CoreVerbalizationSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListOpen;
						}
						else if (RoleIter1 == columnArity - 1)
						{
							if (RoleIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						sbTemp.Append(basicRoleReplacements[unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
						if (RoleIter1 == columnArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleLogicalAndListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace1Replace1 = sbTemp.ToString();
				}
				string snippet1Replace1Replace2 = null;
				string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.Equality, isDeontic, isNegative);
				string snippet1Replace1Replace2Replace1 = null;
				int snippet1Replace1Replace2ReplaceSequenceIter1;
				for (snippet1Replace1Replace2ReplaceSequenceIter1 = 0; snippet1Replace1Replace2ReplaceSequenceIter1 < 1; ++snippet1Replace1Replace2ReplaceSequenceIter1)
				{
					IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
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
						RoleBase primaryRole = includedSequenceRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						factArity = factRoles.Count;
						reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, false, factRoles, true);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						for (int snippet1Replace1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace1Replace2ReplaceFactRoleIter1)
						{
							RoleBase currentRole = factRoles[snippet1Replace1Replace2ReplaceFactRoleIter1];
							string roleReplacement = null;
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[allFacts.IndexOf(includedSequenceRoles[RoleIter1].FactType)][snippet1Replace1Replace2ReplaceFactRoleIter1], snippet1Replace1Replace2ReplaceFactRoleIter1);
							if (includedSequenceRoles.Contains(currentRole.Role))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1Replace1Replace2ReplaceFactRoleIter1] = roleReplacement;
						}
						snippet1Replace1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
						sbTemp.Append(snippet1Replace1Replace2Replace1);
					}
					snippet1Replace1Replace2Replace1 = sbTemp.ToString();
				}
				string snippet1Replace1Replace2Replace2 = null;
				int snippet1Replace1Replace2ReplaceSequenceIter2;
				for (snippet1Replace1Replace2ReplaceSequenceIter2 = 1; snippet1Replace1Replace2ReplaceSequenceIter2 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceSequenceIter2)
				{
					IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace2ReplaceSequenceIter2];
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int RoleIter2 = 0; RoleIter2 < 1; ++RoleIter2)
					{
						RoleBase primaryRole = includedSequenceRoles[RoleIter2];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						factArity = factRoles.Count;
						reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, false, factRoles, true);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						for (int snippet1Replace1Replace2ReplaceFactRoleIter2 = 0; snippet1Replace1Replace2ReplaceFactRoleIter2 < factArity; ++snippet1Replace1Replace2ReplaceFactRoleIter2)
						{
							RoleBase currentRole = factRoles[snippet1Replace1Replace2ReplaceFactRoleIter2];
							string roleReplacement = null;
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[allFacts.IndexOf(includedSequenceRoles[RoleIter2].FactType)][snippet1Replace1Replace2ReplaceFactRoleIter2], snippet1Replace1Replace2ReplaceFactRoleIter2);
							if (includedSequenceRoles.Contains(currentRole.Role))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1Replace1Replace2ReplaceFactRoleIter2] = roleReplacement;
						}
						snippet1Replace1Replace2Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
						sbTemp.Append(snippet1Replace1Replace2Replace2);
					}
					snippet1Replace1Replace2Replace2 = sbTemp.ToString();
				}
				snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1, snippet1Replace1Replace2Replace2);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (constraintRoleArity >= 3 && !isNegative)
			{
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				int snippet1Replace1ReplaceSequenceIter1;
				for (snippet1Replace1ReplaceSequenceIter1 = 0; snippet1Replace1ReplaceSequenceIter1 < 1; ++snippet1Replace1ReplaceSequenceIter1)
				{
					IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1ReplaceSequenceIter1];
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
					{
						RoleBase primaryRole = includedSequenceRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						CoreVerbalizationSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListOpen;
						}
						else if (RoleIter1 == columnArity - 1)
						{
							if (RoleIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						sbTemp.Append(basicRoleReplacements[unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
						if (RoleIter1 == columnArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleLogicalAndListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace1Replace1 = sbTemp.ToString();
				}
				string snippet1Replace1Replace2 = null;
				string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.GroupEquality, isDeontic, isNegative);
				string snippet1Replace1Replace2Replace1 = null;
				int snippet1Replace1Replace2ReplaceSequenceIter1;
				int snippet1Replace1Replace2ReplaceCompositeCount1 = 0;
				int snippet1Replace1Replace2ReplaceCompositeIterator1;
				FactType[] snippet1Replace1Replace2Replace1ItemUniqueFactTypes1 = new FactType[columnArity];
				FactType snippet1Replace1Replace2Replace1ItemTestUniqueFactType1;
				for (snippet1Replace1Replace2ReplaceSequenceIter1 = 0; snippet1Replace1Replace2ReplaceSequenceIter1 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceSequenceIter1)
				{
					IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
					for (snippet1Replace1Replace2ReplaceCompositeIterator1 = 0; snippet1Replace1Replace2ReplaceCompositeIterator1 < columnArity; ++snippet1Replace1Replace2ReplaceCompositeIterator1)
					{
						RoleBase primaryRole = includedSequenceRoles[snippet1Replace1Replace2ReplaceCompositeIterator1];
						if (Array.IndexOf(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, snippet1Replace1Replace2Replace1ItemTestUniqueFactType1 = primaryRole.FactType) == -1)
						{
							snippet1Replace1Replace2Replace1ItemUniqueFactTypes1[snippet1Replace1Replace2ReplaceCompositeIterator1] = snippet1Replace1Replace2Replace1ItemTestUniqueFactType1;
							++snippet1Replace1Replace2ReplaceCompositeCount1;
						}
					}
					Array.Clear(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, 0, snippet1Replace1Replace2Replace1ItemUniqueFactTypes1.Length);
				}
				snippet1Replace1Replace2ReplaceCompositeIterator1 = 0;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (snippet1Replace1Replace2ReplaceSequenceIter1 = 0; snippet1Replace1Replace2ReplaceSequenceIter1 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceSequenceIter1)
				{
					IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
					string snippet1Replace1Replace2Replace1Item1;
					for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
					{
						RoleBase primaryRole = includedSequenceRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						factArity = factRoles.Count;
						if (Array.IndexOf(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, snippet1Replace1Replace2Replace1ItemTestUniqueFactType1 = primaryRole.FactType) == -1)
						{
							snippet1Replace1Replace2Replace1ItemUniqueFactTypes1[RoleIter1] = snippet1Replace1Replace2Replace1ItemTestUniqueFactType1;
							CoreVerbalizationSnippetType listSnippet;
							if (snippet1Replace1Replace2ReplaceCompositeIterator1 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.MultilineIndentedCompoundListOpen;
							}
							else if (snippet1Replace1Replace2ReplaceCompositeIterator1 == snippet1Replace1Replace2ReplaceCompositeCount1 - 1)
							{
								if (snippet1Replace1Replace2ReplaceCompositeIterator1 == 1)
								{
									listSnippet = CoreVerbalizationSnippetType.MultilineIndentedCompoundListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.MultilineIndentedCompoundListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.MultilineIndentedCompoundListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, false, factRoles, true);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1Replace2Replace1ItemFactRoleIter1 = 0; snippet1Replace1Replace2Replace1ItemFactRoleIter1 < factArity; ++snippet1Replace1Replace2Replace1ItemFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace1Replace2Replace1ItemFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[allFacts.IndexOf(includedSequenceRoles[RoleIter1].FactType)][snippet1Replace1Replace2Replace1ItemFactRoleIter1], snippet1Replace1Replace2Replace1ItemFactRoleIter1);
								if (includedSequenceRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								}
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace1Replace2Replace1ItemFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1Replace2Replace1Item1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace2Replace1Item1);
							if (snippet1Replace1Replace2ReplaceCompositeIterator1 == snippet1Replace1Replace2ReplaceCompositeCount1 - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.MultilineIndentedCompoundListClose, isDeontic, isNegative));
							}
							++snippet1Replace1Replace2ReplaceCompositeIterator1;
						}
					}
				}
				snippet1Replace1Replace2Replace1 = sbTemp.ToString();
				snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
					{
						continue;
					}
					if (firstErrorPending)
					{
						firstErrorPending = false;
						writer.WriteLine();
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
	}
	#endregion // EqualityConstraint verbalization
	#region ExclusionConstraint verbalization
	public partial class ExclusionConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					if (firstErrorPending)
					{
						firstErrorPending = false;
						verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenPrimaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							writer.WriteLine();
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			bool isDeontic = (this as IConstraint).Modality == ConstraintModality.Deontic;
			StringBuilder sbTemp = null;
			FactType parentFact;
			string predicatePartFormatString;
			LinkedElementCollection<ReadingOrder> allReadingOrders;
			IList<RoleBase> factRoles = null;
			Nullable<int> unaryRoleIndex = null;
			int factArity = 0;
			int unaryRoleOffset = 0;
			LinkedElementCollection<SetComparisonConstraintRoleSequence> constraintSequences = this.RoleSequenceCollection;
			int constraintRoleArity = constraintSequences.Count;
			IList<Role>[] allConstraintSequences = new IList<Role>[constraintRoleArity];
			for (int i = 0; i < constraintRoleArity; ++i)
			{
				allConstraintSequences[i] = constraintSequences[i].RoleCollection;
			}
			int columnArity = allConstraintSequences[0].Count;
			LinkedElementCollection<FactType> allFacts = this.FactTypeCollection;
			int allFactsCount = allFacts.Count;
			if (allFactsCount == 0)
			{
				#region Error report
				if (errorOwner != null)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
				}
				#endregion // Error report
				return false;
			}
			string[][] allBasicRoleReplacements = new string[allFactsCount][];
			bool[] unaryReplacements = new bool[allFactsCount];
			int contextBasicReplacementIndex;
			int minFactArity = int.MaxValue;
			int maxFactArity = int.MinValue;
			for (int iFact = 0; iFact < allFactsCount; ++iFact)
			{
				FactType currentFact = allFacts[iFact];
				if (currentFact.ReadingRequiredError != null)
				{
					#region Error report
					if (errorOwner != null)
					{
						firstErrorPending = true;
						foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
						{
							ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
							if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
							{
								continue;
							}
							if (firstErrorPending)
							{
								firstErrorPending = false;
								verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
								writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
							}
							else
							{
								writer.WriteLine();
							}
							writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
						}
						if (!firstErrorPending)
						{
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
						}
					}
					#endregion // Error report
					return false;
				}
				allReadingOrders = currentFact.ReadingOrderCollection;
				factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : currentFact.RoleCollection;
				unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
				factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
				unaryRoleOffset = unaryRoleIndex.HasValue ? unaryRoleIndex.Value : 0;
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
					Role factRole = factRoles[i + unaryRoleOffset].Role;
					ObjectType rolePlayer = factRole.RolePlayer;
					string basicReplacement;
					if (rolePlayer != null)
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"));
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				allBasicRoleReplacements[iFact] = basicRoleReplacements;
				unaryReplacements[iFact] = unaryRoleIndex.HasValue;
			}
			IReading[] allConstraintRoleReadings = new IReading[constraintRoleArity];
			string[] roleReplacements = new string[maxFactArity];
			IReading reading;
			VerbalizationHyphenBinder hyphenBinder;
			#endregion // Preliminary
			#region Pattern Matches
			if (columnArity == 1 && constraintRoleArity >= 2 && !isNegative)
			{
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				int snippet1Replace1ReplaceSequenceIter1;
				for (snippet1Replace1ReplaceSequenceIter1 = 0; snippet1Replace1ReplaceSequenceIter1 < 1; ++snippet1Replace1ReplaceSequenceIter1)
				{
					IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1ReplaceSequenceIter1];
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
					{
						RoleBase primaryRole = includedSequenceRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						CoreVerbalizationSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListOpen;
						}
						else if (RoleIter1 == columnArity - 1)
						{
							if (RoleIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						sbTemp.Append(basicRoleReplacements[unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
						if (RoleIter1 == columnArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleLogicalAndListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace1Replace1 = sbTemp.ToString();
				}
				string snippet1Replace1Replace2 = null;
				CoreVerbalizationSnippetType snippet1Replace1ReplaceSnippetType2 = 0;
				if (this.ExclusiveOrMandatoryConstraint != null)
				{
					snippet1Replace1ReplaceSnippetType2 = CoreVerbalizationSnippetType.GroupExclusiveOr;
				}
				else
				{
					snippet1Replace1ReplaceSnippetType2 = CoreVerbalizationSnippetType.GroupExclusion;
				}
				string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(snippet1Replace1ReplaceSnippetType2, isDeontic, isNegative);
				string snippet1Replace1Replace2Replace1 = null;
				int snippet1Replace1Replace2ReplaceSequenceIter1;
				int snippet1Replace1Replace2ReplaceCompositeCount1 = 0;
				int snippet1Replace1Replace2ReplaceCompositeIterator1;
				FactType[] snippet1Replace1Replace2Replace1ItemUniqueFactTypes1 = new FactType[columnArity];
				FactType snippet1Replace1Replace2Replace1ItemTestUniqueFactType1;
				for (snippet1Replace1Replace2ReplaceSequenceIter1 = 0; snippet1Replace1Replace2ReplaceSequenceIter1 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceSequenceIter1)
				{
					IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
					for (snippet1Replace1Replace2ReplaceCompositeIterator1 = 0; snippet1Replace1Replace2ReplaceCompositeIterator1 < columnArity; ++snippet1Replace1Replace2ReplaceCompositeIterator1)
					{
						RoleBase primaryRole = includedSequenceRoles[snippet1Replace1Replace2ReplaceCompositeIterator1];
						if (Array.IndexOf(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, snippet1Replace1Replace2Replace1ItemTestUniqueFactType1 = primaryRole.FactType) == -1)
						{
							snippet1Replace1Replace2Replace1ItemUniqueFactTypes1[snippet1Replace1Replace2ReplaceCompositeIterator1] = snippet1Replace1Replace2Replace1ItemTestUniqueFactType1;
							++snippet1Replace1Replace2ReplaceCompositeCount1;
						}
					}
					Array.Clear(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, 0, snippet1Replace1Replace2Replace1ItemUniqueFactTypes1.Length);
				}
				snippet1Replace1Replace2ReplaceCompositeIterator1 = 0;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (snippet1Replace1Replace2ReplaceSequenceIter1 = 0; snippet1Replace1Replace2ReplaceSequenceIter1 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceSequenceIter1)
				{
					IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
					string snippet1Replace1Replace2Replace1Item1;
					for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
					{
						RoleBase primaryRole = includedSequenceRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						factArity = factRoles.Count;
						if (Array.IndexOf(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, snippet1Replace1Replace2Replace1ItemTestUniqueFactType1 = primaryRole.FactType) == -1)
						{
							snippet1Replace1Replace2Replace1ItemUniqueFactTypes1[RoleIter1] = snippet1Replace1Replace2Replace1ItemTestUniqueFactType1;
							CoreVerbalizationSnippetType listSnippet;
							if (snippet1Replace1Replace2ReplaceCompositeIterator1 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.MultilineIndentedCompoundListOpen;
							}
							else if (snippet1Replace1Replace2ReplaceCompositeIterator1 == snippet1Replace1Replace2ReplaceCompositeCount1 - 1)
							{
								if (snippet1Replace1Replace2ReplaceCompositeIterator1 == 1)
								{
									listSnippet = CoreVerbalizationSnippetType.MultilineIndentedCompoundListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.MultilineIndentedCompoundListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.MultilineIndentedCompoundListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, false, factRoles, true);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1Replace2Replace1ItemFactRoleIter1 = 0; snippet1Replace1Replace2Replace1ItemFactRoleIter1 < factArity; ++snippet1Replace1Replace2Replace1ItemFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace1Replace2Replace1ItemFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[allFacts.IndexOf(includedSequenceRoles[RoleIter1].FactType)][snippet1Replace1Replace2Replace1ItemFactRoleIter1], snippet1Replace1Replace2Replace1ItemFactRoleIter1);
								if (includedSequenceRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								}
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace1Replace2Replace1ItemFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1Replace2Replace1Item1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace2Replace1Item1);
							if (snippet1Replace1Replace2ReplaceCompositeIterator1 == snippet1Replace1Replace2ReplaceCompositeCount1 - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.MultilineIndentedCompoundListClose, isDeontic, isNegative));
							}
							++snippet1Replace1Replace2ReplaceCompositeIterator1;
						}
					}
				}
				snippet1Replace1Replace2Replace1 = sbTemp.ToString();
				snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (allFactsCount == 2 && columnArity == 2 && constraintRoleArity == 2 && !isNegative)
			{
				int binaryLeadReadingColumnIndex = -1;
				for (int testColumnIndex = 0; testColumnIndex < 2; ++testColumnIndex)
				{
					Role testLeadRole = allConstraintSequences[0][testColumnIndex];
					parentFact = testLeadRole.FactType;
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					if (parentFact.GetMatchingReading(parentFact.ReadingOrderCollection, null, testLeadRole, null, false, true, true, null, false) != null)
					{
						testLeadRole = allConstraintSequences[1][testColumnIndex];
						parentFact = testLeadRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						if (parentFact.GetMatchingReading(parentFact.ReadingOrderCollection, null, testLeadRole, null, false, true, true, null, false) != null)
						{
							binaryLeadReadingColumnIndex = testColumnIndex;
							break;
						}
					}
				}
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				CoreVerbalizationSnippetType snippet1ReplaceSnippetType1 = 0;
				if (binaryLeadReadingColumnIndex == 0)
				{
					snippet1ReplaceSnippetType1 = CoreVerbalizationSnippetType.ExclusionBinaryLeadReading;
				}
				else if (binaryLeadReadingColumnIndex == 1)
				{
					snippet1ReplaceSnippetType1 = CoreVerbalizationSnippetType.ExclusionBinaryLeadReading;
				}
				else
				{
					snippet1ReplaceSnippetType1 = CoreVerbalizationSnippetType.ForEachCompactQuantifier;
				}
				string snippet1ReplaceFormat1 = snippets.GetSnippet(snippet1ReplaceSnippetType1, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				if (binaryLeadReadingColumnIndex == 0)
				{
					string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ExclusionCombined, isDeontic, isNegative);
					string snippet1Replace1Replace1Replace1 = null;
					int snippet1Replace1Replace1ReplaceSequenceIter1;
					for (snippet1Replace1Replace1ReplaceSequenceIter1 = 0; snippet1Replace1Replace1ReplaceSequenceIter1 < 1; ++snippet1Replace1Replace1ReplaceSequenceIter1)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace1ReplaceSequenceIter1];
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
							RoleBase primaryRole = includedSequenceRoles[RoleIter1];
							parentFact = primaryRole.FactType;
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
							sbTemp.Append(basicRoleReplacements[unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
						}
						snippet1Replace1Replace1Replace1 = sbTemp.ToString();
					}
					string snippet1Replace1Replace1Replace2 = null;
					int snippet1Replace1Replace1ReplaceSequenceIter2;
					int snippet1Replace1Replace1ReplaceCompositeCount2 = 0;
					int snippet1Replace1Replace1ReplaceCompositeIterator2;
					for (snippet1Replace1Replace1ReplaceSequenceIter2 = 0; snippet1Replace1Replace1ReplaceSequenceIter2 < constraintRoleArity; ++snippet1Replace1Replace1ReplaceSequenceIter2)
					{
						snippet1Replace1Replace1ReplaceCompositeCount2 = snippet1Replace1Replace1ReplaceCompositeCount2 + 1;
					}
					snippet1Replace1Replace1ReplaceCompositeIterator2 = 0;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (snippet1Replace1Replace1ReplaceSequenceIter2 = 0; snippet1Replace1Replace1ReplaceSequenceIter2 < constraintRoleArity; ++snippet1Replace1Replace1ReplaceSequenceIter2)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace1ReplaceSequenceIter2];
						string snippet1Replace1Replace1Replace2Item1;
						for (int RoleIter1 = 0; RoleIter1 < 1; ++RoleIter1)
						{
							RoleBase primaryRole = includedSequenceRoles[RoleIter1];
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							factArity = factRoles.Count;
							CoreVerbalizationSnippetType listSnippet;
							if (snippet1Replace1Replace1ReplaceCompositeIterator2 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListOpen;
							}
							else if (snippet1Replace1Replace1ReplaceCompositeIterator2 == snippet1Replace1Replace1ReplaceCompositeCount2 - 1)
							{
								if (snippet1Replace1Replace1ReplaceCompositeIterator2 == 1)
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, true, true, factRoles, true);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1Replace1Replace2ItemFactRoleIter1 = 0; snippet1Replace1Replace1Replace2ItemFactRoleIter1 < factArity; ++snippet1Replace1Replace1Replace2ItemFactRoleIter1)
							{
								roleReplacements[snippet1Replace1Replace1Replace2ItemFactRoleIter1] = "";
							}
							snippet1Replace1Replace1Replace2Item1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace1Replace2Item1);
							if (snippet1Replace1Replace1ReplaceCompositeIterator2 == snippet1Replace1Replace1ReplaceCompositeCount2 - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleLogicalAndListClose, isDeontic, isNegative));
							}
							++snippet1Replace1Replace1ReplaceCompositeIterator2;
						}
					}
					snippet1Replace1Replace1Replace2 = sbTemp.ToString();
					snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1, snippet1Replace1Replace1Replace2);
				}
				else if (binaryLeadReadingColumnIndex == 1)
				{
					string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ExclusionCombined, isDeontic, isNegative);
					string snippet1Replace1Replace1Replace1 = null;
					int snippet1Replace1Replace1ReplaceSequenceIter1;
					for (snippet1Replace1Replace1ReplaceSequenceIter1 = 0; snippet1Replace1Replace1ReplaceSequenceIter1 < 1; ++snippet1Replace1Replace1ReplaceSequenceIter1)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace1ReplaceSequenceIter1];
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						for (int RoleIter1 = 1; RoleIter1 < columnArity; ++RoleIter1)
						{
							RoleBase primaryRole = includedSequenceRoles[RoleIter1];
							parentFact = primaryRole.FactType;
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
							sbTemp.Append(basicRoleReplacements[unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
						}
						snippet1Replace1Replace1Replace1 = sbTemp.ToString();
					}
					string snippet1Replace1Replace1Replace2 = null;
					int snippet1Replace1Replace1ReplaceSequenceIter2;
					int snippet1Replace1Replace1ReplaceCompositeCount2 = 0;
					int snippet1Replace1Replace1ReplaceCompositeIterator2;
					for (snippet1Replace1Replace1ReplaceSequenceIter2 = 0; snippet1Replace1Replace1ReplaceSequenceIter2 < constraintRoleArity; ++snippet1Replace1Replace1ReplaceSequenceIter2)
					{
						snippet1Replace1Replace1ReplaceCompositeCount2 = snippet1Replace1Replace1ReplaceCompositeCount2 + columnArity;
					}
					snippet1Replace1Replace1ReplaceCompositeIterator2 = 0;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (snippet1Replace1Replace1ReplaceSequenceIter2 = 0; snippet1Replace1Replace1ReplaceSequenceIter2 < constraintRoleArity; ++snippet1Replace1Replace1ReplaceSequenceIter2)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace1ReplaceSequenceIter2];
						string snippet1Replace1Replace1Replace2Item1;
						for (int RoleIter1 = 1; RoleIter1 < columnArity; ++RoleIter1)
						{
							RoleBase primaryRole = includedSequenceRoles[RoleIter1];
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							factArity = factRoles.Count;
							CoreVerbalizationSnippetType listSnippet;
							if (snippet1Replace1Replace1ReplaceCompositeIterator2 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListOpen;
							}
							else if (snippet1Replace1Replace1ReplaceCompositeIterator2 == snippet1Replace1Replace1ReplaceCompositeCount2 - 1)
							{
								if (snippet1Replace1Replace1ReplaceCompositeIterator2 == 1)
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, true, true, factRoles, true);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1Replace1Replace2ItemFactRoleIter1 = 0; snippet1Replace1Replace1Replace2ItemFactRoleIter1 < factArity; ++snippet1Replace1Replace1Replace2ItemFactRoleIter1)
							{
								roleReplacements[snippet1Replace1Replace1Replace2ItemFactRoleIter1] = "";
							}
							snippet1Replace1Replace1Replace2Item1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace1Replace2Item1);
							if (snippet1Replace1Replace1ReplaceCompositeIterator2 == snippet1Replace1Replace1ReplaceCompositeCount2 - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleLogicalAndListClose, isDeontic, isNegative));
							}
							++snippet1Replace1Replace1ReplaceCompositeIterator2;
						}
					}
					snippet1Replace1Replace1Replace2 = sbTemp.ToString();
					snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1, snippet1Replace1Replace1Replace2);
				}
				else
				{
					int snippet1Replace1ReplaceSequenceIter1;
					for (snippet1Replace1ReplaceSequenceIter1 = 0; snippet1Replace1ReplaceSequenceIter1 < 1; ++snippet1Replace1ReplaceSequenceIter1)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1ReplaceSequenceIter1];
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
						{
							RoleBase primaryRole = includedSequenceRoles[RoleIter1];
							parentFact = primaryRole.FactType;
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
							CoreVerbalizationSnippetType listSnippet;
							if (RoleIter1 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListOpen;
							}
							else if (RoleIter1 == columnArity - 1)
							{
								if (RoleIter1 == 1)
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							sbTemp.Append(basicRoleReplacements[unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
							if (RoleIter1 == columnArity - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleLogicalAndListClose, isDeontic, isNegative));
							}
						}
						snippet1Replace1Replace1 = sbTemp.ToString();
					}
				}
				string snippet1Replace1Replace2 = null;
				if (binaryLeadReadingColumnIndex == 0)
				{
					int snippet1Replace1ReplaceSequenceIter2;
					for (snippet1Replace1ReplaceSequenceIter2 = 0; snippet1Replace1ReplaceSequenceIter2 < 1; ++snippet1Replace1ReplaceSequenceIter2)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1ReplaceSequenceIter2];
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						for (int RoleIter2 = 1; RoleIter2 < columnArity; ++RoleIter2)
						{
							RoleBase primaryRole = includedSequenceRoles[RoleIter2];
							parentFact = primaryRole.FactType;
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
							sbTemp.Append(basicRoleReplacements[unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
						}
						snippet1Replace1Replace2 = sbTemp.ToString();
					}
				}
				else if (binaryLeadReadingColumnIndex == 1)
				{
					int snippet1Replace1ReplaceSequenceIter2;
					for (snippet1Replace1ReplaceSequenceIter2 = 0; snippet1Replace1ReplaceSequenceIter2 < 1; ++snippet1Replace1ReplaceSequenceIter2)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1ReplaceSequenceIter2];
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						for (int RoleIter2 = 0; RoleIter2 < 1; ++RoleIter2)
						{
							RoleBase primaryRole = includedSequenceRoles[RoleIter2];
							parentFact = primaryRole.FactType;
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
							sbTemp.Append(basicRoleReplacements[unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
						}
						snippet1Replace1Replace2 = sbTemp.ToString();
					}
				}
				else
				{
					string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.GroupExclusion, isDeontic, isNegative);
					string snippet1Replace1Replace2Replace1 = null;
					int snippet1Replace1Replace2ReplaceSequenceIter1;
					int snippet1Replace1Replace2ReplaceCompositeCount1 = 0;
					int snippet1Replace1Replace2ReplaceCompositeIterator1;
					FactType[] snippet1Replace1Replace2Replace1ItemUniqueFactTypes1 = new FactType[columnArity];
					FactType snippet1Replace1Replace2Replace1ItemTestUniqueFactType1;
					for (snippet1Replace1Replace2ReplaceSequenceIter1 = 0; snippet1Replace1Replace2ReplaceSequenceIter1 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceSequenceIter1)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
						for (snippet1Replace1Replace2ReplaceCompositeIterator1 = 0; snippet1Replace1Replace2ReplaceCompositeIterator1 < columnArity; ++snippet1Replace1Replace2ReplaceCompositeIterator1)
						{
							RoleBase primaryRole = includedSequenceRoles[snippet1Replace1Replace2ReplaceCompositeIterator1];
							if (Array.IndexOf(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, snippet1Replace1Replace2Replace1ItemTestUniqueFactType1 = primaryRole.FactType) == -1)
							{
								snippet1Replace1Replace2Replace1ItemUniqueFactTypes1[snippet1Replace1Replace2ReplaceCompositeIterator1] = snippet1Replace1Replace2Replace1ItemTestUniqueFactType1;
								++snippet1Replace1Replace2ReplaceCompositeCount1;
							}
						}
						Array.Clear(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, 0, snippet1Replace1Replace2Replace1ItemUniqueFactTypes1.Length);
					}
					snippet1Replace1Replace2ReplaceCompositeIterator1 = 0;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (snippet1Replace1Replace2ReplaceSequenceIter1 = 0; snippet1Replace1Replace2ReplaceSequenceIter1 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceSequenceIter1)
					{
						IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
						string snippet1Replace1Replace2Replace1Item1;
						for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
						{
							RoleBase primaryRole = includedSequenceRoles[RoleIter1];
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							factArity = factRoles.Count;
							if (Array.IndexOf(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, snippet1Replace1Replace2Replace1ItemTestUniqueFactType1 = primaryRole.FactType) == -1)
							{
								snippet1Replace1Replace2Replace1ItemUniqueFactTypes1[RoleIter1] = snippet1Replace1Replace2Replace1ItemTestUniqueFactType1;
								CoreVerbalizationSnippetType listSnippet;
								if (snippet1Replace1Replace2ReplaceCompositeIterator1 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.MultilineIndentedCompoundListOpen;
								}
								else if (snippet1Replace1Replace2ReplaceCompositeIterator1 == snippet1Replace1Replace2ReplaceCompositeCount1 - 1)
								{
									if (snippet1Replace1Replace2ReplaceCompositeIterator1 == 1)
									{
										listSnippet = CoreVerbalizationSnippetType.MultilineIndentedCompoundListPairSeparator;
									}
									else
									{
										listSnippet = CoreVerbalizationSnippetType.MultilineIndentedCompoundListFinalSeparator;
									}
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.MultilineIndentedCompoundListSeparator;
								}
								sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
								reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								for (int snippet1Replace1Replace2Replace1ItemFactRoleIter1 = 0; snippet1Replace1Replace2Replace1ItemFactRoleIter1 < factArity; ++snippet1Replace1Replace2Replace1ItemFactRoleIter1)
								{
									RoleBase currentRole = factRoles[snippet1Replace1Replace2Replace1ItemFactRoleIter1];
									string roleReplacement = null;
									string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[allFacts.IndexOf(includedSequenceRoles[RoleIter1].FactType)][snippet1Replace1Replace2Replace1ItemFactRoleIter1], snippet1Replace1Replace2Replace1ItemFactRoleIter1);
									if (includedSequenceRoles.Contains(currentRole.Role))
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
									}
									else
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
									}
									if (roleReplacement == null)
									{
										roleReplacement = basicReplacement;
									}
									roleReplacements[snippet1Replace1Replace2Replace1ItemFactRoleIter1] = roleReplacement;
								}
								snippet1Replace1Replace2Replace1Item1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
								sbTemp.Append(snippet1Replace1Replace2Replace1Item1);
								if (snippet1Replace1Replace2ReplaceCompositeIterator1 == snippet1Replace1Replace2ReplaceCompositeCount1 - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.MultilineIndentedCompoundListClose, isDeontic, isNegative));
								}
								++snippet1Replace1Replace2ReplaceCompositeIterator1;
							}
						}
					}
					snippet1Replace1Replace2Replace1 = sbTemp.ToString();
					snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1);
				}
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (constraintRoleArity >= 2 && !isNegative)
			{
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				int snippet1Replace1ReplaceSequenceIter1;
				for (snippet1Replace1ReplaceSequenceIter1 = 0; snippet1Replace1ReplaceSequenceIter1 < 1; ++snippet1Replace1ReplaceSequenceIter1)
				{
					IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1ReplaceSequenceIter1];
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
					{
						RoleBase primaryRole = includedSequenceRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex = allFacts.IndexOf(parentFact)];
						CoreVerbalizationSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListOpen;
						}
						else if (RoleIter1 == columnArity - 1)
						{
							if (RoleIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						sbTemp.Append(basicRoleReplacements[unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)]);
						if (RoleIter1 == columnArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleLogicalAndListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace1Replace1 = sbTemp.ToString();
				}
				string snippet1Replace1Replace2 = null;
				string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.GroupExclusion, isDeontic, isNegative);
				string snippet1Replace1Replace2Replace1 = null;
				int snippet1Replace1Replace2ReplaceSequenceIter1;
				int snippet1Replace1Replace2ReplaceCompositeCount1 = 0;
				int snippet1Replace1Replace2ReplaceCompositeIterator1;
				FactType[] snippet1Replace1Replace2Replace1ItemUniqueFactTypes1 = new FactType[columnArity];
				FactType snippet1Replace1Replace2Replace1ItemTestUniqueFactType1;
				for (snippet1Replace1Replace2ReplaceSequenceIter1 = 0; snippet1Replace1Replace2ReplaceSequenceIter1 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceSequenceIter1)
				{
					IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
					for (snippet1Replace1Replace2ReplaceCompositeIterator1 = 0; snippet1Replace1Replace2ReplaceCompositeIterator1 < columnArity; ++snippet1Replace1Replace2ReplaceCompositeIterator1)
					{
						RoleBase primaryRole = includedSequenceRoles[snippet1Replace1Replace2ReplaceCompositeIterator1];
						if (Array.IndexOf(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, snippet1Replace1Replace2Replace1ItemTestUniqueFactType1 = primaryRole.FactType) == -1)
						{
							snippet1Replace1Replace2Replace1ItemUniqueFactTypes1[snippet1Replace1Replace2ReplaceCompositeIterator1] = snippet1Replace1Replace2Replace1ItemTestUniqueFactType1;
							++snippet1Replace1Replace2ReplaceCompositeCount1;
						}
					}
					Array.Clear(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, 0, snippet1Replace1Replace2Replace1ItemUniqueFactTypes1.Length);
				}
				snippet1Replace1Replace2ReplaceCompositeIterator1 = 0;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (snippet1Replace1Replace2ReplaceSequenceIter1 = 0; snippet1Replace1Replace2ReplaceSequenceIter1 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceSequenceIter1)
				{
					IList<Role> includedSequenceRoles = allConstraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
					string snippet1Replace1Replace2Replace1Item1;
					for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
					{
						RoleBase primaryRole = includedSequenceRoles[RoleIter1];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						factArity = factRoles.Count;
						if (Array.IndexOf(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, snippet1Replace1Replace2Replace1ItemTestUniqueFactType1 = primaryRole.FactType) == -1)
						{
							snippet1Replace1Replace2Replace1ItemUniqueFactTypes1[RoleIter1] = snippet1Replace1Replace2Replace1ItemTestUniqueFactType1;
							CoreVerbalizationSnippetType listSnippet;
							if (snippet1Replace1Replace2ReplaceCompositeIterator1 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.MultilineIndentedCompoundListOpen;
							}
							else if (snippet1Replace1Replace2ReplaceCompositeIterator1 == snippet1Replace1Replace2ReplaceCompositeCount1 - 1)
							{
								if (snippet1Replace1Replace2ReplaceCompositeIterator1 == 1)
								{
									listSnippet = CoreVerbalizationSnippetType.MultilineIndentedCompoundListPairSeparator;
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.MultilineIndentedCompoundListFinalSeparator;
								}
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.MultilineIndentedCompoundListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, false, false, false, factRoles, true);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1Replace2Replace1ItemFactRoleIter1 = 0; snippet1Replace1Replace2Replace1ItemFactRoleIter1 < factArity; ++snippet1Replace1Replace2Replace1ItemFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace1Replace2Replace1ItemFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[allFacts.IndexOf(includedSequenceRoles[RoleIter1].FactType)][snippet1Replace1Replace2Replace1ItemFactRoleIter1], snippet1Replace1Replace2Replace1ItemFactRoleIter1);
								if (includedSequenceRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								}
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace1Replace2Replace1ItemFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1Replace2Replace1Item1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace2Replace1Item1);
							if (snippet1Replace1Replace2ReplaceCompositeIterator1 == snippet1Replace1Replace2ReplaceCompositeCount1 - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.MultilineIndentedCompoundListClose, isDeontic, isNegative));
							}
							++snippet1Replace1Replace2ReplaceCompositeIterator1;
						}
					}
				}
				snippet1Replace1Replace2Replace1 = sbTemp.ToString();
				snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
					{
						continue;
					}
					if (firstErrorPending)
					{
						firstErrorPending = false;
						writer.WriteLine();
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
					}
					else
					{
						writer.WriteLine();
					}
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
	}
	#endregion // ExclusionConstraint verbalization
	#region FactType.FactTypeInstanceBlockStart verbalization
	public partial class FactType
	{
		#region FactType verbalization block start
		private partial class FactTypeInstanceBlockStart : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static FactTypeInstanceBlockStart myCache;
			public static FactTypeInstanceBlockStart GetVerbalizer()
			{
				FactTypeInstanceBlockStart retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<FactTypeInstanceBlockStart>(ref myCache, null as FactTypeInstanceBlockStart, retVal);
				}
				if (retVal == null)
				{
					retVal = new FactTypeInstanceBlockStart();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<FactTypeInstanceBlockStart>(ref myCache, this, null as FactTypeInstanceBlockStart);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.FactTypeInstanceBlockStart, false, false));
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
			}
		}
		#endregion // FactType verbalization block start
	}
	#endregion // FactType.FactTypeInstanceBlockStart verbalization
	#region FactType.FactTypeInstanceBlockEnd verbalization
	public partial class FactType
	{
		#region FactType verbalization block start
		private partial class FactTypeInstanceBlockEnd : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static FactTypeInstanceBlockEnd myCache;
			public static FactTypeInstanceBlockEnd GetVerbalizer()
			{
				FactTypeInstanceBlockEnd retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<FactTypeInstanceBlockEnd>(ref myCache, null as FactTypeInstanceBlockEnd, retVal);
				}
				if (retVal == null)
				{
					retVal = new FactTypeInstanceBlockEnd();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<FactTypeInstanceBlockEnd>(ref myCache, this, null as FactTypeInstanceBlockEnd);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.FactTypeInstanceBlockEnd, false, false));
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
			}
		}
		#endregion // FactType verbalization block start
	}
	#endregion // FactType.FactTypeInstanceBlockEnd verbalization
	#region FactType.FactTypeInstanceVerbalizer verbalization
	public partial class FactType
	{
		#region FactType Instance Verbalization
		private partial class FactTypeInstanceVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static FactTypeInstanceVerbalizer myCache;
			public static FactTypeInstanceVerbalizer GetVerbalizer()
			{
				FactTypeInstanceVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<FactTypeInstanceVerbalizer>(ref myCache, null as FactTypeInstanceVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new FactTypeInstanceVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<FactTypeInstanceVerbalizer>(ref myCache, this, null as FactTypeInstanceVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				#region Prerequisite error check
				IModelErrorOwner errorOwner = this as IModelErrorOwner;
				bool firstErrorPending;
				if (errorOwner != null)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
					{
						if (firstErrorPending)
						{
							firstErrorPending = false;
							verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenPrimaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
						firstErrorPending = true;
						foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
						{
							ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
							if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
							{
								continue;
							}
							if (firstErrorPending)
							{
								firstErrorPending = false;
								writer.WriteLine();
								writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
							}
							else
							{
								writer.WriteLine();
							}
							writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
						}
						if (!firstErrorPending)
						{
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
						}
						return true;
					}
				}
				#endregion // Prerequisite error check
				FactType parentFact = this.FactType;
				LinkedElementCollection<ReadingOrder> allReadingOrders = parentFact.ReadingOrderCollection;
				IList<RoleBase> factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
				Nullable<int> unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
				int factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
				int unaryRoleOffset = unaryRoleIndex.HasValue ? unaryRoleIndex.Value : 0;
				const bool isDeontic = false;
				string predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				IReading reading;
				VerbalizationHyphenBinder hyphenBinder;
				LinkedElementCollection<FactTypeRoleInstance> instanceRoles = Instance.RoleInstanceCollection;
				int instanceRoleCount = instanceRoles.Count;
				string[] basicRoleReplacements = new string[factArity];
				string textFormat = snippets.GetSnippet(CoreVerbalizationSnippetType.TextInstanceValue, isDeontic, isNegative);
				string nonTextFormat = snippets.GetSnippet(CoreVerbalizationSnippetType.NonTextInstanceValue, isDeontic, isNegative);
				IFormatProvider formatProvider = writer.FormatProvider;
				for (int i = 0; i < factArity; ++i)
				{
					Role factRole = factRoles[i + unaryRoleOffset].Role;
					ObjectType rolePlayer = factRole.RolePlayer;
					string basicReplacement;
					if (rolePlayer != null)
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name, rolePlayer.Id.ToString("D"));
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					FactTypeRoleInstance roleInstance = null;
					for (int j = 0; j < instanceRoleCount; ++j)
					{
						FactTypeRoleInstance testInstance = instanceRoles[j];
						if (testInstance.Role == factRole)
						{
							roleInstance = testInstance;
							break;
						}
					}
					if (roleInstance != null)
					{
						string instanceValue;
						instanceValue = ObjectTypeInstance.GetDisplayString(roleInstance.ObjectTypeInstance, rolePlayer, false, formatProvider, textFormat, nonTextFormat);
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.CombinedObjectAndInstance, isDeontic, isNegative), basicReplacement, instanceValue);
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.CombinedObjectAndInstanceTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				ObjectTypeInstance objectifyingInstance = this.DisplayIdentifier ? this.Instance.ObjectifyingInstance : null;
				#endregion // Preliminary
				#region Pattern Matches
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, false, false, false, factRoles, true);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				FactType.WriteVerbalizerSentence(writer, hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, basicRoleReplacements, true), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				if (objectifyingInstance != null)
				{
					writer.WriteLine();
					string snippetFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.FactTypeInstanceIdentifier, isDeontic, isNegative);
					string snippet2Replace1 = null;
					snippet2Replace1 = ObjectTypeInstance.GetDisplayString(objectifyingInstance, objectifyingInstance.ObjectType, true, formatProvider, textFormat, nonTextFormat);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat2, snippet2Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				#endregion // Pattern Matches
				#region Error report
				if (errorOwner != null)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							writer.WriteLine();
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
				}
				#endregion // Error report
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
			}
		}
		#endregion // FactType Instance Verbalization
	}
	#endregion // FactType.FactTypeInstanceVerbalizer verbalization
	#region ObjectType.ObjectTypeInstanceVerbalizer verbalization
	public partial class ObjectType
	{
		#region ObjectType Instance Verbalization
		private partial class ObjectTypeInstanceVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static ObjectTypeInstanceVerbalizer myCache;
			public static ObjectTypeInstanceVerbalizer GetVerbalizer()
			{
				ObjectTypeInstanceVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<ObjectTypeInstanceVerbalizer>(ref myCache, null as ObjectTypeInstanceVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new ObjectTypeInstanceVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<ObjectTypeInstanceVerbalizer>(ref myCache, this, null as ObjectTypeInstanceVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				#region Prerequisite error check
				IModelErrorOwner errorOwner = this as IModelErrorOwner;
				bool firstErrorPending;
				if (errorOwner != null)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
					{
						if (firstErrorPending)
						{
							firstErrorPending = false;
							verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenPrimaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
						firstErrorPending = true;
						foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
						{
							ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
							if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
							{
								continue;
							}
							if (firstErrorPending)
							{
								firstErrorPending = false;
								writer.WriteLine();
								writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
							}
							else
							{
								writer.WriteLine();
							}
							writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
						}
						if (!firstErrorPending)
						{
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
						}
						return true;
					}
				}
				#endregion // Prerequisite error check
				#endregion // Preliminary
				#region Pattern Matches
				const bool isDeontic = false;
				StringBuilder sbTemp = null;
				int instanceCount = this.Instances.Count;
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (int InstanceIter1 = 0; InstanceIter1 < instanceCount; ++InstanceIter1)
				{
					CoreVerbalizationSnippetType listSnippet;
					if (InstanceIter1 == 0)
					{
						listSnippet = CoreVerbalizationSnippetType.ObjectTypeInstanceListOpen;
					}
					else if (InstanceIter1 == instanceCount - 1)
					{
						if (InstanceIter1 == 1)
						{
							listSnippet = CoreVerbalizationSnippetType.ObjectTypeInstanceListPairSeparator;
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.ObjectTypeInstanceListFinalSeparator;
						}
					}
					else
					{
						listSnippet = CoreVerbalizationSnippetType.ObjectTypeInstanceListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					sbTemp.Append(ObjectTypeInstance.GetDisplayString(this.Instances[InstanceIter1], this.ParentObject, false, writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.TextInstanceValue, isDeontic, isNegative), snippets.GetSnippet(CoreVerbalizationSnippetType.NonTextInstanceValue, isDeontic, isNegative)));
					if (InstanceIter1 == instanceCount - 1)
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeInstanceListClose, isDeontic, isNegative));
					}
				}
				writer.Write(sbTemp.ToString());
				#endregion // Pattern Matches
				#region Error report
				if (errorOwner != null)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							writer.WriteLine();
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
				}
				#endregion // Error report
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
			}
		}
		#endregion // ObjectType Instance Verbalization
	}
	#endregion // ObjectType.ObjectTypeInstanceVerbalizer verbalization
}
