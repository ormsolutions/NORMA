using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;

// Common Public License Copyright Notice
// /**************************************************************************\
// * Natural Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © Neumont University. All rights reserved.                     *
// * Copyright © ORM Solutions, LLC. All rights reserved.                     *
// *                                                                          *
// * The use and distribution terms for this software are covered by the      *
// * Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
// * can be found in the file CPL.txt at the root of this distribution.       *
// * By using this software in any fashion, you are agreeing to be bound by   *
// * the terms of this license.                                               *
// *                                                                          *
// * You must not remove this notice, or any other, from this software.       *
// \**************************************************************************/

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region CoreVerbalizationSnippetType enum
	/// <summary>An enum with one value for each recognized snippet</summary>
	public enum CoreVerbalizationSnippetType
	{
		/// <summary>The 'Acyclicity' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to verbalize acyclic ring constraint in the majority of cases.
		/// Format: No {0} may cycle back to itself via one or more traversals through {1}</remark>
		Acyclicity,
		/// <summary>The 'AcyclicityWithRoleNumbers' format string snippet. Contains 4 replacement fields.</summary>
		/// <remark>Description: Used to verbalize acyclic ring constraints where more than two roles have a role player of the constrained type.
		/// Format: No {0} may cycle back to itself via one or more instances of the role pair: roles {2} and {3} of {1}</remark>
		AcyclicityWithRoleNumbers,
		/// <summary>The 'AggregateBagProjection' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Leave aggregate parameter inputs as a bag of values.
		/// Format: each {0}</remark>
		AggregateBagProjection,
		/// <summary>The 'AggregateParameterComplexAggregationContextListClose' simple snippet value.</summary>
		/// <remark>Description: The closing of a composite aggregation list.
		/// Format: combination</remark>
		AggregateParameterComplexAggregationContextListClose,
		/// <summary>The 'AggregateParameterComplexAggregationContextListOpen' simple snippet value.</summary>
		/// <remark>Description:The opening of a composite aggregation list.
		/// Format: each</remark>
		AggregateParameterComplexAggregationContextListOpen,
		/// <summary>The 'AggregateParameterComplexAggregationContextListSeparator' simple snippet value.</summary>
		/// <remark>Description: The separator of a composite aggregation list.
		/// Format: ,</remark>
		AggregateParameterComplexAggregationContextListSeparator,
		/// <summary>The 'AggregateParameterDecorator' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Decorate a single or complex aggregation context.
		/// Format: {0} of {1}</remark>
		AggregateParameterDecorator,
		/// <summary>The 'AggregateParameterSimpleAggregationContext' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Provide a description of a single aggregation context.
		/// Format: each {0}</remark>
		AggregateParameterSimpleAggregationContext,
		/// <summary>The 'AggregateSetProjection' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Limit values from an aggregate parameter input to distinct values.
		/// Format: each distinct {0}</remark>
		AggregateSetProjection,
		/// <summary>The 'AndLeadListClose' simple snippet value.</summary>
		/// <remark/>
		AndLeadListClose,
		/// <summary>The 'AndLeadListOpen' simple snippet value.</summary>
		/// <remark/>
		AndLeadListOpen,
		/// <summary>The 'AndLeadListSeparator' simple snippet value.</summary>
		/// <remark/>
		AndLeadListSeparator,
		/// <summary>The 'AndNestedListClose' simple snippet value.</summary>
		/// <remark/>
		AndNestedListClose,
		/// <summary>The 'AndNestedListOpen' simple snippet value.</summary>
		/// <remark/>
		AndNestedListOpen,
		/// <summary>The 'AndNestedListCollapsedOpen' simple snippet value.</summary>
		/// <remark/>
		AndNestedListCollapsedOpen,
		/// <summary>The 'AndNestedListSeparator' simple snippet value.</summary>
		/// <remark/>
		AndNestedListSeparator,
		/// <summary>The 'AndTailListClose' simple snippet value.</summary>
		/// <remark/>
		AndTailListClose,
		/// <summary>The 'AndTailListOpen' simple snippet value.</summary>
		/// <remark/>
		AndTailListOpen,
		/// <summary>The 'AndTailListCollapsedOpen' simple snippet value.</summary>
		/// <remark/>
		AndTailListCollapsedOpen,
		/// <summary>The 'AndTailListSeparator' simple snippet value.</summary>
		/// <remark/>
		AndTailListSeparator,
		/// <summary>The 'AtMostOneQuantifier' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used for 'at most one' before an object name to signify the quantity may only be zero or one.  Format: at most one {0}</remark>
		AtMostOneQuantifier,
		/// <summary>The 'AtMostOneTypedOccurrence' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used for a single-role frequency constraint with min=max=1.
		/// Format: at most one {0}</remark>
		AtMostOneTypedOccurrence,
		/// <summary>The 'ChainedListClose' simple snippet value.</summary>
		/// <remark/>
		ChainedListClose,
		/// <summary>The 'ChainedListCollapsedSeparator' simple snippet value.</summary>
		/// <remark>Description: The text for a collapsed separator in a chained list. Generally just a space.</remark>
		ChainedListCollapsedSeparator,
		/// <summary>The 'ChainedListComplexRestrictionBackReferenceSeparator' simple snippet value.</summary>
		/// <remark>Description: A separator for a chained list where the chained restriction introduces additional
		/// fact statements and the start of the next statement is a back reference.
		/// See ChainedListComplexRestrictionSeparator for additional comments.
		/// Format: \n </remark>
		ChainedListComplexRestrictionBackReferenceSeparator,
		/// <summary>The 'ChainedListComplexRestrictionCollapsedLeadSeparator' simple snippet value.</summary>
		/// <remark>Description: Used in place of the ChainedListComplexRestrictionSeparator if the lead role player of
		/// a chained list is the same as the previous statement. Chained lists can collapse the lead role if the list type is
		/// listed in the RolePathCollapsibleLeadDirective snippet.
		/// Format: \n\t and</remark>
		ChainedListComplexRestrictionCollapsedLeadSeparator,
		/// <summary>The 'ChainedListComplexRestrictionSeparator' simple snippet value.</summary>
		/// <remark>Description: A separator for a chained list where the chained restriction introduces
		/// additional fact statements. Note that the complex restriction separator is not used before a TailListOpen of
		/// an operator separated list, which is any split list not specific in the RolePathHeaderListDirective snippet.
		/// Format: \n\t where</remark>
		ChainedListComplexRestrictionSeparator,
		/// <summary>The 'ChainedListLocalRestrictionBackReferenceSeparator' simple snippet value.</summary>
		/// <remark>Description: A separator for a chained list where the chained restriction applies only
		/// to elements contained in the preceding fact statement and the start of the next statement is a back reference.</remark>
		ChainedListLocalRestrictionBackReferenceSeparator,
		/// <summary>The 'ChainedListLocalRestrictionSeparator' simple snippet value.</summary>
		/// <remark>Description: A separator for a chained list where the chained restriction applies only
		/// to elements contained in the preceding fact statement.
		/// Format: where</remark>
		ChainedListLocalRestrictionSeparator,
		/// <summary>The 'ChainedListOpen' simple snippet value.</summary>
		/// <remark>Description: The opening text for a chained list.</remark>
		ChainedListOpen,
		/// <summary>The 'ChainedListTopLevelComplexRestrictionBackReferenceSeparator' simple snippet value.</summary>
		/// <remark>Description: The same as ChainedListComplexRestrictionBackReferenceSeparator, except used for a
		/// top-level restriction. If the non-top-level separator includes an indentation, then this separator should omit the indent.
		/// Format: \n and</remark>
		ChainedListTopLevelComplexRestrictionBackReferenceSeparator,
		/// <summary>The 'ChainedListTopLevelComplexRestrictionCollapsedLeadSeparator' simple snippet value.</summary>
		/// <remark>Description: The same as ChainedListComplexRestrictionCollapsedLeadSeparator, except used for a
		/// top-level restriction. If the non-top-level separator includes an indentation, then this separator should omit the indent.
		/// Format: \n and</remark>
		ChainedListTopLevelComplexRestrictionCollapsedLeadSeparator,
		/// <summary>The 'ChainedListTopLevelComplexRestrictionSeparator' simple snippet value.</summary>
		/// <remark>Description: The same as ChainedListComplexRestrictionSeparator, except used for a top-level
		/// restriction. If the non-top-level separator includes an indentation, then this separator should omit the indent.
		/// Format: \n where</remark>
		ChainedListTopLevelComplexRestrictionSeparator,
		/// <summary>The 'CloseVerbalizationSentence' simple snippet value.</summary>
		/// <remark>Description: Text used to close a verbalized sentence.  Format: .</remark>
		CloseVerbalizationSentence,
		/// <summary>The 'CombinationIdentifier' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Verbalizes a combination.  Format: {0} combination</remark>
		CombinationIdentifier,
		/// <summary>The 'CombinationUniqueness' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Verbalizes uniqueness of a combination.  Format: {0} combination is unique</remark>
		CombinationUniqueness,
		/// <summary>The 'CombinedObjectAndInstance' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to combine an object and an instance. Format: {0} {1}</remark>
		CombinedObjectAndInstance,
		/// <summary>The 'CombinedObjectAndInstanceTypeMissing' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used to say an object and instance combination is missing. Format: Missing {0}</remark>
		CombinedObjectAndInstanceTypeMissing,
		/// <summary>The 'CompactSimpleListClose' simple snippet value.</summary>
		/// <remark>Description: Text used at the end of a compact simple list.</remark>
		CompactSimpleListClose,
		/// <summary>The 'CompactSimpleListFinalSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate the last two items in a compact simple list.  Format: ,</remark>
		CompactSimpleListFinalSeparator,
		/// <summary>The 'CompactSimpleListOpen' simple snippet value.</summary>
		/// <remark>Description: Text used at the beginning of a compact simple list.</remark>
		CompactSimpleListOpen,
		/// <summary>The 'CompactSimpleListPairSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in a compact simple list.  Format: ,</remark>
		CompactSimpleListPairSeparator,
		/// <summary>The 'CompactSimpleListSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in a compact simple list.  Format: ,</remark>
		CompactSimpleListSeparator,
		/// <summary>The 'CompatibleTypesIdentityInequalityOperator' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to specify that two instances of compatible types are not the same instance.
		/// Format: that {0} is not that {1}</remark>
		CompatibleTypesIdentityInequalityOperator,
		/// <summary>The 'CompoundListClose' simple snippet value.</summary>
		/// <remark>Description: Text that is at the end of a compound list.</remark>
		CompoundListClose,
		/// <summary>The 'CompoundListFinalSeparator' simple snippet value.</summary>
		/// <remark>Description: Used after the last item listed in a compound list.  Format: ;</remark>
		CompoundListFinalSeparator,
		/// <summary>The 'CompoundListOpen' simple snippet value.</summary>
		/// <remark>Description: Text that is at the beginning of a compound list.</remark>
		CompoundListOpen,
		/// <summary>The 'CompoundListPairSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in a compound list.  Format: ;</remark>
		CompoundListPairSeparator,
		/// <summary>The 'CompoundListSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in a compound list.  Format: ;</remark>
		CompoundListSeparator,
		/// <summary>The 'Conditional' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Verbalizes a simple conditional statement. Format: if {0} then {1}</remark>
		Conditional,
		/// <summary>The 'ConditionalMultiLine' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Verbalizes a conditional statement on two lines.
		/// Format: if {0}
		///         then {1}</remark>
		ConditionalMultiLine,
		/// <summary>The 'ConditionalMultiLineIndented' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Verbalizes a conditional statement on two lines with the 'then' line indented.
		/// Format: if {0}
		///           then {1}</remark>
		ConditionalMultiLineIndented,
		/// <summary>The 'ConstraintProvidesPreferredIdentifier' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Verbalizes the preferred identifier for an object.
		/// Format: this association with {0}provides the preferred identification scheme for {1}</remark>
		ConstraintProvidesPreferredIdentifier,
		/// <summary>The 'ContextCombinationAssociation' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Verbalizes a range of combination occurrences for a specified type where the combination has been previous specified.  Format: that combination is associated with {1} in this context</remark>
		ContextCombinationAssociation,
		/// <summary>The 'ContextCombinationOccurrence' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Verbalizes a range of combination occurrences where the combination has been previous specified.  Format: that combination occurs {1} in this context</remark>
		ContextCombinationOccurrence,
		/// <summary>The 'DefiniteArticle' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used for 'that' before an object name to signify a back reference to a uniquely qualified object type.</remark>
		DefiniteArticle,
		/// <summary>The 'DerivationNoteVerbalization' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Verbalizes the informal note specified for an element derivation rule. Format: Derivation  Note: {0}</remark>
		DerivationNoteVerbalization,
		/// <summary>The 'DescriptionVerbalization' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Verbalizes the text specified for an element description. Format: Informal Description: {0}</remark>
		DescriptionVerbalization,
		/// <summary>The 'EntityTypeVerbalization' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Verbalizes an object as being an entity type. Format: {0} is an entity type</remark>
		EntityTypeVerbalization,
		/// <summary>The 'Equality' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to verbalize a simple equality constraint. Format: {0} if and only if {1}</remark>
		Equality,
		/// <summary>The 'EqualsListClose' simple snippet value.</summary>
		/// <remark>Description: Text that is at the end of an equals list.</remark>
		EqualsListClose,
		/// <summary>The 'EqualsListFinalSeparator' simple snippet value.</summary>
		/// <remark>Description: Used between the last two items in an equals list.  Format: =</remark>
		EqualsListFinalSeparator,
		/// <summary>The 'EqualsListOpen' simple snippet value.</summary>
		/// <remark>Description: Text that is at the beginning of an equals list.</remark>
		EqualsListOpen,
		/// <summary>The 'EqualsListPairSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in an equals list.  Format: =</remark>
		EqualsListPairSeparator,
		/// <summary>The 'EqualsListSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in an equals list.  Format: =</remark>
		EqualsListSeparator,
		/// <summary>The 'ErrorClosePrimaryReport' simple snippet value.</summary>
		/// <remark>Description: Close a primary error report opened with ErrorOpenPrimaryReport</remark>
		ErrorClosePrimaryReport,
		/// <summary>The 'ErrorCloseSecondaryReport' simple snippet value.</summary>
		/// <remark>Description: Close a secondary error report opened with ErrorOpenSecondaryReport.</remark>
		ErrorCloseSecondaryReport,
		/// <summary>The 'ErrorOpenPrimaryReport' simple snippet value.</summary>
		/// <remark>Description: Used to open a primary error report. Primary error reports block further verbalization.</remark>
		ErrorOpenPrimaryReport,
		/// <summary>The 'ErrorOpenSecondaryReport' simple snippet value.</summary>
		/// <remark>Description: Used to open a secondary error report. Secondary reports contain errors that do not block verbalization.
		/// Replacement: {0}=error text,{1}=error id
		/// Format: Model Error: {0}</remark>
		ErrorOpenSecondaryReport,
		/// <summary>The 'ErrorPrimary' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to verbalize a single primary error.
		/// Replacement: {0}=error text,{1}=error id
		/// Format: Model Error: {0}</remark>
		ErrorPrimary,
		/// <summary>The 'ErrorSecondary' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to verbalize a single secondary error.
		/// Format: Model Error: {0}</remark>
		ErrorSecondary,
		/// <summary>The 'ExactlyOneQuantifier' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used for 'exactly one' before an object name to signify the quantity may only be one.  Format: exactly one {0}</remark>
		ExactlyOneQuantifier,
		/// <summary>The 'ExactlyOneTypedOccurrence' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used for a fallback uniqueness verbalization, or for a frequency constraint with min=max=1.
		/// Format: exactly one {0}</remark>
		ExactlyOneTypedOccurrence,
		/// <summary>The 'ExactlyOneUntypedOccurrence' simple snippet value.</summary>
		/// <remark>Description: Used for a fallback uniqueness verbalization, or for a frequency constraint with min=max=1.
		/// Format: exactly once</remark>
		ExactlyOneUntypedOccurrence,
		/// <summary>The 'ExclusionBinaryLeadReading' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to verbalize an exclusion constraint, on a binary lead reading.
		/// Format: No {0} the same {1}</remark>
		ExclusionBinaryLeadReading,
		/// <summary>The 'ExclusionCombined' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to combine the first role player and the roles played for an Exclusion constraint (i.e: No Person authored and reviewed). Format: {0} {1}</remark>
		ExclusionCombined,
		/// <summary>The 'ExistentialQuantifier' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used for 'some' before an object name to signify the context in which the object is being referred to.  Format: some {0}</remark>
		ExistentialQuantifier,
		/// <summary>The 'FactTypeInstanceBlockEnd' simple snippet value.</summary>
		/// <remark>Description: Formatted single snippet used to end a sample population verbalization of fact types block.</remark>
		FactTypeInstanceBlockEnd,
		/// <summary>The 'FactTypeInstanceBlockStart' simple snippet value.</summary>
		/// <remark>Description: Text and formatting to begin a sample population verbalization of fact types block. Format: Examples:</remark>
		FactTypeInstanceBlockStart,
		/// <summary>The 'FactTypeInstanceIdentifier' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: The text describing the identifier for an objectified FactType instance with an external identifier. Format: Identifier:</remark>
		FactTypeInstanceIdentifier,
		/// <summary>The 'FactTypeListClose' simple snippet value.</summary>
		/// <remark>Description: Text used to close the sample population verbalization list.</remark>
		FactTypeListClose,
		/// <summary>The 'FactTypeListFinalSeparator' simple snippet value.</summary>
		/// <remark>Description: Text used to separate the last two items in a sample population verbalization list. Format: <br/></remark>
		FactTypeListFinalSeparator,
		/// <summary>The 'FactTypeListOpen' simple snippet value.</summary>
		/// <remark>Description:  Text and formatting to begin a sample population verbalization basic predicate text.Format: FactTypes: <br/></remark>
		FactTypeListOpen,
		/// <summary>The 'FactTypeListPairSeparator' simple snippet value.</summary>
		/// <remark>Description: Text used to separate items in a sample population verbalization list. Format: <br/></remark>
		FactTypeListPairSeparator,
		/// <summary>The 'FactTypeListSeparator' simple snippet value.</summary>
		/// <remark>Description: Text used to separate items in a sample population verbalization list. Format: <br/></remark>
		FactTypeListSeparator,
		/// <summary>The 'ForEachCompactQuantifier' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Verbalizes for each instance of an object, some rule applies to those instances.  Format: for each {0}, {1}</remark>
		ForEachCompactQuantifier,
		/// <summary>The 'ForEachIndentedQuantifier' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Verbalizes for each instance of an object then creates a line break where the rules that apply to those instances are listed.  Format: for each {0}, \n{1}</remark>
		ForEachIndentedQuantifier,
		/// <summary>The 'ForEachNegatableCompactQuantifier' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Verbalizes for each instance of an object, some rule applies to those instances.
		/// Format positive: for each {0}, {1}
		/// Format negative: for some {0}, {1}</remark>
		ForEachNegatableCompactQuantifier,
		/// <summary>The 'ForEachNegatableIndentedQuantifier' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Verbalizes for each instance of an object then creates a line break where the rules that apply to those instances are listed.
		/// Format positive: for each {0}, \n{1}
		/// Format negative: for some {0}, \n{1}</remark>
		ForEachNegatableIndentedQuantifier,
		/// <summary>The 'FrequencyNotPopulatedOrRange' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Combined with a frequency range to indicate that a frequency constraint with a lower bound does not imply existence.
		/// Format: either no, or {0}</remark>
		FrequencyNotPopulatedOrRange,
		/// <summary>The 'FrequencyPopulation' format string snippet. Contains 3 replacement fields.</summary>
		/// <remark>Description: Used as for the main body text of a frequency constraint on one FactType.
		/// Format: each {0} in the population of {1} occurs there {2} times</remark>
		FrequencyPopulation,
		/// <summary>The 'FrequencyRangeExact' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used for a frequency range where both the min and max values the same
		/// Format: exactly {0}</remark>
		FrequencyRangeExact,
		/// <summary>The 'FrequencyRangeMaxUnbounded' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used for a frequency range where both the max value is unbounded.
		/// Format: at least {0}</remark>
		FrequencyRangeMaxUnbounded,
		/// <summary>The 'FrequencyRangeMinAndMax' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used for a frequency range where both the min and max values are specified and different.
		/// Format: at least {0} and at most {1}</remark>
		FrequencyRangeMinAndMax,
		/// <summary>The 'FrequencyRangeMinUnbounded' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used for a frequency range where the min value is less than 2.
		/// Format: at most {1}</remark>
		FrequencyRangeMinUnbounded,
		/// <summary>The 'FrequencyTypedCombinationOccurrences' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to specify an instance count or range for a combination of object types.
		/// Format: {1} combinations of {0}</remark>
		FrequencyTypedCombinationOccurrences,
		/// <summary>The 'FrequencyTypedOccurrences' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to specify an instance count or range for a specific object type.
		/// Format: {1} instances of {0}</remark>
		FrequencyTypedOccurrences,
		/// <summary>The 'FrequencyUntypedOccurrences' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used to specify an instance count or range using an untyped occurrence phrase
		/// Format: {0} times</remark>
		FrequencyUntypedOccurrences,
		/// <summary>The 'FullFactTypeDerivation' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to specify a fully derived fact type with its derivation rule.
		/// Format: *{0} if and only if {1}</remark>
		FullFactTypeDerivation,
		/// <summary>The 'FullSubtypeDerivation' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to specify a fully derived subtype derivation rule.
		/// Format: *each {0} is {1}</remark>
		FullSubtypeDerivation,
		/// <summary>The 'GroupEquality' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Assert group equality. Format: all or none of the following hold: {0}</remark>
		GroupEquality,
		/// <summary>The 'GroupExclusion' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Assert group exclusion. Format: at most one of the following holds: {0}
		/// 						</remark>
		GroupExclusion,
		/// <summary>The 'GroupExclusiveOr' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Assert group exclusive-or. Format: exactly one of the following holds: {0}</remark>
		GroupExclusiveOr,
		/// <summary>The 'HeadVariableProjection' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Project a calculation or constant value onto a head variable.
		/// Format: {0} = {1}</remark>
		HeadVariableProjection,
		/// <summary>The 'HyphenBoundPredicatePart' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Format string to combine predicate text from hyphen binding with the replacement field. Format: {0}{{0}}{1}</remark>
		HyphenBoundPredicatePart,
		/// <summary>The 'IdentityEqualityListClose' simple snippet value.</summary>
		/// <remark>Description: Text that is at the end of an identity equality list.</remark>
		IdentityEqualityListClose,
		/// <summary>The 'IdentityEqualityListFinalSeparator' simple snippet value.</summary>
		/// <remark>Description: Used between the last two items in an identity equality list.  Format: that is a</remark>
		IdentityEqualityListFinalSeparator,
		/// <summary>The 'IdentityEqualityListOpen' simple snippet value.</summary>
		/// <remark>Description: Text that is at the beginning of an identity equality list.</remark>
		IdentityEqualityListOpen,
		/// <summary>The 'IdentityEqualityListPairSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in an identity equality list.  Format: that is a</remark>
		IdentityEqualityListPairSeparator,
		/// <summary>The 'IdentityEqualityListSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in an identity equality list.  Format: that is a</remark>
		IdentityEqualityListSeparator,
		/// <summary>The 'IdentityReferenceQuantifier' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used for 'the same' before an object name to signify exactly which object is being reference.  Format: the same {0}</remark>
		IdentityReferenceQuantifier,
		/// <summary>The 'ImpersonalIdentityCorrelation' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Relate two variables of different types that represent the same instance where the first
		/// variable is an impersonal object type.
		/// Format: {0} that is {1}</remark>
		ImpersonalIdentityCorrelation,
		/// <summary>The 'ImpersonalLeadIdentityCorrelation' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: A form of ImpersonalIdentityCorrelation for lead role scenarios. Relate two variables of different
		/// types that represent the same instance where the first variable is an impersonal object type.
		/// Format:  {1} is {0} that</remark>
		ImpersonalLeadIdentityCorrelation,
		/// <summary>The 'ImpersonalPronoun' simple snippet value.</summary>
		/// <remark>Description: Use in place of a role player name to reference an impersonal object type with a clear antecedent.</remark>
		ImpersonalPronoun,
		/// <summary>The 'ImpliedModalNecessityOperator' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used to define the strength of the constraint.  Alethic, positive adds nothing before an object name.  Deontic, positive adds 'it is obligatory that' before an object name.  Alethic, negative adds 'it is impossible that' before an object name.  Deontic, negative adds 'it is forbidden that' before an object name.
		/// Format for alethic and positive: {0}  Format for deontic and positive: it is obligatory that {0}  Format for alethic and negative: it is impossible that {0}  Format for deontic and negative: it is forbidden that {0}</remark>
		ImpliedModalNecessityOperator,
		/// <summary>The 'IndentedCompoundListClose' simple snippet value.</summary>
		/// <remark>Description: Text that is at the end of an indented compound list.</remark>
		IndentedCompoundListClose,
		/// <summary>The 'IndentedCompoundListFinalSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate the last two items in an indented compound list.  Format: ;</remark>
		IndentedCompoundListFinalSeparator,
		/// <summary>The 'IndentedCompoundListOpen' simple snippet value.</summary>
		/// <remark>Description: Text that is at the beginning of an indented compound list.  Format: \n</remark>
		IndentedCompoundListOpen,
		/// <summary>The 'IndentedCompoundListPairSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in an indented compound list.  Format: ;</remark>
		IndentedCompoundListPairSeparator,
		/// <summary>The 'IndentedCompoundListSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in an indented compound list.  Format: ;</remark>
		IndentedCompoundListSeparator,
		/// <summary>The 'IndentedListClose' simple snippet value.</summary>
		/// <remark>Description: Text used at the end of an indented list.</remark>
		IndentedListClose,
		/// <summary>The 'IndentedListFinalSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate the last two items in an indented list.  Format: and</remark>
		IndentedListFinalSeparator,
		/// <summary>The 'IndentedListOpen' simple snippet value.</summary>
		/// <remark>Description: Text used at the beginning of an indented list.  Format: \n</remark>
		IndentedListOpen,
		/// <summary>The 'IndentedListPairSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in an indented list.  Format: and</remark>
		IndentedListPairSeparator,
		/// <summary>The 'IndentedListSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in an indented list.  Format: and</remark>
		IndentedListSeparator,
		/// <summary>The 'IndentedLogicalAndListClose' simple snippet value.</summary>
		/// <remark>Description: Text used at the end of an indented logical and list.</remark>
		IndentedLogicalAndListClose,
		/// <summary>The 'IndentedLogicalAndListFinalSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate the last two items in an indented logical and list.  Format: and that</remark>
		IndentedLogicalAndListFinalSeparator,
		/// <summary>The 'IndentedLogicalAndListOpen' simple snippet value.</summary>
		/// <remark>Description: Text used at the beginning of an indented logical and list.  Format: \n</remark>
		IndentedLogicalAndListOpen,
		/// <summary>The 'IndentedLogicalAndListPairSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in an indented logical and list.  Format: and that</remark>
		IndentedLogicalAndListPairSeparator,
		/// <summary>The 'IndentedLogicalAndListSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in an indented logical and list.  Format: and that</remark>
		IndentedLogicalAndListSeparator,
		/// <summary>The 'IndentedLogicalOrListClose' simple snippet value.</summary>
		/// <remark>Description: Text used at the end of an indented logical or list.</remark>
		IndentedLogicalOrListClose,
		/// <summary>The 'IndentedLogicalOrListFinalSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate the last two items in an indented logical or list.  Format: or</remark>
		IndentedLogicalOrListFinalSeparator,
		/// <summary>The 'IndentedLogicalOrListOpen' simple snippet value.</summary>
		/// <remark>Description: Text used at the beginning of an indented logical or list.  Format: \n</remark>
		IndentedLogicalOrListOpen,
		/// <summary>The 'IndentedLogicalOrListPairSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in an indented logical or list.  Format: or</remark>
		IndentedLogicalOrListPairSeparator,
		/// <summary>The 'IndentedLogicalOrListSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in an indented logical or list.  Format: or</remark>
		IndentedLogicalOrListSeparator,
		/// <summary>The 'IndependentVerbalization' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Verbalizes an object as being independent. Format: {0} is independent (it may have instances that play no other roles)</remark>
		IndependentVerbalization,
		/// <summary>The 'InQuantifier' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to verbalize [RolePlayer in Fact], e.g. Person in Person has Age.
		/// Format: {0} in {1}</remark>
		InQuantifier,
		/// <summary>The 'IsIdentifiedBy' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to specify that one instance is identified by the other instance.
		/// Format: {0} is identified by {1}</remark>
		IsIdentifiedBy,
		/// <summary>The 'LogicalAndOperator' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to 'and' together exactly two other snippets.
		/// Format: {0} and {1}</remark>
		LogicalAndOperator,
		/// <summary>The 'MinClosedMaxClosed' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to verbalize a range inclusion with the minimum bound closed and the maximum bound closed.
		/// Format: at least {0} to at most {1}</remark>
		MinClosedMaxClosed,
		/// <summary>The 'MinClosedMaxOpen' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to verbalize a range inclusion with the minimum bound closed and the maximum bound open.
		/// Format: at least {0} to below {1}</remark>
		MinClosedMaxOpen,
		/// <summary>The 'MinClosedMaxUnbounded' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used to verbalize a range inclusion with the minimum bound closed and the maximum unbounded.
		/// Format: at least {0}</remark>
		MinClosedMaxUnbounded,
		/// <summary>The 'MinOpenMaxClosed' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to verbalize a range inclusion with the minimum bound open and the maximum bound closed.
		/// Format: above {0} to at most {1}</remark>
		MinOpenMaxClosed,
		/// <summary>The 'MinOpenMaxOpen' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to verbalize a range inclusion with the minimum bound open and the maximum bound open.
		/// Format: above {0} to below {1}</remark>
		MinOpenMaxOpen,
		/// <summary>The 'MinOpenMaxUnbounded' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used to verbalize a range inclusion with the minimum bound open and the maximum unbounded.
		/// Format: above {0}</remark>
		MinOpenMaxUnbounded,
		/// <summary>The 'MinUnboundedMaxClosed' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used to verbalize a range inclusion with the minimum unbounded and the maximum bound closed.
		/// Format: at most {1}</remark>
		MinUnboundedMaxClosed,
		/// <summary>The 'MinUnboundedMaxOpen' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used to verbalize a range inclusion with the minimum unbounded and the maximum bound open.
		/// Format: below {1}</remark>
		MinUnboundedMaxOpen,
		/// <summary>The 'ModalNecessityOperator' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used to define the strength of the constraint.  Alethic adds 'it is necessary that' before  an object name.  Deontic adds 'it is obligatory that' before an object name.
		/// Format for alethic: it is necessary that {0}  Format for deontic: it is obligatory that {0}</remark>
		ModalNecessityOperator,
		/// <summary>The 'ModalPossibilityOperator' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used to define the strength of the constraint.  Alethic, positive adds 'it is possible that' before an object name.  Deontic, positive adds 'it is permitted that' before an object name.  Alethic, negative adds 'it is impossible that' before an object name.  Deontic, negative adds 'it is forbidden that' before an object name.
		/// Format for alethic and positive: it is possible that {0}  Format for deontic and positive: it is permitted that {0}  Format for alethic and negative: it is impossible that {0}  Format for deontic and negative: it is forbidden that {0}</remark>
		ModalPossibilityOperator,
		/// <summary>The 'ModelVerbalization' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Format string to verbalize the model element. Replacement field {0} is the Model name, and {1} is the guid id for the model.</remark>
		ModelVerbalization,
		/// <summary>The 'MoreThanOneQuantifier' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used for 'more than one' before an object name to define quantity as more than one.  Format: more than one {0}</remark>
		MoreThanOneQuantifier,
		/// <summary>The 'MultilineIndentedCompoundListClose' simple snippet value.</summary>
		/// <remark>Description: Text used at the end of an indented compound list.  Format:</remark>
		MultilineIndentedCompoundListClose,
		/// <summary>The 'MultilineIndentedCompoundListFinalSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate the last two items in an indented compound list.  Format: ;</remark>
		MultilineIndentedCompoundListFinalSeparator,
		/// <summary>The 'MultilineIndentedCompoundListOpen' simple snippet value.</summary>
		/// <remark>Description: Text used at the beginning of an indented compound list with one item on each line.</remark>
		MultilineIndentedCompoundListOpen,
		/// <summary>The 'MultilineIndentedCompoundListPairSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in an indented compound list.  Format: ;</remark>
		MultilineIndentedCompoundListPairSeparator,
		/// <summary>The 'MultilineIndentedCompoundListSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in an indented compound list.  Format: ;</remark>
		MultilineIndentedCompoundListSeparator,
		/// <summary>The 'MultiValueValueConstraint' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to verbalize a value range constraint with multiple values.
		/// Format: the possible values of {0} are {1}</remark>
		MultiValueValueConstraint,
		/// <summary>The 'NegatedAndLeadListClose' simple snippet value.</summary>
		/// <remark/>
		NegatedAndLeadListClose,
		/// <summary>The 'NegatedAndLeadListOpen' simple snippet value.</summary>
		/// <remark/>
		NegatedAndLeadListOpen,
		/// <summary>The 'NegatedAndLeadListSeparator' simple snippet value.</summary>
		/// <remark/>
		NegatedAndLeadListSeparator,
		/// <summary>The 'NegatedAndNestedListClose' simple snippet value.</summary>
		/// <remark/>
		NegatedAndNestedListClose,
		/// <summary>The 'NegatedAndNestedListOpen' simple snippet value.</summary>
		/// <remark/>
		NegatedAndNestedListOpen,
		/// <summary>The 'NegatedAndNestedListCollapsedOpen' simple snippet value.</summary>
		/// <remark/>
		NegatedAndNestedListCollapsedOpen,
		/// <summary>The 'NegatedAndNestedListSeparator' simple snippet value.</summary>
		/// <remark/>
		NegatedAndNestedListSeparator,
		/// <summary>The 'NegatedAndTailListClose' simple snippet value.</summary>
		/// <remark/>
		NegatedAndTailListClose,
		/// <summary>The 'NegatedAndTailListOpen' simple snippet value.</summary>
		/// <remark/>
		NegatedAndTailListOpen,
		/// <summary>The 'NegatedAndTailListCollapsedOpen' simple snippet value.</summary>
		/// <remark/>
		NegatedAndTailListCollapsedOpen,
		/// <summary>The 'NegatedAndTailListSeparator' simple snippet value.</summary>
		/// <remark/>
		NegatedAndTailListSeparator,
		/// <summary>The 'NegatedChainedListClose' simple snippet value.</summary>
		/// <remark/>
		NegatedChainedListClose,
		/// <summary>The 'NegatedChainedListOpen' simple snippet value.</summary>
		/// <remark/>
		NegatedChainedListOpen,
		/// <summary>The 'NegatedOrLeadListClose' simple snippet value.</summary>
		/// <remark/>
		NegatedOrLeadListClose,
		/// <summary>The 'NegatedOrLeadListOpen' simple snippet value.</summary>
		/// <remark/>
		NegatedOrLeadListOpen,
		/// <summary>The 'NegatedOrLeadListSeparator' simple snippet value.</summary>
		/// <remark/>
		NegatedOrLeadListSeparator,
		/// <summary>The 'NegatedOrNestedListClose' simple snippet value.</summary>
		/// <remark/>
		NegatedOrNestedListClose,
		/// <summary>The 'NegatedOrNestedListOpen' simple snippet value.</summary>
		/// <remark/>
		NegatedOrNestedListOpen,
		/// <summary>The 'NegatedOrNestedListCollapsedOpen' simple snippet value.</summary>
		/// <remark/>
		NegatedOrNestedListCollapsedOpen,
		/// <summary>The 'NegatedOrNestedListSeparator' simple snippet value.</summary>
		/// <remark/>
		NegatedOrNestedListSeparator,
		/// <summary>The 'NegatedOrTailListClose' simple snippet value.</summary>
		/// <remark/>
		NegatedOrTailListClose,
		/// <summary>The 'NegatedOrTailListOpen' simple snippet value.</summary>
		/// <remark/>
		NegatedOrTailListOpen,
		/// <summary>The 'NegatedOrTailListCollapsedOpen' simple snippet value.</summary>
		/// <remark/>
		NegatedOrTailListCollapsedOpen,
		/// <summary>The 'NegatedOrTailListSeparator' simple snippet value.</summary>
		/// <remark/>
		NegatedOrTailListSeparator,
		/// <summary>The 'NegatedVariableExistence' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Assert variable non-existence for an a previous declared variable in an existence statement.
		/// Note that VariableExistence can be used with a negated quantifier if the negated variable has not been previously introduced.
		/// Format: {0} does not exist</remark>
		NegatedVariableExistence,
		/// <summary>The 'NegatedXorLeadListClose' simple snippet value.</summary>
		/// <remark/>
		NegatedXorLeadListClose,
		/// <summary>The 'NegatedXorLeadListOpen' simple snippet value.</summary>
		/// <remark/>
		NegatedXorLeadListOpen,
		/// <summary>The 'NegatedXorLeadListSeparator' simple snippet value.</summary>
		/// <remark/>
		NegatedXorLeadListSeparator,
		/// <summary>The 'NegatedXorNestedListClose' simple snippet value.</summary>
		/// <remark/>
		NegatedXorNestedListClose,
		/// <summary>The 'NegatedXorNestedListOpen' simple snippet value.</summary>
		/// <remark/>
		NegatedXorNestedListOpen,
		/// <summary>The 'NegatedXorNestedListCollapsedOpen' simple snippet value.</summary>
		/// <remark/>
		NegatedXorNestedListCollapsedOpen,
		/// <summary>The 'NegatedXorNestedListSeparator' simple snippet value.</summary>
		/// <remark/>
		NegatedXorNestedListSeparator,
		/// <summary>The 'NegatedXorTailListClose' simple snippet value.</summary>
		/// <remark/>
		NegatedXorTailListClose,
		/// <summary>The 'NegatedXorTailListOpen' simple snippet value.</summary>
		/// <remark/>
		NegatedXorTailListOpen,
		/// <summary>The 'NegatedXorTailListCollapsedOpen' simple snippet value.</summary>
		/// <remark/>
		NegatedXorTailListCollapsedOpen,
		/// <summary>The 'NegatedXorTailListSeparator' simple snippet value.</summary>
		/// <remark/>
		NegatedXorTailListSeparator,
		/// <summary>The 'NegativeReadingForUnaryOnlyDisjunctiveMandatory' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to verbalize that an object is not included in a set.  Format: some {0} participates in none of the following:{1}</remark>
		NegativeReadingForUnaryOnlyDisjunctiveMandatory,
		/// <summary>The 'NonTextInstanceValue' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used to output a non-text instance value. Format: {0}</remark>
		NonTextInstanceValue,
		/// <summary>The 'NotesVerbalization' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Verbalizes the text specified for a model note. Format: Notes: {0}</remark>
		NotesVerbalization,
		/// <summary>The 'ObjectifiesFactTypeVerbalization' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Verbalizes the objectification made for a FactType. Format: {0} objectifies "{1}"</remark>
		ObjectifiesFactTypeVerbalization,
		/// <summary>The 'ObjectType' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Verbalizes an object. {0} is the name, {1} is the guid id for the element</remark>
		ObjectType,
		/// <summary>The 'ObjectTypeInstanceListClose' simple snippet value.</summary>
		/// <remark>Description: Text used to close the sample population verbalization list. Format: .</remark>
		ObjectTypeInstanceListClose,
		/// <summary>The 'ObjectTypeInstanceListFinalSeparator' simple snippet value.</summary>
		/// <remark>Description: Text used to separate the last two items in a sample population verbalization list, or object types. Format: ,</remark>
		ObjectTypeInstanceListFinalSeparator,
		/// <summary>The 'ObjectTypeInstanceListOpen' simple snippet value.</summary>
		/// <remark> Description:  Text and formatting to begin a sample population verbalization of object types block. Format: Examples:</remark>
		ObjectTypeInstanceListOpen,
		/// <summary>The 'ObjectTypeInstanceListPairSeparator' simple snippet value.</summary>
		/// <remark>Description: Text used to separate items in a sample population verbalization list. Format: , </remark>
		ObjectTypeInstanceListPairSeparator,
		/// <summary>The 'ObjectTypeInstanceListSeparator' simple snippet value.</summary>
		/// <remark>Description: Text used to separate items in a sample population verbalization list.	Format: ,</remark>
		ObjectTypeInstanceListSeparator,
		/// <summary>The 'ObjectTypeMissing' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Verbalizes an object type as being missing.</remark>
		ObjectTypeMissing,
		/// <summary>The 'ObjectTypeWithSubscript' format string snippet. Contains 3 replacement fields.</summary>
		/// <remark>Description: Verbalizes an object type with a subscript. {0} is the name, {1} is the guid id for the element, {2} is the subscript</remark>
		ObjectTypeWithSubscript,
		/// <summary>The 'OccursInPopulation' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Verbalizes that an object occurs at most once or more than once in a given population.
		/// Format positive: in each population of {1}, {0} occurs at most once
		/// Format negative: {0} occurs more than once in the same population of {1}</remark>
		OccursInPopulation,
		/// <summary>The 'OneQuantifier' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used for 'at most one' or 'more than one' before an object.
		/// Format positive: at most one {0}
		/// Format negative: more than one {0}
		/// 						</remark>
		OneQuantifier,
		/// <summary>The 'OrLeadListClose' simple snippet value.</summary>
		/// <remark/>
		OrLeadListClose,
		/// <summary>The 'OrLeadListOpen' simple snippet value.</summary>
		/// <remark/>
		OrLeadListOpen,
		/// <summary>The 'OrLeadListSeparator' simple snippet value.</summary>
		/// <remark/>
		OrLeadListSeparator,
		/// <summary>The 'OrNestedListClose' simple snippet value.</summary>
		/// <remark/>
		OrNestedListClose,
		/// <summary>The 'OrNestedListOpen' simple snippet value.</summary>
		/// <remark/>
		OrNestedListOpen,
		/// <summary>The 'OrNestedListCollapsedOpen' simple snippet value.</summary>
		/// <remark/>
		OrNestedListCollapsedOpen,
		/// <summary>The 'OrNestedListSeparator' simple snippet value.</summary>
		/// <remark/>
		OrNestedListSeparator,
		/// <summary>The 'OrTailListClose' simple snippet value.</summary>
		/// <remark/>
		OrTailListClose,
		/// <summary>The 'OrTailListOpen' simple snippet value.</summary>
		/// <remark/>
		OrTailListOpen,
		/// <summary>The 'OrTailListCollapsedOpen' simple snippet value.</summary>
		/// <remark/>
		OrTailListCollapsedOpen,
		/// <summary>The 'OrTailListSeparator' simple snippet value.</summary>
		/// <remark/>
		OrTailListSeparator,
		/// <summary>The 'PartialFactTypeDerivation' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to specify a partially derived fact type with its derivation rule.
		/// Format: +{0} if {1}</remark>
		PartialFactTypeDerivation,
		/// <summary>The 'PartialSubtypeDerivation' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to specify a partially derived subtype derivation rule.
		/// Format: +each derived {0} is {1}</remark>
		PartialSubtypeDerivation,
		/// <summary>The 'PeriodSeparator' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to delimit two snippet replacements by a snippet.
		/// Format:	{0}.{1}</remark>
		PeriodSeparator,
		/// <summary>The 'PersonalIdentityCorrelation' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Relate two variable names of different types that represent the same instance where the first
		/// variable is a personal object type.
		/// Format:  {0} who is {1}</remark>
		PersonalIdentityCorrelation,
		/// <summary>The 'PersonalLeadIdentityCorrelation' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: A form of PersonalIdentityCorrelation for lead role scenarios. Relate two variables of
		/// different types that represent the same instance where the first variable is an personal object type.
		/// Format: {1} is {0} who</remark>
		PersonalLeadIdentityCorrelation,
		/// <summary>The 'PersonalPronoun' simple snippet value.</summary>
		/// <remark>Description: Use in place of a role player name to reference a personal object type with a clear antecedent.</remark>
		PersonalPronoun,
		/// <summary>The 'PluralExistenceImplicationOperator' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Verbalizes an existence condition for a multiple items and a consequent.  Format: there are {0} such that {1}</remark>
		PluralExistenceImplicationOperator,
		/// <summary>The 'PortableDataTypeVerbalization' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Verbalizes an object's portable data type. Format: Portable data type: {0}</remark>
		PortableDataTypeVerbalization,
		/// <summary>The 'PredicatePart' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Format string to decorate predicate text in between replacement fields. Must contain a {{0}}. Replacement field {0} is the FactType name, and {1} is the guid id for the element.</remark>
		PredicatePart,
		/// <summary>The 'QueryNamedParameter' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Describe a query parameter with an associated name.
		/// Format: {0}=name, {1}=type name</remark>
		QueryNamedParameter,
		/// <summary>The 'QueryParameterContainer' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Outer container for listing query parameters. Used as part of a query verbalization.
		/// Format: given {0}</remark>
		QueryParameterContainer,
		/// <summary>The 'QueryUnnamedParameter' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Describe a query parameter with no associated name. Format: given {0}</remark>
		QueryUnnamedParameter,
		/// <summary>The 'QueryVerbalization' format string snippet. Contains 3 replacement fields.</summary>
		/// <remark>Description: Root verbalization for a query. Containers a parameter (defined with QueryParameterContainer if present) and projection lists plus the derivation rule.
		/// Format: {0}select {1} where {2}</remark>
		QueryVerbalization,
		/// <summary>The 'ReferenceModeVerbalization' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Verbalizes the mode in which an object is referenced. Format: Reference Mode: {0}</remark>
		ReferenceModeVerbalization,
		/// <summary>The 'ReferenceScheme' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to enclose and format a reference scheme replacement in brackets.
		/// 							Format:	{0}({1})</remark>
		ReferenceScheme,
		/// <summary>The 'ReferenceSchemeVerbalization' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Verbalizes the way in which an object is referenced. Format: Reference Scheme: {0}</remark>
		ReferenceSchemeVerbalization,
		/// <summary>The 'ReflexivePronoun' simple snippet value.</summary>
		/// <remark>Description: Use in place of a role player name to refer to an object type with a clear antecedent.</remark>
		ReflexivePronoun,
		/// <summary>The 'ReflexiveQuantifier' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used in place of a reflexive pronoun when an antecedent is not guaranteed.  Format: {0} itself</remark>
		ReflexiveQuantifier,
		/// <summary>The 'RolePathCollapsibleLeadDirective' simple snippet value.</summary>
		/// <remark>Description: Specify a space-separated list of items to determine if a list style supports
		/// collapsing a repeated lead role. Allowed values are {Chain, And, Or, Xor, !And, !Or, !Xor, !Chain}. The !Chain directive
		/// here applies to the long form of negation, not the inlined form where the verbose negation constructs are not used.
		/// A collapsed lead completely eliminates a lead role player. For example, '... Person has FirstName and has LastName',
		/// with Person eliminated from the second fact type rendering.
		/// Format: And Or Chain</remark>
		RolePathCollapsibleLeadDirective,
		/// <summary>The 'RolePathCollapsibleListOpenForBackReferenceDirective' simple snippet value.</summary>
		/// <remark>Description: Specify a space-separated list of items to determine if the first item in a list style supports
		/// collapsing to allow a back reference. The allowed values here are the same as the RolePathCollapsibleLeadDirective and
		/// should not intersect the values used for the RolePathHeaderListDirective. A back reference uses a personal or impersonal
		/// pronoun in place of a restatement of the lead role player. The back reference must immediately follow the preceding noun.
		/// This directive allows a backreference to be used by replacing the *[Tail|Nested]ListOpen snippets with the *[Tail|Nested]ListCollapsedOpen
		/// snippets. Lead list types do not support back referencing.
		/// Format: And Or Chain</remark>
		RolePathCollapsibleListOpenForBackReferenceDirective,
		/// <summary>The 'RolePathHeaderListDirective' simple snippet value.</summary>
		/// <remark>Description: Specify a space-separated list of items to determine if split lists are rendered as
		/// integrated or separate blocks. Allowed values are {And, Or, Xor, !And, !Or, !Xor}.
		/// Format: !And !Or Xor !Xor</remark>
		RolePathHeaderListDirective,
		/// <summary>The 'RolePathListCloseOutdentSnippets' simple snippet value.</summary>
		/// <remark>Description: A space separated list of list closure snippet names from this enum that reverse an indentation.
		/// Trailing outdents can be tracked specially during formatting so that external text or outer list separator and close
		/// elements on the same line as the outdent keeps the same indentation level.
		/// Format: ChainedListClose NegatedChainedListClose AndTailListClose AndNestedListClose NegatedAndLeadListClose NegatedAndTailListClose NegatedAndNestedListClose OrTailListClose OrNestedListClose NegatedOrLeadListClose NegatedOrTailListClose NegatedOrNestedListClose XorLeadListClose XorTailListClose XorNestedListClose NegatedXorLeadListClose NegatedXorTailListClose NegatedXorNestedListClose</remark>
		RolePathListCloseOutdentSnippets,
		/// <summary>The 'RolePathOutdentAwareTrailingListSnippets' simple snippet value.</summary>
		/// <remark>Description: A space separated list of list separators and close elements that must be placed before any
		/// active trailing outdent snippets.
		/// Format: NegatedAndLeadListSeparator NegatedAndNestedListSeparator NegatedAndTailListSeparator NegatedOrLeadListSeparator NegatedOrNestedListSeparator NegatedOrTailListSeparator XorLeadListSeparator XorNestedListSeparator XorTailListSeparator NegatedXorLeadListSeparator NegatedXorNestedListSeparator NegatedXorTailListSeparator</remark>
		RolePathOutdentAwareTrailingListSnippets,
		/// <summary>The 'SameTypeIdentityInequalityOperator' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to specify that two instances of the same type are not the same instance.
		/// Format: {0} is not {1}</remark>
		SameTypeIdentityInequalityOperator,
		/// <summary>The 'SelfReference' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Allows the snippet replacement to verbalize itself. Format: {0}</remark>
		SelfReference,
		/// <summary>The 'SimpleListClose' simple snippet value.</summary>
		/// <remark>Description: Text used at the end of a simple list.</remark>
		SimpleListClose,
		/// <summary>The 'SimpleListFinalSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate the final item in simple list. Format: , and</remark>
		SimpleListFinalSeparator,
		/// <summary>The 'SimpleListOpen' simple snippet value.</summary>
		/// <remark>Description:  Text used at the end of a simple list.</remark>
		SimpleListOpen,
		/// <summary>The 'SimpleListPairSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in simple list with two items. Format: and</remark>
		SimpleListPairSeparator,
		/// <summary>The 'SimpleListSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in simple list. Format: ,</remark>
		SimpleListSeparator,
		/// <summary>The 'SimpleLogicalAndListClose' simple snippet value.</summary>
		/// <remark>Description: Text used at the end of a simple logical and list.</remark>
		SimpleLogicalAndListClose,
		/// <summary>The 'SimpleLogicalAndListFinalSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate the last two items in a simple logical and list.  Format: and</remark>
		SimpleLogicalAndListFinalSeparator,
		/// <summary>The 'SimpleLogicalAndListOpen' simple snippet value.</summary>
		/// <remark>Description: Text used at the beginning of a simple logical and list.</remark>
		SimpleLogicalAndListOpen,
		/// <summary>The 'SimpleLogicalAndListPairSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in a simple logical and list.  Format: and</remark>
		SimpleLogicalAndListPairSeparator,
		/// <summary>The 'SimpleLogicalAndListSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in a simple logical and list.  Format: and</remark>
		SimpleLogicalAndListSeparator,
		/// <summary>The 'SimpleLogicalOrListClose' simple snippet value.</summary>
		/// <remark>Description:  Text used at the end of a simple logical or list.</remark>
		SimpleLogicalOrListClose,
		/// <summary>The 'SimpleLogicalOrListFinalSeparator' simple snippet value.</summary>
		/// <remark>Description:  Used to separate the last two items in a simple logical or list.  Format: or</remark>
		SimpleLogicalOrListFinalSeparator,
		/// <summary>The 'SimpleLogicalOrListOpen' simple snippet value.</summary>
		/// <remark>Description: Text used at the beginning of a simple logical or list.</remark>
		SimpleLogicalOrListOpen,
		/// <summary>The 'SimpleLogicalOrListPairSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in a simple logical or list.  Format: or</remark>
		SimpleLogicalOrListPairSeparator,
		/// <summary>The 'SimpleLogicalOrListSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in a simple logical or list.  Format: or</remark>
		SimpleLogicalOrListSeparator,
		/// <summary>The 'SingleValueValueConstraint' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to verbalize a value range constraint with one range, where the min value is equal to the max value.
		/// Format: the possible value of {0} is {1}</remark>
		SingleValueValueConstraint,
		/// <summary>The 'SingularExistenceImplicationOperator' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Verbalizes an existence condition for a single item and a consequent.  Format: there is {0} such that {1}</remark>
		SingularExistenceImplicationOperator,
		/// <summary>The 'StronglyIntransitiveConsequent' format string snippet. Contains 2 replacement fields.</summary>
		/// <remark>Description: Used to verbalize a strongly intransitive ring constraint.
		/// Format: it is not true that {0} is indirectly related to {1} by repeatedly applying this fact type</remark>
		StronglyIntransitiveConsequent,
		/// <summary>The 'SubtypeMetaReading' format string snippet. Contains 3 replacement fields.</summary>
		/// <remark>Description: Used to describe the relationship between a Subtype and its Supertype at the meta level (i.e: Each Man is an instance of Person).
		/// Format: {0}=subtype, {1}=supertype, {2}=SubtypeFact identifier</remark>
		SubtypeMetaReading,
		/// <summary>The 'TextInstanceValue' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used to output a text instance value. Format: '{0}'</remark>
		TextInstanceValue,
		/// <summary>The 'TopLevelIndentedLogicalAndListClose' simple snippet value.</summary>
		/// <remark>Description: Text used at the end of a top level indented logical and list.</remark>
		TopLevelIndentedLogicalAndListClose,
		/// <summary>The 'TopLevelIndentedLogicalAndListFinalSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate the last two items in a top level indented logical and list.  Format: \n and that</remark>
		TopLevelIndentedLogicalAndListFinalSeparator,
		/// <summary>The 'TopLevelIndentedLogicalAndListOpen' simple snippet value.</summary>
		/// <remark>Description: Text used at the beginning of a top level indented logical and list.</remark>
		TopLevelIndentedLogicalAndListOpen,
		/// <summary>The 'TopLevelIndentedLogicalAndListPairSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in a top level indented logical and list.  Format: \n and that</remark>
		TopLevelIndentedLogicalAndListPairSeparator,
		/// <summary>The 'TopLevelIndentedLogicalAndListSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in a top level indented logical and list.  Format: \n and that</remark>
		TopLevelIndentedLogicalAndListSeparator,
		/// <summary>The 'TopLevelIndentedLogicalOrListClose' simple snippet value.</summary>
		/// <remark>Description: Text used at the end of a top level indented logical or list.</remark>
		TopLevelIndentedLogicalOrListClose,
		/// <summary>The 'TopLevelIndentedLogicalOrListFinalSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate the last two items in a top level indented logical or list.  Format: \n or</remark>
		TopLevelIndentedLogicalOrListFinalSeparator,
		/// <summary>The 'TopLevelIndentedLogicalOrListOpen' simple snippet value.</summary>
		/// <remark>Description: Text used at the beginning of a top level indented logical or list.</remark>
		TopLevelIndentedLogicalOrListOpen,
		/// <summary>The 'TopLevelIndentedLogicalOrListPairSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in a top level indented logical or list.  Format: \n or</remark>
		TopLevelIndentedLogicalOrListPairSeparator,
		/// <summary>The 'TopLevelIndentedLogicalOrListSeparator' simple snippet value.</summary>
		/// <remark>Description: Used to separate items in a top level indented logical or list.  Format: \n or</remark>
		TopLevelIndentedLogicalOrListSeparator,
		/// <summary>The 'UniversalQuantifier' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Used for 'each' before an object name to signify the quantity associated with the object.  Format: each {0}</remark>
		UniversalQuantifier,
		/// <summary>The 'ValueTypeVerbalization' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Verbalizes an object as being a value type. Format: {0} is a value type</remark>
		ValueTypeVerbalization,
		/// <summary>The 'VariableExistence' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Assert variable existence as a complete statement, as opposed to the
		/// VariableIntroductionClause used to introduce a variable using a prefix.
		/// Format: {0} exists</remark>
		VariableExistence,
		/// <summary>The 'VariableIntroductionClause' format string snippet. Contains 1 replacement field.</summary>
		/// <remark>Description: Introduce variables inline in the verbalization phrase. The replacement is either
		/// a single value or a list, and the quantifiers (some, no, that) are already included in the replacement list.
		/// Format: for {0},</remark>
		VariableIntroductionClause,
		/// <summary>The 'VariableIntroductionSeparator' simple snippet value.</summary>
		/// <remark>Description: The list separator for introducing multiple variables in a single clause.
		/// Format: and</remark>
		VariableIntroductionSeparator,
		/// <summary>The 'VerbalizerCloseVerbalization' simple snippet value.</summary>
		/// <remark>Description: Used by the verbalizer to close a verbalization opened by VerbalizerOpenVerbalization</remark>
		VerbalizerCloseVerbalization,
		/// <summary>The 'VerbalizerDecreaseIndent' simple snippet value.</summary>
		/// <remark>Description: Used by the verbalizer to close indent opened by VerbalizerIncreaseIndent</remark>
		VerbalizerDecreaseIndent,
		/// <summary>The 'VerbalizerDocumentFooter' simple snippet value.</summary>
		/// <remark>Description: Used by the verbalizer to close a document</remark>
		VerbalizerDocumentFooter,
		/// <summary>The 'VerbalizerDocumentHeader' format string snippet. Contains 14 replacement fields.</summary>
		/// <remark>Description: Used by the verbalizer to open a document. The document header gets replacement fields in the following order:
		/// {0} font-family
		/// {1} font-size
		/// {2} predicate text color
		/// {3} predicate text bold
		/// {4} object name color
		/// {5} object name bold
		/// {6} formal item color
		/// {7} formal item bold
		/// {8} note color
		/// {9} note bold
		/// {10} reference mode color
		/// {11} reference mode bold
		/// {12} instance value color
		/// {13} instance value bold</remark>
		VerbalizerDocumentHeader,
		/// <summary>The 'VerbalizerFontWeightBold' simple snippet value.</summary>
		/// <remark>Description: The text to insert to indicate a bold font.</remark>
		VerbalizerFontWeightBold,
		/// <summary>The 'VerbalizerFontWeightNormal' simple snippet value.</summary>
		/// <remark>Description: The text to insert to indicate a normal font weight.</remark>
		VerbalizerFontWeightNormal,
		/// <summary>The 'VerbalizerIncreaseIndent' simple snippet value.</summary>
		/// <remark>Description: Used by the verbalizer to indent a child verbalization</remark>
		VerbalizerIncreaseIndent,
		/// <summary>The 'VerbalizerNewLine' simple snippet value.</summary>
		/// <remark>Description: Used by the verbalizer to enter a new line. Format: \n</remark>
		VerbalizerNewLine,
		/// <summary>The 'VerbalizerOpenVerbalization' simple snippet value.</summary>
		/// <remark>Description: Used by the verbalizer around a single verbalization</remark>
		VerbalizerOpenVerbalization,
		/// <summary>The 'XorLeadListClose' simple snippet value.</summary>
		/// <remark/>
		XorLeadListClose,
		/// <summary>The 'XorLeadListOpen' simple snippet value.</summary>
		/// <remark/>
		XorLeadListOpen,
		/// <summary>The 'XorLeadListSeparator' simple snippet value.</summary>
		/// <remark/>
		XorLeadListSeparator,
		/// <summary>The 'XorNestedListClose' simple snippet value.</summary>
		/// <remark/>
		XorNestedListClose,
		/// <summary>The 'XorNestedListOpen' simple snippet value.</summary>
		/// <remark/>
		XorNestedListOpen,
		/// <summary>The 'XorNestedListCollapsedOpen' simple snippet value.</summary>
		/// <remark/>
		XorNestedListCollapsedOpen,
		/// <summary>The 'XorNestedListSeparator' simple snippet value.</summary>
		/// <remark/>
		XorNestedListSeparator,
		/// <summary>The 'XorTailListClose' simple snippet value.</summary>
		/// <remark/>
		XorTailListClose,
		/// <summary>The 'XorTailListOpen' simple snippet value.</summary>
		/// <remark/>
		XorTailListOpen,
		/// <summary>The 'XorTailListCollapsedOpen' simple snippet value.</summary>
		/// <remark/>
		XorTailListCollapsedOpen,
		/// <summary>The 'XorTailListSeparator' simple snippet value.</summary>
		/// <remark/>
		XorTailListSeparator,
		/// <summary>The last item in CoreVerbalizationSnippetType</summary>
		Last = CoreVerbalizationSnippetType.XorTailListSeparator,
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
				@"<span class=""quantifier"">no {0} may cycle back to itself via one or more traversals through </span>{1}",
				@"<span class=""quantifier"">no {0} may cycle back to itself via one or more instances of the role pair:</span><br/><span class=""smallIndent""><span class=""quantifier"">roles <span class=""objectType"">{2}</span> and <span class=""objectType"">{3}</span> of </span>{1}</span>",
				@"<span class=""quantifier"">each</span> {0}",
				@"<span class=""quantifier""> combination</span>",
				@"<span class=""quantifier"">each unique </span>",
				@"<span class=""listSeparator"">, </span>",
				@"{0} <span class=""quantifier"">for </span> {1}",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">each distinct</span> {0}",
				"",
				"",
				@"<br/><span class=""quantifier"">and</span> ",
				"</span>",
				"<span>",
				"<span>",
				@"</span><br/><span class=""smallIndent""><span class=""quantifier"">and</span> ",
				"</span>",
				@"<br/><span class=""smallIndent""><span class=""quantifier"">and</span> ",
				@"<br/><span class=""smallIndent"">",
				@"<br/><span class=""quantifier"">and</span> ",
				@"<span class=""quantifier"">at most one</span> {0}",
				@"<span class=""quantifier"">at most one</span> {0}",
				"</span>",
				" ",
				@"<br/></span><span class=""smallIndent"">",
				@"<br/></span><span class=""smallIndent""><span class=""quantifier"">and</span> ",
				@"<br/></span><span class=""smallIndent""><span class=""quantifier"">where</span> ",
				" ",
				@" <span class=""quantifier"">where</span> ",
				"<span>",
				"<br/>",
				@"<br/><span class=""quantifier"">and</span> ",
				@"<br/><span class=""quantifier"">where</span> ",
				@"<span class=""listSeparator"">.</span>",
				@"{0} <span class=""quantifier"">combination</span>",
				@"{0} <span class=""quantifier"">combination is unique</span>",
				@"{0} <span class=""instance"">{1}</span>",
				"Missing {0}",
				"",
				@"<span class=""listSeparator"">, </span>",
				"",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""logicalOperator"">that </span>{0}<span class=""logicalOperator""> is not that </span>{1}",
				"",
				"; ",
				"",
				"; ",
				"; ",
				@"<span class=""quantifier"">if </span>{0}<span class=""quantifier""> then </span>{1}",
				@"<span class=""quantifier"">if </span>{0}<br/><span class=""quantifier"">then </span>{1}",
				@"<span class=""quantifier"">if </span>{0}<br/><span class=""smallIndent""><span class=""quantifier"">then </span>{1}</span>",
				@"<span class=""smallIndent""><span class=""quantifier"">this association with</span> {0} <span class=""quantifier"">provides the preferred identification scheme for</span> {1}</span>",
				@"<span class=""quantifier"">that combination is associated with</span> {0} <span class=""quantifier"">in this context</span>",
				@"<span class=""quantifier"">that combination occurs</span> {0} <span class=""quantifier"">in this context</span>",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">Derivation Note:</span> <span class=""definition"">{0}</span>",
				@"<span class=""quantifier"">Informal Description:</span> <span class=""definition"">{0}</span>",
				@"{0} <span class=""quantifier"">is an entity type</span>",
				@"{0}<span class=""quantifier""> if and only if </span>{1}",
				"",
				@"<span class=""logicalOperator""> = </span>",
				"",
				@"<span class=""logicalOperator""> = </span>",
				@"<span class=""logicalOperator""> = </span>",
				"</span>",
				"</span>",
				@"<span class=""primaryErrorReport"">",
				@"<span class=""secondaryErrorReport"">",
				@"Model Error: <a class=""primaryErrorReport"" href=""elementid:{1}"">{0}</a>",
				@"Model Error: <a class=""secondaryErrorReport"" href=""elementid:{1}"">{0}</a>",
				@"<span class=""quantifier"">exactly one</span> {0}",
				@"<span class=""quantifier"">exactly one</span> {0}",
				@"<span class=""quantifier"">exactly once</span>",
				@"<span class=""quantifier"">no</span> {0}<span class=""quantifier""> the same </span>{1}",
				"{0} {1}",
				@"<span class=""quantifier"">some</span> {0}",
				"</span>",
				@"<br/><span class=""quantifier"">Examples: </span><span class=""smallIndent"">",
				@"<span class=""smallIndent""><span class=""quantifier"">Identifier: <span class=""instance"">{0}</span></span></span>",
				"</span></span>",
				"<br/>",
				@"<br/><span class=""indent""><span class=""quantifier"">Fact Types:</span><span class=""smallIndent""><br/>",
				"<br/>",
				"<br/>",
				@"<span class=""quantifier"">for each</span> {0}, {1}",
				@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">for each</span> {0}, {1}",
				@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">either</span> <span class=""instance"">0</span><span class=""listSeparator"">, </span><span class=""quantifier""> or </span> {0}",
				@"<span class=""quantifier"">each {0} in the population of “{1}” occurs there {2}</span>",
				@"exactly <span class=""instance"">{0}</span>",
				@"at least <span class=""instance"">{0}</span>",
				@"at least <span class=""instance"">{0}</span> and at most <span class=""instance"">{1}</span>",
				@"at most <span class=""instance"">{1}</span>",
				@"<span class=""quantifier"">{1} combinations of</span> {0}",
				@"<span class=""quantifier"">{1} instances of</span> {0}",
				@"<span class=""quantifier"">{0} times</span>",
				@"<span class=""quantifier"">*</span>{0} <span class=""quantifier"">if and only if</span><br/>{1}<br/>",
				@"<span class=""quantifier"">*Each</span> {0} <span class=""quantifier"">is</span> {1}",
				@"<span class=""quantifier"">all or none of the following hold:</span> {0}",
				@"<span class=""quantifier"">at most one of the following holds:</span> {0}",
				@"<span class=""quantifier"">exactly one of the following holds:</span> {0}",
				@"{0} <span class=""logicalOperator"">=</span> {1}",
				"{0}{{0}}{1}",
				"",
				@"<span class=""logicalOperator""> that is a </span>",
				"",
				@"<span class=""logicalOperator""> that is a </span>",
				@"<span class=""logicalOperator""> that is a </span>",
				@"<span class=""quantifier"">the same</span> {0}",
				@"{0} <span class=""quantifier"">that is</span> {1}",
				@"{1} <span class=""quantifier"">is</span> {0} <span class=""quantifier"">that</span>",
				@"<span class=""quantifier"">that</span>",
				"{0}",
				"</span>",
				@"<span class=""listSeparator"">; </span>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">; </span>",
				@"<span class=""listSeparator"">; </span>",
				"</span>",
				@"<span class=""logicalOperator""> and </span><br/>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""logicalOperator""> and </span><br/>",
				@"<span class=""logicalOperator""> and </span><br/>",
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
				@"<span class=""smallIndent"">{0} <span class=""quantifier"">is independent (it may have instances that play no other roles)</span></span>",
				@"{0} <span class=""quantifier"">in</span> {1}",
				@"{0}<span class=""quantifier""> is identified by </span>{1}",
				@"{0}<span class=""logicalOperator""> and </span>{1}",
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
				@"<span class=""quantifier"">Object-Role Model:</span> {0}",
				@"<span class=""quantifier"">more than one</span> {0}",
				"</span>",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<span class=""quantifier"">the possible values of</span> {0} <span class=""quantifier"">are</span> {1}",
				"</span>",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"",
				@"<span class=""quantifier"">it is not true that </span>",
				"</span>",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				@"{0} <span class=""quantifier"">does not exist</span>",
				"</span>",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				@"<span class=""quantifier"">some</span> {0} <span class=""quantifier"">participates in none of the following:</span>{1}",
				@"<span class=""instance"">{0}</span>",
				@"<span class=""quantifier"">Notes:</span> <span class=""note"">{0}</span>",
				@"<span class=""smallIndent"">{0} <span class=""quantifier"">objectifies</span> ""{1}""</span>",
				@"<a class=""objectType"" href=""elementid:{1}"">{0}</a>",
				"",
				@"<span class=""listSeparator"">, </span>",
				@"<br/><span class=""quantifier"">Examples:</span> ",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""objectTypeMissing"">Missing<sub style=""font-size:smaller;"">{0}</sub></span>",
				@"<a class=""objectType"" href=""elementid:{1}"">{0}<sub style=""font-size:smaller;"">{2}</sub></a>",
				@"<span class=""quantifier"">in each population of</span> {1}<span class=""listSeparator"">, </span>{0} <span class=""quantifier"">occurs at most once</span>",
				@"<span class=""quantifier"">at most one</span> {0}",
				"",
				"",
				@"<br/><span class=""quantifier"">or</span> ",
				"</span>",
				"<span>",
				"<span>",
				@"</span><br/><span class=""smallIndent""><span class=""quantifier"">or</span> ",
				"</span>",
				@"<br/><span class=""smallIndent""><span class=""quantifier"">and</span> ",
				@"<br/><span class=""smallIndent"">",
				@"<br/><span class=""quantifier"">or</span> ",
				@"<span class=""quantifier"">+</span>{0} <span class=""quantifier"">if</span><br/>{1}<br/>",
				@"<span class=""quantifier"">+Each derived</span> {0} <span class=""quantifier"">is</span> {1}",
				@"{0}<span class=""listSeparator"">.</span>{1}",
				@"{0} <span class=""quantifier"">who is</span> {1}",
				@"{1} <span class=""quantifier"">is</span> {0} <span class=""quantifier"">who</span>",
				@"<span class=""quantifier"">who</span>",
				@"<span class=""quantifier"">there are </span>{0} <span class=""quantifier"">such that</span><br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""smallIndent""><span class=""quantifier"">Portable data type:</span> {0}</span>",
				@"<a class=""predicateText"" href=""elementid:{1}"">{{0}}</a>",
				@"{1}<span class=""logicalOperator"">=</span>{0}",
				@"<span class=""quantifier"">given</span> {0} ",
				"{0}",
				@"{0}<span class=""quantifier"">select</span> {1} where<br/><span class=""smallIndent"">{2}</span>",
				@"<span class=""smallIndent""><span class=""quantifier"">Reference Mode:</span> <span class=""referenceMode"">{0}</span></span>",
				@"{0}<span class=""listSeparator"">(</span><span class=""referenceMode"">{1}</span><span class=""listSeparator"">)</span>",
				@"<span class=""smallIndent""><span class=""quantifier"">Reference Scheme:</span> {0}</span>",
				@"<span class=""quantifier"">itself</span>",
				@"{0} <span class=""quantifier"">itself</span>",
				"And Or Chain",
				"And Or Chain",
				"!And !Or Xor !Xor",
				"ChainedListClose NegatedChainedListClose AndTailListClose AndNestedListClose NegatedAndLeadListClose NegatedAndTailListClose NegatedAndNestedListClose OrTailListClose OrNestedListClose NegatedOrLeadListClose NegatedOrTailListClose NegatedOrNestedListClose XorLeadListClose XorTailListClose XorNestedListClose NegatedXorLeadListClose NegatedXorTailListClose NegatedXorNestedListClose",
				"NegatedAndLeadListSeparator NegatedAndNestedListSeparator NegatedAndTailListSeparator NegatedOrLeadListSeparator NegatedOrNestedListSeparator NegatedOrTailListSeparator XorLeadListSeparator XorNestedListSeparator XorTailListSeparator NegatedXorLeadListSeparator NegatedXorNestedListSeparator NegatedXorTailListSeparator",
				@"{0}<span class=""logicalOperator""> is not </span>{1}",
				"{0}",
				"",
				@"<span class=""listSeparator"">,</span><span class=""logicalOperator""> and </span>",
				"",
				@"<span class=""logicalOperator""> and </span>",
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
				@"<span class=""quantifier"">there is </span>{0} <span class=""quantifier"">such that</span><br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">it is not true that {0} is indirectly related to {1} by repeatedly applying this fact type</span>",
				@"<span class=""quantifier"">each </span>{0} <a class=""predicateText"" href=""elementid:{2}"">is an instance of</a> {1}",
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
				@"{0} <span class=""quantifier"">exists</span>",
				@"<span class=""quantifier"">for</span> {0}<span class=""listSeparator"">, </span>",
				@"<span class=""logicalOperator""> and </span>",
				"</div>",
				"</span>",
				"</body></html>",
				@"
			<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
	<title>ORM2 Verbalization</title>
	<style type=""text/css"">
		body, table {{ font-family: {0}; font-size: {1}pt; color: {2}; {3} }}
		body {{ padding: 0em .1em; }}
		table.hidden, tr.hidden, td.hidden {{ margin: 0em; padding: 0em; border-collapse: collapse;}}
		td.hidden {{ vertical-align: top; }}
		table.hidden {{ display:inline; }}
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
				@"<div class=""verbalization"">",
				"</span>",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>"});
			sets[1] = new ArrayVerbalizationSet(new string[]{
				@"<span class=""quantifier"">no {0} may cycle back to itself via one or more traversals through </span>{1}",
				@"<span class=""quantifier"">no {0} may cycle back to itself via one or more instances of the role pair:</span><br/><span class=""smallIndent""><span class=""quantifier"">roles <span class=""objectType"">{2}</span> and <span class=""objectType"">{3}</span> of </span>{1}</span>",
				@"<span class=""quantifier"">each</span> {0}",
				@"<span class=""quantifier""> combination</span>",
				@"<span class=""quantifier"">each unique </span>",
				@"<span class=""listSeparator"">, </span>",
				@"{0} <span class=""quantifier"">for </span> {1}",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">each distinct</span> {0}",
				"",
				"",
				@"<br/><span class=""quantifier"">and</span> ",
				"</span>",
				"<span>",
				"<span>",
				@"</span><br/><span class=""smallIndent""><span class=""quantifier"">and</span> ",
				"</span>",
				@"<br/><span class=""smallIndent""><span class=""quantifier"">and</span> ",
				@"<br/><span class=""smallIndent"">",
				@"<br/><span class=""quantifier"">and</span> ",
				@"<span class=""quantifier"">at most one</span> {0}",
				@"<span class=""quantifier"">at most one</span> {0}",
				"</span>",
				" ",
				@"<br/></span><span class=""smallIndent"">",
				@"<br/></span><span class=""smallIndent""><span class=""quantifier"">and</span> ",
				@"<br/></span><span class=""smallIndent""><span class=""quantifier"">where</span> ",
				" ",
				@" <span class=""quantifier"">where</span> ",
				"<span>",
				"<br/>",
				@"<br/><span class=""quantifier"">and</span> ",
				@"<br/><span class=""quantifier"">where</span> ",
				@"<span class=""listSeparator"">.</span>",
				@"{0} <span class=""quantifier"">combination</span>",
				@"{0} <span class=""quantifier"">combination is unique</span>",
				@"{0} <span class=""instance"">{1}</span>",
				"Missing {0}",
				"",
				@"<span class=""listSeparator"">, </span>",
				"",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""logicalOperator"">that </span>{0}<span class=""logicalOperator""> is not that </span>{1}",
				"",
				"; ",
				"",
				"; ",
				"; ",
				@"<span class=""quantifier"">if </span>{0}<span class=""quantifier""> then </span>{1}",
				@"<span class=""quantifier"">if </span>{0}<br/><span class=""quantifier"">then </span>{1}",
				@"<span class=""quantifier"">if </span>{0}<br/><span class=""smallIndent""><span class=""quantifier"">then </span>{1}</span>",
				@"<span class=""smallIndent""><span class=""quantifier"">this association with</span> {0} <span class=""quantifier"">provides the preferred identification scheme for</span> {1}</span>",
				@"<span class=""quantifier"">that combination is associated with</span> {0} <span class=""quantifier"">in this context</span>",
				@"<span class=""quantifier"">that combination occurs</span> {0} <span class=""quantifier"">in this context</span>",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">Derivation Note:</span> <span class=""definition"">{0}</span>",
				@"<span class=""quantifier"">Informal Description:</span> <span class=""definition"">{0}</span>",
				@"{0} <span class=""quantifier"">is an entity type</span>",
				@"{0}<span class=""quantifier""> if and only if </span>{1}",
				"",
				@"<span class=""logicalOperator""> = </span>",
				"",
				@"<span class=""logicalOperator""> = </span>",
				@"<span class=""logicalOperator""> = </span>",
				"</span>",
				"</span>",
				@"<span class=""primaryErrorReport"">",
				@"<span class=""secondaryErrorReport"">",
				@"Model Error: <a class=""primaryErrorReport"" href=""elementid:{1}"">{0}</a>",
				@"Model Error: <a class=""secondaryErrorReport"" href=""elementid:{1}"">{0}</a>",
				@"<span class=""quantifier"">exactly one</span> {0}",
				@"<span class=""quantifier"">exactly one</span> {0}",
				@"<span class=""quantifier"">exactly once</span>",
				@"<span class=""quantifier"">no</span> {0}<span class=""quantifier""> the same </span>{1}",
				"{0} {1}",
				@"<span class=""quantifier"">some</span> {0}",
				"</span>",
				@"<br/><span class=""quantifier"">Examples: </span><span class=""smallIndent"">",
				@"<span class=""smallIndent""><span class=""quantifier"">Identifier: <span class=""instance"">{0}</span></span></span>",
				"</span></span>",
				"<br/>",
				@"<br/><span class=""indent""><span class=""quantifier"">Fact Types:</span><span class=""smallIndent""><br/>",
				"<br/>",
				"<br/>",
				@"<span class=""quantifier"">for each</span> {0}, {1}",
				@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">for each</span> {0}, {1}",
				@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">either</span> <span class=""instance"">0</span><span class=""listSeparator"">, </span><span class=""quantifier""> or </span> {0}",
				@"<span class=""quantifier"">each {0} in the population of “{1}” occurs there {2}</span>",
				@"exactly <span class=""instance"">{0}</span>",
				@"at least <span class=""instance"">{0}</span>",
				@"at least <span class=""instance"">{0}</span> and at most <span class=""instance"">{1}</span>",
				@"at most <span class=""instance"">{1}</span>",
				@"<span class=""quantifier"">{1} combinations of</span> {0}",
				@"<span class=""quantifier"">{1} instances of</span> {0}",
				@"<span class=""quantifier"">{0} times</span>",
				@"<span class=""quantifier"">*</span>{0} <span class=""quantifier"">if and only if</span><br/>{1}<br/>",
				@"<span class=""quantifier"">*Each</span> {0} <span class=""quantifier"">is</span> {1}",
				@"<span class=""quantifier"">all or none of the following hold:</span> {0}",
				@"<span class=""quantifier"">at most one of the following holds:</span> {0}",
				@"<span class=""quantifier"">exactly one of the following holds:</span> {0}",
				@"{0} <span class=""logicalOperator"">=</span> {1}",
				"{0}{{0}}{1}",
				"",
				@"<span class=""logicalOperator""> that is a </span>",
				"",
				@"<span class=""logicalOperator""> that is a </span>",
				@"<span class=""logicalOperator""> that is a </span>",
				@"<span class=""quantifier"">the same</span> {0}",
				@"{0} <span class=""quantifier"">that is</span> {1}",
				@"{1} <span class=""quantifier"">is</span> {0} <span class=""quantifier"">that</span>",
				@"<span class=""quantifier"">that</span>",
				@"<span class=""quantifier"">it is obligatory that</span> {0}",
				"</span>",
				@"<span class=""listSeparator"">; </span>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">; </span>",
				@"<span class=""listSeparator"">; </span>",
				"</span>",
				@"<span class=""logicalOperator""> and </span><br/>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""logicalOperator""> and </span><br/>",
				@"<span class=""logicalOperator""> and </span><br/>",
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
				@"<span class=""smallIndent"">{0} <span class=""quantifier"">is independent (it may have instances that play no other roles)</span></span>",
				@"{0} <span class=""quantifier"">in</span> {1}",
				@"{0}<span class=""quantifier""> is identified by </span>{1}",
				@"{0}<span class=""logicalOperator""> and </span>{1}",
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
				@"<span class=""quantifier"">Object-Role Model:</span> {0}",
				@"<span class=""quantifier"">more than one</span> {0}",
				"</span>",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<span class=""quantifier"">the possible values of</span> {0} <span class=""quantifier"">are</span> {1}",
				"</span>",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"",
				@"<span class=""quantifier"">it is not true that </span>",
				"</span>",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				@"{0} <span class=""quantifier"">does not exist</span>",
				"</span>",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				@"<span class=""quantifier"">some</span> {0} <span class=""quantifier"">participates in none of the following:</span>{1}",
				@"<span class=""instance"">{0}</span>",
				@"<span class=""quantifier"">Notes:</span> <span class=""note"">{0}</span>",
				@"<span class=""smallIndent"">{0} <span class=""quantifier"">objectifies</span> ""{1}""</span>",
				@"<a class=""objectType"" href=""elementid:{1}"">{0}</a>",
				"",
				@"<span class=""listSeparator"">, </span>",
				@"<br/><span class=""quantifier"">Examples:</span> ",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""objectTypeMissing"">Missing<sub style=""font-size:smaller;"">{0}</sub></span>",
				@"<a class=""objectType"" href=""elementid:{1}"">{0}<sub style=""font-size:smaller;"">{2}</sub></a>",
				@"<span class=""quantifier"">in each population of</span> {1}<span class=""listSeparator"">, </span>{0} <span class=""quantifier"">occurs at most once</span>",
				@"<span class=""quantifier"">at most one</span> {0}",
				"",
				"",
				@"<br/><span class=""quantifier"">or</span> ",
				"</span>",
				"<span>",
				"<span>",
				@"</span><br/><span class=""smallIndent""><span class=""quantifier"">or</span> ",
				"</span>",
				@"<br/><span class=""smallIndent""><span class=""quantifier"">and</span> ",
				@"<br/><span class=""smallIndent"">",
				@"<br/><span class=""quantifier"">or</span> ",
				@"<span class=""quantifier"">+</span>{0} <span class=""quantifier"">if</span><br/>{1}<br/>",
				@"<span class=""quantifier"">+Each derived</span> {0} <span class=""quantifier"">is</span> {1}",
				@"{0}<span class=""listSeparator"">.</span>{1}",
				@"{0} <span class=""quantifier"">who is</span> {1}",
				@"{1} <span class=""quantifier"">is</span> {0} <span class=""quantifier"">who</span>",
				@"<span class=""quantifier"">who</span>",
				@"<span class=""quantifier"">there are </span>{0} <span class=""quantifier"">such that</span><br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""smallIndent""><span class=""quantifier"">Portable data type:</span> {0}</span>",
				@"<a class=""predicateText"" href=""elementid:{1}"">{{0}}</a>",
				@"{1}<span class=""logicalOperator"">=</span>{0}",
				@"<span class=""quantifier"">given</span> {0} ",
				"{0}",
				@"{0}<span class=""quantifier"">select</span> {1} where<br/><span class=""smallIndent"">{2}</span>",
				@"<span class=""smallIndent""><span class=""quantifier"">Reference Mode:</span> <span class=""referenceMode"">{0}</span></span>",
				@"{0}<span class=""listSeparator"">(</span><span class=""referenceMode"">{1}</span><span class=""listSeparator"">)</span>",
				@"<span class=""smallIndent""><span class=""quantifier"">Reference Scheme:</span> {0}</span>",
				@"<span class=""quantifier"">itself</span>",
				@"{0} <span class=""quantifier"">itself</span>",
				"And Or Chain",
				"And Or Chain",
				"!And !Or Xor !Xor",
				"ChainedListClose NegatedChainedListClose AndTailListClose AndNestedListClose NegatedAndLeadListClose NegatedAndTailListClose NegatedAndNestedListClose OrTailListClose OrNestedListClose NegatedOrLeadListClose NegatedOrTailListClose NegatedOrNestedListClose XorLeadListClose XorTailListClose XorNestedListClose NegatedXorLeadListClose NegatedXorTailListClose NegatedXorNestedListClose",
				"NegatedAndLeadListSeparator NegatedAndNestedListSeparator NegatedAndTailListSeparator NegatedOrLeadListSeparator NegatedOrNestedListSeparator NegatedOrTailListSeparator XorLeadListSeparator XorNestedListSeparator XorTailListSeparator NegatedXorLeadListSeparator NegatedXorNestedListSeparator NegatedXorTailListSeparator",
				@"{0}<span class=""logicalOperator""> is not </span>{1}",
				"{0}",
				"",
				@"<span class=""listSeparator"">,</span><span class=""logicalOperator""> and </span>",
				"",
				@"<span class=""logicalOperator""> and </span>",
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
				@"<span class=""quantifier"">there is </span>{0} <span class=""quantifier"">such that</span><br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">it is not true that {0} is indirectly related to {1} by repeatedly applying this fact type</span>",
				@"<span class=""quantifier"">each </span>{0} <a class=""predicateText"" href=""elementid:{2}"">is an instance of</a> {1}",
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
				@"{0} <span class=""quantifier"">exists</span>",
				@"<span class=""quantifier"">for</span> {0}<span class=""listSeparator"">, </span>",
				@"<span class=""logicalOperator""> and </span>",
				"</div>",
				"</span>",
				"</body></html>",
				@"
			<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
	<title>ORM2 Verbalization</title>
	<style type=""text/css"">
		body, table {{ font-family: {0}; font-size: {1}pt; color: {2}; {3} }}
		body {{ padding: 0em .1em; }}
		table.hidden, tr.hidden, td.hidden {{ margin: 0em; padding: 0em; border-collapse: collapse;}}
		td.hidden {{ vertical-align: top; }}
		table.hidden {{ display:inline; }}
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
				@"<div class=""verbalization"">",
				"</span>",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>"});
			sets[2] = new ArrayVerbalizationSet(new string[]{
				@"<span class=""quantifier"">no {0} may cycle back to itself via one or more traversals through </span>{1}",
				@"<span class=""quantifier"">no {0} may cycle back to itself via one or more instances of the role pair:</span><br/><span class=""smallIndent""><span class=""quantifier"">roles <span class=""objectType"">{2}</span> and <span class=""objectType"">{3}</span> of </span>{1}</span>",
				@"<span class=""quantifier"">each</span> {0}",
				@"<span class=""quantifier""> combination</span>",
				@"<span class=""quantifier"">each unique </span>",
				@"<span class=""listSeparator"">, </span>",
				@"{0} <span class=""quantifier"">for </span> {1}",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">each distinct</span> {0}",
				"",
				"",
				@"<br/><span class=""quantifier"">and</span> ",
				"</span>",
				"<span>",
				"<span>",
				@"</span><br/><span class=""smallIndent""><span class=""quantifier"">and</span> ",
				"</span>",
				@"<br/><span class=""smallIndent""><span class=""quantifier"">and</span> ",
				@"<br/><span class=""smallIndent"">",
				@"<br/><span class=""quantifier"">and</span> ",
				@"<span class=""quantifier"">at most one</span> {0}",
				@"<span class=""quantifier"">at most one</span> {0}",
				"</span>",
				" ",
				@"<br/></span><span class=""smallIndent"">",
				@"<br/></span><span class=""smallIndent""><span class=""quantifier"">and</span> ",
				@"<br/></span><span class=""smallIndent""><span class=""quantifier"">where</span> ",
				" ",
				@" <span class=""quantifier"">where</span> ",
				"<span>",
				"<br/>",
				@"<br/><span class=""quantifier"">and</span> ",
				@"<br/><span class=""quantifier"">where</span> ",
				@"<span class=""listSeparator"">.</span>",
				@"{0} <span class=""quantifier"">combination</span>",
				@"{0} <span class=""quantifier"">combination is unique</span>",
				@"{0} <span class=""instance"">{1}</span>",
				"Missing {0}",
				"",
				@"<span class=""listSeparator"">, </span>",
				"",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""logicalOperator"">that </span>{0}<span class=""logicalOperator""> is not that </span>{1}",
				"",
				"; ",
				"",
				"; ",
				"; ",
				@"<span class=""quantifier"">if </span>{0}<span class=""quantifier""> then </span>{1}",
				@"<span class=""quantifier"">if </span>{0}<br/><span class=""quantifier"">then </span>{1}",
				@"<span class=""quantifier"">if </span>{0}<br/><span class=""smallIndent""><span class=""quantifier"">then </span>{1}</span>",
				@"<span class=""smallIndent""><span class=""quantifier"">this association with</span> {0} <span class=""quantifier"">provides the preferred identification scheme for</span> {1}</span>",
				@"<span class=""quantifier"">that combination is associated with</span> {0} <span class=""quantifier"">in this context</span>",
				@"<span class=""quantifier"">that combination occurs</span> {0} <span class=""quantifier"">in this context</span>",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">Derivation Note:</span> <span class=""definition"">{0}</span>",
				@"<span class=""quantifier"">Informal Description:</span> <span class=""definition"">{0}</span>",
				@"{0} <span class=""quantifier"">is an entity type</span>",
				@"{0}<span class=""quantifier""> if and only if </span>{1}",
				"",
				@"<span class=""logicalOperator""> = </span>",
				"",
				@"<span class=""logicalOperator""> = </span>",
				@"<span class=""logicalOperator""> = </span>",
				"</span>",
				"</span>",
				@"<span class=""primaryErrorReport"">",
				@"<span class=""secondaryErrorReport"">",
				@"Model Error: <a class=""primaryErrorReport"" href=""elementid:{1}"">{0}</a>",
				@"Model Error: <a class=""secondaryErrorReport"" href=""elementid:{1}"">{0}</a>",
				@"<span class=""quantifier"">exactly one</span> {0}",
				@"<span class=""quantifier"">exactly one</span> {0}",
				@"<span class=""quantifier"">more than once</span>",
				@"<span class=""quantifier"">no</span> {0}<span class=""quantifier""> the same </span>{1}",
				"{0} {1}",
				@"<span class=""quantifier"">no</span> {0}",
				"</span>",
				@"<br/><span class=""quantifier"">Examples: </span><span class=""smallIndent"">",
				@"<span class=""smallIndent""><span class=""quantifier"">Identifier: <span class=""instance"">{0}</span></span></span>",
				"</span></span>",
				"<br/>",
				@"<br/><span class=""indent""><span class=""quantifier"">Fact Types:</span><span class=""smallIndent""><br/>",
				"<br/>",
				"<br/>",
				@"<span class=""quantifier"">for each</span> {0}, {1}",
				@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">for some</span> {0}, {1}",
				@"<span class=""quantifier"">for some</span> {0},<br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">either</span> <span class=""instance"">0</span><span class=""listSeparator"">, </span><span class=""quantifier""> or </span> {0}",
				@"<span class=""quantifier"">each {0} in the population of “{1}” occurs there {2}</span>",
				@"exactly <span class=""instance"">{0}</span>",
				@"at least <span class=""instance"">{0}</span>",
				@"at least <span class=""instance"">{0}</span> and at most <span class=""instance"">{1}</span>",
				@"at most <span class=""instance"">{1}</span>",
				@"<span class=""quantifier"">{1} combinations of</span> {0}",
				@"<span class=""quantifier"">{1} instances of</span> {0}",
				@"<span class=""quantifier"">{0} times</span>",
				@"<span class=""quantifier"">*</span>{0} <span class=""quantifier"">if and only if</span><br/>{1}<br/>",
				@"<span class=""quantifier"">*Each</span> {0} <span class=""quantifier"">is</span> {1}",
				@"<span class=""quantifier"">all or none of the following hold:</span> {0}",
				@"<span class=""quantifier"">at most one of the following holds:</span> {0}",
				@"<span class=""quantifier"">exactly one of the following holds:</span> {0}",
				@"{0} <span class=""logicalOperator"">=</span> {1}",
				"{0}{{0}}{1}",
				"",
				@"<span class=""logicalOperator""> that is a </span>",
				"",
				@"<span class=""logicalOperator""> that is a </span>",
				@"<span class=""logicalOperator""> that is a </span>",
				@"<span class=""quantifier"">the same</span> {0}",
				@"{0} <span class=""quantifier"">that is</span> {1}",
				@"{1} <span class=""quantifier"">is</span> {0} <span class=""quantifier"">that</span>",
				@"<span class=""quantifier"">that</span>",
				@"<span class=""quantifier"">it is impossible that</span> {0}",
				"</span>",
				@"<span class=""listSeparator"">; </span>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">; </span>",
				@"<span class=""listSeparator"">; </span>",
				"</span>",
				@"<span class=""logicalOperator""> and </span><br/>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""logicalOperator""> and </span><br/>",
				@"<span class=""logicalOperator""> and </span><br/>",
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
				@"<span class=""smallIndent"">{0} <span class=""quantifier"">is independent (it may have instances that play no other roles)</span></span>",
				@"{0} <span class=""quantifier"">in</span> {1}",
				@"{0}<span class=""quantifier""> is identified by </span>{1}",
				@"{0}<span class=""logicalOperator""> and </span>{1}",
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
				@"<span class=""quantifier"">Object-Role Model:</span> {0}",
				@"<span class=""quantifier"">more than one</span> {0}",
				"</span>",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<span class=""quantifier"">the possible values of</span> {0} <span class=""quantifier"">are</span> {1}",
				"</span>",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"",
				@"<span class=""quantifier"">it is not true that </span>",
				"</span>",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				@"{0} <span class=""quantifier"">does not exist</span>",
				"</span>",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				@"<span class=""quantifier"">some</span> {0} <span class=""quantifier"">participates in none of the following:</span>{1}",
				@"<span class=""instance"">{0}</span>",
				@"<span class=""quantifier"">Notes:</span> <span class=""note"">{0}</span>",
				@"<span class=""smallIndent"">{0} <span class=""quantifier"">objectifies</span> ""{1}""</span>",
				@"<a class=""objectType"" href=""elementid:{1}"">{0}</a>",
				"",
				@"<span class=""listSeparator"">, </span>",
				@"<br/><span class=""quantifier"">Examples:</span> ",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""objectTypeMissing"">Missing<sub style=""font-size:smaller;"">{0}</sub></span>",
				@"<a class=""objectType"" href=""elementid:{1}"">{0}<sub style=""font-size:smaller;"">{2}</sub></a>",
				@"{0} <span class=""quantifier"">occurs more than once in the same population of</span> {1}",
				@"<span class=""quantifier"">more than one</span> {0}",
				"",
				"",
				@"<br/><span class=""quantifier"">or</span> ",
				"</span>",
				"<span>",
				"<span>",
				@"</span><br/><span class=""smallIndent""><span class=""quantifier"">or</span> ",
				"</span>",
				@"<br/><span class=""smallIndent""><span class=""quantifier"">and</span> ",
				@"<br/><span class=""smallIndent"">",
				@"<br/><span class=""quantifier"">or</span> ",
				@"<span class=""quantifier"">+</span>{0} <span class=""quantifier"">if</span><br/>{1}<br/>",
				@"<span class=""quantifier"">+Each derived</span> {0} <span class=""quantifier"">is</span> {1}",
				@"{0}<span class=""listSeparator"">.</span>{1}",
				@"{0} <span class=""quantifier"">who is</span> {1}",
				@"{1} <span class=""quantifier"">is</span> {0} <span class=""quantifier"">who</span>",
				@"<span class=""quantifier"">who</span>",
				@"<span class=""quantifier"">there are </span>{0} <span class=""quantifier"">such that</span><br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""smallIndent""><span class=""quantifier"">Portable data type:</span> {0}</span>",
				@"<a class=""predicateText"" href=""elementid:{1}"">{{0}}</a>",
				@"{1}<span class=""logicalOperator"">=</span>{0}",
				@"<span class=""quantifier"">given</span> {0} ",
				"{0}",
				@"{0}<span class=""quantifier"">select</span> {1} where<br/><span class=""smallIndent"">{2}</span>",
				@"<span class=""smallIndent""><span class=""quantifier"">Reference Mode:</span> <span class=""referenceMode"">{0}</span></span>",
				@"{0}<span class=""listSeparator"">(</span><span class=""referenceMode"">{1}</span><span class=""listSeparator"">)</span>",
				@"<span class=""smallIndent""><span class=""quantifier"">Reference Scheme:</span> {0}</span>",
				@"<span class=""quantifier"">itself</span>",
				@"{0} <span class=""quantifier"">itself</span>",
				"And Or Chain",
				"And Or Chain",
				"!And !Or Xor !Xor",
				"ChainedListClose NegatedChainedListClose AndTailListClose AndNestedListClose NegatedAndLeadListClose NegatedAndTailListClose NegatedAndNestedListClose OrTailListClose OrNestedListClose NegatedOrLeadListClose NegatedOrTailListClose NegatedOrNestedListClose XorLeadListClose XorTailListClose XorNestedListClose NegatedXorLeadListClose NegatedXorTailListClose NegatedXorNestedListClose",
				"NegatedAndLeadListSeparator NegatedAndNestedListSeparator NegatedAndTailListSeparator NegatedOrLeadListSeparator NegatedOrNestedListSeparator NegatedOrTailListSeparator XorLeadListSeparator XorNestedListSeparator XorTailListSeparator NegatedXorLeadListSeparator NegatedXorNestedListSeparator NegatedXorTailListSeparator",
				@"{0}<span class=""logicalOperator""> is not </span>{1}",
				"{0}",
				"",
				@"<span class=""listSeparator"">,</span><span class=""logicalOperator""> and </span>",
				"",
				@"<span class=""logicalOperator""> and </span>",
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
				@"<span class=""quantifier"">there is </span>{0} <span class=""quantifier"">such that</span><br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">it is not true that {0} is indirectly related to {1} by repeatedly applying this fact type</span>",
				@"<span class=""quantifier"">some </span>{0} <a class=""predicateText"" href=""elementid:{2}"">is not an instance of</a> {1}",
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
				@"<span class=""quantifier"">some</span> {0}",
				@"{0} <span class=""quantifier"">is a value type</span>",
				@"{0} <span class=""quantifier"">exists</span>",
				@"<span class=""quantifier"">for</span> {0}<span class=""listSeparator"">, </span>",
				@"<span class=""logicalOperator""> and </span>",
				"</div>",
				"</span>",
				"</body></html>",
				@"
			<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
	<title>ORM2 Verbalization</title>
	<style type=""text/css"">
		body, table {{ font-family: {0}; font-size: {1}pt; color: {2}; {3} }}
		body {{ padding: 0em .1em; }}
		table.hidden, tr.hidden, td.hidden {{ margin: 0em; padding: 0em; border-collapse: collapse;}}
		td.hidden {{ vertical-align: top; }}
		table.hidden {{ display:inline; }}
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
				@"<div class=""verbalization"">",
				"</span>",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>"});
			sets[3] = new ArrayVerbalizationSet(new string[]{
				@"<span class=""quantifier"">no {0} may cycle back to itself via one or more traversals through </span>{1}",
				@"<span class=""quantifier"">no {0} may cycle back to itself via one or more instances of the role pair:</span><br/><span class=""smallIndent""><span class=""quantifier"">roles <span class=""objectType"">{2}</span> and <span class=""objectType"">{3}</span> of </span>{1}</span>",
				@"<span class=""quantifier"">each</span> {0}",
				@"<span class=""quantifier""> combination</span>",
				@"<span class=""quantifier"">each unique </span>",
				@"<span class=""listSeparator"">, </span>",
				@"{0} <span class=""quantifier"">for </span> {1}",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">each distinct</span> {0}",
				"",
				"",
				@"<br/><span class=""quantifier"">and</span> ",
				"</span>",
				"<span>",
				"<span>",
				@"</span><br/><span class=""smallIndent""><span class=""quantifier"">and</span> ",
				"</span>",
				@"<br/><span class=""smallIndent""><span class=""quantifier"">and</span> ",
				@"<br/><span class=""smallIndent"">",
				@"<br/><span class=""quantifier"">and</span> ",
				@"<span class=""quantifier"">at most one</span> {0}",
				@"<span class=""quantifier"">at most one</span> {0}",
				"</span>",
				" ",
				@"<br/></span><span class=""smallIndent"">",
				@"<br/></span><span class=""smallIndent""><span class=""quantifier"">and</span> ",
				@"<br/></span><span class=""smallIndent""><span class=""quantifier"">where</span> ",
				" ",
				@" <span class=""quantifier"">where</span> ",
				"<span>",
				"<br/>",
				@"<br/><span class=""quantifier"">and</span> ",
				@"<br/><span class=""quantifier"">where</span> ",
				@"<span class=""listSeparator"">.</span>",
				@"{0} <span class=""quantifier"">combination</span>",
				@"{0} <span class=""quantifier"">combination is unique</span>",
				@"{0} <span class=""instance"">{1}</span>",
				"Missing {0}",
				"",
				@"<span class=""listSeparator"">, </span>",
				"",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""logicalOperator"">that </span>{0}<span class=""logicalOperator""> is not that </span>{1}",
				"",
				"; ",
				"",
				"; ",
				"; ",
				@"<span class=""quantifier"">if </span>{0}<span class=""quantifier""> then </span>{1}",
				@"<span class=""quantifier"">if </span>{0}<br/><span class=""quantifier"">then </span>{1}",
				@"<span class=""quantifier"">if </span>{0}<br/><span class=""smallIndent""><span class=""quantifier"">then </span>{1}</span>",
				@"<span class=""smallIndent""><span class=""quantifier"">this association with</span> {0} <span class=""quantifier"">provides the preferred identification scheme for</span> {1}</span>",
				@"<span class=""quantifier"">that combination is associated with</span> {0} <span class=""quantifier"">in this context</span>",
				@"<span class=""quantifier"">that combination occurs</span> {0} <span class=""quantifier"">in this context</span>",
				@"<span class=""quantifier"">that</span> {0}",
				@"<span class=""quantifier"">Derivation Note:</span> <span class=""definition"">{0}</span>",
				@"<span class=""quantifier"">Informal Description:</span> <span class=""definition"">{0}</span>",
				@"{0} <span class=""quantifier"">is an entity type</span>",
				@"{0}<span class=""quantifier""> if and only if </span>{1}",
				"",
				@"<span class=""logicalOperator""> = </span>",
				"",
				@"<span class=""logicalOperator""> = </span>",
				@"<span class=""logicalOperator""> = </span>",
				"</span>",
				"</span>",
				@"<span class=""primaryErrorReport"">",
				@"<span class=""secondaryErrorReport"">",
				@"Model Error: <a class=""primaryErrorReport"" href=""elementid:{1}"">{0}</a>",
				@"Model Error: <a class=""secondaryErrorReport"" href=""elementid:{1}"">{0}</a>",
				@"<span class=""quantifier"">exactly one</span> {0}",
				@"<span class=""quantifier"">exactly one</span> {0}",
				@"<span class=""quantifier"">more than once</span>",
				@"<span class=""quantifier"">no</span> {0}<span class=""quantifier""> the same </span>{1}",
				"{0} {1}",
				@"<span class=""quantifier"">no</span> {0}",
				"</span>",
				@"<br/><span class=""quantifier"">Examples: </span><span class=""smallIndent"">",
				@"<span class=""smallIndent""><span class=""quantifier"">Identifier: <span class=""instance"">{0}</span></span></span>",
				"</span></span>",
				"<br/>",
				@"<br/><span class=""indent""><span class=""quantifier"">Fact Types:</span><span class=""smallIndent""><br/>",
				"<br/>",
				"<br/>",
				@"<span class=""quantifier"">for each</span> {0}, {1}",
				@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">for some</span> {0}, {1}",
				@"<span class=""quantifier"">for some</span> {0},<br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">either</span> <span class=""instance"">0</span><span class=""listSeparator"">, </span><span class=""quantifier""> or </span> {0}",
				@"<span class=""quantifier"">each {0} in the population of “{1}” occurs there {2}</span>",
				@"exactly <span class=""instance"">{0}</span>",
				@"at least <span class=""instance"">{0}</span>",
				@"at least <span class=""instance"">{0}</span> and at most <span class=""instance"">{1}</span>",
				@"at most <span class=""instance"">{1}</span>",
				@"<span class=""quantifier"">{1} combinations of</span> {0}",
				@"<span class=""quantifier"">{1} instances of</span> {0}",
				@"<span class=""quantifier"">{0} times</span>",
				@"<span class=""quantifier"">*</span>{0} <span class=""quantifier"">if and only if</span><br/>{1}<br/>",
				@"<span class=""quantifier"">*Each</span> {0} <span class=""quantifier"">is</span> {1}",
				@"<span class=""quantifier"">all or none of the following hold:</span> {0}",
				@"<span class=""quantifier"">at most one of the following holds:</span> {0}",
				@"<span class=""quantifier"">exactly one of the following holds:</span> {0}",
				@"{0} <span class=""logicalOperator"">=</span> {1}",
				"{0}{{0}}{1}",
				"",
				@"<span class=""logicalOperator""> that is a </span>",
				"",
				@"<span class=""logicalOperator""> that is a </span>",
				@"<span class=""logicalOperator""> that is a </span>",
				@"<span class=""quantifier"">the same</span> {0}",
				@"{0} <span class=""quantifier"">that is</span> {1}",
				@"{1} <span class=""quantifier"">is</span> {0} <span class=""quantifier"">that</span>",
				@"<span class=""quantifier"">that</span>",
				@"<span class=""quantifier"">it is forbidden that</span> {0}",
				"</span>",
				@"<span class=""listSeparator"">; </span>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">; </span>",
				@"<span class=""listSeparator"">; </span>",
				"</span>",
				@"<span class=""logicalOperator""> and </span><br/>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""logicalOperator""> and </span><br/>",
				@"<span class=""logicalOperator""> and </span><br/>",
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
				@"<span class=""smallIndent"">{0} <span class=""quantifier"">is independent (it may have instances that play no other roles)</span></span>",
				@"{0} <span class=""quantifier"">in</span> {1}",
				@"{0}<span class=""quantifier""> is identified by </span>{1}",
				@"{0}<span class=""logicalOperator""> and </span>{1}",
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
				@"<span class=""quantifier"">Object-Role Model:</span> {0}",
				@"<span class=""quantifier"">more than one</span> {0}",
				"</span>",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<span class=""listSeparator"">; </span><br/>",
				@"<span class=""quantifier"">the possible values of</span> {0} <span class=""quantifier"">are</span> {1}",
				"</span>",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">at least one of the following is <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"",
				@"<span class=""quantifier"">it is not true that </span>",
				"</span>",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">all of the following are <em>false:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				@"{0} <span class=""quantifier"">does not exist</span>",
				"</span>",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">either none or many of the following are <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				@"<span class=""quantifier"">some</span> {0} <span class=""quantifier"">participates in none of the following:</span>{1}",
				@"<span class=""instance"">{0}</span>",
				@"<span class=""quantifier"">Notes:</span> <span class=""note"">{0}</span>",
				@"<span class=""smallIndent"">{0} <span class=""quantifier"">objectifies</span> ""{1}""</span>",
				@"<a class=""objectType"" href=""elementid:{1}"">{0}</a>",
				"",
				@"<span class=""listSeparator"">, </span>",
				@"<br/><span class=""quantifier"">Examples:</span> ",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""listSeparator"">, </span>",
				@"<span class=""objectTypeMissing"">Missing<sub style=""font-size:smaller;"">{0}</sub></span>",
				@"<a class=""objectType"" href=""elementid:{1}"">{0}<sub style=""font-size:smaller;"">{2}</sub></a>",
				@"{0} <span class=""quantifier"">occurs more than once in the same population of</span> {1}",
				@"<span class=""quantifier"">more than one</span> {0}",
				"",
				"",
				@"<br/><span class=""quantifier"">or</span> ",
				"</span>",
				"<span>",
				"<span>",
				@"</span><br/><span class=""smallIndent""><span class=""quantifier"">or</span> ",
				"</span>",
				@"<br/><span class=""smallIndent""><span class=""quantifier"">and</span> ",
				@"<br/><span class=""smallIndent"">",
				@"<br/><span class=""quantifier"">or</span> ",
				@"<span class=""quantifier"">+</span>{0} <span class=""quantifier"">if</span><br/>{1}<br/>",
				@"<span class=""quantifier"">+Each derived</span> {0} <span class=""quantifier"">is</span> {1}",
				@"{0}<span class=""listSeparator"">.</span>{1}",
				@"{0} <span class=""quantifier"">who is</span> {1}",
				@"{1} <span class=""quantifier"">is</span> {0} <span class=""quantifier"">who</span>",
				@"<span class=""quantifier"">who</span>",
				@"<span class=""quantifier"">there are </span>{0} <span class=""quantifier"">such that</span><br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""smallIndent""><span class=""quantifier"">Portable data type:</span> {0}</span>",
				@"<a class=""predicateText"" href=""elementid:{1}"">{{0}}</a>",
				@"{1}<span class=""logicalOperator"">=</span>{0}",
				@"<span class=""quantifier"">given</span> {0} ",
				"{0}",
				@"{0}<span class=""quantifier"">select</span> {1} where<br/><span class=""smallIndent"">{2}</span>",
				@"<span class=""smallIndent""><span class=""quantifier"">Reference Mode:</span> <span class=""referenceMode"">{0}</span></span>",
				@"{0}<span class=""listSeparator"">(</span><span class=""referenceMode"">{1}</span><span class=""listSeparator"">)</span>",
				@"<span class=""smallIndent""><span class=""quantifier"">Reference Scheme:</span> {0}</span>",
				@"<span class=""quantifier"">itself</span>",
				@"{0} <span class=""quantifier"">itself</span>",
				"And Or Chain",
				"And Or Chain",
				"!And !Or Xor !Xor",
				"ChainedListClose NegatedChainedListClose AndTailListClose AndNestedListClose NegatedAndLeadListClose NegatedAndTailListClose NegatedAndNestedListClose OrTailListClose OrNestedListClose NegatedOrLeadListClose NegatedOrTailListClose NegatedOrNestedListClose XorLeadListClose XorTailListClose XorNestedListClose NegatedXorLeadListClose NegatedXorTailListClose NegatedXorNestedListClose",
				"NegatedAndLeadListSeparator NegatedAndNestedListSeparator NegatedAndTailListSeparator NegatedOrLeadListSeparator NegatedOrNestedListSeparator NegatedOrTailListSeparator XorLeadListSeparator XorNestedListSeparator XorTailListSeparator NegatedXorLeadListSeparator NegatedXorNestedListSeparator NegatedXorTailListSeparator",
				@"{0}<span class=""logicalOperator""> is not </span>{1}",
				"{0}",
				"",
				@"<span class=""listSeparator"">,</span><span class=""logicalOperator""> and </span>",
				"",
				@"<span class=""logicalOperator""> and </span>",
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
				@"<span class=""quantifier"">there is </span>{0} <span class=""quantifier"">such that</span><br/><span class=""smallIndent"">{1}</span>",
				@"<span class=""quantifier"">it is not true that {0} is indirectly related to {1} by repeatedly applying this fact type</span>",
				@"<span class=""quantifier"">some </span>{0} <a class=""predicateText"" href=""elementid:{2}"">is not an instance of</a> {1}",
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
				@"<span class=""quantifier"">some</span> {0}",
				@"{0} <span class=""quantifier"">is a value type</span>",
				@"{0} <span class=""quantifier"">exists</span>",
				@"<span class=""quantifier"">for</span> {0}<span class=""listSeparator"">, </span>",
				@"<span class=""logicalOperator""> and </span>",
				"</div>",
				"</span>",
				"</body></html>",
				@"
			<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
	<title>ORM2 Verbalization</title>
	<style type=""text/css"">
		body, table {{ font-family: {0}; font-size: {1}pt; color: {2}; {3} }}
		body {{ padding: 0em .1em; }}
		table.hidden, tr.hidden, td.hidden {{ margin: 0em; padding: 0em; border-collapse: collapse;}}
		td.hidden {{ vertical-align: top; }}
		table.hidden {{ display:inline; }}
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
				@"<div class=""verbalization"">",
				"</span>",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>",
				"</span>",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""quantifier"">exactly one of the following is <em>true:</em></span><br/><span class=""smallIndent"">",
				@"<span class=""listSeparator"">;</span><br/>"});
		}
		/// <summary>Converts enum value of CoreVerbalizationSnippetType to an integer index value.</summary>
		protected override int ValueToIndex(CoreVerbalizationSnippetType enumValue)
		{
			return (int)enumValue;
		}
	}
	#endregion // CoreVerbalizationSets class
	#region ORMModel verbalization
	public partial class ORMModel : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			#region Preliminary
			bool isNegative = 0 != (sign & VerbalizationSign.Negative);
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			bool blockingErrors = false;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					blockingErrors = true;
					if (verbalizationContext.TestVerbalizedLocally(error))
					{
						continue;
					}
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
				}
				if (blockingErrors)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
			string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModelVerbalization, isDeontic, isNegative);
			string snippet1Replace1 = null;
			snippet1Replace1 = this.Name;
			string snippet1Replace2 = null;
			snippet1Replace2 = this.Id.ToString("D");
			FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
		}
	}
	#endregion // ORMModel verbalization
	#region FactType verbalization
	public partial class FactType : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			#region Preliminary
			bool isNegative = 0 != (sign & VerbalizationSign.Negative);
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			bool blockingErrors = false;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					blockingErrors = true;
					if (verbalizationContext.TestVerbalizedLocally(error))
					{
						continue;
					}
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
				}
				if (blockingErrors)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
			// UNDONE: RolePathVerbalizerPending Introduce snippet-integrated role path helper patterns
			// to the verbalization generator instead of hand-coding derivation rules
			FactTypeDerivationRule derivationRule;
			RolePathVerbalizer pathVerbalizer;
			if ((derivationRule = this.DerivationRule as FactTypeDerivationRule) != null && (pathVerbalizer = RolePathVerbalizer.Create(derivationRule, new StandardRolePathRenderer(snippets, verbalizationContext, writer.FormatProvider))).HasPathVerbalization(derivationRule))
			{
				reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, false, isNegative), predicatePartFormatString);
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippets.GetSnippet(derivationRule.DerivationCompleteness == DerivationCompleteness.FullyDerived ? CoreVerbalizationSnippetType.FullFactTypeDerivation : CoreVerbalizationSnippetType.PartialFactTypeDerivation, false, false), hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
					{
						return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead);
					}), pathVerbalizer.RenderPathVerbalization(derivationRule, null)), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, false, isNegative));
			}
			else
			{
				string[] basicRoleReplacements = new string[factArity];
				for (int i = 0; i < factArity; ++i)
				{
					Role factRole = factRoles[i + unaryRoleOffset].Role;
					ObjectType rolePlayer = factRole.RolePlayer;
					string basicReplacement;
					if (rolePlayer != null)
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				FactType.WriteVerbalizerSentence(writer, hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, basicRoleReplacements, true), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
		}
	}
	#endregion // FactType verbalization
	#region SubtypeFact verbalization
	public partial class SubtypeFact : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected new bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			#region Preliminary
			bool isNegative = 0 != (sign & VerbalizationSign.Negative);
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			bool blockingErrors = false;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					blockingErrors = true;
					if (verbalizationContext.TestVerbalizedLocally(error))
					{
						continue;
					}
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
				}
				if (blockingErrors)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
			ObjectType supertype = this.Supertype;
			ObjectType subtype = this.Subtype;
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
			string snippet1Replace1 = null;
			string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.SubtypeMetaReading, isDeontic, isNegative);
			string snippet1Replace1Replace1 = null;
			string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative);
			string snippet1Replace1Replace1Replace1 = null;
			snippet1Replace1Replace1Replace1 = VerbalizationHelper.NormalizeObjectTypeName(subtype.Name, verbalizationContext.VerbalizationOptions);
			string snippet1Replace1Replace1Replace2 = null;
			snippet1Replace1Replace1Replace2 = subtype.Id.ToString("D");
			snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1, snippet1Replace1Replace1Replace2);
			string snippet1Replace1Replace2 = null;
			string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative);
			string snippet1Replace1Replace2Replace1 = null;
			snippet1Replace1Replace2Replace1 = VerbalizationHelper.NormalizeObjectTypeName(supertype, verbalizationContext.VerbalizationOptions);
			string snippet1Replace1Replace2Replace2 = null;
			snippet1Replace1Replace2Replace2 = supertype.Id.ToString("D");
			snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1, snippet1Replace1Replace2Replace2);
			string snippet1Replace1Replace3 = null;
			snippet1Replace1Replace3 = this.Id.ToString("D");
			snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2, snippet1Replace1Replace3);
			FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
		}
	}
	#endregion // SubtypeFact verbalization
	#region QueryBase verbalization
	public partial class QueryBase : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected new bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			#region Preliminary
			bool isNegative = 0 != (sign & VerbalizationSign.Negative);
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			bool blockingErrors = false;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					blockingErrors = true;
					if (verbalizationContext.TestVerbalizedLocally(error))
					{
						continue;
					}
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
				}
				if (blockingErrors)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
			IList<RoleBase> factRoles = this.RoleCollection;
			int factArity = factRoles.Count;
			IList<QueryParameter> queryParameters = this.ParameterCollection;
			int queryParameterCount = queryParameters.Count;
			QueryDerivationRule derivationRule;
			RolePathVerbalizer pathVerbalizer;
			#endregion // Preliminary
			#region Pattern Matches
			if ((derivationRule = (QueryDerivationRule)this.DerivationRule) != null && (pathVerbalizer = RolePathVerbalizer.Create(derivationRule, new StandardRolePathRenderer(snippets, verbalizationContext, writer.FormatProvider))).HasPathVerbalization(derivationRule))
			{
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.QueryVerbalization, isDeontic, isNegative);
				string snippet1Replace1 = null;
				if (queryParameterCount != 0)
				{
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.QueryParameterContainer, isDeontic, isNegative);
					string snippet1Replace1Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int i = 0; i < queryParameterCount; ++i)
					{
						QueryParameter queryParameter = queryParameters[i];
						string queryParameterName = queryParameter.Name;
						CoreVerbalizationSnippetType listSnippet;
						if (i == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListOpen;
						}
						else if (i == queryParameterCount - 1)
						{
							if (i == 1)
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
						CoreVerbalizationSnippetType snippet1Replace1ReplaceSnippetType1 = 0;
						if (!string.IsNullOrEmpty(queryParameterName))
						{
							snippet1Replace1ReplaceSnippetType1 = CoreVerbalizationSnippetType.QueryNamedParameter;
						}
						else
						{
							snippet1Replace1ReplaceSnippetType1 = CoreVerbalizationSnippetType.QueryUnnamedParameter;
						}
						string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(snippet1Replace1ReplaceSnippetType1, isDeontic, isNegative);
						string snippet1Replace1Replace1Replace1 = null;
						snippet1Replace1Replace1Replace1 = pathVerbalizer.RenderAssociatedRolePlayer(queryParameter, null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead);
						string snippet1Replace1Replace1Replace2 = null;
						snippet1Replace1Replace1Replace2 = queryParameterName;
						snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1, snippet1Replace1Replace1Replace2);
						sbTemp.Append(snippet1Replace1Replace1);
						if (i == queryParameterCount - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleLogicalAndListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace1Replace1 = sbTemp.ToString();
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1);
				}
				string snippet1Replace2 = null;
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
					CoreVerbalizationSnippetType listSnippet;
					if (RoleIter2 == 0)
					{
						listSnippet = CoreVerbalizationSnippetType.SimpleLogicalAndListOpen;
					}
					else if (RoleIter2 == factArity - 1)
					{
						if (RoleIter2 == 1)
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
					snippet1Replace2 = pathVerbalizer.RenderAssociatedRolePlayer(factRoles[RoleIter2], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead);
					sbTemp.Append(snippet1Replace2);
					if (RoleIter2 == factArity - 1)
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleLogicalAndListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace2 = sbTemp.ToString();
				string snippet1Replace3 = null;
				snippet1Replace3 = pathVerbalizer.RenderPathVerbalization(derivationRule, null);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2, snippet1Replace3), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
		}
	}
	#endregion // QueryBase verbalization
	#region ObjectType verbalization
	public partial class ObjectType : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			#region Preliminary
			bool isNegative = 0 != (sign & VerbalizationSign.Negative);
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			bool blockingErrors = false;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					blockingErrors = true;
					if (verbalizationContext.TestVerbalizedLocally(error))
					{
						continue;
					}
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
				}
				if (blockingErrors)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
			SubtypeDerivationRule derivationRule;
			RolePathVerbalizer pathVerbalizer;
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
			variableSnippet1Replace1Replace1 = VerbalizationHelper.NormalizeObjectTypeName(this, verbalizationContext.VerbalizationOptions);
			string variableSnippet1Replace1Replace2 = null;
			variableSnippet1Replace1Replace2 = this.Id.ToString("D");
			variableSnippet1Replace1 = string.Format(writer.FormatProvider, variableSnippet1ReplaceFormat1, variableSnippet1Replace1Replace1, variableSnippet1Replace1Replace2);
			FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, variableSnippetFormat1, variableSnippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			if ((derivationRule = this.DerivationRule) != null && (pathVerbalizer = RolePathVerbalizer.Create(derivationRule, new StandardRolePathRenderer(snippets, verbalizationContext, writer.FormatProvider))).HasPathVerbalization(derivationRule))
			{
				CoreVerbalizationSnippetType variableSnippetSnippetType2 = 0;
				if (derivationRule.DerivationCompleteness == DerivationCompleteness.PartiallyDerived)
				{
					variableSnippetSnippetType2 = CoreVerbalizationSnippetType.PartialSubtypeDerivation;
				}
				else
				{
					variableSnippetSnippetType2 = CoreVerbalizationSnippetType.FullSubtypeDerivation;
				}
				writer.WriteLine();
				string variableSnippetFormat2 = snippets.GetSnippet(variableSnippetSnippetType2, isDeontic, isNegative);
				string variableSnippet2Replace1 = null;
				variableSnippet2Replace1 = pathVerbalizer.RenderAssociatedRolePlayer(derivationRule, null, RolePathRolePlayerRenderingOptions.None);
				string variableSnippet2Replace2 = null;
				variableSnippet2Replace2 = pathVerbalizer.RenderPathVerbalization(derivationRule, null);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, variableSnippetFormat2, variableSnippet2Replace1, variableSnippet2Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
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
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				writer.WriteLine();
				string snippetFormat4 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectifiesFactTypeVerbalization, isDeontic, isNegative);
				string snippet4Replace1 = null;
				string snippet4ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative);
				string snippet4Replace1Replace1 = null;
				snippet4Replace1Replace1 = VerbalizationHelper.NormalizeObjectTypeName(this, verbalizationContext.VerbalizationOptions);
				string snippet4Replace1Replace2 = null;
				snippet4Replace1Replace2 = this.Id.ToString("D");
				snippet4Replace1 = string.Format(writer.FormatProvider, snippet4ReplaceFormat1, snippet4Replace1Replace1, snippet4Replace1Replace2);
				string snippet4Replace2 = null;
				reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				snippet4Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, basicRoleReplacements, true);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat4, snippet4Replace1, snippet4Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			if (preferredIdentifier != null && !(preferredIdentifier.IsInternal && preferredIdentifier.FactTypeCollection[0].NestingType != null))
			{
				writer.WriteLine();
				string snippetFormat5 = snippets.GetSnippet(CoreVerbalizationSnippetType.ReferenceSchemeVerbalization, isDeontic, isNegative);
				string snippet5Replace1 = null;
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
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
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
					reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					snippet5Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, basicRoleReplacements, true);
					sbTemp.Append(snippet5Replace1);
					if (RoleIter1 == constraintRoleArity - 1)
					{
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
					}
				}
				snippet5Replace1 = sbTemp.ToString();
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat5, snippet5Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			if (identifyingObjectType != null && identifyingObjectType.HasReferenceMode)
			{
				writer.WriteLine();
				string snippetFormat6 = snippets.GetSnippet(CoreVerbalizationSnippetType.ReferenceModeVerbalization, isDeontic, isNegative);
				string snippet6Replace1 = null;
				snippet6Replace1 = identifyingObjectType.ReferenceModeDecoratedString;
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat6, snippet6Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			if (this.IsIndependent)
			{
				writer.WriteLine();
				string snippetFormat7 = snippets.GetSnippet(CoreVerbalizationSnippetType.IndependentVerbalization, isDeontic, isNegative);
				string snippet7Replace1 = null;
				string snippet7ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative);
				string snippet7Replace1Replace1 = null;
				snippet7Replace1Replace1 = VerbalizationHelper.NormalizeObjectTypeName(this, verbalizationContext.VerbalizationOptions);
				string snippet7Replace1Replace2 = null;
				snippet7Replace1Replace2 = this.Id.ToString("D");
				snippet7Replace1 = string.Format(writer.FormatProvider, snippet7ReplaceFormat1, snippet7Replace1Replace1, snippet7Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat7, snippet7Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			if (this.DataType != null && this.DataTypeNotSpecifiedError == null)
			{
				writer.WriteLine();
				string snippetFormat8 = snippets.GetSnippet(CoreVerbalizationSnippetType.PortableDataTypeVerbalization, isDeontic, isNegative);
				string snippet8Replace1 = null;
				snippet8Replace1 = this.DataType.ToString();
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat8, snippet8Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			if ((bool)verbalizationContext.VerbalizationOptions[CoreVerbalizationOption.FactTypesWithObjectType] && verbalizationContext.VerbalizationTarget == ORMCoreDomainModel.VerbalizationTargetName)
			{
				writer.WriteLine();
				string snippetFormat9 = snippets.GetSnippet(CoreVerbalizationSnippetType.SelfReference, isDeontic, isNegative);
				string snippet9Replace1 = null;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				LinkedElementCollection<Role> playedRoles = this.PlayedRoleCollection;
				int playedRoleCount = playedRoles.Count;
				FactType[] snippet9ReplaceUniqueFactTypes1 = new FactType[playedRoleCount];
				FactType snippet9ReplaceTestUniqueFactType1;
				int snippet9ReplaceFilteredIter1;
				int snippet9ReplaceFilteredCount1 = 0;
				for (snippet9ReplaceFilteredIter1 = 0; snippet9ReplaceFilteredIter1 < playedRoleCount; ++snippet9ReplaceFilteredIter1)
				{
					RoleBase primaryRole = playedRoles[snippet9ReplaceFilteredIter1];
					if (primaryRole.FactType.ReadingRequiredError == null && Array.IndexOf(snippet9ReplaceUniqueFactTypes1, snippet9ReplaceTestUniqueFactType1 = primaryRole.FactType) == -1)
					{
						snippet9ReplaceUniqueFactTypes1[snippet9ReplaceFilteredIter1] = snippet9ReplaceTestUniqueFactType1;
						++snippet9ReplaceFilteredCount1;
					}
				}
				Array.Clear(snippet9ReplaceUniqueFactTypes1, 0, snippet9ReplaceUniqueFactTypes1.Length);
				snippet9ReplaceFilteredIter1 = 0;
				for (int RoleIter1 = 0; RoleIter1 < playedRoleCount; ++RoleIter1)
				{
					RoleBase primaryRole = playedRoles[RoleIter1];
					FactType parentFact = primaryRole.FactType;
					SubtypeFact parentSubtypeFact = parentFact as SubtypeFact;
					string predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					LinkedElementCollection<ReadingOrder> allReadingOrders = parentFact.ReadingOrderCollection;
					IList<RoleBase> factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentSubtypeFact == null ? parentFact.RoleCollection : (IList<RoleBase>)new RoleBase[]{
						parentSubtypeFact.SubtypeRole,
						parentSubtypeFact.SupertypeRole};
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
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
						}
						basicRoleReplacements[i] = basicReplacement;
					}
					if (primaryRole.FactType.ReadingRequiredError == null && Array.IndexOf(snippet9ReplaceUniqueFactTypes1, snippet9ReplaceTestUniqueFactType1 = primaryRole.FactType) == -1)
					{
						snippet9ReplaceUniqueFactTypes1[RoleIter1] = snippet9ReplaceTestUniqueFactType1;
						CoreVerbalizationSnippetType listSnippet;
						if (snippet9ReplaceFilteredIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.FactTypeListOpen;
						}
						else if (snippet9ReplaceFilteredIter1 == snippet9ReplaceFilteredCount1 - 1)
						{
							if (snippet9ReplaceFilteredIter1 == 1)
							{
								listSnippet = CoreVerbalizationSnippetType.FactTypeListPairSeparator;
							}
							else
							{
								listSnippet = CoreVerbalizationSnippetType.FactTypeListFinalSeparator;
							}
						}
						else
						{
							listSnippet = CoreVerbalizationSnippetType.FactTypeListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						if (parentSubtypeFact != null)
						{
							snippet9Replace1 = FactType.CreateVerbalizerSentence(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.SubtypeMetaReading, isDeontic, false), basicRoleReplacements[0], basicRoleReplacements[1], parentFact.Id.ToString("D")), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
						}
						else
						{
							reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							snippet9Replace1 = FactType.CreateVerbalizerSentence(hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, basicRoleReplacements, true), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
						}
						sbTemp.Append(snippet9Replace1);
						if (snippet9ReplaceFilteredIter1 == snippet9ReplaceFilteredCount1 - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.FactTypeListClose, isDeontic, isNegative));
						}
						++snippet9ReplaceFilteredIter1;
					}
				}
				snippet9Replace1 = sbTemp.ToString();
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat9, snippet9Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			#endregion // Pattern Matches
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
		}
	}
	#endregion // ObjectType verbalization
	#region Definition verbalization
	public partial class Definition : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			#region Preliminary
			bool isNegative = 0 != (sign & VerbalizationSign.Negative);
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			bool blockingErrors = false;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					blockingErrors = true;
					if (verbalizationContext.TestVerbalizedLocally(error))
					{
						continue;
					}
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
				}
				if (blockingErrors)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
			string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.DescriptionVerbalization, isDeontic, isNegative);
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
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
		}
	}
	#endregion // Definition verbalization
	#region Note verbalization
	public partial class Note : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			#region Preliminary
			bool isNegative = 0 != (sign & VerbalizationSign.Negative);
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			bool blockingErrors = false;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					blockingErrors = true;
					if (verbalizationContext.TestVerbalizedLocally(error))
					{
						continue;
					}
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
				}
				if (blockingErrors)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
		}
	}
	#endregion // Note verbalization
	#region DerivationNote verbalization
	public partial class DerivationNote : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			#region Preliminary
			bool isNegative = 0 != (sign & VerbalizationSign.Negative);
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			bool blockingErrors = false;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					blockingErrors = true;
					if (verbalizationContext.TestVerbalizedLocally(error))
					{
						continue;
					}
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
				}
				if (blockingErrors)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
			string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.DerivationNoteVerbalization, isDeontic, isNegative);
			string snippet1Replace1 = null;
			snippet1Replace1 = this.Body;
			FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
		}
	}
	#endregion // DerivationNote verbalization
	#region Role.ErrorReport verbalization
	public partial class Role
	{
		private partial class ErrorReport : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static ErrorReport myCache;
			public static ErrorReport GetVerbalizer()
			{
				ErrorReport retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<ErrorReport>(ref myCache, null as ErrorReport, retVal);
				}
				if (retVal == null)
				{
					retVal = new ErrorReport();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<ErrorReport>(ref myCache, this, null as ErrorReport);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				#region Error report
				IModelErrorOwner errorOwner = this as IModelErrorOwner;
				bool firstErrorPending;
				if (errorOwner != null)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
					{
						if (verbalizationContext.TestVerbalizedLocally(error))
						{
							continue;
						}
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					}
					bool blockingErrorsReported = !firstErrorPending;
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
						{
							continue;
						}
						if (firstErrorPending)
						{
							firstErrorPending = false;
							if (blockingErrorsReported)
							{
								writer.WriteLine();
							}
							else
							{
								verbalizationContext.BeginVerbalization(VerbalizationContent.ErrorReport);
							}
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorOpenSecondaryReport, false, false));
						}
						else
						{
							writer.WriteLine();
						}
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return blockingErrorsReported || firstErrorPending;
				}
				#endregion // Error report
				return false;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // Role.ErrorReport verbalization
	#region SubsetConstraint verbalization
	public partial class SubsetConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			bool blockingErrors = false;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					blockingErrors = true;
					if (verbalizationContext.TestVerbalizedLocally(error))
					{
						continue;
					}
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
				}
				if (blockingErrors)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			bool isNegative = 0 != (sign & VerbalizationSign.Negative);
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
			IList<ConstraintRoleSequenceHasRole>[] allConstraintRoleSequences = new IList<ConstraintRoleSequenceHasRole>[constraintRoleArity];
			for (int i = 0; i < constraintRoleArity; ++i)
			{
				allConstraintRoleSequences[i] = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(constraintSequences[i]);
			}
			int columnArity = allConstraintRoleSequences[0].Count;
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
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
				}
				#endregion // Error report
				return false;
			}
			RolePathVerbalizer pathVerbalizer = RolePathVerbalizer.Create(this, new StandardRolePathRenderer(snippets, verbalizationContext, writer.FormatProvider));
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
							if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
							writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
					RoleBase primaryRole = allConstraintRoleSequences[readingMatchIndex1][0].Role;
					parentFact = primaryRole.FactType;
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.None);
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
						IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1ReplaceSequenceIter1];
						ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1ReplaceSequenceIter1].JoinPath;
						if (pathVerbalizer.HasPathVerbalization(joinPath))
						{
							pathVerbalizer.Options = RolePathVerbalizerOptions.MarkTrailingOutdentStart;
							if (sbTemp == null)
							{
								sbTemp = new StringBuilder();
							}
							else
							{
								sbTemp.Length = 0;
							}
							snippet1Replace1Replace1 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
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
							for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
							{
								RoleBase primaryRole = includedConstraintRoles[RoleIter1].Role;
								parentFact = primaryRole.FactType;
								predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
								allReadingOrders = parentFact.ReadingOrderCollection;
								factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
								reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								snippet1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
									{
										foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
										{
											if (replaceRole == constraintRole.Role)
											{
												return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
											}
										}
										return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
									});
								sbTemp.Append(snippet1Replace1Replace1);
							}
							snippet1Replace1Replace1 = sbTemp.ToString();
						}
					}
					string snippet1Replace1Replace2 = null;
					int snippet1Replace1ReplaceSequenceIter2;
					for (snippet1Replace1ReplaceSequenceIter2 = 1; snippet1Replace1ReplaceSequenceIter2 < constraintRoleArity; ++snippet1Replace1ReplaceSequenceIter2)
					{
						IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1ReplaceSequenceIter2];
						ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1ReplaceSequenceIter2].JoinPath;
						if (pathVerbalizer.HasPathVerbalization(joinPath))
						{
							pathVerbalizer.Options = RolePathVerbalizerOptions.MarkTrailingOutdentStart;
							if (sbTemp == null)
							{
								sbTemp = new StringBuilder();
							}
							else
							{
								sbTemp.Length = 0;
							}
							snippet1Replace1Replace2 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
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
							for (int RoleIter2 = 0; RoleIter2 < columnArity; ++RoleIter2)
							{
								RoleBase primaryRole = includedConstraintRoles[RoleIter2].Role;
								parentFact = primaryRole.FactType;
								predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
								allReadingOrders = parentFact.ReadingOrderCollection;
								factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
								reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
									{
										foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
										{
											if (replaceRole == constraintRole.Role)
											{
												return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
											}
										}
										return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
									});
								sbTemp.Append(snippet1Replace1Replace2);
							}
							snippet1Replace1Replace2 = sbTemp.ToString();
						}
					}
					snippet1Replace1 = RolePathVerbalizer.FormatResolveOutdent(writer.FormatProvider, null, writer.NewLine, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
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
						IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1ReplaceSequenceIter1];
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
							sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead));
						}
						snippet1Replace1Replace1 = sbTemp.ToString();
					}
					string snippet1Replace1Replace2 = null;
					string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.Conditional, isDeontic, isNegative);
					string snippet1Replace1Replace2Replace1 = null;
					int snippet1Replace1Replace2ReplaceSequenceIter1;
					for (snippet1Replace1Replace2ReplaceSequenceIter1 = 0; snippet1Replace1Replace2ReplaceSequenceIter1 < 1; ++snippet1Replace1Replace2ReplaceSequenceIter1)
					{
						IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
						ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1].JoinPath;
						if (pathVerbalizer.HasPathVerbalization(joinPath))
						{
							pathVerbalizer.Options = RolePathVerbalizerOptions.MarkTrailingOutdentStart;
							if (sbTemp == null)
							{
								sbTemp = new StringBuilder();
							}
							else
							{
								sbTemp.Length = 0;
							}
							snippet1Replace1Replace2Replace1 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
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
							for (int RoleIter1 = 0; RoleIter1 < 1; ++RoleIter1)
							{
								RoleBase primaryRole = includedConstraintRoles[RoleIter1].Role;
								parentFact = primaryRole.FactType;
								predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
								allReadingOrders = parentFact.ReadingOrderCollection;
								factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
								reading = parentFact.GetMatchingReading(allReadingOrders, null, null, (System.Collections.IList)factRoles, factRoles, MatchingReadingOptions.AllowAnyOrder);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								snippet1Replace1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
									{
										foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
										{
											if (replaceRole == constraintRole.Role)
											{
												return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
											}
										}
										return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
									});
								sbTemp.Append(snippet1Replace1Replace2Replace1);
							}
							snippet1Replace1Replace2Replace1 = sbTemp.ToString();
						}
					}
					string snippet1Replace1Replace2Replace2 = null;
					int snippet1Replace1Replace2ReplaceSequenceIter2;
					for (snippet1Replace1Replace2ReplaceSequenceIter2 = 1; snippet1Replace1Replace2ReplaceSequenceIter2 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceSequenceIter2)
					{
						IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace2ReplaceSequenceIter2];
						ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1Replace2ReplaceSequenceIter2].JoinPath;
						if (pathVerbalizer.HasPathVerbalization(joinPath))
						{
							pathVerbalizer.Options = RolePathVerbalizerOptions.MarkTrailingOutdentStart;
							if (sbTemp == null)
							{
								sbTemp = new StringBuilder();
							}
							else
							{
								sbTemp.Length = 0;
							}
							snippet1Replace1Replace2Replace2 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
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
							for (int RoleIter2 = 0; RoleIter2 < 1; ++RoleIter2)
							{
								RoleBase primaryRole = includedConstraintRoles[RoleIter2].Role;
								parentFact = primaryRole.FactType;
								predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
								allReadingOrders = parentFact.ReadingOrderCollection;
								factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
								reading = parentFact.GetMatchingReading(allReadingOrders, null, null, (System.Collections.IList)factRoles, factRoles, MatchingReadingOptions.AllowAnyOrder);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								snippet1Replace1Replace2Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
									{
										foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
										{
											if (replaceRole == constraintRole.Role)
											{
												return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
											}
										}
										return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
									});
								sbTemp.Append(snippet1Replace1Replace2Replace2);
							}
							snippet1Replace1Replace2Replace2 = sbTemp.ToString();
						}
					}
					snippet1Replace1Replace2 = RolePathVerbalizer.FormatResolveOutdent(writer.FormatProvider, null, writer.NewLine, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1, snippet1Replace1Replace2Replace2);
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
			}
			else if (!isNegative)
			{
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.Conditional, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				int snippet1Replace1ReplaceSequenceIter1;
				int snippet1Replace1ReplaceCompositeCount1 = 0;
				int snippet1Replace1ReplaceCompositeIterator1;
				FactType[] snippet1Replace1Replace1ItemUniqueFactTypes1 = new FactType[columnArity];
				FactType snippet1Replace1Replace1ItemTestUniqueFactType1;
				for (snippet1Replace1ReplaceSequenceIter1 = 0; snippet1Replace1ReplaceSequenceIter1 < 1; ++snippet1Replace1ReplaceSequenceIter1)
				{
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1ReplaceSequenceIter1];
					ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1ReplaceSequenceIter1].JoinPath;
					if (pathVerbalizer.HasPathVerbalization(joinPath))
					{
						++snippet1Replace1ReplaceCompositeCount1;
					}
					else
					{
						for (snippet1Replace1ReplaceCompositeIterator1 = 0; snippet1Replace1ReplaceCompositeIterator1 < columnArity; ++snippet1Replace1ReplaceCompositeIterator1)
						{
							RoleBase primaryRole = includedConstraintRoles[snippet1Replace1ReplaceCompositeIterator1].Role;
							if (Array.IndexOf(snippet1Replace1Replace1ItemUniqueFactTypes1, snippet1Replace1Replace1ItemTestUniqueFactType1 = primaryRole.FactType) == -1)
							{
								snippet1Replace1Replace1ItemUniqueFactTypes1[snippet1Replace1ReplaceCompositeIterator1] = snippet1Replace1Replace1ItemTestUniqueFactType1;
								++snippet1Replace1ReplaceCompositeCount1;
							}
						}
					}
					Array.Clear(snippet1Replace1Replace1ItemUniqueFactTypes1, 0, snippet1Replace1Replace1ItemUniqueFactTypes1.Length);
				}
				snippet1Replace1ReplaceCompositeIterator1 = 0;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (snippet1Replace1ReplaceSequenceIter1 = 0; snippet1Replace1ReplaceSequenceIter1 < 1; ++snippet1Replace1ReplaceSequenceIter1)
				{
					string snippet1Replace1Replace1Item1;
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1ReplaceSequenceIter1];
					ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1ReplaceSequenceIter1].JoinPath;
					if (pathVerbalizer.HasPathVerbalization(joinPath))
					{
						pathVerbalizer.Options = RolePathVerbalizerOptions.MarkTrailingOutdentStart;
						CoreVerbalizationSnippetType listSnippet;
						if (snippet1Replace1ReplaceCompositeIterator1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.CompoundListOpen;
						}
						else if (snippet1Replace1ReplaceCompositeIterator1 == snippet1Replace1ReplaceCompositeCount1 - 1)
						{
							if (snippet1Replace1ReplaceCompositeIterator1 == 1)
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
						sbTemp.Append(pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp));
						if (snippet1Replace1ReplaceCompositeIterator1 == snippet1Replace1ReplaceCompositeCount1 - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
						}
						++snippet1Replace1ReplaceCompositeIterator1;
					}
					else
					{
						for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
						{
							RoleBase primaryRole = includedConstraintRoles[RoleIter1].Role;
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							if (Array.IndexOf(snippet1Replace1Replace1ItemUniqueFactTypes1, snippet1Replace1Replace1ItemTestUniqueFactType1 = primaryRole.FactType) == -1)
							{
								snippet1Replace1Replace1ItemUniqueFactTypes1[RoleIter1] = snippet1Replace1Replace1ItemTestUniqueFactType1;
								CoreVerbalizationSnippetType listSnippet;
								if (snippet1Replace1ReplaceCompositeIterator1 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.CompoundListOpen;
								}
								else if (snippet1Replace1ReplaceCompositeIterator1 == snippet1Replace1ReplaceCompositeCount1 - 1)
								{
									if (snippet1Replace1ReplaceCompositeIterator1 == 1)
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
								reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								snippet1Replace1Replace1Item1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
									{
										foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
										{
											if (replaceRole == constraintRole.Role)
											{
												return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
											}
										}
										return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
									});
								sbTemp.Append(snippet1Replace1Replace1Item1);
								if (snippet1Replace1ReplaceCompositeIterator1 == snippet1Replace1ReplaceCompositeCount1 - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
								}
								++snippet1Replace1ReplaceCompositeIterator1;
							}
						}
					}
					Array.Clear(snippet1Replace1Replace1ItemUniqueFactTypes1, 0, snippet1Replace1Replace1ItemUniqueFactTypes1.Length);
				}
				snippet1Replace1Replace1 = sbTemp.ToString();
				string snippet1Replace1Replace2 = null;
				int snippet1Replace1ReplaceSequenceIter2;
				int snippet1Replace1ReplaceCompositeCount2 = 0;
				int snippet1Replace1ReplaceCompositeIterator2;
				FactType[] snippet1Replace1Replace2ItemUniqueFactTypes1 = new FactType[columnArity];
				FactType snippet1Replace1Replace2ItemTestUniqueFactType1;
				for (snippet1Replace1ReplaceSequenceIter2 = 1; snippet1Replace1ReplaceSequenceIter2 < constraintRoleArity; ++snippet1Replace1ReplaceSequenceIter2)
				{
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1ReplaceSequenceIter2];
					ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1ReplaceSequenceIter2].JoinPath;
					if (pathVerbalizer.HasPathVerbalization(joinPath))
					{
						++snippet1Replace1ReplaceCompositeCount2;
					}
					else
					{
						for (snippet1Replace1ReplaceCompositeIterator2 = 0; snippet1Replace1ReplaceCompositeIterator2 < columnArity; ++snippet1Replace1ReplaceCompositeIterator2)
						{
							RoleBase primaryRole = includedConstraintRoles[snippet1Replace1ReplaceCompositeIterator2].Role;
							if (Array.IndexOf(snippet1Replace1Replace2ItemUniqueFactTypes1, snippet1Replace1Replace2ItemTestUniqueFactType1 = primaryRole.FactType) == -1)
							{
								snippet1Replace1Replace2ItemUniqueFactTypes1[snippet1Replace1ReplaceCompositeIterator2] = snippet1Replace1Replace2ItemTestUniqueFactType1;
								++snippet1Replace1ReplaceCompositeCount2;
							}
						}
					}
					Array.Clear(snippet1Replace1Replace2ItemUniqueFactTypes1, 0, snippet1Replace1Replace2ItemUniqueFactTypes1.Length);
				}
				snippet1Replace1ReplaceCompositeIterator2 = 0;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				for (snippet1Replace1ReplaceSequenceIter2 = 1; snippet1Replace1ReplaceSequenceIter2 < constraintRoleArity; ++snippet1Replace1ReplaceSequenceIter2)
				{
					string snippet1Replace1Replace2Item1;
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1ReplaceSequenceIter2];
					ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1ReplaceSequenceIter2].JoinPath;
					if (pathVerbalizer.HasPathVerbalization(joinPath))
					{
						pathVerbalizer.Options = RolePathVerbalizerOptions.MarkTrailingOutdentStart;
						CoreVerbalizationSnippetType listSnippet;
						if (snippet1Replace1ReplaceCompositeIterator2 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.CompoundListOpen;
						}
						else if (snippet1Replace1ReplaceCompositeIterator2 == snippet1Replace1ReplaceCompositeCount2 - 1)
						{
							if (snippet1Replace1ReplaceCompositeIterator2 == 1)
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
						sbTemp.Append(pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp));
						if (snippet1Replace1ReplaceCompositeIterator2 == snippet1Replace1ReplaceCompositeCount2 - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
						}
						++snippet1Replace1ReplaceCompositeIterator2;
					}
					else
					{
						for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
						{
							RoleBase primaryRole = includedConstraintRoles[RoleIter1].Role;
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							if (Array.IndexOf(snippet1Replace1Replace2ItemUniqueFactTypes1, snippet1Replace1Replace2ItemTestUniqueFactType1 = primaryRole.FactType) == -1)
							{
								snippet1Replace1Replace2ItemUniqueFactTypes1[RoleIter1] = snippet1Replace1Replace2ItemTestUniqueFactType1;
								CoreVerbalizationSnippetType listSnippet;
								if (snippet1Replace1ReplaceCompositeIterator2 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.CompoundListOpen;
								}
								else if (snippet1Replace1ReplaceCompositeIterator2 == snippet1Replace1ReplaceCompositeCount2 - 1)
								{
									if (snippet1Replace1ReplaceCompositeIterator2 == 1)
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
								reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								snippet1Replace1Replace2Item1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
									{
										foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
										{
											if (replaceRole == constraintRole.Role)
											{
												return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
											}
										}
										return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
									});
								sbTemp.Append(snippet1Replace1Replace2Item1);
								if (snippet1Replace1ReplaceCompositeIterator2 == snippet1Replace1ReplaceCompositeCount2 - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
								}
								++snippet1Replace1ReplaceCompositeIterator2;
							}
						}
					}
					Array.Clear(snippet1Replace1Replace2ItemUniqueFactTypes1, 0, snippet1Replace1Replace2ItemUniqueFactTypes1.Length);
				}
				snippet1Replace1Replace2 = sbTemp.ToString();
				snippet1Replace1 = RolePathVerbalizer.FormatResolveOutdent(writer.FormatProvider, null, writer.NewLine, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
			}
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
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
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
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
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
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
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, includedRoles, factRoles, MatchingReadingOptions.None);
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
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						if (reading != null)
						{
							verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
							string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
							string snippet1Replace1 = null;
							string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
							string snippet1Replace1Replace1 = null;
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
							snippet1Replace1Replace1 = sbTemp.ToString();
							string snippet1Replace1Replace2 = null;
							reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1Replace1ReplaceFactRoleIter2], snippet1Replace1ReplaceFactRoleIter2);
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
								roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
							FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
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
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, includedRoles, factRoles, MatchingReadingOptions.AllowAnyOrder);
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
				else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
				{
					return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
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
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
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
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
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
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, includedRoles, factRoles, MatchingReadingOptions.None);
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
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
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
							reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
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
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
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
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
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
				string[,] basicRoleReplacements = new string[factArity, 3];
				VerbalizationSubscripter subscripter = new VerbalizationSubscripter(writer.FormatProvider);
				for (int i = 0; i < factArity; ++i)
				{
					Role factRole = factRoles[i + unaryRoleOffset].Role;
					ObjectType rolePlayer = factRole.RolePlayer;
					string basicReplacement;
					string basicSubscriptedReplacement = null;
					if (rolePlayer != null)
					{
						bool useSubscript = false;
						int j = 0;
						for (; j < factArity; ++j)
						{
							if (i != j && rolePlayer == factRoles[j].Role.RolePlayer)
							{
								useSubscript = true;
								break;
							}
						}
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
						if (useSubscript)
						{
							basicSubscriptedReplacement = subscripter.PrepareSubscriptFormatString(snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
						}
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i, 0] = basicReplacement;
					if (basicSubscriptedReplacement == null)
					{
						basicRoleReplacements[i, 1] = basicReplacement;
						basicRoleReplacements[i, 2] = null;
					}
					else
					{
						basicRoleReplacements[i, 1] = basicSubscriptedReplacement;
						basicRoleReplacements[i, 2] = string.Empty;
					}
				}
				string[] roleReplacements = new string[factArity];
				IReading reading;
				VerbalizationHyphenBinder hyphenBinder;
				#endregion // Preliminary
				#region Pattern Matches
				if (factArity == 2 && !isNegative)
				{
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, includedRoles, factRoles, MatchingReadingOptions.InvertLeadRoles);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					if (reading != null)
					{
						verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
						string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
						string snippet1Replace1 = null;
						for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
						{
							RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
							string roleReplacement = null;
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1ReplaceFactRoleIter1, 0], snippet1ReplaceFactRoleIter1);
							if (includedRoles.Contains(currentRole.Role))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.MoreThanOneQuantifier, isDeontic, isNegative), basicReplacement);
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
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						if (reading != null)
						{
							verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
							string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
							string snippet1Replace1 = null;
							if (factRoles[0].Role.RolePlayer == factRoles[1].Role.RolePlayer)
							{
								string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachNegatableCompactQuantifier, isDeontic, true);
								string snippet1Replace1Replace1 = null;
								if (sbTemp == null)
								{
									sbTemp = new StringBuilder();
								}
								else
								{
									sbTemp.Length = 0;
								}
								bool snippet1Replace1ReplaceIsFirstPass1 = true;
								for (int RoleIter1 = 0; RoleIter1 < factArity && snippet1Replace1ReplaceIsFirstPass1; ++RoleIter1)
								{
									if (includedRoles.IndexOf(factRoles[RoleIter1].Role) == -1)
									{
										sbTemp.Append(subscripter.GetSubscriptedName(RoleIter1, basicRoleReplacements));
										snippet1Replace1ReplaceIsFirstPass1 = false;
									}
								}
								snippet1Replace1Replace1 = sbTemp.ToString();
								string snippet1Replace1Replace2 = null;
								for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
								{
									RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
									string roleReplacement = null;
									if (includedRoles.Contains(currentRole.Role))
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.MoreThanOneQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2));
									}
									else
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, basicRoleReplacements), snippet1Replace1ReplaceFactRoleIter2));
									}
									if (roleReplacement == null)
									{
										roleReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2);
									}
									roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
								}
								snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
								snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
							}
							else
							{
								for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
								{
									RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
									string roleReplacement = null;
									string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1ReplaceFactRoleIter1, 0], snippet1ReplaceFactRoleIter1);
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
							}
							FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
						}
					}
				}
				else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
				{
					return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
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
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				#region Prerequisite error check
				IModelErrorOwner errorOwner = this as IModelErrorOwner;
				bool firstErrorPending;
				bool blockingErrors = false;
				if (errorOwner != null)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
					{
						blockingErrors = true;
						if (verbalizationContext.TestVerbalizedLocally(error))
						{
							continue;
						}
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					}
					if (blockingErrors)
					{
						firstErrorPending = true;
						foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
						{
							ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
							if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
							writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
						}
						if (!firstErrorPending)
						{
							writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
						}
						return true;
					}
				}
				#endregion // Prerequisite error check
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
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
					#region Error report
					if (errorOwner != null)
					{
						firstErrorPending = true;
						foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
						{
							ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
							if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
							writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
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
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, includedRoles, factRoles, MatchingReadingOptions.None);
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
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						if (reading != null)
						{
							verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
							string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
							string snippet1Replace1 = null;
							string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
							string snippet1Replace1Replace1 = null;
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
							snippet1Replace1Replace1 = sbTemp.ToString();
							string snippet1Replace1Replace2 = null;
							reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1Replace1ReplaceFactRoleIter2], snippet1Replace1ReplaceFactRoleIter2);
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
								roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
							FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
						}
					}
				}
				else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
				{
					return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
				}
				#endregion // Pattern Matches
				#region Error report
				if (errorOwner != null)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
				}
				#endregion // Error report
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // FactType.CombinedMandatoryUniqueVerbalizer verbalization
	#region MandatoryConstraint verbalization
	public partial class MandatoryConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			bool blockingErrors = false;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					blockingErrors = true;
					if (verbalizationContext.TestVerbalizedLocally(error))
					{
						continue;
					}
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
				}
				if (blockingErrors)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
			this.VerbalizeByConstraintArity(writer, snippetsDictionary, verbalizationContext, sign);
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
		}
	}
	#endregion // MandatoryConstraint verbalization
	#region MandatoryConstraint.SimpleMandatoryVerbalizer verbalization
	public partial class MandatoryConstraint
	{
		private partial class SimpleMandatoryVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static SimpleMandatoryVerbalizer myCache;
			public static SimpleMandatoryVerbalizer GetVerbalizer()
			{
				SimpleMandatoryVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<SimpleMandatoryVerbalizer>(ref myCache, null as SimpleMandatoryVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new SimpleMandatoryVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<SimpleMandatoryVerbalizer>(ref myCache, this, null as SimpleMandatoryVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
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
					return false;
				}
				string[][,] allBasicRoleReplacements = new string[allFactsCount][,];
				VerbalizationSubscripter subscripter = new VerbalizationSubscripter(writer.FormatProvider);
				bool[] unaryReplacements = new bool[allFactsCount];
				int minFactArity = int.MaxValue;
				int maxFactArity = int.MinValue;
				for (int iFact = 0; iFact < allFactsCount; ++iFact)
				{
					FactType currentFact = allFacts[iFact];
					if (currentFact.ReadingRequiredError != null)
					{
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
					string[,] basicRoleReplacements = new string[factArity, 3];
					for (int i = 0; i < factArity; ++i)
					{
						Role factRole = factRoles[i + unaryRoleOffset].Role;
						ObjectType rolePlayer = factRole.RolePlayer;
						string basicReplacement;
						string basicSubscriptedReplacement = null;
						if (rolePlayer != null)
						{
							bool useSubscript = false;
							int j = 0;
							for (; j < factArity; ++j)
							{
								if (i != j && rolePlayer == factRoles[j].Role.RolePlayer)
								{
									useSubscript = true;
									break;
								}
							}
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							if (useSubscript)
							{
								basicSubscriptedReplacement = subscripter.PrepareSubscriptFormatString(snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							}
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
						}
						basicRoleReplacements[i, 0] = basicReplacement;
						if (basicSubscriptedReplacement == null)
						{
							basicRoleReplacements[i, 1] = basicReplacement;
							basicRoleReplacements[i, 2] = null;
						}
						else
						{
							basicRoleReplacements[i, 1] = basicSubscriptedReplacement;
							basicRoleReplacements[i, 2] = string.Empty;
						}
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
					reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					for (int snippet1Replace1ReplaceFactRoleIter1 = 0; snippet1Replace1ReplaceFactRoleIter1 < factArity; ++snippet1Replace1ReplaceFactRoleIter1)
					{
						RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter1, 0], snippet1Replace1ReplaceFactRoleIter1);
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1Replace1ReplaceFactRoleIter1] = roleReplacement;
					}
					snippet1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (constraintRoleArity == 1 && factArity == 2 && maxFactArity <= 2)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, factRoles, MatchingReadingOptions.NoFrontText);
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
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1, 0], snippet1ReplaceFactRoleIter1);
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
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						if (reading != null)
						{
							verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
							string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
							string snippet1Replace1 = null;
							string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachNegatableCompactQuantifier, isDeontic, isNegative);
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
								int ResolvedRoleIndex1 = unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1]);
								sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(ResolvedRoleIndex1, allBasicRoleReplacements[0]), ResolvedRoleIndex1));
							}
							snippet1Replace1Replace1 = sbTemp.ToString();
							string snippet1Replace1Replace2 = null;
							reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
								string roleReplacement = null;
								if (allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2));
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2);
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
							FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
						}
					}
				}
				else if (constraintRoleArity == 1 && minFactArity >= 3 && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, factRoles, MatchingReadingOptions.None);
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
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1, 0], snippet1ReplaceFactRoleIter1);
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
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						if (reading != null)
						{
							verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
							string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
							string snippet1Replace1 = null;
							string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachIndentedQuantifier, isDeontic, isNegative);
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
								int ResolvedRoleIndex1 = unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1]);
								sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(ResolvedRoleIndex1, allBasicRoleReplacements[0]), ResolvedRoleIndex1));
							}
							snippet1Replace1Replace1 = sbTemp.ToString();
							string snippet1Replace1Replace2 = null;
							reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
								string roleReplacement = null;
								if (allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2));
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2);
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
							FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
						}
					}
				}
				else if (constraintRoleArity == 1 && minFactArity >= 3)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, factRoles, MatchingReadingOptions.None);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					if (reading != null)
					{
						verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
						string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace1 = null;
						bool useNegation = true;
						for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
						{
							RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
							string roleReplacement = null;
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1, 0], snippet1ReplaceFactRoleIter1);
							if (allConstraintRoles.Contains(currentRole.Role))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
							}
							else if (useNegation)
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								useNegation = false;
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, false), basicReplacement);
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
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						if (reading != null)
						{
							verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
							string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachNegatableIndentedQuantifier, isDeontic, isNegative);
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
								sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(ResolvedRoleIndex1, allBasicRoleReplacements[0]), ResolvedRoleIndex1));
								if (RoleIter1 == constraintRoleArity - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
								}
							}
							snippet1Replace1 = sbTemp.ToString();
							string snippet1Replace2 = null;
							string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
							string snippet1Replace2Replace1 = null;
							reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							bool useNegation = true;
							for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
								string roleReplacement = null;
								if (allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace2ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1));
								}
								else if (useNegation)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace2ReplaceFactRoleIter1, 0], snippet1Replace2ReplaceFactRoleIter1));
									useNegation = false;
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, false), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace2ReplaceFactRoleIter1, 0], snippet1Replace2ReplaceFactRoleIter1));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace2ReplaceFactRoleIter1, 0], snippet1Replace2ReplaceFactRoleIter1);
								}
								roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
							FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
						}
					}
				}
				else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
				{
					return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // MandatoryConstraint.SimpleMandatoryVerbalizer verbalization
	#region MandatoryConstraint.DisjunctiveMandatoryVerbalizer verbalization
	public partial class MandatoryConstraint
	{
		private partial class DisjunctiveMandatoryVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static DisjunctiveMandatoryVerbalizer myCache;
			public static DisjunctiveMandatoryVerbalizer GetVerbalizer()
			{
				DisjunctiveMandatoryVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<DisjunctiveMandatoryVerbalizer>(ref myCache, null as DisjunctiveMandatoryVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new DisjunctiveMandatoryVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<DisjunctiveMandatoryVerbalizer>(ref myCache, this, null as DisjunctiveMandatoryVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
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
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
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
				ObjectType singleConstrainedRolePlayerType = allConstraintRoles[0].RolePlayer;
				for (int testSameRolePlayerIndex = 1; testSameRolePlayerIndex < constraintRoleArity; ++testSameRolePlayerIndex)
				{
					if (allConstraintRoles[testSameRolePlayerIndex].RolePlayer != singleConstrainedRolePlayerType)
					{
						singleConstrainedRolePlayerType = null;
						break;
					}
				}
				#endregion // Preliminary
				#region Pattern Matches
				if (isNegative && maxFactArity <= 1 && singleConstrainedRolePlayerType != null)
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
						contextBasicReplacementIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex];
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
						contextBasicReplacementIndex = FactIter2;
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex];
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
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
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
				else if (!isNegative && maxFactArity <= 1 && singleConstrainedRolePlayerType != null)
				{
					bool missingReading1 = false;
					for (int readingMatchIndex1 = 0; !missingReading1 && readingMatchIndex1 < constraintRoleArity; ++readingMatchIndex1)
					{
						RoleBase primaryRole = allConstraintRoles[readingMatchIndex1];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.NoFrontText);
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
							contextBasicReplacementIndex = allFacts.IndexOf(parentFact);
							string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex];
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
							contextBasicReplacementIndex = allFacts.IndexOf(parentFact);
							string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex];
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
							contextBasicReplacementIndex = allFacts.IndexOf(parentFact);
							string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex];
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
							reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.NoFrontText | MatchingReadingOptions.AllowAnyOrder);
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
				else if (!isNegative && singleConstrainedRolePlayerType != null)
				{
					RolePathVerbalizer pathVerbalizer = RolePathVerbalizer.Create(this, new StandardRolePathRenderer(snippets, verbalizationContext, writer.FormatProvider));
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(this);
					bool missingReading1 = false;
					for (int readingMatchIndex1 = 0; !missingReading1 && readingMatchIndex1 < constraintRoleArity; ++readingMatchIndex1)
					{
						RoleBase primaryRole = allConstraintRoles[readingMatchIndex1];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.NoFrontText);
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
							contextBasicReplacementIndex = allFacts.IndexOf(parentFact);
							string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex];
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
						string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace1 = null;
						string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
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
							CoreVerbalizationSnippetType listSnippet;
							listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead | RolePathRolePlayerRenderingOptions.ResolveSupertype));
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
						}
						snippet1Replace1Replace1 = sbTemp.ToString();
						string snippet1Replace1Replace2 = null;
						int snippet1Replace1ReplaceCompositeCount2 = 0;
						int snippet1Replace1ReplaceCompositeIterator2;
						for (snippet1Replace1ReplaceCompositeIterator2 = 0; snippet1Replace1ReplaceCompositeIterator2 < constraintRoleArity; ++snippet1Replace1ReplaceCompositeIterator2)
						{
							if (factArity >= 2)
							{
								++snippet1Replace1ReplaceCompositeCount2;
							}
						}
						for (snippet1Replace1ReplaceCompositeIterator2 = 0; snippet1Replace1ReplaceCompositeIterator2 < constraintRoleArity; ++snippet1Replace1ReplaceCompositeIterator2)
						{
							if (factArity == 1)
							{
								++snippet1Replace1ReplaceCompositeCount2;
							}
						}
						snippet1Replace1ReplaceCompositeIterator2 = 0;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						string snippet1Replace1Replace2Item1 = null;
						for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity; ++RoleIter1)
						{
							RoleBase primaryRole = includedConstraintRoles[RoleIter1].Role;
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
							factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
							if (factArity >= 2)
							{
								CoreVerbalizationSnippetType listSnippet;
								if (snippet1Replace1ReplaceCompositeIterator2 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.IndentedLogicalOrListOpen;
								}
								else if (snippet1Replace1ReplaceCompositeIterator2 == snippet1Replace1ReplaceCompositeCount2 - 1)
								{
									if (snippet1Replace1ReplaceCompositeIterator2 == 1)
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
								reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.NoFrontText | MatchingReadingOptions.AllowAnyOrder);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								snippet1Replace1Replace2Item1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
									{
										foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
										{
											if (replaceRole == constraintRole.Role)
											{
												if (primaryRole == replaceRole)
												{
													return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
												}
												break;
											}
										}
										return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
									});
								sbTemp.Append(snippet1Replace1Replace2Item1);
								if (snippet1Replace1ReplaceCompositeIterator2 == snippet1Replace1ReplaceCompositeCount2 - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.IndentedLogicalOrListClose, isDeontic, isNegative));
								}
								++snippet1Replace1ReplaceCompositeIterator2;
							}
						}
						string snippet1Replace1Replace2Item2 = null;
						for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity; ++RoleIter2)
						{
							RoleBase primaryRole = includedConstraintRoles[RoleIter2].Role;
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
							factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
							if (factArity == 1)
							{
								CoreVerbalizationSnippetType listSnippet;
								if (snippet1Replace1ReplaceCompositeIterator2 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.IndentedLogicalOrListOpen;
								}
								else if (snippet1Replace1ReplaceCompositeIterator2 == snippet1Replace1ReplaceCompositeCount2 - 1)
								{
									if (snippet1Replace1ReplaceCompositeIterator2 == 1)
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
								reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								snippet1Replace1Replace2Item2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
									{
										foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
										{
											if (replaceRole == constraintRole.Role)
											{
												return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
											}
										}
										return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
									});
								sbTemp.Append(snippet1Replace1Replace2Item2);
								if (snippet1Replace1ReplaceCompositeIterator2 == snippet1Replace1ReplaceCompositeCount2 - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.IndentedLogicalOrListClose, isDeontic, isNegative));
								}
								++snippet1Replace1ReplaceCompositeIterator2;
							}
						}
						snippet1Replace1Replace2 = sbTemp.ToString();
						snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
				}
				else if (!isNegative)
				{
					RolePathVerbalizer pathVerbalizer = RolePathVerbalizer.Create(this, new StandardRolePathRenderer(snippets, verbalizationContext, writer.FormatProvider));
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(this);
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
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
						CoreVerbalizationSnippetType listSnippet;
						listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead | RolePathRolePlayerRenderingOptions.ResolveSupertype));
						sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
					}
					snippet1Replace1Replace1 = sbTemp.ToString();
					string snippet1Replace1Replace2 = null;
					int snippet1Replace1ReplaceCompositeCount2 = 0;
					int snippet1Replace1ReplaceCompositeIterator2;
					for (snippet1Replace1ReplaceCompositeIterator2 = 0; snippet1Replace1ReplaceCompositeIterator2 < constraintRoleArity; ++snippet1Replace1ReplaceCompositeIterator2)
					{
						if (factArity >= 2)
						{
							++snippet1Replace1ReplaceCompositeCount2;
						}
					}
					for (snippet1Replace1ReplaceCompositeIterator2 = 0; snippet1Replace1ReplaceCompositeIterator2 < constraintRoleArity; ++snippet1Replace1ReplaceCompositeIterator2)
					{
						if (factArity == 1)
						{
							++snippet1Replace1ReplaceCompositeCount2;
						}
					}
					snippet1Replace1ReplaceCompositeIterator2 = 0;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					string snippet1Replace1Replace2Item1 = null;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity; ++RoleIter1)
					{
						RoleBase primaryRole = includedConstraintRoles[RoleIter1].Role;
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
						if (factArity >= 2)
						{
							CoreVerbalizationSnippetType listSnippet;
							if (snippet1Replace1ReplaceCompositeIterator2 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.IndentedLogicalOrListOpen;
							}
							else if (snippet1Replace1ReplaceCompositeIterator2 == snippet1Replace1ReplaceCompositeCount2 - 1)
							{
								if (snippet1Replace1ReplaceCompositeIterator2 == 1)
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
							reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.NoFrontText | MatchingReadingOptions.AllowAnyOrder);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							snippet1Replace1Replace2Item1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
								{
									foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
									{
										if (replaceRole == constraintRole.Role)
										{
											return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
										}
									}
									return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
								});
							sbTemp.Append(snippet1Replace1Replace2Item1);
							if (snippet1Replace1ReplaceCompositeIterator2 == snippet1Replace1ReplaceCompositeCount2 - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.IndentedLogicalOrListClose, isDeontic, isNegative));
							}
							++snippet1Replace1ReplaceCompositeIterator2;
						}
					}
					string snippet1Replace1Replace2Item2 = null;
					for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity; ++RoleIter2)
					{
						RoleBase primaryRole = includedConstraintRoles[RoleIter2].Role;
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
						factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
						if (factArity == 1)
						{
							CoreVerbalizationSnippetType listSnippet;
							if (snippet1Replace1ReplaceCompositeIterator2 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.IndentedLogicalOrListOpen;
							}
							else if (snippet1Replace1ReplaceCompositeIterator2 == snippet1Replace1ReplaceCompositeCount2 - 1)
							{
								if (snippet1Replace1ReplaceCompositeIterator2 == 1)
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
							reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							snippet1Replace1Replace2Item2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
								{
									foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
									{
										if (replaceRole == constraintRole.Role)
										{
											return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
										}
									}
									return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
								});
							sbTemp.Append(snippet1Replace1Replace2Item2);
							if (snippet1Replace1ReplaceCompositeIterator2 == snippet1Replace1ReplaceCompositeCount2 - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.IndentedLogicalOrListClose, isDeontic, isNegative));
							}
							++snippet1Replace1ReplaceCompositeIterator2;
						}
					}
					snippet1Replace1Replace2 = sbTemp.ToString();
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
				{
					return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // MandatoryConstraint.DisjunctiveMandatoryVerbalizer verbalization
	#region UniquenessConstraint verbalization
	public partial class UniquenessConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			bool blockingErrors = false;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					blockingErrors = true;
					if (verbalizationContext.TestVerbalizedLocally(error))
					{
						continue;
					}
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
				}
				if (blockingErrors)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
			this.VerbalizeParts(writer, snippetsDictionary, verbalizationContext, sign);
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
		}
	}
	#endregion // UniquenessConstraint verbalization
	#region UniquenessConstraint.UniquenessConstraintVerbalizer verbalization
	public partial class UniquenessConstraint
	{
		private partial class UniquenessConstraintVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static UniquenessConstraintVerbalizer myCache;
			public static UniquenessConstraintVerbalizer GetVerbalizer()
			{
				UniquenessConstraintVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<UniquenessConstraintVerbalizer>(ref myCache, null as UniquenessConstraintVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new UniquenessConstraintVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<UniquenessConstraintVerbalizer>(ref myCache, this, null as UniquenessConstraintVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
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
					return false;
				}
				string[][,] allBasicRoleReplacements = new string[allFactsCount][,];
				VerbalizationSubscripter subscripter = new VerbalizationSubscripter(writer.FormatProvider);
				bool[] unaryReplacements = new bool[allFactsCount];
				int contextBasicReplacementIndex;
				int minFactArity = int.MaxValue;
				int maxFactArity = int.MinValue;
				for (int iFact = 0; iFact < allFactsCount; ++iFact)
				{
					FactType currentFact = allFacts[iFact];
					if (currentFact.ReadingRequiredError != null)
					{
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
					string[,] basicRoleReplacements = new string[factArity, 3];
					bool generateSubscripts = allFactsCount == 1;
					for (int i = 0; i < factArity; ++i)
					{
						Role factRole = factRoles[i + unaryRoleOffset].Role;
						ObjectType rolePlayer = factRole.RolePlayer;
						string basicReplacement;
						string basicSubscriptedReplacement = null;
						if (rolePlayer != null)
						{
							bool useSubscript = false;
							if (generateSubscripts)
							{
								int j = 0;
								for (; j < factArity; ++j)
								{
									if (i != j && rolePlayer == factRoles[j].Role.RolePlayer)
									{
										useSubscript = true;
										break;
									}
								}
							}
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							if (useSubscript)
							{
								basicSubscriptedReplacement = subscripter.PrepareSubscriptFormatString(snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							}
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
						}
						basicRoleReplacements[i, 0] = basicReplacement;
						if (basicSubscriptedReplacement == null)
						{
							basicRoleReplacements[i, 1] = basicReplacement;
							basicRoleReplacements[i, 2] = null;
						}
						else
						{
							basicRoleReplacements[i, 1] = basicSubscriptedReplacement;
							basicRoleReplacements[i, 2] = string.Empty;
						}
					}
					allBasicRoleReplacements[iFact] = basicRoleReplacements;
					unaryReplacements[iFact] = unaryRoleIndex.HasValue;
				}
				int constraintRoleArity = allConstraintRoles.Count;
				ConstraintRoleSequenceJoinPath joinPath = this.JoinPath;
				LeadRolePath singleLeadRolePath = null;
				bool isTrivialOppositeRolePath = false;
				if (joinPath != null && joinPath.IsAutomatic)
				{
					ObjectType rootObjectType;
					if (null != (singleLeadRolePath = joinPath.SingleLeadRolePath) && null != (rootObjectType = singleLeadRolePath.RootObjectType))
					{
						int i = 0;
						for (; i < constraintRoleArity; ++i)
						{
							RoleBase oppositeRole = allConstraintRoles[i].OppositeRole;
							if (oppositeRole == null || oppositeRole.Role.RolePlayer != rootObjectType)
							{
								break;
							}
						}
						isTrivialOppositeRolePath = i == constraintRoleArity;
					}
				}
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
						sbTemp.Append(allBasicRoleReplacements[0][unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1]), 0]);
						if (RoleIter1 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace1Replace1Replace1 = sbTemp.ToString();
					snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1);
					string snippet1Replace1Replace2 = null;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
					{
						RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
						string roleReplacement = null;
						string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2);
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
					}
					snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (allFactsCount == 1 && factArity == 1)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
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
						sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][ResolvedRoleIndex1, 0], ResolvedRoleIndex1));
						if (RoleIter1 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace1Replace1Replace1 = sbTemp.ToString();
					snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1);
					string snippet1Replace1Replace2 = null;
					for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
					{
						RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
						string roleReplacement = null;
						string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2);
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
					}
					snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (allFactsCount == 1 && factArity == constraintRoleArity && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.OccursInPopulation, isDeontic, isNegative);
					string snippet1Replace1Replace1 = null;
					string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.CombinationIdentifier, isDeontic, isNegative);
					string snippet1Replace1Replace1Replace1 = null;
					string snippet1Replace1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.UniversalQuantifier, isDeontic, isNegative);
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
						sbTemp.Append(allBasicRoleReplacements[0][unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1]), 0]);
						if (RoleIter1 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompactSimpleListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace1Replace1Replace1Replace1 = sbTemp.ToString();
					snippet1Replace1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1Replace1);
					snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1);
					string snippet1Replace1Replace2 = null;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
					{
						RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
						string roleReplacement = null;
						string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2);
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
					}
					snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (allFactsCount == 1 && factArity == constraintRoleArity)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
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
						sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][ResolvedRoleIndex1, 0], ResolvedRoleIndex1));
						if (RoleIter1 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompactSimpleListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace1Replace1Replace1Replace1 = sbTemp.ToString();
					snippet1Replace1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1Replace1);
					snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1);
					string snippet1Replace1Replace2 = null;
					for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
					{
						RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
						string roleReplacement = null;
						string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2);
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
					}
					snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (allFactsCount == 1 && factArity == 2)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, factRoles, MatchingReadingOptions.NoFrontText);
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
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1, 0], snippet1ReplaceFactRoleIter1);
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
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						if (reading != null)
						{
							verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
							string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
							string snippet1Replace1 = null;
							string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachNegatableCompactQuantifier, isDeontic, isNegative);
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
								int ResolvedRoleIndex1 = unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1]);
								sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][ResolvedRoleIndex1, 0], ResolvedRoleIndex1));
							}
							snippet1Replace1Replace1 = sbTemp.ToString();
							string snippet1Replace1Replace2 = null;
							for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2);
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
								roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
							FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
						}
					}
				}
				else if (allFactsCount == 1 && factArity - constraintRoleArity > 1 && isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachNegatableIndentedQuantifier, isDeontic, isNegative);
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
						sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(ResolvedRoleIndex1, allBasicRoleReplacements[0]), ResolvedRoleIndex1));
						if (RoleIter1 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace1Replace1 = sbTemp.ToString();
					string snippet1Replace1Replace2 = null;
					string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.SingularExistenceImplicationOperator, isDeontic, isNegative);
					string snippet1Replace1Replace2Replace1 = null;
					string snippet1Replace1Replace2ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.MoreThanOneQuantifier, isDeontic, isNegative);
					string snippet1Replace1Replace2Replace1Replace1 = null;
					string snippet1Replace1Replace2Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.CombinationIdentifier, isDeontic, isNegative);
					string snippet1Replace1Replace2Replace1Replace1Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					int snippet1Replace1Replace2Replace1Replace1ReplaceFilteredIter1;
					int snippet1Replace1Replace2Replace1Replace1ReplaceFilteredCount1 = 0;
					for (snippet1Replace1Replace2Replace1Replace1ReplaceFilteredIter1 = 0; snippet1Replace1Replace2Replace1Replace1ReplaceFilteredIter1 < factArity; ++snippet1Replace1Replace2Replace1Replace1ReplaceFilteredIter1)
					{
						if (allConstraintRoles.IndexOf(factRoles[snippet1Replace1Replace2Replace1Replace1ReplaceFilteredIter1].Role) == -1)
						{
							++snippet1Replace1Replace2Replace1Replace1ReplaceFilteredCount1;
						}
					}
					snippet1Replace1Replace2Replace1Replace1ReplaceFilteredIter1 = 0;
					for (int RoleIter1 = 0; RoleIter1 < factArity; ++RoleIter1)
					{
						if (allConstraintRoles.IndexOf(factRoles[RoleIter1].Role) == -1)
						{
							CoreVerbalizationSnippetType listSnippet;
							if (snippet1Replace1Replace2Replace1Replace1ReplaceFilteredIter1 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
							}
							else if (snippet1Replace1Replace2Replace1Replace1ReplaceFilteredIter1 == snippet1Replace1Replace2Replace1Replace1ReplaceFilteredCount1 - 1)
							{
								if (snippet1Replace1Replace2Replace1Replace1ReplaceFilteredIter1 == 1)
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
							sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(RoleIter1, allBasicRoleReplacements[0]), RoleIter1));
							if (snippet1Replace1Replace2Replace1Replace1ReplaceFilteredIter1 == snippet1Replace1Replace2Replace1Replace1ReplaceFilteredCount1 - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
							}
							++snippet1Replace1Replace2Replace1Replace1ReplaceFilteredIter1;
						}
					}
					snippet1Replace1Replace2Replace1Replace1Replace1 = sbTemp.ToString();
					snippet1Replace1Replace2Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace2Replace1ReplaceFormat1, snippet1Replace1Replace2Replace1Replace1Replace1);
					snippet1Replace1Replace2Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace2ReplaceFormat1, snippet1Replace1Replace2Replace1Replace1);
					string snippet1Replace1Replace2Replace2 = null;
					for (int snippet1Replace1Replace2ReplaceFactRoleIter2 = 0; snippet1Replace1Replace2ReplaceFactRoleIter2 < factArity; ++snippet1Replace1Replace2ReplaceFactRoleIter2)
					{
						RoleBase currentRole = factRoles[snippet1Replace1Replace2ReplaceFactRoleIter2];
						string roleReplacement = null;
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1Replace2ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1Replace2ReplaceFactRoleIter2));
						if (roleReplacement == null)
						{
							roleReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1Replace2ReplaceFactRoleIter2, 0], snippet1Replace1Replace2ReplaceFactRoleIter2);
						}
						roleReplacements[snippet1Replace1Replace2ReplaceFactRoleIter2] = roleReplacement;
					}
					snippet1Replace1Replace2Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
					snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1, snippet1Replace1Replace2Replace2);
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (allFactsCount == 1)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachNegatableIndentedQuantifier, isDeontic, isNegative);
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
						sbTemp.Append(hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(ResolvedRoleIndex1, allBasicRoleReplacements[0]), ResolvedRoleIndex1));
						if (RoleIter1 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
						}
					}
					snippet1Replace1Replace1 = sbTemp.ToString();
					string snippet1Replace1Replace2 = null;
					for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
					{
						RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
						string roleReplacement = null;
						if (allConstraintRoles.Contains(currentRole.Role))
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2));
						}
						else if (!allConstraintRoles.Contains(currentRole.Role))
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.OneQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2));
						}
						if (roleReplacement == null)
						{
							roleReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2);
						}
						roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
					}
					snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (isTrivialOppositeRolePath && !isNegative && minFactArity >= 2 && maxFactArity <= 2)
				{
					RolePathVerbalizer pathVerbalizer = RolePathVerbalizer.Create(this, new StandardRolePathRenderer(snippets, verbalizationContext, writer.FormatProvider));
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(this);
					bool missingReading1 = false;
					for (int readingMatchIndex1 = 0; !missingReading1 && readingMatchIndex1 < constraintRoleArity; ++readingMatchIndex1)
					{
						RoleBase primaryRole = allConstraintRoles[readingMatchIndex1];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.InvertLeadRoles | MatchingReadingOptions.NoFrontText);
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
							RoleBase primaryRole = includedConstraintRoles[RoleIter1].Role;
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
							contextBasicReplacementIndex = allFacts.IndexOf(parentFact);
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
							reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter1], hyphenBinder.GetRoleFormatString(unaryReplacements[contextBasicReplacementIndex] ? 0 : FactType.IndexOfRole(factRoles, primaryRole)), RolePathRolePlayerRenderingOptions.None));
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
							RoleBase primaryRole = includedConstraintRoles[RoleIter2].Role;
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
							factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
							contextBasicReplacementIndex = allFacts.IndexOf(parentFact);
							string[,] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex];
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
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1ReplaceFactRoleIter2, 0], snippet1ReplaceFactRoleIter2);
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
								{
									foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
									{
										if (replaceRole == constraintRole.Role)
										{
											return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
										}
									}
									if (snippet1ReplaceIsFirstPass2)
									{
										return string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.AtMostOneQuantifier, isDeontic, isNegative), pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None));
									}
									return "";
								});
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
						string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace1 = null;
						string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachNegatableIndentedQuantifier, isDeontic, isNegative);
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
							sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead));
							if (RoleIter1 == constraintRoleArity - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
							}
						}
						snippet1Replace1Replace1 = sbTemp.ToString();
						string snippet1Replace1Replace2 = null;
						string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.SingularExistenceImplicationOperator, isDeontic, isNegative);
						string snippet1Replace1Replace2Replace1 = null;
						string snippet1Replace1Replace2ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.OneQuantifier, isDeontic, isNegative);
						string snippet1Replace1Replace2Replace1Replace1 = null;
						snippet1Replace1Replace2Replace1Replace1 = pathVerbalizer.RenderAssociatedRolePlayer(singleLeadRolePath.PathRoot, null, RolePathRolePlayerRenderingOptions.None);
						snippet1Replace1Replace2Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace2ReplaceFormat1, snippet1Replace1Replace2Replace1Replace1);
						string snippet1Replace1Replace2Replace2 = null;
						if (pathVerbalizer.HasPathVerbalization(joinPath))
						{
							pathVerbalizer.Options = RolePathVerbalizerOptions.None;
							if (sbTemp == null)
							{
								sbTemp = new StringBuilder();
							}
							else
							{
								sbTemp.Length = 0;
							}
							snippet1Replace1Replace2Replace2 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
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
							FactType[] snippet1Replace1Replace2ReplaceUniqueFactTypes2 = new FactType[constraintRoleArity];
							FactType snippet1Replace1Replace2ReplaceTestUniqueFactType2;
							int snippet1Replace1Replace2ReplaceFilteredIter2;
							int snippet1Replace1Replace2ReplaceFilteredCount2 = 0;
							for (snippet1Replace1Replace2ReplaceFilteredIter2 = 0; snippet1Replace1Replace2ReplaceFilteredIter2 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceFilteredIter2)
							{
								RoleBase primaryRole = allConstraintRoles[snippet1Replace1Replace2ReplaceFilteredIter2];
								if (Array.IndexOf(snippet1Replace1Replace2ReplaceUniqueFactTypes2, snippet1Replace1Replace2ReplaceTestUniqueFactType2 = primaryRole.FactType) == -1)
								{
									snippet1Replace1Replace2ReplaceUniqueFactTypes2[snippet1Replace1Replace2ReplaceFilteredIter2] = snippet1Replace1Replace2ReplaceTestUniqueFactType2;
									++snippet1Replace1Replace2ReplaceFilteredCount2;
								}
							}
							Array.Clear(snippet1Replace1Replace2ReplaceUniqueFactTypes2, 0, snippet1Replace1Replace2ReplaceUniqueFactTypes2.Length);
							snippet1Replace1Replace2ReplaceFilteredIter2 = 0;
							for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity; ++RoleIter2)
							{
								RoleBase primaryRole = allConstraintRoles[RoleIter2];
								parentFact = primaryRole.FactType;
								predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
								allReadingOrders = parentFact.ReadingOrderCollection;
								factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
								unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
								factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
								contextBasicReplacementIndex = allFacts.IndexOf(parentFact);
								string[,] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex];
								if (Array.IndexOf(snippet1Replace1Replace2ReplaceUniqueFactTypes2, snippet1Replace1Replace2ReplaceTestUniqueFactType2 = primaryRole.FactType) == -1)
								{
									snippet1Replace1Replace2ReplaceUniqueFactTypes2[RoleIter2] = snippet1Replace1Replace2ReplaceTestUniqueFactType2;
									CoreVerbalizationSnippetType listSnippet;
									if (snippet1Replace1Replace2ReplaceFilteredIter2 == 0)
									{
										listSnippet = CoreVerbalizationSnippetType.CompoundListOpen;
									}
									else if (snippet1Replace1Replace2ReplaceFilteredIter2 == snippet1Replace1Replace2ReplaceFilteredCount2 - 1)
									{
										if (snippet1Replace1Replace2ReplaceFilteredIter2 == 1)
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
									reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
									hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
									for (int snippet1Replace1Replace2ReplaceFactRoleIter2 = 0; snippet1Replace1Replace2ReplaceFactRoleIter2 < factArity; ++snippet1Replace1Replace2ReplaceFactRoleIter2)
									{
										RoleBase currentRole = factRoles[snippet1Replace1Replace2ReplaceFactRoleIter2];
										string roleReplacement = null;
										string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1Replace1Replace2ReplaceFactRoleIter2, 0], snippet1Replace1Replace2ReplaceFactRoleIter2);
										if (roleReplacement == null)
										{
											roleReplacement = basicReplacement;
										}
										roleReplacements[snippet1Replace1Replace2ReplaceFactRoleIter2] = roleReplacement;
									}
									snippet1Replace1Replace2Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
										{
											foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
											{
												if (replaceRole == constraintRole.Role)
												{
													return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
												}
											}
											return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
										});
									sbTemp.Append(snippet1Replace1Replace2Replace2);
									if (snippet1Replace1Replace2ReplaceFilteredIter2 == snippet1Replace1Replace2ReplaceFilteredCount2 - 1)
									{
										sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
									}
									++snippet1Replace1Replace2ReplaceFilteredIter2;
								}
							}
							snippet1Replace1Replace2Replace2 = sbTemp.ToString();
						}
						snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1, snippet1Replace1Replace2Replace2);
						snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
				}
				else if (isTrivialOppositeRolePath && isNegative && minFactArity >= 2 && maxFactArity <= 2)
				{
					RolePathVerbalizer pathVerbalizer = RolePathVerbalizer.Create(this, new StandardRolePathRenderer(snippets, verbalizationContext, writer.FormatProvider));
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(this);
					bool missingReading1 = false;
					for (int readingMatchIndex1 = 0; !missingReading1 && readingMatchIndex1 < constraintRoleArity; ++readingMatchIndex1)
					{
						RoleBase primaryRole = allConstraintRoles[readingMatchIndex1];
						parentFact = primaryRole.FactType;
						predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.InvertLeadRoles | MatchingReadingOptions.NoFrontText);
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
							RoleBase primaryRole = includedConstraintRoles[RoleIter1].Role;
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
							factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
							contextBasicReplacementIndex = allFacts.IndexOf(parentFact);
							string[,] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex];
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
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1ReplaceFactRoleIter1, 0], snippet1ReplaceFactRoleIter1);
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
								{
									foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
									{
										if (replaceRole == constraintRole.Role)
										{
											return string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative), pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None));
										}
									}
									if (snippet1ReplaceIsFirstPass1)
									{
										return string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.MoreThanOneQuantifier, isDeontic, isNegative), pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None));
									}
									return "";
								});
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
						string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace1 = null;
						string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachNegatableIndentedQuantifier, isDeontic, isNegative);
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
							sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead));
							if (RoleIter1 == constraintRoleArity - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
							}
						}
						snippet1Replace1Replace1 = sbTemp.ToString();
						string snippet1Replace1Replace2 = null;
						string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.SingularExistenceImplicationOperator, isDeontic, isNegative);
						string snippet1Replace1Replace2Replace1 = null;
						string snippet1Replace1Replace2ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.OneQuantifier, isDeontic, isNegative);
						string snippet1Replace1Replace2Replace1Replace1 = null;
						snippet1Replace1Replace2Replace1Replace1 = pathVerbalizer.RenderAssociatedRolePlayer(singleLeadRolePath.PathRoot, null, RolePathRolePlayerRenderingOptions.None);
						snippet1Replace1Replace2Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace2ReplaceFormat1, snippet1Replace1Replace2Replace1Replace1);
						string snippet1Replace1Replace2Replace2 = null;
						if (pathVerbalizer.HasPathVerbalization(joinPath))
						{
							pathVerbalizer.Options = RolePathVerbalizerOptions.None;
							if (sbTemp == null)
							{
								sbTemp = new StringBuilder();
							}
							else
							{
								sbTemp.Length = 0;
							}
							snippet1Replace1Replace2Replace2 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
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
							FactType[] snippet1Replace1Replace2ReplaceUniqueFactTypes2 = new FactType[constraintRoleArity];
							FactType snippet1Replace1Replace2ReplaceTestUniqueFactType2;
							int snippet1Replace1Replace2ReplaceFilteredIter2;
							int snippet1Replace1Replace2ReplaceFilteredCount2 = 0;
							for (snippet1Replace1Replace2ReplaceFilteredIter2 = 0; snippet1Replace1Replace2ReplaceFilteredIter2 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceFilteredIter2)
							{
								RoleBase primaryRole = allConstraintRoles[snippet1Replace1Replace2ReplaceFilteredIter2];
								if (Array.IndexOf(snippet1Replace1Replace2ReplaceUniqueFactTypes2, snippet1Replace1Replace2ReplaceTestUniqueFactType2 = primaryRole.FactType) == -1)
								{
									snippet1Replace1Replace2ReplaceUniqueFactTypes2[snippet1Replace1Replace2ReplaceFilteredIter2] = snippet1Replace1Replace2ReplaceTestUniqueFactType2;
									++snippet1Replace1Replace2ReplaceFilteredCount2;
								}
							}
							Array.Clear(snippet1Replace1Replace2ReplaceUniqueFactTypes2, 0, snippet1Replace1Replace2ReplaceUniqueFactTypes2.Length);
							snippet1Replace1Replace2ReplaceFilteredIter2 = 0;
							for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity; ++RoleIter2)
							{
								RoleBase primaryRole = allConstraintRoles[RoleIter2];
								parentFact = primaryRole.FactType;
								predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
								allReadingOrders = parentFact.ReadingOrderCollection;
								factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
								unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
								factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
								contextBasicReplacementIndex = allFacts.IndexOf(parentFact);
								string[,] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex];
								if (Array.IndexOf(snippet1Replace1Replace2ReplaceUniqueFactTypes2, snippet1Replace1Replace2ReplaceTestUniqueFactType2 = primaryRole.FactType) == -1)
								{
									snippet1Replace1Replace2ReplaceUniqueFactTypes2[RoleIter2] = snippet1Replace1Replace2ReplaceTestUniqueFactType2;
									CoreVerbalizationSnippetType listSnippet;
									if (snippet1Replace1Replace2ReplaceFilteredIter2 == 0)
									{
										listSnippet = CoreVerbalizationSnippetType.CompoundListOpen;
									}
									else if (snippet1Replace1Replace2ReplaceFilteredIter2 == snippet1Replace1Replace2ReplaceFilteredCount2 - 1)
									{
										if (snippet1Replace1Replace2ReplaceFilteredIter2 == 1)
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
									reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
									hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
									for (int snippet1Replace1Replace2ReplaceFactRoleIter2 = 0; snippet1Replace1Replace2ReplaceFactRoleIter2 < factArity; ++snippet1Replace1Replace2ReplaceFactRoleIter2)
									{
										RoleBase currentRole = factRoles[snippet1Replace1Replace2ReplaceFactRoleIter2];
										string roleReplacement = null;
										string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1Replace1Replace2ReplaceFactRoleIter2, 0], snippet1Replace1Replace2ReplaceFactRoleIter2);
										if (roleReplacement == null)
										{
											roleReplacement = basicReplacement;
										}
										roleReplacements[snippet1Replace1Replace2ReplaceFactRoleIter2] = roleReplacement;
									}
									snippet1Replace1Replace2Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
										{
											foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
											{
												if (replaceRole == constraintRole.Role)
												{
													return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
												}
											}
											return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
										});
									sbTemp.Append(snippet1Replace1Replace2Replace2);
									if (snippet1Replace1Replace2ReplaceFilteredIter2 == snippet1Replace1Replace2ReplaceFilteredCount2 - 1)
									{
										sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
									}
									++snippet1Replace1Replace2ReplaceFilteredIter2;
								}
							}
							snippet1Replace1Replace2Replace2 = sbTemp.ToString();
						}
						snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1, snippet1Replace1Replace2Replace2);
						snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
				}
				else if (minFactArity >= 1 && maxFactArity <= 1)
				{
					RolePathVerbalizer pathVerbalizer = RolePathVerbalizer.Create(this, new StandardRolePathRenderer(snippets, verbalizationContext, writer.FormatProvider));
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(this);
					IList<object> preProjectionKeys = pathVerbalizer.GetPreProjectionPrimaryNodeKeys(includedConstraintRoles);
					int preProjectionKeyCount = preProjectionKeys != null ? preProjectionKeys.Count : 0;
					if (0 != preProjectionKeyCount)
					{
						verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
						string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace1 = null;
						if (1 == preProjectionKeyCount && pathVerbalizer.KeyedVariableLeadsVerbalization(joinPath, preProjectionKeys[0]))
						{
							pathVerbalizer.Options = RolePathVerbalizerOptions.None;
							pathVerbalizer.LeadVariableQuantifier = new CoreSnippetIdentifier(CoreVerbalizationSnippetType.OneQuantifier, isDeontic, isNegative);
							if (sbTemp == null)
							{
								sbTemp = new StringBuilder();
							}
							else
							{
								sbTemp.Length = 0;
							}
							snippet1Replace1 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
						}
						else
						{
							string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.SingularExistenceImplicationOperator, isDeontic, isNegative);
							string snippet1Replace1Replace1 = null;
							string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.OneQuantifier, isDeontic, isNegative);
							string snippet1Replace1Replace1Replace1 = null;
							if (sbTemp == null)
							{
								sbTemp = new StringBuilder();
							}
							else
							{
								sbTemp.Length = 0;
							}
							for (int RoleIter1 = 0; RoleIter1 < preProjectionKeyCount; ++RoleIter1)
							{
								CoreVerbalizationSnippetType listSnippet;
								if (RoleIter1 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
								}
								else if (RoleIter1 == preProjectionKeyCount - 1)
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
								sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(preProjectionKeys[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead));
								if (RoleIter1 == preProjectionKeyCount - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
								}
							}
							snippet1Replace1Replace1Replace1 = sbTemp.ToString();
							snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1);
							string snippet1Replace1Replace2 = null;
							if (pathVerbalizer.HasPathVerbalization(joinPath))
							{
								pathVerbalizer.Options = RolePathVerbalizerOptions.None;
								if (sbTemp == null)
								{
									sbTemp = new StringBuilder();
								}
								else
								{
									sbTemp.Length = 0;
								}
								snippet1Replace1Replace2 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
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
								FactType[] snippet1Replace1ReplaceUniqueFactTypes2 = new FactType[constraintRoleArity];
								FactType snippet1Replace1ReplaceTestUniqueFactType2;
								int snippet1Replace1ReplaceFilteredIter2;
								int snippet1Replace1ReplaceFilteredCount2 = 0;
								for (snippet1Replace1ReplaceFilteredIter2 = 0; snippet1Replace1ReplaceFilteredIter2 < constraintRoleArity; ++snippet1Replace1ReplaceFilteredIter2)
								{
									RoleBase primaryRole = allConstraintRoles[snippet1Replace1ReplaceFilteredIter2];
									if (Array.IndexOf(snippet1Replace1ReplaceUniqueFactTypes2, snippet1Replace1ReplaceTestUniqueFactType2 = primaryRole.FactType) == -1)
									{
										snippet1Replace1ReplaceUniqueFactTypes2[snippet1Replace1ReplaceFilteredIter2] = snippet1Replace1ReplaceTestUniqueFactType2;
										++snippet1Replace1ReplaceFilteredCount2;
									}
								}
								Array.Clear(snippet1Replace1ReplaceUniqueFactTypes2, 0, snippet1Replace1ReplaceUniqueFactTypes2.Length);
								snippet1Replace1ReplaceFilteredIter2 = 0;
								for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity; ++RoleIter2)
								{
									RoleBase primaryRole = allConstraintRoles[RoleIter2];
									parentFact = primaryRole.FactType;
									predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
									allReadingOrders = parentFact.ReadingOrderCollection;
									factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
									unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
									factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
									contextBasicReplacementIndex = allFacts.IndexOf(parentFact);
									string[,] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex];
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
										reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
										hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
										for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
										{
											RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
											string roleReplacement = null;
											string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2);
											if (roleReplacement == null)
											{
												roleReplacement = basicReplacement;
											}
											roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
										}
										snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
											{
												foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
												{
													if (replaceRole == constraintRole.Role)
													{
														return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
													}
												}
												return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
											});
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
						}
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
				}
				else
				{
					RolePathVerbalizer pathVerbalizer = RolePathVerbalizer.Create(this, new StandardRolePathRenderer(snippets, verbalizationContext, writer.FormatProvider));
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(this);
					IList<object> preProjectionKeys = pathVerbalizer.GetPreProjectionPrimaryNodeKeys(includedConstraintRoles);
					int preProjectionKeyCount = preProjectionKeys != null ? preProjectionKeys.Count : 0;
					if (0 != preProjectionKeyCount)
					{
						verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
						string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace1 = null;
						string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachNegatableIndentedQuantifier, isDeontic, isNegative);
						string snippet1Replace1Replace1 = null;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						int snippet1Replace1ReplaceFilteredIter1;
						int snippet1Replace1ReplaceFilteredCount1 = 0;
						for (snippet1Replace1ReplaceFilteredIter1 = 0; snippet1Replace1ReplaceFilteredIter1 < constraintRoleArity; ++snippet1Replace1ReplaceFilteredIter1)
						{
							ObjectType snippet1Replace1ReplaceRolePlayer1 = includedConstraintRoles[snippet1Replace1ReplaceFilteredIter1].Role.RolePlayer;
							if (snippet1Replace1ReplaceRolePlayer1 == null || !snippet1Replace1ReplaceRolePlayer1.IsImplicitBooleanValue)
							{
								++snippet1Replace1ReplaceFilteredCount1;
							}
						}
						snippet1Replace1ReplaceFilteredIter1 = 0;
						for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity; ++RoleIter1)
						{
							ObjectType snippet1Replace1ReplaceRolePlayer1 = includedConstraintRoles[RoleIter1].Role.RolePlayer;
							if (snippet1Replace1ReplaceRolePlayer1 == null || !snippet1Replace1ReplaceRolePlayer1.IsImplicitBooleanValue)
							{
								CoreVerbalizationSnippetType listSnippet;
								if (snippet1Replace1ReplaceFilteredIter1 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
								}
								else if (snippet1Replace1ReplaceFilteredIter1 == snippet1Replace1ReplaceFilteredCount1 - 1)
								{
									if (snippet1Replace1ReplaceFilteredIter1 == 1)
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
								sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead));
								if (snippet1Replace1ReplaceFilteredIter1 == snippet1Replace1ReplaceFilteredCount1 - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
								}
								++snippet1Replace1ReplaceFilteredIter1;
							}
						}
						snippet1Replace1Replace1 = sbTemp.ToString();
						string snippet1Replace1Replace2 = null;
						if (1 == preProjectionKeyCount && pathVerbalizer.KeyedVariableLeadsVerbalization(joinPath, preProjectionKeys[0]))
						{
							pathVerbalizer.Options = RolePathVerbalizerOptions.None;
							pathVerbalizer.LeadVariableQuantifier = new CoreSnippetIdentifier(CoreVerbalizationSnippetType.OneQuantifier, isDeontic, isNegative);
							if (sbTemp == null)
							{
								sbTemp = new StringBuilder();
							}
							else
							{
								sbTemp.Length = 0;
							}
							snippet1Replace1Replace2 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
						}
						else
						{
							string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.SingularExistenceImplicationOperator, isDeontic, isNegative);
							string snippet1Replace1Replace2Replace1 = null;
							string snippet1Replace1Replace2ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.OneQuantifier, isDeontic, isNegative);
							string snippet1Replace1Replace2Replace1Replace1 = null;
							if (sbTemp == null)
							{
								sbTemp = new StringBuilder();
							}
							else
							{
								sbTemp.Length = 0;
							}
							for (int RoleIter1 = 0; RoleIter1 < preProjectionKeyCount; ++RoleIter1)
							{
								CoreVerbalizationSnippetType listSnippet;
								if (RoleIter1 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
								}
								else if (RoleIter1 == preProjectionKeyCount - 1)
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
								sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(preProjectionKeys[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead));
								if (RoleIter1 == preProjectionKeyCount - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
								}
							}
							snippet1Replace1Replace2Replace1Replace1 = sbTemp.ToString();
							snippet1Replace1Replace2Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace2ReplaceFormat1, snippet1Replace1Replace2Replace1Replace1);
							string snippet1Replace1Replace2Replace2 = null;
							if (pathVerbalizer.HasPathVerbalization(joinPath))
							{
								pathVerbalizer.Options = RolePathVerbalizerOptions.None;
								if (sbTemp == null)
								{
									sbTemp = new StringBuilder();
								}
								else
								{
									sbTemp.Length = 0;
								}
								snippet1Replace1Replace2Replace2 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
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
								FactType[] snippet1Replace1Replace2ReplaceUniqueFactTypes2 = new FactType[constraintRoleArity];
								FactType snippet1Replace1Replace2ReplaceTestUniqueFactType2;
								int snippet1Replace1Replace2ReplaceFilteredIter2;
								int snippet1Replace1Replace2ReplaceFilteredCount2 = 0;
								for (snippet1Replace1Replace2ReplaceFilteredIter2 = 0; snippet1Replace1Replace2ReplaceFilteredIter2 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceFilteredIter2)
								{
									RoleBase primaryRole = allConstraintRoles[snippet1Replace1Replace2ReplaceFilteredIter2];
									if (Array.IndexOf(snippet1Replace1Replace2ReplaceUniqueFactTypes2, snippet1Replace1Replace2ReplaceTestUniqueFactType2 = primaryRole.FactType) == -1)
									{
										snippet1Replace1Replace2ReplaceUniqueFactTypes2[snippet1Replace1Replace2ReplaceFilteredIter2] = snippet1Replace1Replace2ReplaceTestUniqueFactType2;
										++snippet1Replace1Replace2ReplaceFilteredCount2;
									}
								}
								Array.Clear(snippet1Replace1Replace2ReplaceUniqueFactTypes2, 0, snippet1Replace1Replace2ReplaceUniqueFactTypes2.Length);
								snippet1Replace1Replace2ReplaceFilteredIter2 = 0;
								for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity; ++RoleIter2)
								{
									RoleBase primaryRole = allConstraintRoles[RoleIter2];
									parentFact = primaryRole.FactType;
									predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
									allReadingOrders = parentFact.ReadingOrderCollection;
									factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
									unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
									factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
									contextBasicReplacementIndex = allFacts.IndexOf(parentFact);
									string[,] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex];
									if (Array.IndexOf(snippet1Replace1Replace2ReplaceUniqueFactTypes2, snippet1Replace1Replace2ReplaceTestUniqueFactType2 = primaryRole.FactType) == -1)
									{
										snippet1Replace1Replace2ReplaceUniqueFactTypes2[RoleIter2] = snippet1Replace1Replace2ReplaceTestUniqueFactType2;
										CoreVerbalizationSnippetType listSnippet;
										if (snippet1Replace1Replace2ReplaceFilteredIter2 == 0)
										{
											listSnippet = CoreVerbalizationSnippetType.CompoundListOpen;
										}
										else if (snippet1Replace1Replace2ReplaceFilteredIter2 == snippet1Replace1Replace2ReplaceFilteredCount2 - 1)
										{
											if (snippet1Replace1Replace2ReplaceFilteredIter2 == 1)
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
										reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
										hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
										for (int snippet1Replace1Replace2ReplaceFactRoleIter2 = 0; snippet1Replace1Replace2ReplaceFactRoleIter2 < factArity; ++snippet1Replace1Replace2ReplaceFactRoleIter2)
										{
											RoleBase currentRole = factRoles[snippet1Replace1Replace2ReplaceFactRoleIter2];
											string roleReplacement = null;
											string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet1Replace1Replace2ReplaceFactRoleIter2, 0], snippet1Replace1Replace2ReplaceFactRoleIter2);
											if (roleReplacement == null)
											{
												roleReplacement = basicReplacement;
											}
											roleReplacements[snippet1Replace1Replace2ReplaceFactRoleIter2] = roleReplacement;
										}
										snippet1Replace1Replace2Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
											{
												foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
												{
													if (replaceRole == constraintRole.Role)
													{
														return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
													}
												}
												return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
											});
										sbTemp.Append(snippet1Replace1Replace2Replace2);
										if (snippet1Replace1Replace2ReplaceFilteredIter2 == snippet1Replace1Replace2ReplaceFilteredCount2 - 1)
										{
											sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
										}
										++snippet1Replace1Replace2ReplaceFilteredIter2;
									}
								}
								snippet1Replace1Replace2Replace2 = sbTemp.ToString();
							}
							snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1, snippet1Replace1Replace2Replace2);
						}
						snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
					if (preProjectionKeyCount == 0)
					{
						if (0 != preProjectionKeyCount)
						{
							writer.WriteLine();
						}
						string snippetFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet2Replace1 = null;
						string snippet2ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachNegatableIndentedQuantifier, isDeontic, isNegative);
						string snippet2Replace1Replace1 = null;
						string snippet2Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.CombinationIdentifier, isDeontic, isNegative);
						string snippet2Replace1Replace1Replace1 = null;
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
							sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead));
							if (RoleIter1 == constraintRoleArity - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
							}
						}
						snippet2Replace1Replace1Replace1 = sbTemp.ToString();
						snippet2Replace1Replace1 = string.Format(writer.FormatProvider, snippet2Replace1ReplaceFormat1, snippet2Replace1Replace1Replace1);
						string snippet2Replace1Replace2 = null;
						string snippet2Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLine, isDeontic, isNegative);
						string snippet2Replace1Replace2Replace1 = null;
						if (pathVerbalizer.HasPathVerbalization(joinPath))
						{
							pathVerbalizer.Options = RolePathVerbalizerOptions.None;
							if (sbTemp == null)
							{
								sbTemp = new StringBuilder();
							}
							else
							{
								sbTemp.Length = 0;
							}
							snippet2Replace1Replace2Replace1 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
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
							FactType[] snippet2Replace1Replace2ReplaceUniqueFactTypes1 = new FactType[constraintRoleArity];
							FactType snippet2Replace1Replace2ReplaceTestUniqueFactType1;
							int snippet2Replace1Replace2ReplaceFilteredIter1;
							int snippet2Replace1Replace2ReplaceFilteredCount1 = 0;
							for (snippet2Replace1Replace2ReplaceFilteredIter1 = 0; snippet2Replace1Replace2ReplaceFilteredIter1 < constraintRoleArity; ++snippet2Replace1Replace2ReplaceFilteredIter1)
							{
								RoleBase primaryRole = allConstraintRoles[snippet2Replace1Replace2ReplaceFilteredIter1];
								if (Array.IndexOf(snippet2Replace1Replace2ReplaceUniqueFactTypes1, snippet2Replace1Replace2ReplaceTestUniqueFactType1 = primaryRole.FactType) == -1)
								{
									snippet2Replace1Replace2ReplaceUniqueFactTypes1[snippet2Replace1Replace2ReplaceFilteredIter1] = snippet2Replace1Replace2ReplaceTestUniqueFactType1;
									++snippet2Replace1Replace2ReplaceFilteredCount1;
								}
							}
							Array.Clear(snippet2Replace1Replace2ReplaceUniqueFactTypes1, 0, snippet2Replace1Replace2ReplaceUniqueFactTypes1.Length);
							snippet2Replace1Replace2ReplaceFilteredIter1 = 0;
							for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity; ++RoleIter1)
							{
								RoleBase primaryRole = allConstraintRoles[RoleIter1];
								parentFact = primaryRole.FactType;
								predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
								allReadingOrders = parentFact.ReadingOrderCollection;
								factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
								unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
								factArity = unaryRoleIndex.HasValue ? 1 : factRoles.Count;
								contextBasicReplacementIndex = allFacts.IndexOf(parentFact);
								string[,] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex];
								if (Array.IndexOf(snippet2Replace1Replace2ReplaceUniqueFactTypes1, snippet2Replace1Replace2ReplaceTestUniqueFactType1 = primaryRole.FactType) == -1)
								{
									snippet2Replace1Replace2ReplaceUniqueFactTypes1[RoleIter1] = snippet2Replace1Replace2ReplaceTestUniqueFactType1;
									CoreVerbalizationSnippetType listSnippet;
									if (snippet2Replace1Replace2ReplaceFilteredIter1 == 0)
									{
										listSnippet = CoreVerbalizationSnippetType.CompoundListOpen;
									}
									else if (snippet2Replace1Replace2ReplaceFilteredIter1 == snippet2Replace1Replace2ReplaceFilteredCount1 - 1)
									{
										if (snippet2Replace1Replace2ReplaceFilteredIter1 == 1)
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
									reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
									hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
									for (int snippet2Replace1Replace2ReplaceFactRoleIter1 = 0; snippet2Replace1Replace2ReplaceFactRoleIter1 < factArity; ++snippet2Replace1Replace2ReplaceFactRoleIter1)
									{
										RoleBase currentRole = factRoles[snippet2Replace1Replace2ReplaceFactRoleIter1];
										string roleReplacement = null;
										string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(basicRoleReplacements[snippet2Replace1Replace2ReplaceFactRoleIter1, 0], snippet2Replace1Replace2ReplaceFactRoleIter1);
										if (roleReplacement == null)
										{
											roleReplacement = basicReplacement;
										}
										roleReplacements[snippet2Replace1Replace2ReplaceFactRoleIter1] = roleReplacement;
									}
									snippet2Replace1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
										{
											foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
											{
												if (replaceRole == constraintRole.Role)
												{
													return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
												}
											}
											return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
										});
									sbTemp.Append(snippet2Replace1Replace2Replace1);
									if (snippet2Replace1Replace2ReplaceFilteredIter1 == snippet2Replace1Replace2ReplaceFilteredCount1 - 1)
									{
										sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
									}
									++snippet2Replace1Replace2ReplaceFilteredIter1;
								}
							}
							snippet2Replace1Replace2Replace1 = sbTemp.ToString();
						}
						string snippet2Replace1Replace2Replace2 = null;
						string snippet2Replace1Replace2ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ContextCombinationOccurrence, isDeontic, isNegative);
						string snippet2Replace1Replace2Replace2Replace1 = null;
						string snippet2Replace1Replace2Replace2ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ExactlyOneUntypedOccurrence, isDeontic, isNegative);
						snippet2Replace1Replace2Replace2Replace1 = string.Format(writer.FormatProvider, snippet2Replace1Replace2Replace2ReplaceFormat1);
						snippet2Replace1Replace2Replace2 = string.Format(writer.FormatProvider, snippet2Replace1Replace2ReplaceFormat2, snippet2Replace1Replace2Replace2Replace1);
						snippet2Replace1Replace2 = string.Format(writer.FormatProvider, snippet2Replace1ReplaceFormat2, snippet2Replace1Replace2Replace1, snippet2Replace1Replace2Replace2);
						snippet2Replace1 = string.Format(writer.FormatProvider, snippet2ReplaceFormat1, snippet2Replace1Replace1, snippet2Replace1Replace2);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat2, snippet2Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // UniquenessConstraint.UniquenessConstraintVerbalizer verbalization
	#region UniquenessConstraint.UniquenessPossibilityVerbalizer verbalization
	public partial class UniquenessConstraint
	{
		private partial class UniquenessPossibilityVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static UniquenessPossibilityVerbalizer myCache;
			public static UniquenessPossibilityVerbalizer GetVerbalizer()
			{
				UniquenessPossibilityVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<UniquenessPossibilityVerbalizer>(ref myCache, null as UniquenessPossibilityVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new UniquenessPossibilityVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<UniquenessPossibilityVerbalizer>(ref myCache, this, null as UniquenessPossibilityVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
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
					return false;
				}
				string[][,] allBasicRoleReplacements = new string[allFactsCount][,];
				VerbalizationSubscripter subscripter = new VerbalizationSubscripter(writer.FormatProvider);
				bool[] unaryReplacements = new bool[allFactsCount];
				int minFactArity = int.MaxValue;
				int maxFactArity = int.MinValue;
				for (int iFact = 0; iFact < allFactsCount; ++iFact)
				{
					FactType currentFact = allFacts[iFact];
					if (currentFact.ReadingRequiredError != null)
					{
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
					string[,] basicRoleReplacements = new string[factArity, 3];
					bool generateSubscripts = allFactsCount == 1;
					for (int i = 0; i < factArity; ++i)
					{
						Role factRole = factRoles[i + unaryRoleOffset].Role;
						ObjectType rolePlayer = factRole.RolePlayer;
						string basicReplacement;
						string basicSubscriptedReplacement = null;
						if (rolePlayer != null)
						{
							bool useSubscript = false;
							if (generateSubscripts)
							{
								int j = 0;
								for (; j < factArity; ++j)
								{
									if (i != j && rolePlayer == factRoles[j].Role.RolePlayer)
									{
										useSubscript = true;
										break;
									}
								}
							}
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							if (useSubscript)
							{
								basicSubscriptedReplacement = subscripter.PrepareSubscriptFormatString(snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							}
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
						}
						basicRoleReplacements[i, 0] = basicReplacement;
						if (basicSubscriptedReplacement == null)
						{
							basicRoleReplacements[i, 1] = basicReplacement;
							basicRoleReplacements[i, 2] = null;
						}
						else
						{
							basicRoleReplacements[i, 1] = basicSubscriptedReplacement;
							basicRoleReplacements[i, 2] = string.Empty;
						}
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
				if (allFactsCount == 1 && factArity == constraintRoleArity && factArity == 2)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, false);
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
						CoreVerbalizationSnippetType listSnippet;
						if (RoleIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.TopLevelIndentedLogicalAndListOpen;
						}
						else if (RoleIter1 == constraintRoleArity - 1)
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
						reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.None);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						if (reading != null)
						{
							for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1, 0], snippet1ReplaceFactRoleIter1);
								if (primaryRole == currentRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, false), basicReplacement);
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
						}
						else
						{
							reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							if (reading != null)
							{
								string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachNegatableCompactQuantifier, isDeontic, true);
								string snippet1Replace1Replace1 = null;
								RoleBase contextPrimaryRole = primaryRole;
								int contextTempStringBuildLength = sbTemp.Length;
								bool snippet1Replace1ReplaceIsFirstPass1 = true;
								for (int ContextRoleIter1 = 0; ContextRoleIter1 < constraintRoleArity && snippet1Replace1ReplaceIsFirstPass1; ++ContextRoleIter1)
								{
									primaryRole = allConstraintRoles[ContextRoleIter1];
									if (contextPrimaryRole == primaryRole)
									{
										sbTemp.Append(subscripter.GetSubscriptedName(unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[ContextRoleIter1]), allBasicRoleReplacements[0]));
										snippet1Replace1ReplaceIsFirstPass1 = false;
									}
								}
								snippet1Replace1Replace1 = sbTemp.ToString(contextTempStringBuildLength, sbTemp.Length - contextTempStringBuildLength);
								primaryRole = contextPrimaryRole;
								sbTemp.Length = contextTempStringBuildLength;
								string snippet1Replace1Replace2 = null;
								for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
								{
									RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
									string roleReplacement = null;
									if (primaryRole == currentRole)
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2));
									}
									else
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.MoreThanOneQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2));
									}
									if (roleReplacement == null)
									{
										roleReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2);
									}
									roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
								}
								snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
								snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
							}
						}
						sbTemp.Append(snippet1Replace1);
						if (RoleIter1 == constraintRoleArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.TopLevelIndentedLogicalAndListClose, isDeontic, isNegative));
						}
						subscripter.ResetSubscripts(allBasicRoleReplacements);
					}
					snippet1Replace1 = sbTemp.ToString();
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, false));
				}
				else if (allFactsCount == 1 && minFactArity >= 3 && factArity == constraintRoleArity)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, false);
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
						RoleBase primaryRole = factRoles[factArity - RoleIter1 - 1];
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
						string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachNegatableCompactQuantifier, isDeontic, true);
						string snippet1Replace1Replace1 = null;
						RoleBase contextPrimaryRole = primaryRole;
						int contextTempStringBuildLength = sbTemp.Length;
						int snippet1Replace1ReplaceFilteredIter1;
						int snippet1Replace1ReplaceFilteredCount1 = 0;
						for (snippet1Replace1ReplaceFilteredIter1 = 0; snippet1Replace1ReplaceFilteredIter1 < factArity; ++snippet1Replace1ReplaceFilteredIter1)
						{
							primaryRole = factRoles[snippet1Replace1ReplaceFilteredIter1];
							if (contextPrimaryRole != primaryRole)
							{
								++snippet1Replace1ReplaceFilteredCount1;
							}
						}
						snippet1Replace1ReplaceFilteredIter1 = 0;
						for (int ContextRoleIter1 = 0; ContextRoleIter1 < factArity; ++ContextRoleIter1)
						{
							primaryRole = factRoles[ContextRoleIter1];
							if (contextPrimaryRole != primaryRole)
							{
								if (snippet1Replace1ReplaceFilteredIter1 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
								}
								else if (snippet1Replace1ReplaceFilteredIter1 == snippet1Replace1ReplaceFilteredCount1 - 1)
								{
									if (snippet1Replace1ReplaceFilteredIter1 == 1)
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
								sbTemp.Append(subscripter.GetSubscriptedName(ContextRoleIter1, allBasicRoleReplacements[0]));
								if (snippet1Replace1ReplaceFilteredIter1 == snippet1Replace1ReplaceFilteredCount1 - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
								}
								++snippet1Replace1ReplaceFilteredIter1;
							}
						}
						snippet1Replace1Replace1 = sbTemp.ToString(contextTempStringBuildLength, sbTemp.Length - contextTempStringBuildLength);
						primaryRole = contextPrimaryRole;
						sbTemp.Length = contextTempStringBuildLength;
						string snippet1Replace1Replace2 = null;
						reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
						{
							RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
							string roleReplacement = null;
							string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2);
							if (primaryRole != currentRole)
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.MoreThanOneQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
						}
						snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
						snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
						sbTemp.Append(snippet1Replace1);
						if (RoleIter1 == factArity - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.TopLevelIndentedLogicalAndListClose, isDeontic, isNegative));
						}
						subscripter.ResetSubscripts(allBasicRoleReplacements);
					}
					snippet1Replace1 = sbTemp.ToString();
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, false));
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // UniquenessConstraint.UniquenessPossibilityVerbalizer verbalization
	#region UniquenessConstraint.UniquenessPreferredVerbalizer verbalization
	public partial class UniquenessConstraint
	{
		private partial class UniquenessPreferredVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static UniquenessPreferredVerbalizer myCache;
			public static UniquenessPreferredVerbalizer GetVerbalizer()
			{
				UniquenessPreferredVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<UniquenessPreferredVerbalizer>(ref myCache, null as UniquenessPreferredVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new UniquenessPreferredVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<UniquenessPreferredVerbalizer>(ref myCache, this, null as UniquenessPreferredVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
				StringBuilder sbTemp = null;
				FactType parentFact;
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
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
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
				#endregion // Preliminary
				#region Pattern Matches
				if (this.IsPreferred)
				{
					ObjectType preferredFor = this.PreferredIdentifierFor;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConstraintProvidesPreferredIdentifier, isDeontic, isNegative);
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
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						contextBasicReplacementIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[contextBasicReplacementIndex];
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
					snippet1Replace1 = sbTemp.ToString();
					string snippet1Replace2 = null;
					string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative);
					string snippet1Replace2Replace1 = null;
					snippet1Replace2Replace1 = VerbalizationHelper.NormalizeObjectTypeName(preferredFor.Name, verbalizationContext.VerbalizationOptions);
					string snippet1Replace2Replace2 = null;
					snippet1Replace2Replace2 = preferredFor.Id.ToString("D");
					snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1, snippet1Replace2Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // UniquenessConstraint.UniquenessPreferredVerbalizer verbalization
	#region FrequencyConstraint verbalization
	public partial class FrequencyConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			bool blockingErrors = false;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					blockingErrors = true;
					if (verbalizationContext.TestVerbalizedLocally(error))
					{
						continue;
					}
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
				}
				if (blockingErrors)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			bool isNegative = 0 != (sign & VerbalizationSign.Negative);
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
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
							if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
							writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"), subscript + 1);
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
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
			ConstraintRoleSequenceJoinPath joinPath = this.JoinPath;
			LeadRolePath singleLeadRolePath = null;
			bool isTrivialOppositeRolePath = false;
			if (joinPath != null && joinPath.IsAutomatic)
			{
				ObjectType rootObjectType;
				if (null != (singleLeadRolePath = joinPath.SingleLeadRolePath) && null != (rootObjectType = singleLeadRolePath.RootObjectType))
				{
					int i = 0;
					for (; i < constraintRoleArity; ++i)
					{
						RoleBase oppositeRole = allConstraintRoles[i].OppositeRole;
						if (oppositeRole == null || oppositeRole.Role.RolePlayer != rootObjectType)
						{
							break;
						}
					}
					isTrivialOppositeRolePath = i == constraintRoleArity;
				}
			}
			IReading[] allConstraintRoleReadings = new IReading[constraintRoleArity];
			string[] roleReplacements = new string[maxFactArity];
			IReading reading;
			VerbalizationHyphenBinder hyphenBinder;
			#endregion // Preliminary
			#region Pattern Matches
			if (allFactsCount == 1 && factArity == 2 && constraintRoleArity == 1 && !isNegative && this.MinFrequency == 1 && this.MaxFrequency != 0)
			{
				parentFact = allFacts[0];
				predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
				allReadingOrders = parentFact.ReadingOrderCollection;
				reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, factRoles, MatchingReadingOptions.None);
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
							if (this.MaxFrequency == 1 && this.MinFrequency == 1)
							{
								string snippet1Replace1PredicateFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.AtMostOneTypedOccurrence, isDeontic, isNegative);
								snippet1Replace1Predicate1 = string.Format(writer.FormatProvider, snippet1Replace1PredicateFormat1, basicReplacement);
							}
							else
							{
								string snippet1Replace1PredicateFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.FrequencyTypedOccurrences, isDeontic, isNegative);
								string snippet1Replace1Predicate1Replace1 = null;
								string snippet1Replace1Predicate1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.FrequencyRangeMinUnbounded, isDeontic, isNegative);
								string snippet1Replace1Predicate1Replace1Replace1 = null;
								snippet1Replace1Predicate1Replace1Replace1 = this.MinFrequency.ToString(CultureInfo.CurrentCulture);
								string snippet1Replace1Predicate1Replace1Replace2 = null;
								snippet1Replace1Predicate1Replace1Replace2 = this.MaxFrequency.ToString(CultureInfo.CurrentCulture);
								snippet1Replace1Predicate1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Predicate1ReplaceFormat1, snippet1Replace1Predicate1Replace1Replace1, snippet1Replace1Predicate1Replace1Replace2);
								snippet1Replace1Predicate1 = string.Format(writer.FormatProvider, snippet1Replace1PredicateFormat1, basicReplacement, snippet1Replace1Predicate1Replace1);
							}
							roleReplacement = snippet1Replace1Predicate1;
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
					reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
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
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, allBasicRoleReplacements[0], true);
						string snippet1Replace1Replace3 = null;
						if (this.MaxFrequency == 1 && this.MinFrequency == 1)
						{
							string snippet1Replace1ReplaceFormat3 = snippets.GetSnippet(CoreVerbalizationSnippetType.ExactlyOneUntypedOccurrence, isDeontic, isNegative);
							snippet1Replace1Replace3 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat3);
						}
						else
						{
							string snippet1Replace1ReplaceFormat3 = snippets.GetSnippet(CoreVerbalizationSnippetType.FrequencyUntypedOccurrences, isDeontic, isNegative);
							string snippet1Replace1Replace3Replace1 = null;
							CoreVerbalizationSnippetType snippet1Replace1Replace3ReplaceSnippetType1 = 0;
							if (this.MinFrequency == this.MaxFrequency)
							{
								snippet1Replace1Replace3ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeExact;
							}
							else if (this.MaxFrequency == 0)
							{
								snippet1Replace1Replace3ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeMaxUnbounded;
							}
							else if (this.MinFrequency == 1)
							{
								snippet1Replace1Replace3ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeMinUnbounded;
							}
							else
							{
								snippet1Replace1Replace3ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeMinAndMax;
							}
							string snippet1Replace1Replace3ReplaceFormat1 = snippets.GetSnippet(snippet1Replace1Replace3ReplaceSnippetType1, isDeontic, isNegative);
							string snippet1Replace1Replace3Replace1Replace1 = null;
							snippet1Replace1Replace3Replace1Replace1 = this.MinFrequency.ToString(CultureInfo.CurrentCulture);
							string snippet1Replace1Replace3Replace1Replace2 = null;
							snippet1Replace1Replace3Replace1Replace2 = this.MaxFrequency.ToString(CultureInfo.CurrentCulture);
							snippet1Replace1Replace3Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace3ReplaceFormat1, snippet1Replace1Replace3Replace1Replace1, snippet1Replace1Replace3Replace1Replace2);
							snippet1Replace1Replace3 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat3, snippet1Replace1Replace3Replace1);
						}
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
				reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
				hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
				snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, allBasicRoleReplacements[0], true);
				string snippet1Replace1Replace3 = null;
				if (this.MaxFrequency == 1 && this.MinFrequency == 1)
				{
					string snippet1Replace1ReplaceFormat3 = snippets.GetSnippet(CoreVerbalizationSnippetType.ExactlyOneUntypedOccurrence, isDeontic, isNegative);
					snippet1Replace1Replace3 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat3);
				}
				else
				{
					string snippet1Replace1ReplaceFormat3 = snippets.GetSnippet(CoreVerbalizationSnippetType.FrequencyUntypedOccurrences, isDeontic, isNegative);
					string snippet1Replace1Replace3Replace1 = null;
					CoreVerbalizationSnippetType snippet1Replace1Replace3ReplaceSnippetType1 = 0;
					if (this.MinFrequency == this.MaxFrequency)
					{
						snippet1Replace1Replace3ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeExact;
					}
					else if (this.MaxFrequency == 0)
					{
						snippet1Replace1Replace3ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeMaxUnbounded;
					}
					else if (this.MinFrequency == 1)
					{
						snippet1Replace1Replace3ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeMinUnbounded;
					}
					else
					{
						snippet1Replace1Replace3ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeMinAndMax;
					}
					string snippet1Replace1Replace3ReplaceFormat1 = snippets.GetSnippet(snippet1Replace1Replace3ReplaceSnippetType1, isDeontic, isNegative);
					string snippet1Replace1Replace3Replace1Replace1 = null;
					snippet1Replace1Replace3Replace1Replace1 = this.MinFrequency.ToString(CultureInfo.CurrentCulture);
					string snippet1Replace1Replace3Replace1Replace2 = null;
					snippet1Replace1Replace3Replace1Replace2 = this.MaxFrequency.ToString(CultureInfo.CurrentCulture);
					snippet1Replace1Replace3Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace3ReplaceFormat1, snippet1Replace1Replace3Replace1Replace1, snippet1Replace1Replace3Replace1Replace2);
					snippet1Replace1Replace3 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat3, snippet1Replace1Replace3Replace1);
				}
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2, snippet1Replace1Replace3);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (!isNegative && minFactArity >= 1 && maxFactArity <= 1)
			{
				RolePathVerbalizer pathVerbalizer = RolePathVerbalizer.Create(this, new StandardRolePathRenderer(snippets, verbalizationContext, writer.FormatProvider));
				IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(this);
				IList<object> preProjectionKeys = pathVerbalizer.GetPreProjectionPrimaryNodeKeys(includedConstraintRoles);
				int preProjectionKeyCount = preProjectionKeys != null ? preProjectionKeys.Count : 0;
				if (0 != preProjectionKeyCount)
				{
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					CoreVerbalizationSnippetType snippet1ReplaceSnippetType1 = 0;
					if (this.MaxFrequency == 1 && this.MinFrequency == 1)
					{
						snippet1ReplaceSnippetType1 = CoreVerbalizationSnippetType.SingularExistenceImplicationOperator;
					}
					else
					{
						snippet1ReplaceSnippetType1 = CoreVerbalizationSnippetType.PluralExistenceImplicationOperator;
					}
					string snippet1ReplaceFormat1 = snippets.GetSnippet(snippet1ReplaceSnippetType1, isDeontic, isNegative);
					string snippet1Replace1Replace1 = null;
					if (this.MaxFrequency == 1 && this.MinFrequency == 1)
					{
						string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.AtMostOneTypedOccurrence, isDeontic, isNegative);
						string snippet1Replace1Replace1Replace1 = null;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						for (int RoleIter1 = 0; RoleIter1 < preProjectionKeyCount; ++RoleIter1)
						{
							CoreVerbalizationSnippetType listSnippet;
							if (RoleIter1 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
							}
							else if (RoleIter1 == preProjectionKeyCount - 1)
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
							sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(preProjectionKeys[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead));
							if (RoleIter1 == preProjectionKeyCount - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
							}
						}
						snippet1Replace1Replace1Replace1 = sbTemp.ToString();
						snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1);
					}
					else
					{
						CoreVerbalizationSnippetType snippet1Replace1ReplaceSnippetType1 = 0;
						if (1 == preProjectionKeyCount)
						{
							snippet1Replace1ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyTypedOccurrences;
						}
						else
						{
							snippet1Replace1ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyTypedCombinationOccurrences;
						}
						string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(snippet1Replace1ReplaceSnippetType1, isDeontic, isNegative);
						string snippet1Replace1Replace1Replace1 = null;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						for (int RoleIter1 = 0; RoleIter1 < preProjectionKeyCount; ++RoleIter1)
						{
							CoreVerbalizationSnippetType listSnippet;
							if (RoleIter1 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
							}
							else if (RoleIter1 == preProjectionKeyCount - 1)
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
							sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(preProjectionKeys[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead));
							if (RoleIter1 == preProjectionKeyCount - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
							}
						}
						snippet1Replace1Replace1Replace1 = sbTemp.ToString();
						string snippet1Replace1Replace1Replace2 = null;
						CoreVerbalizationSnippetType snippet1Replace1Replace1ReplaceSnippetType2 = 0;
						if (this.MinFrequency == 1)
						{
							snippet1Replace1Replace1ReplaceSnippetType2 = CoreVerbalizationSnippetType.SelfReference;
						}
						else
						{
							snippet1Replace1Replace1ReplaceSnippetType2 = CoreVerbalizationSnippetType.FrequencyNotPopulatedOrRange;
						}
						string snippet1Replace1Replace1ReplaceFormat2 = snippets.GetSnippet(snippet1Replace1Replace1ReplaceSnippetType2, isDeontic, isNegative);
						string snippet1Replace1Replace1Replace2Replace1 = null;
						CoreVerbalizationSnippetType snippet1Replace1Replace1Replace2ReplaceSnippetType1 = 0;
						if (this.MinFrequency == this.MaxFrequency)
						{
							snippet1Replace1Replace1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeExact;
						}
						else if (this.MaxFrequency == 0)
						{
							snippet1Replace1Replace1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeMaxUnbounded;
						}
						else if (this.MinFrequency == 1)
						{
							snippet1Replace1Replace1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeMinUnbounded;
						}
						else
						{
							snippet1Replace1Replace1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeMinAndMax;
						}
						string snippet1Replace1Replace1Replace2ReplaceFormat1 = snippets.GetSnippet(snippet1Replace1Replace1Replace2ReplaceSnippetType1, isDeontic, isNegative);
						string snippet1Replace1Replace1Replace2Replace1Replace1 = null;
						snippet1Replace1Replace1Replace2Replace1Replace1 = this.MinFrequency.ToString(CultureInfo.CurrentCulture);
						string snippet1Replace1Replace1Replace2Replace1Replace2 = null;
						snippet1Replace1Replace1Replace2Replace1Replace2 = this.MaxFrequency.ToString(CultureInfo.CurrentCulture);
						snippet1Replace1Replace1Replace2Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace1Replace2ReplaceFormat1, snippet1Replace1Replace1Replace2Replace1Replace1, snippet1Replace1Replace1Replace2Replace1Replace2);
						snippet1Replace1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1Replace1ReplaceFormat2, snippet1Replace1Replace1Replace2Replace1);
						snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1, snippet1Replace1Replace1Replace2);
					}
					string snippet1Replace1Replace2 = null;
					if (pathVerbalizer.HasPathVerbalization(joinPath))
					{
						pathVerbalizer.Options = RolePathVerbalizerOptions.None;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						snippet1Replace1Replace2 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
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
						FactType[] snippet1Replace1ReplaceUniqueFactTypes2 = new FactType[constraintRoleArity];
						FactType snippet1Replace1ReplaceTestUniqueFactType2;
						int snippet1Replace1ReplaceFilteredIter2;
						int snippet1Replace1ReplaceFilteredCount2 = 0;
						for (snippet1Replace1ReplaceFilteredIter2 = 0; snippet1Replace1ReplaceFilteredIter2 < constraintRoleArity; ++snippet1Replace1ReplaceFilteredIter2)
						{
							RoleBase primaryRole = allConstraintRoles[snippet1Replace1ReplaceFilteredIter2];
							if (Array.IndexOf(snippet1Replace1ReplaceUniqueFactTypes2, snippet1Replace1ReplaceTestUniqueFactType2 = primaryRole.FactType) == -1)
							{
								snippet1Replace1ReplaceUniqueFactTypes2[snippet1Replace1ReplaceFilteredIter2] = snippet1Replace1ReplaceTestUniqueFactType2;
								++snippet1Replace1ReplaceFilteredCount2;
							}
						}
						Array.Clear(snippet1Replace1ReplaceUniqueFactTypes2, 0, snippet1Replace1ReplaceUniqueFactTypes2.Length);
						snippet1Replace1ReplaceFilteredIter2 = 0;
						for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity; ++RoleIter2)
						{
							RoleBase primaryRole = allConstraintRoles[RoleIter2];
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
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
								reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
									{
										foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
										{
											if (replaceRole == constraintRole.Role)
											{
												return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
											}
										}
										return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
									});
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
			else if (!isNegative)
			{
				RolePathVerbalizer pathVerbalizer = RolePathVerbalizer.Create(this, new StandardRolePathRenderer(snippets, verbalizationContext, writer.FormatProvider));
				IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(this);
				IList<object> preProjectionKeys = pathVerbalizer.GetPreProjectionPrimaryNodeKeys(includedConstraintRoles);
				int preProjectionKeyCount = preProjectionKeys != null ? preProjectionKeys.Count : 0;
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ForEachIndentedQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				CoreVerbalizationSnippetType snippet1Replace1ReplaceSnippetType1 = 0;
				if (preProjectionKeyCount == 0)
				{
					snippet1Replace1ReplaceSnippetType1 = CoreVerbalizationSnippetType.CombinationIdentifier;
				}
				else
				{
					snippet1Replace1ReplaceSnippetType1 = CoreVerbalizationSnippetType.SelfReference;
				}
				string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(snippet1Replace1ReplaceSnippetType1, isDeontic, isNegative);
				string snippet1Replace1Replace1Replace1 = null;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				int snippet1Replace1Replace1ReplaceFilteredIter1;
				int snippet1Replace1Replace1ReplaceFilteredCount1 = 0;
				for (snippet1Replace1Replace1ReplaceFilteredIter1 = 0; snippet1Replace1Replace1ReplaceFilteredIter1 < constraintRoleArity; ++snippet1Replace1Replace1ReplaceFilteredIter1)
				{
					ObjectType snippet1Replace1Replace1ReplaceRolePlayer1 = includedConstraintRoles[snippet1Replace1Replace1ReplaceFilteredIter1].Role.RolePlayer;
					if (snippet1Replace1Replace1ReplaceRolePlayer1 == null || !snippet1Replace1Replace1ReplaceRolePlayer1.IsImplicitBooleanValue)
					{
						++snippet1Replace1Replace1ReplaceFilteredCount1;
					}
				}
				snippet1Replace1Replace1ReplaceFilteredIter1 = 0;
				for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity; ++RoleIter1)
				{
					ObjectType snippet1Replace1Replace1ReplaceRolePlayer1 = includedConstraintRoles[RoleIter1].Role.RolePlayer;
					if (snippet1Replace1Replace1ReplaceRolePlayer1 == null || !snippet1Replace1Replace1ReplaceRolePlayer1.IsImplicitBooleanValue)
					{
						CoreVerbalizationSnippetType listSnippet;
						if (snippet1Replace1Replace1ReplaceFilteredIter1 == 0)
						{
							listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
						}
						else if (snippet1Replace1Replace1ReplaceFilteredIter1 == snippet1Replace1Replace1ReplaceFilteredCount1 - 1)
						{
							if (snippet1Replace1Replace1ReplaceFilteredIter1 == 1)
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
						sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead));
						if (snippet1Replace1Replace1ReplaceFilteredIter1 == snippet1Replace1Replace1ReplaceFilteredCount1 - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
						}
						++snippet1Replace1Replace1ReplaceFilteredIter1;
					}
				}
				snippet1Replace1Replace1Replace1 = sbTemp.ToString();
				snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1);
				string snippet1Replace1Replace2 = null;
				if (0 != preProjectionKeyCount)
				{
					CoreVerbalizationSnippetType snippet1Replace1ReplaceSnippetType2 = 0;
					if (this.MaxFrequency == 1 && this.MinFrequency == 1)
					{
						snippet1Replace1ReplaceSnippetType2 = CoreVerbalizationSnippetType.SingularExistenceImplicationOperator;
					}
					else
					{
						snippet1Replace1ReplaceSnippetType2 = CoreVerbalizationSnippetType.PluralExistenceImplicationOperator;
					}
					string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(snippet1Replace1ReplaceSnippetType2, isDeontic, isNegative);
					string snippet1Replace1Replace2Replace1 = null;
					if (this.MaxFrequency == 1 && this.MinFrequency == 1)
					{
						string snippet1Replace1Replace2ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.AtMostOneTypedOccurrence, isDeontic, isNegative);
						string snippet1Replace1Replace2Replace1Replace1 = null;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						for (int RoleIter1 = 0; RoleIter1 < preProjectionKeyCount; ++RoleIter1)
						{
							CoreVerbalizationSnippetType listSnippet;
							if (RoleIter1 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
							}
							else if (RoleIter1 == preProjectionKeyCount - 1)
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
							sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(preProjectionKeys[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead));
							if (RoleIter1 == preProjectionKeyCount - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
							}
						}
						snippet1Replace1Replace2Replace1Replace1 = sbTemp.ToString();
						snippet1Replace1Replace2Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace2ReplaceFormat1, snippet1Replace1Replace2Replace1Replace1);
					}
					else
					{
						CoreVerbalizationSnippetType snippet1Replace1Replace2ReplaceSnippetType1 = 0;
						if (1 == preProjectionKeyCount)
						{
							snippet1Replace1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyTypedOccurrences;
						}
						else
						{
							snippet1Replace1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyTypedCombinationOccurrences;
						}
						string snippet1Replace1Replace2ReplaceFormat1 = snippets.GetSnippet(snippet1Replace1Replace2ReplaceSnippetType1, isDeontic, isNegative);
						string snippet1Replace1Replace2Replace1Replace1 = null;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						for (int RoleIter1 = 0; RoleIter1 < preProjectionKeyCount; ++RoleIter1)
						{
							CoreVerbalizationSnippetType listSnippet;
							if (RoleIter1 == 0)
							{
								listSnippet = CoreVerbalizationSnippetType.SimpleListOpen;
							}
							else if (RoleIter1 == preProjectionKeyCount - 1)
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
							sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(preProjectionKeys[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead));
							if (RoleIter1 == preProjectionKeyCount - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.SimpleListClose, isDeontic, isNegative));
							}
						}
						snippet1Replace1Replace2Replace1Replace1 = sbTemp.ToString();
						string snippet1Replace1Replace2Replace1Replace2 = null;
						CoreVerbalizationSnippetType snippet1Replace1Replace2Replace1ReplaceSnippetType2 = 0;
						if (this.MinFrequency == 1)
						{
							snippet1Replace1Replace2Replace1ReplaceSnippetType2 = CoreVerbalizationSnippetType.SelfReference;
						}
						else
						{
							snippet1Replace1Replace2Replace1ReplaceSnippetType2 = CoreVerbalizationSnippetType.FrequencyNotPopulatedOrRange;
						}
						string snippet1Replace1Replace2Replace1ReplaceFormat2 = snippets.GetSnippet(snippet1Replace1Replace2Replace1ReplaceSnippetType2, isDeontic, isNegative);
						string snippet1Replace1Replace2Replace1Replace2Replace1 = null;
						CoreVerbalizationSnippetType snippet1Replace1Replace2Replace1Replace2ReplaceSnippetType1 = 0;
						if (this.MinFrequency == this.MaxFrequency)
						{
							snippet1Replace1Replace2Replace1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeExact;
						}
						else if (this.MaxFrequency == 0)
						{
							snippet1Replace1Replace2Replace1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeMaxUnbounded;
						}
						else if (this.MinFrequency == 1)
						{
							snippet1Replace1Replace2Replace1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeMinUnbounded;
						}
						else
						{
							snippet1Replace1Replace2Replace1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeMinAndMax;
						}
						string snippet1Replace1Replace2Replace1Replace2ReplaceFormat1 = snippets.GetSnippet(snippet1Replace1Replace2Replace1Replace2ReplaceSnippetType1, isDeontic, isNegative);
						string snippet1Replace1Replace2Replace1Replace2Replace1Replace1 = null;
						snippet1Replace1Replace2Replace1Replace2Replace1Replace1 = this.MinFrequency.ToString(CultureInfo.CurrentCulture);
						string snippet1Replace1Replace2Replace1Replace2Replace1Replace2 = null;
						snippet1Replace1Replace2Replace1Replace2Replace1Replace2 = this.MaxFrequency.ToString(CultureInfo.CurrentCulture);
						snippet1Replace1Replace2Replace1Replace2Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace2Replace1Replace2ReplaceFormat1, snippet1Replace1Replace2Replace1Replace2Replace1Replace1, snippet1Replace1Replace2Replace1Replace2Replace1Replace2);
						snippet1Replace1Replace2Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1Replace2Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1Replace2Replace1);
						snippet1Replace1Replace2Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace2ReplaceFormat1, snippet1Replace1Replace2Replace1Replace1, snippet1Replace1Replace2Replace1Replace2);
					}
					string snippet1Replace1Replace2Replace2 = null;
					if (pathVerbalizer.HasPathVerbalization(joinPath))
					{
						pathVerbalizer.Options = RolePathVerbalizerOptions.None;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						snippet1Replace1Replace2Replace2 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
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
						FactType[] snippet1Replace1Replace2ReplaceUniqueFactTypes2 = new FactType[constraintRoleArity];
						FactType snippet1Replace1Replace2ReplaceTestUniqueFactType2;
						int snippet1Replace1Replace2ReplaceFilteredIter2;
						int snippet1Replace1Replace2ReplaceFilteredCount2 = 0;
						for (snippet1Replace1Replace2ReplaceFilteredIter2 = 0; snippet1Replace1Replace2ReplaceFilteredIter2 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceFilteredIter2)
						{
							RoleBase primaryRole = allConstraintRoles[snippet1Replace1Replace2ReplaceFilteredIter2];
							if (Array.IndexOf(snippet1Replace1Replace2ReplaceUniqueFactTypes2, snippet1Replace1Replace2ReplaceTestUniqueFactType2 = primaryRole.FactType) == -1)
							{
								snippet1Replace1Replace2ReplaceUniqueFactTypes2[snippet1Replace1Replace2ReplaceFilteredIter2] = snippet1Replace1Replace2ReplaceTestUniqueFactType2;
								++snippet1Replace1Replace2ReplaceFilteredCount2;
							}
						}
						Array.Clear(snippet1Replace1Replace2ReplaceUniqueFactTypes2, 0, snippet1Replace1Replace2ReplaceUniqueFactTypes2.Length);
						snippet1Replace1Replace2ReplaceFilteredIter2 = 0;
						for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity; ++RoleIter2)
						{
							RoleBase primaryRole = allConstraintRoles[RoleIter2];
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
							if (Array.IndexOf(snippet1Replace1Replace2ReplaceUniqueFactTypes2, snippet1Replace1Replace2ReplaceTestUniqueFactType2 = primaryRole.FactType) == -1)
							{
								snippet1Replace1Replace2ReplaceUniqueFactTypes2[RoleIter2] = snippet1Replace1Replace2ReplaceTestUniqueFactType2;
								CoreVerbalizationSnippetType listSnippet;
								if (snippet1Replace1Replace2ReplaceFilteredIter2 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.CompoundListOpen;
								}
								else if (snippet1Replace1Replace2ReplaceFilteredIter2 == snippet1Replace1Replace2ReplaceFilteredCount2 - 1)
								{
									if (snippet1Replace1Replace2ReplaceFilteredIter2 == 1)
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
								reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								snippet1Replace1Replace2Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
									{
										foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
										{
											if (replaceRole == constraintRole.Role)
											{
												return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
											}
										}
										return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
									});
								sbTemp.Append(snippet1Replace1Replace2Replace2);
								if (snippet1Replace1Replace2ReplaceFilteredIter2 == snippet1Replace1Replace2ReplaceFilteredCount2 - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
								}
								++snippet1Replace1Replace2ReplaceFilteredIter2;
							}
						}
						snippet1Replace1Replace2Replace2 = sbTemp.ToString();
					}
					snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1, snippet1Replace1Replace2Replace2);
				}
				else
				{
					string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLine, isDeontic, isNegative);
					string snippet1Replace1Replace2Replace1 = null;
					if (pathVerbalizer.HasPathVerbalization(joinPath))
					{
						pathVerbalizer.Options = RolePathVerbalizerOptions.None;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						snippet1Replace1Replace2Replace1 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
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
						FactType[] snippet1Replace1Replace2ReplaceUniqueFactTypes1 = new FactType[constraintRoleArity];
						FactType snippet1Replace1Replace2ReplaceTestUniqueFactType1;
						int snippet1Replace1Replace2ReplaceFilteredIter1;
						int snippet1Replace1Replace2ReplaceFilteredCount1 = 0;
						for (snippet1Replace1Replace2ReplaceFilteredIter1 = 0; snippet1Replace1Replace2ReplaceFilteredIter1 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceFilteredIter1)
						{
							RoleBase primaryRole = allConstraintRoles[snippet1Replace1Replace2ReplaceFilteredIter1];
							if (Array.IndexOf(snippet1Replace1Replace2ReplaceUniqueFactTypes1, snippet1Replace1Replace2ReplaceTestUniqueFactType1 = primaryRole.FactType) == -1)
							{
								snippet1Replace1Replace2ReplaceUniqueFactTypes1[snippet1Replace1Replace2ReplaceFilteredIter1] = snippet1Replace1Replace2ReplaceTestUniqueFactType1;
								++snippet1Replace1Replace2ReplaceFilteredCount1;
							}
						}
						Array.Clear(snippet1Replace1Replace2ReplaceUniqueFactTypes1, 0, snippet1Replace1Replace2ReplaceUniqueFactTypes1.Length);
						snippet1Replace1Replace2ReplaceFilteredIter1 = 0;
						for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity; ++RoleIter1)
						{
							RoleBase primaryRole = allConstraintRoles[RoleIter1];
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
							if (Array.IndexOf(snippet1Replace1Replace2ReplaceUniqueFactTypes1, snippet1Replace1Replace2ReplaceTestUniqueFactType1 = primaryRole.FactType) == -1)
							{
								snippet1Replace1Replace2ReplaceUniqueFactTypes1[RoleIter1] = snippet1Replace1Replace2ReplaceTestUniqueFactType1;
								CoreVerbalizationSnippetType listSnippet;
								if (snippet1Replace1Replace2ReplaceFilteredIter1 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.CompoundListOpen;
								}
								else if (snippet1Replace1Replace2ReplaceFilteredIter1 == snippet1Replace1Replace2ReplaceFilteredCount1 - 1)
								{
									if (snippet1Replace1Replace2ReplaceFilteredIter1 == 1)
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
								reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								snippet1Replace1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
									{
										foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
										{
											if (replaceRole == constraintRole.Role)
											{
												return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
											}
										}
										return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.None);
									});
								sbTemp.Append(snippet1Replace1Replace2Replace1);
								if (snippet1Replace1Replace2ReplaceFilteredIter1 == snippet1Replace1Replace2ReplaceFilteredCount1 - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompoundListClose, isDeontic, isNegative));
								}
								++snippet1Replace1Replace2ReplaceFilteredIter1;
							}
						}
						snippet1Replace1Replace2Replace1 = sbTemp.ToString();
					}
					string snippet1Replace1Replace2Replace2 = null;
					string snippet1Replace1Replace2ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ContextCombinationOccurrence, isDeontic, isNegative);
					string snippet1Replace1Replace2Replace2Replace1 = null;
					if (this.MaxFrequency == 1 && this.MinFrequency == 1)
					{
						string snippet1Replace1Replace2Replace2ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ExactlyOneUntypedOccurrence, isDeontic, isNegative);
						snippet1Replace1Replace2Replace2Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace2Replace2ReplaceFormat1);
					}
					else
					{
						string snippet1Replace1Replace2Replace2ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.FrequencyUntypedOccurrences, isDeontic, isNegative);
						string snippet1Replace1Replace2Replace2Replace1Replace1 = null;
						CoreVerbalizationSnippetType snippet1Replace1Replace2Replace2Replace1ReplaceSnippetType1 = 0;
						if (this.MinFrequency == this.MaxFrequency)
						{
							snippet1Replace1Replace2Replace2Replace1ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeExact;
						}
						else if (this.MaxFrequency == 0)
						{
							snippet1Replace1Replace2Replace2Replace1ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeMaxUnbounded;
						}
						else if (this.MinFrequency == 1)
						{
							snippet1Replace1Replace2Replace2Replace1ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeMinUnbounded;
						}
						else
						{
							snippet1Replace1Replace2Replace2Replace1ReplaceSnippetType1 = CoreVerbalizationSnippetType.FrequencyRangeMinAndMax;
						}
						string snippet1Replace1Replace2Replace2Replace1ReplaceFormat1 = snippets.GetSnippet(snippet1Replace1Replace2Replace2Replace1ReplaceSnippetType1, isDeontic, isNegative);
						string snippet1Replace1Replace2Replace2Replace1Replace1Replace1 = null;
						snippet1Replace1Replace2Replace2Replace1Replace1Replace1 = this.MinFrequency.ToString(CultureInfo.CurrentCulture);
						string snippet1Replace1Replace2Replace2Replace1Replace1Replace2 = null;
						snippet1Replace1Replace2Replace2Replace1Replace1Replace2 = this.MaxFrequency.ToString(CultureInfo.CurrentCulture);
						snippet1Replace1Replace2Replace2Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace2Replace2Replace1ReplaceFormat1, snippet1Replace1Replace2Replace2Replace1Replace1Replace1, snippet1Replace1Replace2Replace2Replace1Replace1Replace2);
						snippet1Replace1Replace2Replace2Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace2Replace2ReplaceFormat1, snippet1Replace1Replace2Replace2Replace1Replace1);
					}
					snippet1Replace1Replace2Replace2 = string.Format(writer.FormatProvider, snippet1Replace1Replace2ReplaceFormat2, snippet1Replace1Replace2Replace2Replace1);
					snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1, snippet1Replace1Replace2Replace2);
				}
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
			}
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
		}
	}
	#endregion // FrequencyConstraint verbalization
	#region RoleValueConstraint verbalization
	public partial class RoleValueConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			bool blockingErrors = false;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					blockingErrors = true;
					if (verbalizationContext.TestVerbalizedLocally(error))
					{
						continue;
					}
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
				}
				if (blockingErrors)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			bool isNegative = 0 != (sign & VerbalizationSign.Negative);
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
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
					basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
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
							sbTemp.Append(basicRoleReplacements[FactType.IndexOfRole(factRoles, includedRoles[ContextRoleIter1])]);
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
				reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
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
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
		}
	}
	#endregion // RoleValueConstraint verbalization
	#region ValueTypeValueConstraint verbalization
	public partial class ValueTypeValueConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			bool blockingErrors = false;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					blockingErrors = true;
					if (verbalizationContext.TestVerbalizedLocally(error))
					{
						continue;
					}
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
				}
				if (blockingErrors)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			bool isNegative = 0 != (sign & VerbalizationSign.Negative);
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
			variableSnippet1Replace1Replace1 = VerbalizationHelper.NormalizeObjectTypeName(this.ValueType, verbalizationContext.VerbalizationOptions);
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
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
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
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
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
				variableSnippet1Replace1Replace1 = VerbalizationHelper.NormalizeObjectTypeName(this.ParentObjectType, verbalizationContext.VerbalizationOptions);
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
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // ObjectType.NearestValueConstraintVerbalizer verbalization
	#region RingConstraint verbalization
	public partial class RingConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			bool blockingErrors = false;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					blockingErrors = true;
					if (verbalizationContext.TestVerbalizedLocally(error))
					{
						continue;
					}
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
				}
				if (blockingErrors)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
			this.VerbalizeByRingType(writer, snippetsDictionary, verbalizationContext, sign);
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
		}
	}
	#endregion // RingConstraint verbalization
	#region RingConstraint.AcyclicRingVerbalizer verbalization
	public partial class RingConstraint
	{
		private partial class AcyclicRingVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static AcyclicRingVerbalizer myCache;
			public static AcyclicRingVerbalizer GetVerbalizer()
			{
				AcyclicRingVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<AcyclicRingVerbalizer>(ref myCache, null as AcyclicRingVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new AcyclicRingVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<AcyclicRingVerbalizer>(ref myCache, this, null as AcyclicRingVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
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
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
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
				if (constraintRoleArity == 2 && allFactsCount == 1 && !isNegative && allConstraintRoles[0].RolePlayer == allConstraintRoles[1].RolePlayer && ORMSolutions.ORMArchitect.Framework.Utility.EnumerableTrueForAll<RoleBase>(factRoles, delegate(RoleBase matchRoleBase)
					{
						Role matchRole = matchRoleBase.Role;
						return allConstraintRoles.Contains(matchRole) || allConstraintRoles[0].RolePlayer != matchRole.RolePlayer;
					}))
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, factRoles, MatchingReadingOptions.AllowAnyOrder);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.Acyclicity, isDeontic, isNegative);
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
						sbTemp.Append(allBasicRoleReplacements[0][unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1])]);
					}
					snippet1Replace1Replace1 = sbTemp.ToString();
					string snippet1Replace1Replace2 = null;
					snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, allBasicRoleReplacements[0], true);
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (constraintRoleArity == 2 && allFactsCount == 1 && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, factRoles, MatchingReadingOptions.AllowAnyOrder);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.AcyclicityWithRoleNumbers, isDeontic, isNegative);
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
						sbTemp.Append(allBasicRoleReplacements[0][unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1])]);
					}
					snippet1Replace1Replace1 = sbTemp.ToString();
					string snippet1Replace1Replace2 = null;
					snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, allBasicRoleReplacements[0], true);
					string snippet1Replace1Replace3 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int RoleIter3 = 0; RoleIter3 < 1; ++RoleIter3)
					{
						sbTemp.Append((reading.RoleCollection.IndexOf(allConstraintRoles[RoleIter3]) + 1).ToString(writer.FormatProvider));
						sbTemp.Append(snippet1Replace1Replace3);
					}
					snippet1Replace1Replace3 = sbTemp.ToString();
					string snippet1Replace1Replace4 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int RoleIter4 = 1; RoleIter4 < constraintRoleArity; ++RoleIter4)
					{
						sbTemp.Append((reading.RoleCollection.IndexOf(allConstraintRoles[RoleIter4]) + 1).ToString(writer.FormatProvider));
						sbTemp.Append(snippet1Replace1Replace4);
					}
					snippet1Replace1Replace4 = sbTemp.ToString();
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2, snippet1Replace1Replace3, snippet1Replace1Replace4);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
				{
					return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // RingConstraint.AcyclicRingVerbalizer verbalization
	#region RingConstraint.AntisymmetricRingVerbalizer verbalization
	public partial class RingConstraint
	{
		private partial class AntisymmetricRingVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static AntisymmetricRingVerbalizer myCache;
			public static AntisymmetricRingVerbalizer GetVerbalizer()
			{
				AntisymmetricRingVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<AntisymmetricRingVerbalizer>(ref myCache, null as AntisymmetricRingVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new AntisymmetricRingVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<AntisymmetricRingVerbalizer>(ref myCache, this, null as AntisymmetricRingVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
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
					return false;
				}
				string[][,] allBasicRoleReplacements = new string[allFactsCount][,];
				VerbalizationSubscripter subscripter = new VerbalizationSubscripter(writer.FormatProvider);
				bool[] unaryReplacements = new bool[allFactsCount];
				int minFactArity = int.MaxValue;
				int maxFactArity = int.MinValue;
				for (int iFact = 0; iFact < allFactsCount; ++iFact)
				{
					FactType currentFact = allFacts[iFact];
					if (currentFact.ReadingRequiredError != null)
					{
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
					string[,] basicRoleReplacements = new string[factArity, 3];
					bool generateSubscripts = factArity >= 2;
					for (int i = 0; i < factArity; ++i)
					{
						Role factRole = factRoles[i + unaryRoleOffset].Role;
						ObjectType rolePlayer = factRole.RolePlayer;
						string basicReplacement;
						string basicSubscriptedReplacement = null;
						if (rolePlayer != null)
						{
							bool useSubscript = false;
							if (generateSubscripts)
							{
								int j = 0;
								for (; j < factArity; ++j)
								{
									if (i != j && rolePlayer == factRoles[j].Role.RolePlayer)
									{
										useSubscript = true;
										break;
									}
								}
							}
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							if (useSubscript)
							{
								basicSubscriptedReplacement = subscripter.PrepareSubscriptFormatString(snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							}
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
						}
						basicRoleReplacements[i, 0] = basicReplacement;
						if (basicSubscriptedReplacement == null)
						{
							basicRoleReplacements[i, 1] = basicReplacement;
							basicRoleReplacements[i, 2] = null;
						}
						else
						{
							basicRoleReplacements[i, 1] = basicSubscriptedReplacement;
							basicRoleReplacements[i, 2] = string.Empty;
						}
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
				if (constraintRoleArity == 2 && allFactsCount == 1 && allConstraintRoles[0].RolePlayer == allConstraintRoles[1].RolePlayer && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLineIndented, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.LogicalAndOperator, isDeontic, isNegative);
					string snippet1Replace1Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace1ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1Replace1ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter1 = 0; snippet1Replace1ReplaceFactRoleIter1 < factArity; ++snippet1Replace1ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter1];
								string roleReplacement = null;
								if (allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter1);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter1, 0], snippet1Replace1ReplaceFactRoleIter1));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter1);
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace1);
							snippet1Replace1ReplaceIsFirstPass1 = false;
						}
					}
					snippet1Replace1Replace1 = sbTemp.ToString();
					string snippet1Replace1Replace2 = null;
					string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.SameTypeIdentityInequalityOperator, isDeontic, isNegative);
					string snippet1Replace1Replace2Replace1 = null;
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
						sbTemp.Append(subscripter.GetSubscriptedName(unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, primaryRole), allBasicRoleReplacements[0]));
					}
					snippet1Replace1Replace2Replace1 = sbTemp.ToString();
					string snippet1Replace1Replace2Replace2 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int RoleIter2 = 1; RoleIter2 < constraintRoleArity; ++RoleIter2)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter2];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						sbTemp.Append(subscripter.GetSubscriptedName(unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, primaryRole), allBasicRoleReplacements[0]));
					}
					snippet1Replace1Replace2Replace2 = sbTemp.ToString();
					snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1, snippet1Replace1Replace2Replace2);
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					string snippet1Replace2 = null;
					string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, true);
					string snippet1Replace2Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace2ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1Replace2ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factRoles.IndexOf(allConstraintRoles[allConstraintRoles.IndexOf(currentRole.Role) == 0 ? 1 : 0]), allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1);
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factRoles.IndexOf(primaryRole), allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace2ReplaceFactRoleIter1, 0], snippet1Replace2ReplaceFactRoleIter1));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace2ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1);
								}
								roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace2Replace1);
							snippet1Replace2ReplaceIsFirstPass1 = false;
						}
					}
					snippet1Replace2Replace1 = sbTemp.ToString();
					snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (constraintRoleArity == 2 && allFactsCount == 1 && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLineIndented, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.LogicalAndOperator, isDeontic, isNegative);
					string snippet1Replace1Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace1ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1Replace1ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter1 = 0; snippet1Replace1ReplaceFactRoleIter1 < factArity; ++snippet1Replace1ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter1];
								string roleReplacement = null;
								if (allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter1));
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter1, 0], snippet1Replace1ReplaceFactRoleIter1));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter1);
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace1);
							snippet1Replace1ReplaceIsFirstPass1 = false;
						}
					}
					snippet1Replace1Replace1 = sbTemp.ToString();
					string snippet1Replace1Replace2 = null;
					string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.CompatibleTypesIdentityInequalityOperator, isDeontic, isNegative);
					string snippet1Replace1Replace2Replace1 = null;
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
						sbTemp.Append(subscripter.GetSubscriptedName(unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, primaryRole), allBasicRoleReplacements[0]));
					}
					snippet1Replace1Replace2Replace1 = sbTemp.ToString();
					string snippet1Replace1Replace2Replace2 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int RoleIter2 = 1; RoleIter2 < constraintRoleArity; ++RoleIter2)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter2];
						parentFact = primaryRole.FactType;
						allReadingOrders = parentFact.ReadingOrderCollection;
						factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
						sbTemp.Append(subscripter.GetSubscriptedName(unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, primaryRole), allBasicRoleReplacements[0]));
					}
					snippet1Replace1Replace2Replace2 = sbTemp.ToString();
					snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1, snippet1Replace1Replace2Replace2);
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					string snippet1Replace2 = null;
					string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, true);
					string snippet1Replace2Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace2ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1Replace2ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factRoles.IndexOf(allConstraintRoles[allConstraintRoles.IndexOf(currentRole.Role) == 0 ? 1 : 0]), allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1));
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factRoles.IndexOf(primaryRole), allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1));
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace2ReplaceFactRoleIter1, 0], snippet1Replace2ReplaceFactRoleIter1));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace2ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1);
								}
								roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace2Replace1);
							snippet1Replace2ReplaceIsFirstPass1 = false;
						}
					}
					snippet1Replace2Replace1 = sbTemp.ToString();
					snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
				{
					return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // RingConstraint.AntisymmetricRingVerbalizer verbalization
	#region RingConstraint.AsymmetricRingVerbalizer verbalization
	public partial class RingConstraint
	{
		private partial class AsymmetricRingVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static AsymmetricRingVerbalizer myCache;
			public static AsymmetricRingVerbalizer GetVerbalizer()
			{
				AsymmetricRingVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<AsymmetricRingVerbalizer>(ref myCache, null as AsymmetricRingVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new AsymmetricRingVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<AsymmetricRingVerbalizer>(ref myCache, this, null as AsymmetricRingVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
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
					return false;
				}
				string[][,] allBasicRoleReplacements = new string[allFactsCount][,];
				VerbalizationSubscripter subscripter = new VerbalizationSubscripter(writer.FormatProvider);
				bool[] unaryReplacements = new bool[allFactsCount];
				int minFactArity = int.MaxValue;
				int maxFactArity = int.MinValue;
				for (int iFact = 0; iFact < allFactsCount; ++iFact)
				{
					FactType currentFact = allFacts[iFact];
					if (currentFact.ReadingRequiredError != null)
					{
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
					string[,] basicRoleReplacements = new string[factArity, 3];
					bool generateSubscripts = factArity >= 2;
					for (int i = 0; i < factArity; ++i)
					{
						Role factRole = factRoles[i + unaryRoleOffset].Role;
						ObjectType rolePlayer = factRole.RolePlayer;
						string basicReplacement;
						string basicSubscriptedReplacement = null;
						if (rolePlayer != null)
						{
							bool useSubscript = false;
							if (generateSubscripts)
							{
								int j = 0;
								for (; j < factArity; ++j)
								{
									if (i != j && rolePlayer == factRoles[j].Role.RolePlayer)
									{
										useSubscript = true;
										break;
									}
								}
							}
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							if (useSubscript)
							{
								basicSubscriptedReplacement = subscripter.PrepareSubscriptFormatString(snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							}
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
						}
						basicRoleReplacements[i, 0] = basicReplacement;
						if (basicSubscriptedReplacement == null)
						{
							basicRoleReplacements[i, 1] = basicReplacement;
							basicRoleReplacements[i, 2] = null;
						}
						else
						{
							basicRoleReplacements[i, 1] = basicSubscriptedReplacement;
							basicRoleReplacements[i, 2] = string.Empty;
						}
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
				if (constraintRoleArity == 2 && allFactsCount == 1 && allConstraintRoles[0].RolePlayer == allConstraintRoles[1].RolePlayer && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLineIndented, isDeontic, isNegative);
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
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
								string roleReplacement = null;
								if (allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1ReplaceFactRoleIter1);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1, 0], snippet1ReplaceFactRoleIter1));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1ReplaceFactRoleIter1);
								}
								roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1);
							snippet1ReplaceIsFirstPass1 = false;
						}
					}
					snippet1Replace1 = sbTemp.ToString();
					string snippet1Replace2 = null;
					string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, true);
					string snippet1Replace2Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace2ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1Replace2ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factRoles.IndexOf(allConstraintRoles[allConstraintRoles.IndexOf(currentRole.Role) == 0 ? 1 : 0]), allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1);
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factRoles.IndexOf(primaryRole), allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace2ReplaceFactRoleIter1, 0], snippet1Replace2ReplaceFactRoleIter1));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace2ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1);
								}
								roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace2Replace1);
							snippet1Replace2ReplaceIsFirstPass1 = false;
						}
					}
					snippet1Replace2Replace1 = sbTemp.ToString();
					snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (constraintRoleArity == 2 && allFactsCount == 1 && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLineIndented, isDeontic, isNegative);
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
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
								string roleReplacement = null;
								if (allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1ReplaceFactRoleIter1));
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1, 0], snippet1ReplaceFactRoleIter1));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1ReplaceFactRoleIter1);
								}
								roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1);
							snippet1ReplaceIsFirstPass1 = false;
						}
					}
					snippet1Replace1 = sbTemp.ToString();
					string snippet1Replace2 = null;
					string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, true);
					string snippet1Replace2Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace2ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1Replace2ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factRoles.IndexOf(allConstraintRoles[allConstraintRoles.IndexOf(currentRole.Role) == 0 ? 1 : 0]), allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1));
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factRoles.IndexOf(primaryRole), allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1));
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace2ReplaceFactRoleIter1, 0], snippet1Replace2ReplaceFactRoleIter1));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace2ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1);
								}
								roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace2Replace1);
							snippet1Replace2ReplaceIsFirstPass1 = false;
						}
					}
					snippet1Replace2Replace1 = sbTemp.ToString();
					snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
				{
					return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // RingConstraint.AsymmetricRingVerbalizer verbalization
	#region RingConstraint.IrreflexiveRingVerbalizer verbalization
	public partial class RingConstraint
	{
		private partial class IrreflexiveRingVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static IrreflexiveRingVerbalizer myCache;
			public static IrreflexiveRingVerbalizer GetVerbalizer()
			{
				IrreflexiveRingVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<IrreflexiveRingVerbalizer>(ref myCache, null as IrreflexiveRingVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new IrreflexiveRingVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<IrreflexiveRingVerbalizer>(ref myCache, this, null as IrreflexiveRingVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
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
					return false;
				}
				string[][,] allBasicRoleReplacements = new string[allFactsCount][,];
				VerbalizationSubscripter subscripter = new VerbalizationSubscripter(writer.FormatProvider);
				bool[] unaryReplacements = new bool[allFactsCount];
				int minFactArity = int.MaxValue;
				int maxFactArity = int.MinValue;
				for (int iFact = 0; iFact < allFactsCount; ++iFact)
				{
					FactType currentFact = allFacts[iFact];
					if (currentFact.ReadingRequiredError != null)
					{
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
					string[,] basicRoleReplacements = new string[factArity, 3];
					bool generateSubscripts = factArity >= 2;
					for (int i = 0; i < factArity; ++i)
					{
						Role factRole = factRoles[i + unaryRoleOffset].Role;
						ObjectType rolePlayer = factRole.RolePlayer;
						string basicReplacement;
						string basicSubscriptedReplacement = null;
						if (rolePlayer != null)
						{
							bool useSubscript = false;
							if (generateSubscripts)
							{
								int j = 0;
								for (; j < factArity; ++j)
								{
									if (i != j && rolePlayer == factRoles[j].Role.RolePlayer)
									{
										useSubscript = true;
										break;
									}
								}
							}
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							if (useSubscript)
							{
								basicSubscriptedReplacement = subscripter.PrepareSubscriptFormatString(snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							}
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
						}
						basicRoleReplacements[i, 0] = basicReplacement;
						if (basicSubscriptedReplacement == null)
						{
							basicRoleReplacements[i, 1] = basicReplacement;
							basicRoleReplacements[i, 2] = null;
						}
						else
						{
							basicRoleReplacements[i, 1] = basicSubscriptedReplacement;
							basicRoleReplacements[i, 2] = string.Empty;
						}
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
				if (constraintRoleArity == 2 && allFactsCount == 1 && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, factRoles, MatchingReadingOptions.NoFrontText);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					if (reading != null)
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
						for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1ReplaceIsFirstPass1; ++RoleIter1)
						{
							RoleBase primaryRole = allConstraintRoles[RoleIter1];
							if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.NoFrontText)) != null)
							{
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
								{
									RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
									string roleReplacement = null;
									if (primaryRole == currentRole)
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, true), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1, 0], snippet1ReplaceFactRoleIter1));
									}
									else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][factRoles.IndexOf(primaryRole), 0], snippet1ReplaceFactRoleIter1));
									}
									else
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1ReplaceFactRoleIter1));
									}
									if (roleReplacement == null)
									{
										roleReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1, 0], snippet1ReplaceFactRoleIter1);
									}
									roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
								}
								snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
								sbTemp.Append(snippet1Replace1);
								snippet1ReplaceIsFirstPass1 = false;
							}
						}
						snippet1Replace1 = sbTemp.ToString();
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
					else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
					{
						return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
					}
				}
				else if (constraintRoleArity == 2 && allFactsCount == 1)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, factRoles, MatchingReadingOptions.NoFrontText);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					if (reading != null)
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
						for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1ReplaceIsFirstPass1; ++RoleIter1)
						{
							RoleBase primaryRole = allConstraintRoles[RoleIter1];
							if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.NoFrontText)) != null)
							{
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
								{
									RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
									string roleReplacement = null;
									if (primaryRole == currentRole)
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, false), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1, 0], snippet1ReplaceFactRoleIter1));
									}
									else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][factRoles.IndexOf(primaryRole), 0], snippet1ReplaceFactRoleIter1));
									}
									else
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, false), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1ReplaceFactRoleIter1));
									}
									if (roleReplacement == null)
									{
										roleReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1, 0], snippet1ReplaceFactRoleIter1);
									}
									roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
								}
								snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
								sbTemp.Append(snippet1Replace1);
								snippet1ReplaceIsFirstPass1 = false;
							}
						}
						snippet1Replace1 = sbTemp.ToString();
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
					else
					{
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						if (reading != null)
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
							for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1ReplaceIsFirstPass1; ++RoleIter1)
							{
								RoleBase primaryRole = allConstraintRoles[RoleIter1];
								if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
								{
									hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
									for (int snippet1ReplaceFactRoleIter1 = 0; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
									{
										RoleBase currentRole = factRoles[snippet1ReplaceFactRoleIter1];
										string roleReplacement = null;
										if (primaryRole == currentRole)
										{
											roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, false), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1, 0], snippet1ReplaceFactRoleIter1));
										}
										else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
										{
											roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][factRoles.IndexOf(primaryRole), 0], snippet1ReplaceFactRoleIter1));
										}
										else
										{
											roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, false), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1ReplaceFactRoleIter1));
										}
										if (roleReplacement == null)
										{
											roleReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1ReplaceFactRoleIter1, 0], snippet1ReplaceFactRoleIter1);
										}
										roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
									}
									snippet1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
									sbTemp.Append(snippet1Replace1);
									snippet1ReplaceIsFirstPass1 = false;
								}
							}
							snippet1Replace1 = sbTemp.ToString();
							FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
						}
					}
				}
				else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
				{
					return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // RingConstraint.IrreflexiveRingVerbalizer verbalization
	#region RingConstraint.IntransitiveRingVerbalizer verbalization
	public partial class RingConstraint
	{
		private partial class IntransitiveRingVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static IntransitiveRingVerbalizer myCache;
			public static IntransitiveRingVerbalizer GetVerbalizer()
			{
				IntransitiveRingVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<IntransitiveRingVerbalizer>(ref myCache, null as IntransitiveRingVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new IntransitiveRingVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<IntransitiveRingVerbalizer>(ref myCache, this, null as IntransitiveRingVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
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
					return false;
				}
				string[][,] allBasicRoleReplacements = new string[allFactsCount][,];
				VerbalizationSubscripter subscripter = new VerbalizationSubscripter(writer.FormatProvider);
				bool[] unaryReplacements = new bool[allFactsCount];
				int minFactArity = int.MaxValue;
				int maxFactArity = int.MinValue;
				for (int iFact = 0; iFact < allFactsCount; ++iFact)
				{
					FactType currentFact = allFacts[iFact];
					if (currentFact.ReadingRequiredError != null)
					{
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
					string[,] basicRoleReplacements = new string[factArity, 4];
					bool generateSubscripts = factArity >= 2;
					for (int i = 0; i < factArity; ++i)
					{
						Role factRole = factRoles[i + unaryRoleOffset].Role;
						ObjectType rolePlayer = factRole.RolePlayer;
						string basicReplacement;
						string basicSubscriptedReplacement = null;
						string basicDynamicSubscriptedReplacement = null;
						if (rolePlayer != null)
						{
							bool useSubscript = false;
							if (generateSubscripts)
							{
								int j = 0;
								for (; j < factArity; ++j)
								{
									if (i != j && rolePlayer == factRoles[j].Role.RolePlayer)
									{
										useSubscript = true;
										break;
									}
								}
							}
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							if (useSubscript)
							{
								basicSubscriptedReplacement = subscripter.PrepareSubscriptFormatString(snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							}
							basicDynamicSubscriptedReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"), "{0}");
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
						}
						basicRoleReplacements[i, 0] = basicReplacement;
						if (basicSubscriptedReplacement == null)
						{
							basicRoleReplacements[i, 1] = basicReplacement;
							basicRoleReplacements[i, 2] = null;
						}
						else
						{
							basicRoleReplacements[i, 1] = basicSubscriptedReplacement;
							basicRoleReplacements[i, 2] = string.Empty;
						}
						basicRoleReplacements[i, 3] = basicDynamicSubscriptedReplacement ?? basicReplacement;
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
				if (constraintRoleArity == 2 && allFactsCount == 1 && allConstraintRoles[0].RolePlayer == allConstraintRoles[1].RolePlayer && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLineIndented, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.LogicalAndOperator, isDeontic, isNegative);
					string snippet1Replace1Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace1ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1Replace1ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter1 = 0; snippet1Replace1ReplaceFactRoleIter1 < factArity; ++snippet1Replace1ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter1);
								if (allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = basicReplacement;
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								}
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace1);
							snippet1Replace1ReplaceIsFirstPass1 = false;
						}
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
					bool snippet1Replace1ReplaceIsFirstPass2 = true;
					for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity && snippet1Replace1ReplaceIsFirstPass2; ++RoleIter2)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter2];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factRoles.IndexOf(allConstraintRoles[allConstraintRoles.IndexOf(currentRole.Role) == 0 ? 1 : 0]), allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2);
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(string.Format(writer.FormatProvider, allBasicRoleReplacements[0][factRoles.IndexOf(primaryRole), 3], ORMSolutions.ORMArchitect.Framework.Utility.EnumerableTrueCount(factRoles, delegate(RoleBase matchRoleBase)
										{
											ObjectType compareToRolePlayer = primaryRole.Role.RolePlayer;
											return compareToRolePlayer != null && compareToRolePlayer == matchRoleBase.Role.RolePlayer;
										}) + 1), snippet1Replace1ReplaceFactRoleIter2);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2);
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace2);
							snippet1Replace1ReplaceIsFirstPass2 = false;
						}
					}
					snippet1Replace1Replace2 = sbTemp.ToString();
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					string snippet1Replace2 = null;
					string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, true);
					string snippet1Replace2Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace2ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1Replace2ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace2ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1);
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(string.Format(writer.FormatProvider, allBasicRoleReplacements[0][snippet1Replace2ReplaceFactRoleIter1, 3], ORMSolutions.ORMArchitect.Framework.Utility.EnumerableTrueCount(factRoles, delegate(RoleBase matchRoleBase)
										{
											ObjectType compareToRolePlayer = currentRole.Role.RolePlayer;
											return compareToRolePlayer != null && compareToRolePlayer == matchRoleBase.Role.RolePlayer;
										}) + 1), snippet1Replace2ReplaceFactRoleIter1);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace2ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace2ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1);
								}
								roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace2Replace1);
							snippet1Replace2ReplaceIsFirstPass1 = false;
						}
					}
					snippet1Replace2Replace1 = sbTemp.ToString();
					snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (constraintRoleArity == 2 && allFactsCount == 1 && factArity == 2 && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLineIndented, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.LogicalAndOperator, isDeontic, isNegative);
					string snippet1Replace1Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace1ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1Replace1ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.None)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter1 = 0; snippet1Replace1ReplaceFactRoleIter1 < factArity; ++snippet1Replace1ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter1];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter1, 0], snippet1Replace1ReplaceFactRoleIter1));
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(string.Format(writer.FormatProvider, allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter1, 3], ORMSolutions.ORMArchitect.Framework.Utility.EnumerableTrueCount(factRoles, delegate(RoleBase matchRoleBase)
										{
											ObjectType compareToRolePlayer = currentRole.Role.RolePlayer;
											return compareToRolePlayer != null && compareToRolePlayer == matchRoleBase.Role.RolePlayer;
										}) + 0), snippet1Replace1ReplaceFactRoleIter1);
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter1, 0], snippet1Replace1ReplaceFactRoleIter1);
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace1);
							snippet1Replace1ReplaceIsFirstPass1 = false;
						}
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
					bool snippet1Replace1ReplaceIsFirstPass2 = true;
					for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity && snippet1Replace1ReplaceIsFirstPass2; ++RoleIter2)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter2];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.None)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(string.Format(writer.FormatProvider, allBasicRoleReplacements[0][factRoles.IndexOf(allConstraintRoles[allConstraintRoles.IndexOf(currentRole.Role) == 0 ? 1 : 0]), 3], ORMSolutions.ORMArchitect.Framework.Utility.EnumerableTrueCount(factRoles, delegate(RoleBase matchRoleBase)
										{
											ObjectType compareToRolePlayer = allConstraintRoles[allConstraintRoles.IndexOf(currentRole.Role) == 0 ? 1 : 0].Role.RolePlayer;
											return compareToRolePlayer != null && compareToRolePlayer == matchRoleBase.Role.RolePlayer;
										}) + 0), snippet1Replace1ReplaceFactRoleIter2);
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(string.Format(writer.FormatProvider, allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 3], ORMSolutions.ORMArchitect.Framework.Utility.EnumerableTrueCount(factRoles, delegate(RoleBase matchRoleBase)
										{
											ObjectType compareToRolePlayer = currentRole.Role.RolePlayer;
											return compareToRolePlayer != null && compareToRolePlayer == matchRoleBase.Role.RolePlayer;
										}) + 1), snippet1Replace1ReplaceFactRoleIter2);
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2);
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace2);
							snippet1Replace1ReplaceIsFirstPass2 = false;
						}
					}
					snippet1Replace1Replace2 = sbTemp.ToString();
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					string snippet1Replace2 = null;
					string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, true);
					string snippet1Replace2Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace2ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1Replace2ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.None)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace2ReplaceFactRoleIter1, 0], snippet1Replace2ReplaceFactRoleIter1));
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(string.Format(writer.FormatProvider, allBasicRoleReplacements[0][snippet1Replace2ReplaceFactRoleIter1, 3], ORMSolutions.ORMArchitect.Framework.Utility.EnumerableTrueCount(factRoles, delegate(RoleBase matchRoleBase)
										{
											ObjectType compareToRolePlayer = currentRole.Role.RolePlayer;
											return compareToRolePlayer != null && compareToRolePlayer == matchRoleBase.Role.RolePlayer;
										}) + 1), snippet1Replace2ReplaceFactRoleIter1);
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace2ReplaceFactRoleIter1, 0], snippet1Replace2ReplaceFactRoleIter1);
								}
								roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace2Replace1);
							snippet1Replace2ReplaceIsFirstPass1 = false;
						}
					}
					snippet1Replace2Replace1 = sbTemp.ToString();
					snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (constraintRoleArity == 2 && allFactsCount == 1 && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLineIndented, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.LogicalAndOperator, isDeontic, isNegative);
					string snippet1Replace1Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace1ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1Replace1ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter1 = 0; snippet1Replace1ReplaceFactRoleIter1 < factArity; ++snippet1Replace1ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter1);
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace1);
							snippet1Replace1ReplaceIsFirstPass1 = false;
						}
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
					bool snippet1Replace1ReplaceIsFirstPass2 = true;
					for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity && snippet1Replace1ReplaceIsFirstPass2; ++RoleIter2)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter2];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factRoles.IndexOf(allConstraintRoles[allConstraintRoles.IndexOf(currentRole.Role) == 0 ? 1 : 0]), allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2));
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(string.Format(writer.FormatProvider, allBasicRoleReplacements[0][factRoles.IndexOf(primaryRole), 3], ORMSolutions.ORMArchitect.Framework.Utility.EnumerableTrueCount(factRoles, delegate(RoleBase matchRoleBase)
										{
											ObjectType compareToRolePlayer = primaryRole.Role.RolePlayer;
											return compareToRolePlayer != null && compareToRolePlayer == matchRoleBase.Role.RolePlayer;
										}) + 1), snippet1Replace1ReplaceFactRoleIter2));
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2);
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace2);
							snippet1Replace1ReplaceIsFirstPass2 = false;
						}
					}
					snippet1Replace1Replace2 = sbTemp.ToString();
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					string snippet1Replace2 = null;
					string snippet1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.ModalPossibilityOperator, isDeontic, true);
					string snippet1Replace2Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace2ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1Replace2ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace2ReplaceFactRoleIter1 = 0; snippet1Replace2ReplaceFactRoleIter1 < factArity; ++snippet1Replace2ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace2ReplaceFactRoleIter1];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace2ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1));
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(string.Format(writer.FormatProvider, allBasicRoleReplacements[0][factRoles.IndexOf(primaryRole), 3], ORMSolutions.ORMArchitect.Framework.Utility.EnumerableTrueCount(factRoles, delegate(RoleBase matchRoleBase)
										{
											ObjectType compareToRolePlayer = primaryRole.Role.RolePlayer;
											return compareToRolePlayer != null && compareToRolePlayer == matchRoleBase.Role.RolePlayer;
										}) + 1), snippet1Replace2ReplaceFactRoleIter1));
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace2ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace2ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace2ReplaceFactRoleIter1);
								}
								roleReplacements[snippet1Replace2ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace2Replace1);
							snippet1Replace2ReplaceIsFirstPass1 = false;
						}
					}
					snippet1Replace2Replace1 = sbTemp.ToString();
					snippet1Replace2 = string.Format(writer.FormatProvider, snippet1ReplaceFormat2, snippet1Replace2Replace1);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
				{
					return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // RingConstraint.IntransitiveRingVerbalizer verbalization
	#region RingConstraint.StronglyIntransitiveRingVerbalizer verbalization
	public partial class RingConstraint
	{
		private partial class StronglyIntransitiveRingVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static StronglyIntransitiveRingVerbalizer myCache;
			public static StronglyIntransitiveRingVerbalizer GetVerbalizer()
			{
				StronglyIntransitiveRingVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<StronglyIntransitiveRingVerbalizer>(ref myCache, null as StronglyIntransitiveRingVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new StronglyIntransitiveRingVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<StronglyIntransitiveRingVerbalizer>(ref myCache, this, null as StronglyIntransitiveRingVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
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
					return false;
				}
				string[][,] allBasicRoleReplacements = new string[allFactsCount][,];
				VerbalizationSubscripter subscripter = new VerbalizationSubscripter(writer.FormatProvider);
				bool[] unaryReplacements = new bool[allFactsCount];
				int minFactArity = int.MaxValue;
				int maxFactArity = int.MinValue;
				for (int iFact = 0; iFact < allFactsCount; ++iFact)
				{
					FactType currentFact = allFacts[iFact];
					if (currentFact.ReadingRequiredError != null)
					{
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
					string[,] basicRoleReplacements = new string[factArity, 3];
					bool generateSubscripts = factArity >= 2;
					for (int i = 0; i < factArity; ++i)
					{
						Role factRole = factRoles[i + unaryRoleOffset].Role;
						ObjectType rolePlayer = factRole.RolePlayer;
						string basicReplacement;
						string basicSubscriptedReplacement = null;
						if (rolePlayer != null)
						{
							bool useSubscript = false;
							if (generateSubscripts)
							{
								int j = 0;
								for (; j < factArity; ++j)
								{
									if (i != j && rolePlayer == factRoles[j].Role.RolePlayer)
									{
										useSubscript = true;
										break;
									}
								}
							}
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							if (useSubscript)
							{
								basicSubscriptedReplacement = subscripter.PrepareSubscriptFormatString(snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							}
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
						}
						basicRoleReplacements[i, 0] = basicReplacement;
						if (basicSubscriptedReplacement == null)
						{
							basicRoleReplacements[i, 1] = basicReplacement;
							basicRoleReplacements[i, 2] = null;
						}
						else
						{
							basicRoleReplacements[i, 1] = basicSubscriptedReplacement;
							basicRoleReplacements[i, 2] = string.Empty;
						}
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
				if (constraintRoleArity == 2 && allFactsCount == 1 && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLineIndented, isDeontic, isNegative);
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
						reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						for (int snippet1Replace1ReplaceFactRoleIter1 = 0; snippet1Replace1ReplaceFactRoleIter1 < factArity; ++snippet1Replace1ReplaceFactRoleIter1)
						{
							RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter1];
							string roleReplacement = null;
							if (primaryRole == currentRole)
							{
								roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter1);
							}
							else if (allConstraintRoles.Contains(currentRole.Role))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter1));
							}
							else if (!allConstraintRoles.Contains(currentRole.Role))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter1, 0], snippet1Replace1ReplaceFactRoleIter1));
							}
							if (roleReplacement == null)
							{
								roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter1);
							}
							roleReplacements[snippet1Replace1ReplaceFactRoleIter1] = roleReplacement;
						}
						snippet1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
						sbTemp.Append(snippet1Replace1Replace1);
					}
					snippet1Replace1Replace1 = sbTemp.ToString();
					string snippet1Replace1Replace2 = null;
					string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.StronglyIntransitiveConsequent, isDeontic, isNegative);
					string snippet1Replace1Replace2Replace1 = null;
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
						sbTemp.Append(subscripter.GetSubscriptedName(unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter1]), allBasicRoleReplacements[0]));
					}
					snippet1Replace1Replace2Replace1 = sbTemp.ToString();
					string snippet1Replace1Replace2Replace2 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					for (int RoleIter2 = 1; RoleIter2 < constraintRoleArity; ++RoleIter2)
					{
						sbTemp.Append(subscripter.GetSubscriptedName(unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[RoleIter2]), allBasicRoleReplacements[0]));
					}
					snippet1Replace1Replace2Replace2 = sbTemp.ToString();
					snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1, snippet1Replace1Replace2Replace2);
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
				{
					return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // RingConstraint.StronglyIntransitiveRingVerbalizer verbalization
	#region RingConstraint.SymmetricRingVerbalizer verbalization
	public partial class RingConstraint
	{
		private partial class SymmetricRingVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static SymmetricRingVerbalizer myCache;
			public static SymmetricRingVerbalizer GetVerbalizer()
			{
				SymmetricRingVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<SymmetricRingVerbalizer>(ref myCache, null as SymmetricRingVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new SymmetricRingVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<SymmetricRingVerbalizer>(ref myCache, this, null as SymmetricRingVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
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
					return false;
				}
				string[][,] allBasicRoleReplacements = new string[allFactsCount][,];
				VerbalizationSubscripter subscripter = new VerbalizationSubscripter(writer.FormatProvider);
				bool[] unaryReplacements = new bool[allFactsCount];
				int minFactArity = int.MaxValue;
				int maxFactArity = int.MinValue;
				for (int iFact = 0; iFact < allFactsCount; ++iFact)
				{
					FactType currentFact = allFacts[iFact];
					if (currentFact.ReadingRequiredError != null)
					{
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
					string[,] basicRoleReplacements = new string[factArity, 3];
					bool generateSubscripts = factArity >= 2;
					for (int i = 0; i < factArity; ++i)
					{
						Role factRole = factRoles[i + unaryRoleOffset].Role;
						ObjectType rolePlayer = factRole.RolePlayer;
						string basicReplacement;
						string basicSubscriptedReplacement = null;
						if (rolePlayer != null)
						{
							bool useSubscript = false;
							if (generateSubscripts)
							{
								int j = 0;
								for (; j < factArity; ++j)
								{
									if (i != j && rolePlayer == factRoles[j].Role.RolePlayer)
									{
										useSubscript = true;
										break;
									}
								}
							}
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							if (useSubscript)
							{
								basicSubscriptedReplacement = subscripter.PrepareSubscriptFormatString(snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							}
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
						}
						basicRoleReplacements[i, 0] = basicReplacement;
						if (basicSubscriptedReplacement == null)
						{
							basicRoleReplacements[i, 1] = basicReplacement;
							basicRoleReplacements[i, 2] = null;
						}
						else
						{
							basicRoleReplacements[i, 1] = basicSubscriptedReplacement;
							basicRoleReplacements[i, 2] = string.Empty;
						}
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
				if (constraintRoleArity == 2 && allFactsCount == 1 && allConstraintRoles[0].RolePlayer == allConstraintRoles[1].RolePlayer && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLineIndented, isDeontic, isNegative);
					string snippet1Replace1Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace1ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1Replace1ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter1 = 0; snippet1Replace1ReplaceFactRoleIter1 < factArity; ++snippet1Replace1ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter1];
								string roleReplacement = null;
								if (allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter1);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter1, 0], snippet1Replace1ReplaceFactRoleIter1));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter1);
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace1);
							snippet1Replace1ReplaceIsFirstPass1 = false;
						}
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
					bool snippet1Replace1ReplaceIsFirstPass2 = true;
					for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity && snippet1Replace1ReplaceIsFirstPass2; ++RoleIter2)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter2];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factRoles.IndexOf(allConstraintRoles[allConstraintRoles.IndexOf(currentRole.Role) == 0 ? 1 : 0]), allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2);
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factRoles.IndexOf(primaryRole), allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2);
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace2);
							snippet1Replace1ReplaceIsFirstPass2 = false;
						}
					}
					snippet1Replace1Replace2 = sbTemp.ToString();
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (constraintRoleArity == 2 && allFactsCount == 1 && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLineIndented, isDeontic, isNegative);
					string snippet1Replace1Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace1ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1Replace1ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter1 = 0; snippet1Replace1ReplaceFactRoleIter1 < factArity; ++snippet1Replace1ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter1];
								string roleReplacement = null;
								if (allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter1));
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter1, 0], snippet1Replace1ReplaceFactRoleIter1));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter1);
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace1);
							snippet1Replace1ReplaceIsFirstPass1 = false;
						}
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
					bool snippet1Replace1ReplaceIsFirstPass2 = true;
					for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity && snippet1Replace1ReplaceIsFirstPass2; ++RoleIter2)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter2];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factRoles.IndexOf(allConstraintRoles[allConstraintRoles.IndexOf(currentRole.Role) == 0 ? 1 : 0]), allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2));
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factRoles.IndexOf(primaryRole), allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2));
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2);
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace2);
							snippet1Replace1ReplaceIsFirstPass2 = false;
						}
					}
					snippet1Replace1Replace2 = sbTemp.ToString();
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
				{
					return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // RingConstraint.SymmetricRingVerbalizer verbalization
	#region RingConstraint.ReflexiveRingVerbalizer verbalization
	public partial class RingConstraint
	{
		private partial class ReflexiveRingVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static ReflexiveRingVerbalizer myCache;
			public static ReflexiveRingVerbalizer GetVerbalizer()
			{
				ReflexiveRingVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<ReflexiveRingVerbalizer>(ref myCache, null as ReflexiveRingVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new ReflexiveRingVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<ReflexiveRingVerbalizer>(ref myCache, this, null as ReflexiveRingVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
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
					return false;
				}
				string[][,] allBasicRoleReplacements = new string[allFactsCount][,];
				VerbalizationSubscripter subscripter = new VerbalizationSubscripter(writer.FormatProvider);
				bool[] unaryReplacements = new bool[allFactsCount];
				int minFactArity = int.MaxValue;
				int maxFactArity = int.MinValue;
				for (int iFact = 0; iFact < allFactsCount; ++iFact)
				{
					FactType currentFact = allFacts[iFact];
					if (currentFact.ReadingRequiredError != null)
					{
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
					string[,] basicRoleReplacements = new string[factArity, 3];
					bool generateSubscripts = factArity >= 2;
					for (int i = 0; i < factArity; ++i)
					{
						Role factRole = factRoles[i + unaryRoleOffset].Role;
						ObjectType rolePlayer = factRole.RolePlayer;
						string basicReplacement;
						string basicSubscriptedReplacement = null;
						if (rolePlayer != null)
						{
							bool useSubscript = false;
							if (generateSubscripts)
							{
								int j = 0;
								for (; j < factArity; ++j)
								{
									if (i != j && rolePlayer == factRoles[j].Role.RolePlayer)
									{
										useSubscript = true;
										break;
									}
								}
							}
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							if (useSubscript)
							{
								basicSubscriptedReplacement = subscripter.PrepareSubscriptFormatString(snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							}
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
						}
						basicRoleReplacements[i, 0] = basicReplacement;
						if (basicSubscriptedReplacement == null)
						{
							basicRoleReplacements[i, 1] = basicReplacement;
							basicRoleReplacements[i, 2] = null;
						}
						else
						{
							basicRoleReplacements[i, 1] = basicSubscriptedReplacement;
							basicRoleReplacements[i, 2] = string.Empty;
						}
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
				if (constraintRoleArity == 2 && allFactsCount == 1 && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, factRoles, MatchingReadingOptions.None);
					hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
					if (reading != null)
					{
						verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						bool factTextIsFirstPass1 = true;
						for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && factTextIsFirstPass1; ++RoleIter1)
						{
							RoleBase primaryRole = allConstraintRoles[RoleIter1];
							if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.None)) != null)
							{
								string factText1 = null;
								string factTextFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
								string factText1Replace1 = null;
								string factText1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLineIndented, isDeontic, isNegative);
								string factText1Replace1Replace1 = null;
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								for (int factText1Replace1ReplaceFactRoleIter1 = 0; factText1Replace1ReplaceFactRoleIter1 < factArity; ++factText1Replace1ReplaceFactRoleIter1)
								{
									RoleBase currentRole = factRoles[factText1Replace1ReplaceFactRoleIter1];
									string roleReplacement = null;
									if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factText1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), factText1Replace1ReplaceFactRoleIter1));
									}
									else if (!allConstraintRoles.Contains(currentRole.Role))
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][factText1Replace1ReplaceFactRoleIter1, 0], factText1Replace1ReplaceFactRoleIter1));
									}
									if (roleReplacement == null)
									{
										roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factText1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), factText1Replace1ReplaceFactRoleIter1);
									}
									roleReplacements[factText1Replace1ReplaceFactRoleIter1] = roleReplacement;
								}
								factText1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
								string factText1Replace1Replace2 = null;
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								for (int factText1Replace1ReplaceFactRoleIter2 = 0; factText1Replace1ReplaceFactRoleIter2 < factArity; ++factText1Replace1ReplaceFactRoleIter2)
								{
									RoleBase currentRole = factRoles[factText1Replace1ReplaceFactRoleIter2];
									string roleReplacement = null;
									if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ReflexivePronoun, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factText1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), factText1Replace1ReplaceFactRoleIter2));
									}
									else if (!allConstraintRoles.Contains(currentRole.Role))
									{
										roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][factText1Replace1ReplaceFactRoleIter2, 0], factText1Replace1ReplaceFactRoleIter2));
									}
									if (roleReplacement == null)
									{
										roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factText1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), factText1Replace1ReplaceFactRoleIter2);
									}
									roleReplacements[factText1Replace1ReplaceFactRoleIter2] = roleReplacement;
								}
								factText1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
								factText1Replace1 = string.Format(writer.FormatProvider, factText1ReplaceFormat1, factText1Replace1Replace1, factText1Replace1Replace2);
								factText1 = string.Format(writer.FormatProvider, factTextFormat1, factText1Replace1);
								sbTemp.Append(factText1);
								factTextIsFirstPass1 = false;
							}
						}
						FactType.WriteVerbalizerSentence(writer, sbTemp.ToString(), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
					else
					{
						reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						if (reading != null)
						{
							verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
							if (sbTemp == null)
							{
								sbTemp = new StringBuilder();
							}
							else
							{
								sbTemp.Length = 0;
							}
							bool factTextIsFirstPass1 = true;
							for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && factTextIsFirstPass1; ++RoleIter1)
							{
								RoleBase primaryRole = allConstraintRoles[RoleIter1];
								if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
								{
									string factText1 = null;
									string factTextFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
									string factText1Replace1 = null;
									string factText1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLineIndented, isDeontic, isNegative);
									string factText1Replace1Replace1 = null;
									for (int factText1Replace1ReplaceFactRoleIter1 = 0; factText1Replace1ReplaceFactRoleIter1 < factArity; ++factText1Replace1ReplaceFactRoleIter1)
									{
										RoleBase currentRole = factRoles[factText1Replace1ReplaceFactRoleIter1];
										string roleReplacement = null;
										if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
										{
											roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factText1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), factText1Replace1ReplaceFactRoleIter1));
										}
										else if (!allConstraintRoles.Contains(currentRole.Role))
										{
											roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][factText1Replace1ReplaceFactRoleIter1, 0], factText1Replace1ReplaceFactRoleIter1));
										}
										if (roleReplacement == null)
										{
											roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factText1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), factText1Replace1ReplaceFactRoleIter1);
										}
										roleReplacements[factText1Replace1ReplaceFactRoleIter1] = roleReplacement;
									}
									factText1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
									string factText1Replace1Replace2 = null;
									for (int factText1Replace1ReplaceFactRoleIter2 = 0; factText1Replace1ReplaceFactRoleIter2 < factArity; ++factText1Replace1ReplaceFactRoleIter2)
									{
										RoleBase currentRole = factRoles[factText1Replace1ReplaceFactRoleIter2];
										string roleReplacement = null;
										if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
										{
											roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ReflexiveQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factRoles.IndexOf(primaryRole), allBasicRoleReplacements[0]), factText1Replace1ReplaceFactRoleIter2));
										}
										else if (!allConstraintRoles.Contains(currentRole.Role))
										{
											roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][factText1Replace1ReplaceFactRoleIter2, 0], factText1Replace1ReplaceFactRoleIter2));
										}
										if (roleReplacement == null)
										{
											roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factText1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), factText1Replace1ReplaceFactRoleIter2);
										}
										roleReplacements[factText1Replace1ReplaceFactRoleIter2] = roleReplacement;
									}
									factText1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
									factText1Replace1 = string.Format(writer.FormatProvider, factText1ReplaceFormat1, factText1Replace1Replace1, factText1Replace1Replace2);
									factText1 = string.Format(writer.FormatProvider, factTextFormat1, factText1Replace1);
									sbTemp.Append(factText1);
									factTextIsFirstPass1 = false;
								}
							}
							FactType.WriteVerbalizerSentence(writer, sbTemp.ToString(), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
						}
					}
				}
				else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
				{
					return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // RingConstraint.ReflexiveRingVerbalizer verbalization
	#region RingConstraint.PurelyReflexiveRingVerbalizer verbalization
	public partial class RingConstraint
	{
		private partial class PurelyReflexiveRingVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static PurelyReflexiveRingVerbalizer myCache;
			public static PurelyReflexiveRingVerbalizer GetVerbalizer()
			{
				PurelyReflexiveRingVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<PurelyReflexiveRingVerbalizer>(ref myCache, null as PurelyReflexiveRingVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new PurelyReflexiveRingVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<PurelyReflexiveRingVerbalizer>(ref myCache, this, null as PurelyReflexiveRingVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
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
					return false;
				}
				string[][,] allBasicRoleReplacements = new string[allFactsCount][,];
				VerbalizationSubscripter subscripter = new VerbalizationSubscripter(writer.FormatProvider);
				bool[] unaryReplacements = new bool[allFactsCount];
				int minFactArity = int.MaxValue;
				int maxFactArity = int.MinValue;
				for (int iFact = 0; iFact < allFactsCount; ++iFact)
				{
					FactType currentFact = allFacts[iFact];
					if (currentFact.ReadingRequiredError != null)
					{
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
					string[,] basicRoleReplacements = new string[factArity, 3];
					bool generateSubscripts = factArity >= 2;
					for (int i = 0; i < factArity; ++i)
					{
						Role factRole = factRoles[i + unaryRoleOffset].Role;
						ObjectType rolePlayer = factRole.RolePlayer;
						string basicReplacement;
						string basicSubscriptedReplacement = null;
						if (rolePlayer != null)
						{
							bool useSubscript = false;
							if (generateSubscripts)
							{
								int j = 0;
								for (; j < factArity; ++j)
								{
									if (i != j && rolePlayer == factRoles[j].Role.RolePlayer)
									{
										useSubscript = true;
										break;
									}
								}
							}
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							if (useSubscript)
							{
								basicSubscriptedReplacement = subscripter.PrepareSubscriptFormatString(snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							}
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
						}
						basicRoleReplacements[i, 0] = basicReplacement;
						if (basicSubscriptedReplacement == null)
						{
							basicRoleReplacements[i, 1] = basicReplacement;
							basicRoleReplacements[i, 2] = null;
						}
						else
						{
							basicRoleReplacements[i, 1] = basicSubscriptedReplacement;
							basicRoleReplacements[i, 2] = string.Empty;
						}
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
				if (constraintRoleArity == 2 && allFactsCount == 1 && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
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
						string factText1 = null;
						string factTextFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string factText1Replace1 = null;
						string factText1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLineIndented, isDeontic, isNegative);
						string factText1Replace1Replace1 = null;
						reading = parentFact.GetMatchingReading(allReadingOrders, null, null, allConstraintRoles, factRoles, MatchingReadingOptions.AllowAnyOrder);
						hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
						for (int factText1Replace1ReplaceFactRoleIter1 = 0; factText1Replace1ReplaceFactRoleIter1 < factArity; ++factText1Replace1ReplaceFactRoleIter1)
						{
							RoleBase currentRole = factRoles[factText1Replace1ReplaceFactRoleIter1];
							string roleReplacement = null;
							if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factText1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), factText1Replace1ReplaceFactRoleIter1));
							}
							else if (!allConstraintRoles.Contains(currentRole.Role))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][factText1Replace1ReplaceFactRoleIter1, 0], factText1Replace1ReplaceFactRoleIter1));
							}
							if (roleReplacement == null)
							{
								roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factText1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), factText1Replace1ReplaceFactRoleIter1);
							}
							roleReplacements[factText1Replace1ReplaceFactRoleIter1] = roleReplacement;
						}
						factText1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
						string factText1Replace1Replace2 = null;
						RoleBase contextPrimaryRole = primaryRole;
						int contextTempStringBuildLength = sbTemp.Length;
						int factText1Replace1ReplaceFilteredIter2;
						int factText1Replace1ReplaceFilteredCount2 = 0;
						for (factText1Replace1ReplaceFilteredIter2 = 0; factText1Replace1ReplaceFilteredIter2 < constraintRoleArity; ++factText1Replace1ReplaceFilteredIter2)
						{
							primaryRole = allConstraintRoles[factText1Replace1ReplaceFilteredIter2];
							if (allConstraintRoles.Contains(primaryRole.Role))
							{
								++factText1Replace1ReplaceFilteredCount2;
							}
						}
						factText1Replace1ReplaceFilteredIter2 = 0;
						for (int ContextRoleIter2 = 0; ContextRoleIter2 < constraintRoleArity; ++ContextRoleIter2)
						{
							primaryRole = allConstraintRoles[ContextRoleIter2];
							if (allConstraintRoles.Contains(primaryRole.Role))
							{
								CoreVerbalizationSnippetType listSnippet;
								if (factText1Replace1ReplaceFilteredIter2 == 0)
								{
									listSnippet = CoreVerbalizationSnippetType.EqualsListOpen;
								}
								else if (factText1Replace1ReplaceFilteredIter2 == factText1Replace1ReplaceFilteredCount2 - 1)
								{
									if (factText1Replace1ReplaceFilteredIter2 == 1)
									{
										listSnippet = CoreVerbalizationSnippetType.EqualsListPairSeparator;
									}
									else
									{
										listSnippet = CoreVerbalizationSnippetType.EqualsListFinalSeparator;
									}
								}
								else
								{
									listSnippet = CoreVerbalizationSnippetType.EqualsListSeparator;
								}
								sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
								sbTemp.Append(subscripter.GetSubscriptedName(unaryReplacements[0] ? 0 : FactType.IndexOfRole(factRoles, allConstraintRoles[ContextRoleIter2]), allBasicRoleReplacements[0]));
								if (factText1Replace1ReplaceFilteredIter2 == factText1Replace1ReplaceFilteredCount2 - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.EqualsListClose, isDeontic, isNegative));
								}
								++factText1Replace1ReplaceFilteredIter2;
							}
						}
						factText1Replace1Replace2 = sbTemp.ToString(contextTempStringBuildLength, sbTemp.Length - contextTempStringBuildLength);
						primaryRole = contextPrimaryRole;
						sbTemp.Length = contextTempStringBuildLength;
						factText1Replace1 = string.Format(writer.FormatProvider, factText1ReplaceFormat1, factText1Replace1Replace1, factText1Replace1Replace2);
						factText1 = string.Format(writer.FormatProvider, factTextFormat1, factText1Replace1);
						sbTemp.Append(factText1);
					}
					FactType.WriteVerbalizerSentence(writer, sbTemp.ToString(), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
				{
					return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // RingConstraint.PurelyReflexiveRingVerbalizer verbalization
	#region RingConstraint.TransitiveRingVerbalizer verbalization
	public partial class RingConstraint
	{
		private partial class TransitiveRingVerbalizer : IVerbalize, IDisposable
		{
			#region Cache management
			// Cache an instance so we only create one helper in single-threaded scenarios
			private static TransitiveRingVerbalizer myCache;
			public static TransitiveRingVerbalizer GetVerbalizer()
			{
				TransitiveRingVerbalizer retVal = myCache;
				if (retVal != null)
				{
					retVal = System.Threading.Interlocked.CompareExchange<TransitiveRingVerbalizer>(ref myCache, null as TransitiveRingVerbalizer, retVal);
				}
				if (retVal == null)
				{
					retVal = new TransitiveRingVerbalizer();
				}
				return retVal;
			}
			void IDisposable.Dispose()
			{
				this.DisposeHelper();
				if (myCache == null)
				{
					System.Threading.Interlocked.CompareExchange<TransitiveRingVerbalizer>(ref myCache, this, null as TransitiveRingVerbalizer);
				}
			}
			#endregion // Cache management
			/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				bool isDeontic = this.Modality == ConstraintModality.Deontic;
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
					return false;
				}
				string[][,] allBasicRoleReplacements = new string[allFactsCount][,];
				VerbalizationSubscripter subscripter = new VerbalizationSubscripter(writer.FormatProvider);
				bool[] unaryReplacements = new bool[allFactsCount];
				int minFactArity = int.MaxValue;
				int maxFactArity = int.MinValue;
				for (int iFact = 0; iFact < allFactsCount; ++iFact)
				{
					FactType currentFact = allFacts[iFact];
					if (currentFact.ReadingRequiredError != null)
					{
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
					string[,] basicRoleReplacements = new string[factArity, 4];
					bool generateSubscripts = factArity >= 2;
					for (int i = 0; i < factArity; ++i)
					{
						Role factRole = factRoles[i + unaryRoleOffset].Role;
						ObjectType rolePlayer = factRole.RolePlayer;
						string basicReplacement;
						string basicSubscriptedReplacement = null;
						string basicDynamicSubscriptedReplacement = null;
						if (rolePlayer != null)
						{
							bool useSubscript = false;
							if (generateSubscripts)
							{
								int j = 0;
								for (; j < factArity; ++j)
								{
									if (i != j && rolePlayer == factRoles[j].Role.RolePlayer)
									{
										useSubscript = true;
										break;
									}
								}
							}
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							if (useSubscript)
							{
								basicSubscriptedReplacement = subscripter.PrepareSubscriptFormatString(snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
							}
							basicDynamicSubscriptedReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"), "{0}");
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
						}
						basicRoleReplacements[i, 0] = basicReplacement;
						if (basicSubscriptedReplacement == null)
						{
							basicRoleReplacements[i, 1] = basicReplacement;
							basicRoleReplacements[i, 2] = null;
						}
						else
						{
							basicRoleReplacements[i, 1] = basicSubscriptedReplacement;
							basicRoleReplacements[i, 2] = string.Empty;
						}
						basicRoleReplacements[i, 3] = basicDynamicSubscriptedReplacement ?? basicReplacement;
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
				if (constraintRoleArity == 2 && allFactsCount == 1 && allConstraintRoles[0].RolePlayer == allConstraintRoles[1].RolePlayer && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLineIndented, isDeontic, isNegative);
					string snippet1Replace1Replace1 = null;
					string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.LogicalAndOperator, isDeontic, isNegative);
					string snippet1Replace1Replace1Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace1Replace1ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1Replace1Replace1ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1Replace1ReplaceFactRoleIter1 = 0; snippet1Replace1Replace1ReplaceFactRoleIter1 < factArity; ++snippet1Replace1Replace1ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace1Replace1ReplaceFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace1Replace1ReplaceFactRoleIter1);
								if (allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = basicReplacement;
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								}
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace1Replace1ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace1Replace1);
							snippet1Replace1Replace1ReplaceIsFirstPass1 = false;
						}
					}
					snippet1Replace1Replace1Replace1 = sbTemp.ToString();
					string snippet1Replace1Replace1Replace2 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace1Replace1ReplaceIsFirstPass2 = true;
					for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity && snippet1Replace1Replace1ReplaceIsFirstPass2; ++RoleIter2)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter2];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1Replace1ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1Replace1ReplaceFactRoleIter2];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factRoles.IndexOf(allConstraintRoles[allConstraintRoles.IndexOf(currentRole.Role) == 0 ? 1 : 0]), allBasicRoleReplacements[0]), snippet1Replace1Replace1ReplaceFactRoleIter2);
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(string.Format(writer.FormatProvider, allBasicRoleReplacements[0][factRoles.IndexOf(primaryRole), 3], ORMSolutions.ORMArchitect.Framework.Utility.EnumerableTrueCount(factRoles, delegate(RoleBase matchRoleBase)
										{
											ObjectType compareToRolePlayer = primaryRole.Role.RolePlayer;
											return compareToRolePlayer != null && compareToRolePlayer == matchRoleBase.Role.RolePlayer;
										}) + 1), snippet1Replace1Replace1ReplaceFactRoleIter2);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1Replace1ReplaceFactRoleIter2));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1Replace1ReplaceFactRoleIter2);
								}
								roleReplacements[snippet1Replace1Replace1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace1Replace2);
							snippet1Replace1Replace1ReplaceIsFirstPass2 = false;
						}
					}
					snippet1Replace1Replace1Replace2 = sbTemp.ToString();
					snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1, snippet1Replace1Replace1Replace2);
					string snippet1Replace1Replace2 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace1ReplaceIsFirstPass2 = true;
					for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity && snippet1Replace1ReplaceIsFirstPass2; ++RoleIter2)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter2];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2);
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(string.Format(writer.FormatProvider, allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 3], ORMSolutions.ORMArchitect.Framework.Utility.EnumerableTrueCount(factRoles, delegate(RoleBase matchRoleBase)
										{
											ObjectType compareToRolePlayer = currentRole.Role.RolePlayer;
											return compareToRolePlayer != null && compareToRolePlayer == matchRoleBase.Role.RolePlayer;
										}) + 1), snippet1Replace1ReplaceFactRoleIter2);
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2);
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace2);
							snippet1Replace1ReplaceIsFirstPass2 = false;
						}
					}
					snippet1Replace1Replace2 = sbTemp.ToString();
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (constraintRoleArity == 2 && allFactsCount == 1 && factArity == 2 && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLineIndented, isDeontic, isNegative);
					string snippet1Replace1Replace1 = null;
					string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.LogicalAndOperator, isDeontic, isNegative);
					string snippet1Replace1Replace1Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace1Replace1ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1Replace1Replace1ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.None)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1Replace1ReplaceFactRoleIter1 = 0; snippet1Replace1Replace1ReplaceFactRoleIter1 < factArity; ++snippet1Replace1Replace1ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace1Replace1ReplaceFactRoleIter1];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1Replace1ReplaceFactRoleIter1, 0], snippet1Replace1Replace1ReplaceFactRoleIter1));
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(string.Format(writer.FormatProvider, allBasicRoleReplacements[0][snippet1Replace1Replace1ReplaceFactRoleIter1, 3], ORMSolutions.ORMArchitect.Framework.Utility.EnumerableTrueCount(factRoles, delegate(RoleBase matchRoleBase)
										{
											ObjectType compareToRolePlayer = currentRole.Role.RolePlayer;
											return compareToRolePlayer != null && compareToRolePlayer == matchRoleBase.Role.RolePlayer;
										}) + 0), snippet1Replace1Replace1ReplaceFactRoleIter1);
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1Replace1ReplaceFactRoleIter1, 0], snippet1Replace1Replace1ReplaceFactRoleIter1);
								}
								roleReplacements[snippet1Replace1Replace1ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace1Replace1);
							snippet1Replace1Replace1ReplaceIsFirstPass1 = false;
						}
					}
					snippet1Replace1Replace1Replace1 = sbTemp.ToString();
					string snippet1Replace1Replace1Replace2 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace1Replace1ReplaceIsFirstPass2 = true;
					for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity && snippet1Replace1Replace1ReplaceIsFirstPass2; ++RoleIter2)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter2];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.None)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1Replace1ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1Replace1ReplaceFactRoleIter2];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(string.Format(writer.FormatProvider, allBasicRoleReplacements[0][factRoles.IndexOf(allConstraintRoles[allConstraintRoles.IndexOf(currentRole.Role) == 0 ? 1 : 0]), 3], ORMSolutions.ORMArchitect.Framework.Utility.EnumerableTrueCount(factRoles, delegate(RoleBase matchRoleBase)
										{
											ObjectType compareToRolePlayer = allConstraintRoles[allConstraintRoles.IndexOf(currentRole.Role) == 0 ? 1 : 0].Role.RolePlayer;
											return compareToRolePlayer != null && compareToRolePlayer == matchRoleBase.Role.RolePlayer;
										}) + 0), snippet1Replace1Replace1ReplaceFactRoleIter2);
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(string.Format(writer.FormatProvider, allBasicRoleReplacements[0][snippet1Replace1Replace1ReplaceFactRoleIter2, 3], ORMSolutions.ORMArchitect.Framework.Utility.EnumerableTrueCount(factRoles, delegate(RoleBase matchRoleBase)
										{
											ObjectType compareToRolePlayer = currentRole.Role.RolePlayer;
											return compareToRolePlayer != null && compareToRolePlayer == matchRoleBase.Role.RolePlayer;
										}) + 1), snippet1Replace1Replace1ReplaceFactRoleIter2);
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1Replace1ReplaceFactRoleIter2);
								}
								roleReplacements[snippet1Replace1Replace1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace1Replace2);
							snippet1Replace1Replace1ReplaceIsFirstPass2 = false;
						}
					}
					snippet1Replace1Replace1Replace2 = sbTemp.ToString();
					snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1, snippet1Replace1Replace1Replace2);
					string snippet1Replace1Replace2 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace1ReplaceIsFirstPass2 = true;
					for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity && snippet1Replace1ReplaceIsFirstPass2; ++RoleIter2)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter2];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.None)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2));
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(string.Format(writer.FormatProvider, allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 3], ORMSolutions.ORMArchitect.Framework.Utility.EnumerableTrueCount(factRoles, delegate(RoleBase matchRoleBase)
										{
											ObjectType compareToRolePlayer = currentRole.Role.RolePlayer;
											return compareToRolePlayer != null && compareToRolePlayer == matchRoleBase.Role.RolePlayer;
										}) + 1), snippet1Replace1ReplaceFactRoleIter2);
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(allBasicRoleReplacements[0][snippet1Replace1ReplaceFactRoleIter2, 0], snippet1Replace1ReplaceFactRoleIter2);
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace2);
							snippet1Replace1ReplaceIsFirstPass2 = false;
						}
					}
					snippet1Replace1Replace2 = sbTemp.ToString();
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (constraintRoleArity == 2 && allFactsCount == 1 && !isNegative)
				{
					parentFact = allFacts[0];
					predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
					allReadingOrders = parentFact.ReadingOrderCollection;
					verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					string snippet1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.ConditionalMultiLineIndented, isDeontic, isNegative);
					string snippet1Replace1Replace1 = null;
					string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(CoreVerbalizationSnippetType.LogicalAndOperator, isDeontic, isNegative);
					string snippet1Replace1Replace1Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace1Replace1ReplaceIsFirstPass1 = true;
					for (int RoleIter1 = 0; RoleIter1 < constraintRoleArity && snippet1Replace1Replace1ReplaceIsFirstPass1; ++RoleIter1)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter1];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1Replace1ReplaceFactRoleIter1 = 0; snippet1Replace1Replace1ReplaceFactRoleIter1 < factArity; ++snippet1Replace1Replace1ReplaceFactRoleIter1)
							{
								RoleBase currentRole = factRoles[snippet1Replace1Replace1ReplaceFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1Replace1ReplaceFactRoleIter1, allBasicRoleReplacements[0]), snippet1Replace1Replace1ReplaceFactRoleIter1);
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace1Replace1ReplaceFactRoleIter1] = roleReplacement;
							}
							snippet1Replace1Replace1Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace1Replace1);
							snippet1Replace1Replace1ReplaceIsFirstPass1 = false;
						}
					}
					snippet1Replace1Replace1Replace1 = sbTemp.ToString();
					string snippet1Replace1Replace1Replace2 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace1Replace1ReplaceIsFirstPass2 = true;
					for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity && snippet1Replace1Replace1ReplaceIsFirstPass2; ++RoleIter2)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter2];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1Replace1ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1Replace1ReplaceFactRoleIter2];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(factRoles.IndexOf(allConstraintRoles[allConstraintRoles.IndexOf(currentRole.Role) == 0 ? 1 : 0]), allBasicRoleReplacements[0]), snippet1Replace1Replace1ReplaceFactRoleIter2));
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(string.Format(writer.FormatProvider, allBasicRoleReplacements[0][factRoles.IndexOf(primaryRole), 3], ORMSolutions.ORMArchitect.Framework.Utility.EnumerableTrueCount(factRoles, delegate(RoleBase matchRoleBase)
										{
											ObjectType compareToRolePlayer = primaryRole.Role.RolePlayer;
											return compareToRolePlayer != null && compareToRolePlayer == matchRoleBase.Role.RolePlayer;
										}) + 1), snippet1Replace1Replace1ReplaceFactRoleIter2));
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1Replace1ReplaceFactRoleIter2));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1Replace1ReplaceFactRoleIter2);
								}
								roleReplacements[snippet1Replace1Replace1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace1Replace2);
							snippet1Replace1Replace1ReplaceIsFirstPass2 = false;
						}
					}
					snippet1Replace1Replace1Replace2 = sbTemp.ToString();
					snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1, snippet1Replace1Replace1Replace2);
					string snippet1Replace1Replace2 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					bool snippet1Replace1ReplaceIsFirstPass2 = true;
					for (int RoleIter2 = 0; RoleIter2 < constraintRoleArity && snippet1Replace1ReplaceIsFirstPass2; ++RoleIter2)
					{
						RoleBase primaryRole = allConstraintRoles[RoleIter2];
						if ((reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder)) != null)
						{
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							for (int snippet1Replace1ReplaceFactRoleIter2 = 0; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
							{
								RoleBase currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
								string roleReplacement = null;
								if (primaryRole == currentRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2));
								}
								else if (primaryRole != currentRole && allConstraintRoles.Contains(currentRole.Role))
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(string.Format(writer.FormatProvider, allBasicRoleReplacements[0][factRoles.IndexOf(primaryRole), 3], ORMSolutions.ORMArchitect.Framework.Utility.EnumerableTrueCount(factRoles, delegate(RoleBase matchRoleBase)
										{
											ObjectType compareToRolePlayer = primaryRole.Role.RolePlayer;
											return compareToRolePlayer != null && compareToRolePlayer == matchRoleBase.Role.RolePlayer;
										}) + 1), snippet1Replace1ReplaceFactRoleIter2));
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, isDeontic, isNegative), hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2));
								}
								if (roleReplacement == null)
								{
									roleReplacement = hyphenBinder.HyphenBindRoleReplacement(subscripter.GetSubscriptedName(snippet1Replace1ReplaceFactRoleIter2, allBasicRoleReplacements[0]), snippet1Replace1ReplaceFactRoleIter2);
								}
								roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
							}
							snippet1Replace1Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, roleReplacements, false);
							sbTemp.Append(snippet1Replace1Replace2);
							snippet1Replace1ReplaceIsFirstPass2 = false;
						}
					}
					snippet1Replace1Replace2 = sbTemp.ToString();
					snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
				{
					return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
				}
				#endregion // Pattern Matches
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
	}
	#endregion // RingConstraint.TransitiveRingVerbalizer verbalization
	#region EqualityConstraint verbalization
	public partial class EqualityConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			bool blockingErrors = false;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					blockingErrors = true;
					if (verbalizationContext.TestVerbalizedLocally(error))
					{
						continue;
					}
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
				}
				if (blockingErrors)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			bool isNegative = 0 != (sign & VerbalizationSign.Negative);
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
			IList<ConstraintRoleSequenceHasRole>[] allConstraintRoleSequences = new IList<ConstraintRoleSequenceHasRole>[constraintRoleArity];
			for (int i = 0; i < constraintRoleArity; ++i)
			{
				allConstraintRoleSequences[i] = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(constraintSequences[i]);
			}
			int columnArity = allConstraintRoleSequences[0].Count;
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
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
				}
				#endregion // Error report
				return false;
			}
			RolePathVerbalizer pathVerbalizer = RolePathVerbalizer.Create(this, new StandardRolePathRenderer(snippets, verbalizationContext, writer.FormatProvider));
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
							if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
							writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1ReplaceSequenceIter1];
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
						sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead | RolePathRolePlayerRenderingOptions.MinimizeHeadSubscripting | RolePathRolePlayerRenderingOptions.ResolveSupertype));
					}
					snippet1Replace1Replace1 = sbTemp.ToString();
				}
				string snippet1Replace1Replace2 = null;
				string snippet1Replace1ReplaceFormat2 = snippets.GetSnippet(CoreVerbalizationSnippetType.Equality, isDeontic, isNegative);
				string snippet1Replace1Replace2Replace1 = null;
				int snippet1Replace1Replace2ReplaceSequenceIter1;
				for (snippet1Replace1Replace2ReplaceSequenceIter1 = 0; snippet1Replace1Replace2ReplaceSequenceIter1 < 1; ++snippet1Replace1Replace2ReplaceSequenceIter1)
				{
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
					ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1].JoinPath;
					if (pathVerbalizer.HasPathVerbalization(joinPath))
					{
						pathVerbalizer.Options = RolePathVerbalizerOptions.MarkTrailingOutdentStart;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						snippet1Replace1Replace2Replace1 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
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
						for (int RoleIter1 = 0; RoleIter1 < 1; ++RoleIter1)
						{
							RoleBase primaryRole = includedConstraintRoles[RoleIter1].Role;
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							snippet1Replace1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
								{
									foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
									{
										if (replaceRole == constraintRole.Role)
										{
											return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
										}
									}
									return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
								});
							sbTemp.Append(snippet1Replace1Replace2Replace1);
						}
						snippet1Replace1Replace2Replace1 = sbTemp.ToString();
					}
				}
				string snippet1Replace1Replace2Replace2 = null;
				int snippet1Replace1Replace2ReplaceSequenceIter2;
				for (snippet1Replace1Replace2ReplaceSequenceIter2 = 1; snippet1Replace1Replace2ReplaceSequenceIter2 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceSequenceIter2)
				{
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace2ReplaceSequenceIter2];
					ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1Replace2ReplaceSequenceIter2].JoinPath;
					if (pathVerbalizer.HasPathVerbalization(joinPath))
					{
						pathVerbalizer.Options = RolePathVerbalizerOptions.MarkTrailingOutdentStart;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						snippet1Replace1Replace2Replace2 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
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
						for (int RoleIter2 = 0; RoleIter2 < 1; ++RoleIter2)
						{
							RoleBase primaryRole = includedConstraintRoles[RoleIter2].Role;
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							snippet1Replace1Replace2Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
								{
									foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
									{
										if (replaceRole == constraintRole.Role)
										{
											return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
										}
									}
									return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
								});
							sbTemp.Append(snippet1Replace1Replace2Replace2);
						}
						snippet1Replace1Replace2Replace2 = sbTemp.ToString();
					}
				}
				snippet1Replace1Replace2 = RolePathVerbalizer.FormatResolveOutdent(writer.FormatProvider, null, writer.NewLine, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1, snippet1Replace1Replace2Replace2);
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
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1ReplaceSequenceIter1];
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
						sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead | RolePathRolePlayerRenderingOptions.MinimizeHeadSubscripting | RolePathRolePlayerRenderingOptions.ResolveSupertype));
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
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
					ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1].JoinPath;
					if (pathVerbalizer.HasPathVerbalization(joinPath))
					{
						pathVerbalizer.Options = RolePathVerbalizerOptions.MarkTrailingOutdentStart;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						snippet1Replace1Replace2Replace1 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
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
						for (int RoleIter1 = 0; RoleIter1 < 1; ++RoleIter1)
						{
							RoleBase primaryRole = includedConstraintRoles[RoleIter1].Role;
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							snippet1Replace1Replace2Replace1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
								{
									foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
									{
										if (replaceRole == constraintRole.Role)
										{
											return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
										}
									}
									return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
								});
							sbTemp.Append(snippet1Replace1Replace2Replace1);
						}
						snippet1Replace1Replace2Replace1 = sbTemp.ToString();
					}
				}
				string snippet1Replace1Replace2Replace2 = null;
				int snippet1Replace1Replace2ReplaceSequenceIter2;
				for (snippet1Replace1Replace2ReplaceSequenceIter2 = 1; snippet1Replace1Replace2ReplaceSequenceIter2 < constraintRoleArity; ++snippet1Replace1Replace2ReplaceSequenceIter2)
				{
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace2ReplaceSequenceIter2];
					ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1Replace2ReplaceSequenceIter2].JoinPath;
					if (pathVerbalizer.HasPathVerbalization(joinPath))
					{
						pathVerbalizer.Options = RolePathVerbalizerOptions.MarkTrailingOutdentStart;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						snippet1Replace1Replace2Replace2 = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
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
						for (int RoleIter2 = 0; RoleIter2 < 1; ++RoleIter2)
						{
							RoleBase primaryRole = includedConstraintRoles[RoleIter2].Role;
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
							reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
							hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
							snippet1Replace1Replace2Replace2 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
								{
									foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
									{
										if (replaceRole == constraintRole.Role)
										{
											return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
										}
									}
									return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
								});
							sbTemp.Append(snippet1Replace1Replace2Replace2);
						}
						snippet1Replace1Replace2Replace2 = sbTemp.ToString();
					}
				}
				snippet1Replace1Replace2 = RolePathVerbalizer.FormatResolveOutdent(writer.FormatProvider, null, writer.NewLine, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1, snippet1Replace1Replace2Replace2);
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
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1ReplaceSequenceIter1];
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
						sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead | RolePathRolePlayerRenderingOptions.MinimizeHeadSubscripting | RolePathRolePlayerRenderingOptions.ResolveSupertype));
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
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
					ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1].JoinPath;
					if (pathVerbalizer.HasPathVerbalization(joinPath))
					{
						++snippet1Replace1Replace2ReplaceCompositeCount1;
					}
					else
					{
						for (snippet1Replace1Replace2ReplaceCompositeIterator1 = 0; snippet1Replace1Replace2ReplaceCompositeIterator1 < columnArity; ++snippet1Replace1Replace2ReplaceCompositeIterator1)
						{
							RoleBase primaryRole = includedConstraintRoles[snippet1Replace1Replace2ReplaceCompositeIterator1].Role;
							if (Array.IndexOf(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, snippet1Replace1Replace2Replace1ItemTestUniqueFactType1 = primaryRole.FactType) == -1)
							{
								snippet1Replace1Replace2Replace1ItemUniqueFactTypes1[snippet1Replace1Replace2ReplaceCompositeIterator1] = snippet1Replace1Replace2Replace1ItemTestUniqueFactType1;
								++snippet1Replace1Replace2ReplaceCompositeCount1;
							}
						}
					}
					Array.Clear(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, 0, snippet1Replace1Replace2Replace1ItemUniqueFactTypes1.Length);
				}
				snippet1Replace1Replace2ReplaceCompositeIterator1 = 0;
				string[] snippet1Replace1Replace2ReplaceCompositeFields1 = new string[snippet1Replace1Replace2ReplaceCompositeCount1];
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
					string snippet1Replace1Replace2Replace1Item1;
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
					ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1].JoinPath;
					if (pathVerbalizer.HasPathVerbalization(joinPath))
					{
						pathVerbalizer.Options = RolePathVerbalizerOptions.MarkTrailingOutdentStart;
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
						sbTemp.AppendFormat("{{{0}}}", snippet1Replace1Replace2ReplaceCompositeIterator1);
						snippet1Replace1Replace2ReplaceCompositeFields1[snippet1Replace1Replace2ReplaceCompositeIterator1] = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
						if (snippet1Replace1Replace2ReplaceCompositeIterator1 == snippet1Replace1Replace2ReplaceCompositeCount1 - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.MultilineIndentedCompoundListClose, isDeontic, isNegative));
						}
						++snippet1Replace1Replace2ReplaceCompositeIterator1;
					}
					else
					{
						for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
						{
							RoleBase primaryRole = includedConstraintRoles[RoleIter1].Role;
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
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
								sbTemp.AppendFormat("{{{0}}}", snippet1Replace1Replace2ReplaceCompositeIterator1);
								reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								snippet1Replace1Replace2Replace1Item1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
									{
										foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
										{
											if (replaceRole == constraintRole.Role)
											{
												return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
											}
										}
										return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
									});
								snippet1Replace1Replace2ReplaceCompositeFields1[snippet1Replace1Replace2ReplaceCompositeIterator1] = snippet1Replace1Replace2Replace1Item1;
								if (snippet1Replace1Replace2ReplaceCompositeIterator1 == snippet1Replace1Replace2ReplaceCompositeCount1 - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.MultilineIndentedCompoundListClose, isDeontic, isNegative));
								}
								++snippet1Replace1Replace2ReplaceCompositeIterator1;
							}
						}
					}
					Array.Clear(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, 0, snippet1Replace1Replace2Replace1ItemUniqueFactTypes1.Length);
				}
				string snippet1Replace1Replace2ReplaceFormat1 = sbTemp.ToString();
				sbTemp.Length = 0;
				RolePathVerbalizer.FormatResolveOutdent(writer.FormatProvider, sbTemp, writer.NewLine, snippet1Replace1Replace2ReplaceFormat1, snippet1Replace1Replace2ReplaceCompositeFields1);
				snippet1Replace1Replace2Replace1 = sbTemp.ToString();
				snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
			}
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
		}
	}
	#endregion // EqualityConstraint verbalization
	#region ExclusionConstraint verbalization
	public partial class ExclusionConstraint : IVerbalize
	{
		/// <summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			#region Preliminary
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			#region Prerequisite error check
			IModelErrorOwner errorOwner = this as IModelErrorOwner;
			bool firstErrorPending;
			bool blockingErrors = false;
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
				{
					blockingErrors = true;
					if (verbalizationContext.TestVerbalizedLocally(error))
					{
						continue;
					}
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
				}
				if (blockingErrors)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
					{
						ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
					return true;
				}
			}
			#endregion // Prerequisite error check
			bool isNegative = 0 != (sign & VerbalizationSign.Negative);
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
			IList<ConstraintRoleSequenceHasRole>[] allConstraintRoleSequences = new IList<ConstraintRoleSequenceHasRole>[constraintRoleArity];
			for (int i = 0; i < constraintRoleArity; ++i)
			{
				allConstraintRoleSequences[i] = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(constraintSequences[i]);
			}
			int columnArity = allConstraintRoleSequences[0].Count;
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
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
				}
				#endregion // Error report
				return false;
			}
			RolePathVerbalizer pathVerbalizer = RolePathVerbalizer.Create(this, new StandardRolePathRenderer(snippets, verbalizationContext, writer.FormatProvider));
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
							if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
							writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1ReplaceSequenceIter1];
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
						sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead | RolePathRolePlayerRenderingOptions.MinimizeHeadSubscripting | RolePathRolePlayerRenderingOptions.ResolveSupertype));
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
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
					ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1].JoinPath;
					if (pathVerbalizer.HasPathVerbalization(joinPath))
					{
						++snippet1Replace1Replace2ReplaceCompositeCount1;
					}
					else
					{
						for (snippet1Replace1Replace2ReplaceCompositeIterator1 = 0; snippet1Replace1Replace2ReplaceCompositeIterator1 < columnArity; ++snippet1Replace1Replace2ReplaceCompositeIterator1)
						{
							RoleBase primaryRole = includedConstraintRoles[snippet1Replace1Replace2ReplaceCompositeIterator1].Role;
							if (Array.IndexOf(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, snippet1Replace1Replace2Replace1ItemTestUniqueFactType1 = primaryRole.FactType) == -1)
							{
								snippet1Replace1Replace2Replace1ItemUniqueFactTypes1[snippet1Replace1Replace2ReplaceCompositeIterator1] = snippet1Replace1Replace2Replace1ItemTestUniqueFactType1;
								++snippet1Replace1Replace2ReplaceCompositeCount1;
							}
						}
					}
					Array.Clear(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, 0, snippet1Replace1Replace2Replace1ItemUniqueFactTypes1.Length);
				}
				snippet1Replace1Replace2ReplaceCompositeIterator1 = 0;
				string[] snippet1Replace1Replace2ReplaceCompositeFields1 = new string[snippet1Replace1Replace2ReplaceCompositeCount1];
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
					string snippet1Replace1Replace2Replace1Item1;
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
					ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1].JoinPath;
					if (pathVerbalizer.HasPathVerbalization(joinPath))
					{
						pathVerbalizer.Options = RolePathVerbalizerOptions.MarkTrailingOutdentStart;
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
						sbTemp.AppendFormat("{{{0}}}", snippet1Replace1Replace2ReplaceCompositeIterator1);
						snippet1Replace1Replace2ReplaceCompositeFields1[snippet1Replace1Replace2ReplaceCompositeIterator1] = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
						if (snippet1Replace1Replace2ReplaceCompositeIterator1 == snippet1Replace1Replace2ReplaceCompositeCount1 - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.MultilineIndentedCompoundListClose, isDeontic, isNegative));
						}
						++snippet1Replace1Replace2ReplaceCompositeIterator1;
					}
					else
					{
						for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
						{
							RoleBase primaryRole = includedConstraintRoles[RoleIter1].Role;
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
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
								sbTemp.AppendFormat("{{{0}}}", snippet1Replace1Replace2ReplaceCompositeIterator1);
								reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								snippet1Replace1Replace2Replace1Item1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
									{
										foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
										{
											if (replaceRole == constraintRole.Role)
											{
												return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
											}
										}
										return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
									});
								snippet1Replace1Replace2ReplaceCompositeFields1[snippet1Replace1Replace2ReplaceCompositeIterator1] = snippet1Replace1Replace2Replace1Item1;
								if (snippet1Replace1Replace2ReplaceCompositeIterator1 == snippet1Replace1Replace2ReplaceCompositeCount1 - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.MultilineIndentedCompoundListClose, isDeontic, isNegative));
								}
								++snippet1Replace1Replace2ReplaceCompositeIterator1;
							}
						}
					}
					Array.Clear(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, 0, snippet1Replace1Replace2Replace1ItemUniqueFactTypes1.Length);
				}
				string snippet1Replace1Replace2ReplaceFormat1 = sbTemp.ToString();
				sbTemp.Length = 0;
				RolePathVerbalizer.FormatResolveOutdent(writer.FormatProvider, sbTemp, writer.NewLine, snippet1Replace1Replace2ReplaceFormat1, snippet1Replace1Replace2ReplaceCompositeFields1);
				snippet1Replace1Replace2Replace1 = sbTemp.ToString();
				snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (allFactsCount == 2 && columnArity == 2 && constraintRoleArity == 2 && !isNegative)
			{
				// Check for an optimized verbalization form if we have simple infix forms
				// with no hyphen binding on both fact types and the role players have the same type.
				int binaryLeadReadingColumnIndex = -1;
				for (int testColumnIndex = 0; testColumnIndex < 2; ++testColumnIndex)
				{
					Role firstLeadRole = allConstraintRoleSequences[0][testColumnIndex].Role;
					parentFact = firstLeadRole.FactType;
					if (parentFact.GetMatchingReading(parentFact.ReadingOrderCollection, null, firstLeadRole, null, null, MatchingReadingOptions.NoFrontText | MatchingReadingOptions.NoTrailingText | MatchingReadingOptions.NotHyphenBound) != null)
					{
						Role secondLeadRole = allConstraintRoleSequences[1][testColumnIndex].Role;
						if (firstLeadRole.RolePlayer != secondLeadRole.RolePlayer)
						{
							break;
						}
						parentFact = secondLeadRole.FactType;
						if (parentFact.GetMatchingReading(parentFact.ReadingOrderCollection, null, secondLeadRole, null, null, MatchingReadingOptions.NoFrontText | MatchingReadingOptions.NoTrailingText | MatchingReadingOptions.NotHyphenBound) != null)
						{
							int otherColumnIndex = (testColumnIndex + 1) % 2;
							if (allConstraintRoleSequences[0][otherColumnIndex].Role.RolePlayer != allConstraintRoleSequences[1][otherColumnIndex].Role.RolePlayer)
							{
								break;
							}
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
						IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace1ReplaceSequenceIter1];
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
							sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter1], null, RolePathRolePlayerRenderingOptions.None));
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
						string snippet1Replace1Replace1Replace2Item1;
						IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace1ReplaceSequenceIter2];
						for (int RoleIter1 = 0; RoleIter1 < 1; ++RoleIter1)
						{
							RoleBase primaryRole = includedConstraintRoles[RoleIter1].Role;
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
							reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.NoFrontText | MatchingReadingOptions.NoTrailingText | MatchingReadingOptions.NotHyphenBound | MatchingReadingOptions.AllowAnyOrder);
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
						IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace1ReplaceSequenceIter1];
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
							sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter1], null, RolePathRolePlayerRenderingOptions.None));
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
						string snippet1Replace1Replace1Replace2Item1;
						IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace1ReplaceSequenceIter2];
						for (int RoleIter1 = 1; RoleIter1 < columnArity; ++RoleIter1)
						{
							RoleBase primaryRole = includedConstraintRoles[RoleIter1].Role;
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
							reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.NoFrontText | MatchingReadingOptions.NoTrailingText | MatchingReadingOptions.NotHyphenBound | MatchingReadingOptions.AllowAnyOrder);
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
						IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1ReplaceSequenceIter1];
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
							sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead | RolePathRolePlayerRenderingOptions.MinimizeHeadSubscripting | RolePathRolePlayerRenderingOptions.ResolveSupertype));
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
						IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1ReplaceSequenceIter2];
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
							sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter2], null, RolePathRolePlayerRenderingOptions.None));
						}
						snippet1Replace1Replace2 = sbTemp.ToString();
					}
				}
				else if (binaryLeadReadingColumnIndex == 1)
				{
					int snippet1Replace1ReplaceSequenceIter2;
					for (snippet1Replace1ReplaceSequenceIter2 = 0; snippet1Replace1ReplaceSequenceIter2 < 1; ++snippet1Replace1ReplaceSequenceIter2)
					{
						IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1ReplaceSequenceIter2];
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
							sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter2], null, RolePathRolePlayerRenderingOptions.None));
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
						IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
						ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1].JoinPath;
						if (pathVerbalizer.HasPathVerbalization(joinPath))
						{
							++snippet1Replace1Replace2ReplaceCompositeCount1;
						}
						else
						{
							for (snippet1Replace1Replace2ReplaceCompositeIterator1 = 0; snippet1Replace1Replace2ReplaceCompositeIterator1 < columnArity; ++snippet1Replace1Replace2ReplaceCompositeIterator1)
							{
								RoleBase primaryRole = includedConstraintRoles[snippet1Replace1Replace2ReplaceCompositeIterator1].Role;
								if (Array.IndexOf(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, snippet1Replace1Replace2Replace1ItemTestUniqueFactType1 = primaryRole.FactType) == -1)
								{
									snippet1Replace1Replace2Replace1ItemUniqueFactTypes1[snippet1Replace1Replace2ReplaceCompositeIterator1] = snippet1Replace1Replace2Replace1ItemTestUniqueFactType1;
									++snippet1Replace1Replace2ReplaceCompositeCount1;
								}
							}
						}
						Array.Clear(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, 0, snippet1Replace1Replace2Replace1ItemUniqueFactTypes1.Length);
					}
					snippet1Replace1Replace2ReplaceCompositeIterator1 = 0;
					string[] snippet1Replace1Replace2ReplaceCompositeFields1 = new string[snippet1Replace1Replace2ReplaceCompositeCount1];
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
						string snippet1Replace1Replace2Replace1Item1;
						IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
						ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1].JoinPath;
						if (pathVerbalizer.HasPathVerbalization(joinPath))
						{
							pathVerbalizer.Options = RolePathVerbalizerOptions.MarkTrailingOutdentStart;
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
							sbTemp.AppendFormat("{{{0}}}", snippet1Replace1Replace2ReplaceCompositeIterator1);
							snippet1Replace1Replace2ReplaceCompositeFields1[snippet1Replace1Replace2ReplaceCompositeIterator1] = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
							if (snippet1Replace1Replace2ReplaceCompositeIterator1 == snippet1Replace1Replace2ReplaceCompositeCount1 - 1)
							{
								sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.MultilineIndentedCompoundListClose, isDeontic, isNegative));
							}
							++snippet1Replace1Replace2ReplaceCompositeIterator1;
						}
						else
						{
							for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
							{
								RoleBase primaryRole = includedConstraintRoles[RoleIter1].Role;
								parentFact = primaryRole.FactType;
								predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
								allReadingOrders = parentFact.ReadingOrderCollection;
								factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
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
									sbTemp.AppendFormat("{{{0}}}", snippet1Replace1Replace2ReplaceCompositeIterator1);
									reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
									hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
									snippet1Replace1Replace2Replace1Item1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
										{
											foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
											{
												if (replaceRole == constraintRole.Role)
												{
													return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
												}
											}
											return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
										});
									snippet1Replace1Replace2ReplaceCompositeFields1[snippet1Replace1Replace2ReplaceCompositeIterator1] = snippet1Replace1Replace2Replace1Item1;
									if (snippet1Replace1Replace2ReplaceCompositeIterator1 == snippet1Replace1Replace2ReplaceCompositeCount1 - 1)
									{
										sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.MultilineIndentedCompoundListClose, isDeontic, isNegative));
									}
									++snippet1Replace1Replace2ReplaceCompositeIterator1;
								}
							}
						}
						Array.Clear(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, 0, snippet1Replace1Replace2Replace1ItemUniqueFactTypes1.Length);
					}
					string snippet1Replace1Replace2ReplaceFormat1 = sbTemp.ToString();
					sbTemp.Length = 0;
					RolePathVerbalizer.FormatResolveOutdent(writer.FormatProvider, sbTemp, writer.NewLine, snippet1Replace1Replace2ReplaceFormat1, snippet1Replace1Replace2ReplaceCompositeFields1);
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
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1ReplaceSequenceIter1];
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
						sbTemp.Append(pathVerbalizer.RenderAssociatedRolePlayer(includedConstraintRoles[RoleIter1], null, RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead | RolePathRolePlayerRenderingOptions.MinimizeHeadSubscripting | RolePathRolePlayerRenderingOptions.ResolveSupertype));
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
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
					ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1].JoinPath;
					if (pathVerbalizer.HasPathVerbalization(joinPath))
					{
						++snippet1Replace1Replace2ReplaceCompositeCount1;
					}
					else
					{
						for (snippet1Replace1Replace2ReplaceCompositeIterator1 = 0; snippet1Replace1Replace2ReplaceCompositeIterator1 < columnArity; ++snippet1Replace1Replace2ReplaceCompositeIterator1)
						{
							RoleBase primaryRole = includedConstraintRoles[snippet1Replace1Replace2ReplaceCompositeIterator1].Role;
							if (Array.IndexOf(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, snippet1Replace1Replace2Replace1ItemTestUniqueFactType1 = primaryRole.FactType) == -1)
							{
								snippet1Replace1Replace2Replace1ItemUniqueFactTypes1[snippet1Replace1Replace2ReplaceCompositeIterator1] = snippet1Replace1Replace2Replace1ItemTestUniqueFactType1;
								++snippet1Replace1Replace2ReplaceCompositeCount1;
							}
						}
					}
					Array.Clear(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, 0, snippet1Replace1Replace2Replace1ItemUniqueFactTypes1.Length);
				}
				snippet1Replace1Replace2ReplaceCompositeIterator1 = 0;
				string[] snippet1Replace1Replace2ReplaceCompositeFields1 = new string[snippet1Replace1Replace2ReplaceCompositeCount1];
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
					string snippet1Replace1Replace2Replace1Item1;
					IList<ConstraintRoleSequenceHasRole> includedConstraintRoles = allConstraintRoleSequences[snippet1Replace1Replace2ReplaceSequenceIter1];
					ConstraintRoleSequenceJoinPath joinPath = constraintSequences[snippet1Replace1Replace2ReplaceSequenceIter1].JoinPath;
					if (pathVerbalizer.HasPathVerbalization(joinPath))
					{
						pathVerbalizer.Options = RolePathVerbalizerOptions.MarkTrailingOutdentStart;
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
						sbTemp.AppendFormat("{{{0}}}", snippet1Replace1Replace2ReplaceCompositeIterator1);
						snippet1Replace1Replace2ReplaceCompositeFields1[snippet1Replace1Replace2ReplaceCompositeIterator1] = pathVerbalizer.RenderPathVerbalization(joinPath, sbTemp);
						if (snippet1Replace1Replace2ReplaceCompositeIterator1 == snippet1Replace1Replace2ReplaceCompositeCount1 - 1)
						{
							sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.MultilineIndentedCompoundListClose, isDeontic, isNegative));
						}
						++snippet1Replace1Replace2ReplaceCompositeIterator1;
					}
					else
					{
						for (int RoleIter1 = 0; RoleIter1 < columnArity; ++RoleIter1)
						{
							RoleBase primaryRole = includedConstraintRoles[RoleIter1].Role;
							parentFact = primaryRole.FactType;
							predicatePartFormatString = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart, isDeontic, isNegative), parentFact.Name, parentFact.Id.ToString("D"));
							allReadingOrders = parentFact.ReadingOrderCollection;
							factRoles = allReadingOrders.Count != 0 ? allReadingOrders[0].RoleCollection : parentFact.RoleCollection;
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
								sbTemp.AppendFormat("{{{0}}}", snippet1Replace1Replace2ReplaceCompositeIterator1);
								reading = parentFact.GetMatchingReading(allReadingOrders, null, primaryRole, null, factRoles, MatchingReadingOptions.AllowAnyOrder);
								hyphenBinder = new VerbalizationHyphenBinder(reading, writer.FormatProvider, factRoles, unaryRoleIndex, snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart, isDeontic, isNegative), predicatePartFormatString);
								snippet1Replace1Replace2Replace1Item1 = hyphenBinder.PopulatePredicateText(reading, writer.FormatProvider, predicatePartFormatString, factRoles, delegate(RoleBase replaceRole, string hyphenBindingFormatString)
									{
										foreach (ConstraintRoleSequenceHasRole constraintRole in includedConstraintRoles)
										{
											if (replaceRole == constraintRole.Role)
											{
												return pathVerbalizer.RenderAssociatedRolePlayer(constraintRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
											}
										}
										return pathVerbalizer.RenderAssociatedRolePlayer(replaceRole, hyphenBindingFormatString, RolePathRolePlayerRenderingOptions.Quantify);
									});
								snippet1Replace1Replace2ReplaceCompositeFields1[snippet1Replace1Replace2ReplaceCompositeIterator1] = snippet1Replace1Replace2Replace1Item1;
								if (snippet1Replace1Replace2ReplaceCompositeIterator1 == snippet1Replace1Replace2ReplaceCompositeCount1 - 1)
								{
									sbTemp.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.MultilineIndentedCompoundListClose, isDeontic, isNegative));
								}
								++snippet1Replace1Replace2ReplaceCompositeIterator1;
							}
						}
					}
					Array.Clear(snippet1Replace1Replace2Replace1ItemUniqueFactTypes1, 0, snippet1Replace1Replace2Replace1ItemUniqueFactTypes1.Length);
				}
				string snippet1Replace1Replace2ReplaceFormat1 = sbTemp.ToString();
				sbTemp.Length = 0;
				RolePathVerbalizer.FormatResolveOutdent(writer.FormatProvider, sbTemp, writer.NewLine, snippet1Replace1Replace2ReplaceFormat1, snippet1Replace1Replace2ReplaceCompositeFields1);
				snippet1Replace1Replace2Replace1 = sbTemp.ToString();
				snippet1Replace1Replace2 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat2, snippet1Replace1Replace2Replace1);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(CoreVerbalizationSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (0 != (sign & VerbalizationSign.AttemptOppositeSign))
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative ? VerbalizationSign.Positive : VerbalizationSign.Negative);
			}
			#endregion // Pattern Matches
			#region Error report
			if (errorOwner != null)
			{
				firstErrorPending = true;
				foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
				{
					ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
					if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
					writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
				}
				if (!firstErrorPending)
				{
					writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
				}
			}
			#endregion // Error report
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
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
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.FactTypeInstanceBlockStart, false, false));
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
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
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
				writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.FactTypeInstanceBlockEnd, false, false));
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
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
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				#region Prerequisite error check
				IModelErrorOwner errorOwner = this as IModelErrorOwner;
				bool firstErrorPending;
				bool blockingErrors = false;
				if (errorOwner != null)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
					{
						blockingErrors = true;
						if (verbalizationContext.TestVerbalizedLocally(error))
						{
							continue;
						}
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					}
					if (blockingErrors)
					{
						firstErrorPending = true;
						foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
						{
							ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
							if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
							writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType, isDeontic, isNegative), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer, verbalizationContext.VerbalizationOptions), rolePlayer.Id.ToString("D"));
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
				reading = parentFact.GetMatchingReading(allReadingOrders, null, factRoles[0], null, factRoles, MatchingReadingOptions.AllowAnyOrder);
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
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
				}
				#endregion // Error report
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
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
			protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				#region Preliminary
				bool isNegative = 0 != (sign & VerbalizationSign.Negative);
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
				#region Prerequisite error check
				IModelErrorOwner errorOwner = this as IModelErrorOwner;
				bool firstErrorPending;
				bool blockingErrors = false;
				if (errorOwner != null)
				{
					firstErrorPending = true;
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.BlockVerbalization))
					{
						blockingErrors = true;
						if (verbalizationContext.TestVerbalizedLocally(error))
						{
							continue;
						}
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorPrimary, false, false), error.ErrorText, error.Id.ToString("D")));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorClosePrimaryReport, false, false));
					}
					if (blockingErrors)
					{
						firstErrorPending = true;
						foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.Verbalize))
						{
							ModelErrorDisplayFilter errorDisplayFilter = error.Model.ModelErrorDisplayFilter;
							if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
							writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
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
						if (errorDisplayFilter != null && !errorDisplayFilter.ShouldDisplay(error) || verbalizationContext.TestVerbalizedLocally(error))
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
						writer.Write(string.Format(writer.FormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorSecondary, false, false), error.ErrorText, error.Id.ToString("D")));
					}
					if (!firstErrorPending)
					{
						writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.ErrorCloseSecondaryReport, false, false));
					}
				}
				#endregion // Error report
				return true;
			}
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				return this.GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
			}
		}
		#endregion // ObjectType Instance Verbalization
	}
	#endregion // ObjectType.ObjectTypeInstanceVerbalizer verbalization
}
