#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using ORMSolutions.ORMArchitect.Framework.Design;
using System.ComponentModel;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region Element Type question
	/// <summary>
	/// element type enum question answers
	/// </summary>
	[TypeConverter(typeof(EnumConverter<SurveyElementType, ORMModel>))]
	public enum SurveyElementType
	{
		/// <summary>
		/// ORM element Object Type
		/// </summary>
		ObjectType,
		/// <summary>
		/// ORM element Fact Type
		/// </summary>
		FactType,
		/// <summary>
		/// ORM element Constraint
		/// </summary>
		ExternalConstraint,
		/// <summary>
		/// Grouping facility
		/// </summary>
		Grouping,
		/// <summary>
		/// Name generation settings
		/// </summary>
		NameGenerator,
	}
	#endregion // Element Type question
	#region FactType Detail Question
	/// <summary>
	/// FactType detail survey answers
	/// </summary>
	[TypeConverter(typeof(EnumConverter<SurveyFactTypeDetailType, ORMModel>))]
	public enum SurveyFactTypeDetailType
	{
		/// <summary>
		/// A Role node
		/// </summary>
		Role,
		/// <summary>
		/// An internal constraint node
		/// </summary>
		InternalConstraint,
		/// <summary>
		/// A list of implicit fact types
		/// </summary>
		ImpliedFactType,
	}
	#endregion // FactType Detail Question
	#region Error State question
	/// <summary>
	/// ErrorState survey answers
	/// </summary>
	public enum SurveyErrorState
	{
		/// <summary>
		/// ORM element does not contain an error
		/// </summary>
		NoError,
		/// <summary>
		/// ORM element contains error
		/// </summary>
		HasError,
	}
	#endregion // Error State question
	#region  Role Type question
	/// <summary>
	/// Role type survey answers
	/// </summary>
	public enum SurveyRoleType
	{
		/// <summary>
		/// The role is a <see cref="SubtypeMetaRole"/>
		/// </summary>
		Subtype,
		/// <summary>
		/// The role is a <see cref="SupertypeMetaRole"/>
		/// </summary>
		Supertype,
	}
	#endregion // Role Type question
	#region Survey Glyph questions
	/// <summary>
	/// Glyph type enum question answers
	/// </summary>
	public enum SurveyQuestionGlyph
	{
		/// <summary>
		/// Value type object
		/// </summary>
		ValueType,
		/// <summary>
		/// Entity type object
		/// </summary>
		EntityType,
		/// <summary>
		/// ObjectType should be set, but is not. Use
		/// if an object type main image is expected.
		/// </summary>
		ObjectTypeNotSet,
		/// <summary>
		/// Unary fact type
		/// </summary>
		UnaryFactType,
		/// <summary>
		/// Binary fact type
		/// </summary>
		BinaryFactType,
		/// <summary>
		/// Ternary fact type
		/// </summary>
		TernaryFactType,
		/// <summary>
		/// Nary fact type
		/// </summary>
		NaryFactType,
		/// <summary>
		/// Objectified fact type
		/// </summary>
		ObjectifiedFactType,
		/// <summary>
		/// Internal uniqueness constraint
		/// </summary>
		InternalUniquenessConstraint,
		/// <summary>
		/// External uniqueness constraint
		/// </summary>
		ExternalUniquenessConstraint,
		/// <summary>
		/// preferred uniqueness constraint
		/// </summary>
		ExternalUniquenessConstraintIsPreferred,
		/// <summary>
		/// Exclusion constraint
		/// </summary>
		ExclusionConstraint,
		/// <summary>
		/// ExclusionOR constraint
		/// </summary>
		ExclusiveOrConstraint,
		/// <summary>
		/// Disjunctive mandatory constraint
		/// </summary>
		DisjunctiveMandatoryConstraint,
		/// <summary>
		/// Equality Constraint
		/// </summary>
		EqualityConstraint,
		/// <summary>
		/// Frequency constraint
		/// </summary>
		FrequencyConstraint,
		/// <summary>
		/// Subset constraint
		/// </summary>
		SubsetConstraint,
		/// <summary>
		/// Simple mandatory constraint
		/// </summary>
		SimpleMandatoryConstraint,
		/// <summary>
		/// undefined ring constraint
		/// </summary>
		RingUndefined,
		/// <summary>
		/// Acyclic ring 
		/// </summary>
		RingAcyclic,
		/// <summary>
		///Acyclic and Intransitive
		/// </summary>
		RingAcyclicIntransitive,
		/// <summary>
		///Acyclic StronglyIntransitive
		/// </summary>
		RingAcyclicStronglyIntransitive,
		/// <summary>
		///Acyclic Transitive
		/// </summary>
		RingAcyclicTransitive,
		/// <summary>
		///Antisymmetric
		/// </summary>
		RingAntisymmetric,
		/// <summary>
		///Asymmetric
		/// </summary>
		RingAsymmetric,
		/// <summary>
		///Asymmetric and Intransitive
		/// </summary>
		RingAsymmetricIntransitive,
		/// <summary>
		///Asymmetric StronglyIntransitive
		/// </summary>
		RingAsymmetricStronglyIntransitive,
		/// <summary>
		///Intransitive
		/// </summary>
		RingIntransitive,
		/// <summary>
		///Irreflexive
		/// </summary>
		RingIrreflexive,
		/// <summary>
		///Purely Reflexive
		/// </summary>
		RingPurelyReflexive,
		/// <summary>
		///Reflexive
		/// </summary>
		RingReflexive,
		/// <summary>
		///Reflexive Antisymmetric
		/// </summary>
		RingReflexiveAntisymmetric,
		/// <summary>
		///Reflexive Symmetric
		/// </summary>
		RingReflexiveSymmetric,
		/// <summary>
		///Reflexive Transitive
		/// </summary>
		RingReflexiveTransitive,
		/// <summary>
		///Reflexive Transitive Antisymmetric
		/// </summary>
		RingReflexiveTransitiveAntisymmetric,
		/// <summary>
		///Strongly Intransitive
		/// </summary>
		RingStronglyIntransitive,
		/// <summary>
		///Symmetric
		/// </summary>
		RingSymmetric,
		/// <summary>
		///Symmetric and Intransitive
		/// </summary>
		RingSymmetricIntransitive,
		/// <summary>
		///Symmetric and Irreflexive
		/// </summary>
		RingSymmetricIrreflexive,
		/// <summary>
		///Symmetric StronglyIntransitive
		/// </summary>
		RingSymmetricStronglyIntransitive,
		/// <summary>
		///Symmetric Transitive
		/// </summary>
		RingSymmetricTransitive,
		/// <summary>
		///Transitive
		/// </summary>
		RingTransitive,
		/// <summary>
		///Transitive Antisymmetric
		/// </summary>
		RingTransitiveAntisymmetric,
		/// <summary>
		///Transitive Asymmetric
		/// </summary>
		RingTransitiveAsymmetric,
		/// <summary>
		///Transitive Irreflexive
		/// </summary>
		RingTransitiveIrreflexive,
		/// <summary>
		/// ValueComparison Undefined
		/// </summary>
		ValueComparisonUndefined,
		/// <summary>
		/// ValueComparison Equal
		/// </summary>
		ValueComparisonEqual,
		/// <summary>
		/// ValueComparison Not Equal
		/// </summary>
		ValueComparisonNotEqual,
		/// <summary>
		/// ValueComparison Less Than
		/// </summary>
		ValueComparisonLessThan,
		/// <summary>
		/// ValueComparison Greater Than
		/// </summary>
		ValueComparisonGreaterThan,
		/// <summary>
		/// ValueComparison Less Than Or Equal
		/// </summary>
		ValueComparisonLessThanOrEqual,
		/// <summary>
		/// ValueComparison Greater Than Or Equal
		/// </summary>
		ValueComparisonGreaterThanOrEqual,
		/// <summary>
		/// Internal uniqueness constraint
		/// </summary>
		InternalUniquenessConstraintDeontic,
		/// <summary>
		/// External uniqueness constraint deontic
		/// </summary>
		ExternalUniquenessConstraintDeontic,
		/// <summary>
		/// preferred uniqueness constraint deontic 
		/// </summary>
		ExternalUniquenessConstraintIsPreferredDeontic,
		/// <summary>
		/// Exclusion constraint deontic
		/// </summary>
		ExclusionConstraintDeontic,
		/// <summary>
		/// ExclusionOR constraint deontic
		/// </summary>
		ExclusiveOrConstraintDeontic,
		/// <summary>
		/// Disjunctive mandatory constraint deontic
		/// </summary>
		DisjunctiveMandatoryConstraintDeontic,
		/// <summary>
		/// Equality Constraint deontic
		/// </summary>
		EqualityConstraintDeontic,
		/// <summary>
		/// Frequency constraint deontic
		/// </summary>
		FrequencyConstraintDeontic,
		/// <summary>
		/// Subset constraint deontic
		/// </summary>
		SubsetConstraintDeontic,
		/// <summary>
		/// Simple mandatory constraint deontic
		/// </summary>
		SimpleMandatoryConstraintDeontic,
		/// <summary>
		/// undefined ring constraint deontic
		/// </summary>
		RingUndefinedDeontic,
		/// <summary>
		/// Acyclic ring deontic
		/// </summary>
		RingAcyclicDeontic,
		/// <summary>
		///Acyclic and Intransitive deontic
		/// </summary>
		RingAcyclicIntransitiveDeontic,
		/// <summary>
		///Acyclic StronglyIntransitive deontic
		/// </summary>
		RingAcyclicStronglyIntransitiveDeontic,
		/// <summary>
		///Acyclic Transitive deontic
		/// </summary>
		RingAcyclicTransitiveDeontic,
		/// <summary>
		///Antisymmetric deontic
		/// </summary>
		RingAntisymmetricDeontic,
		/// <summary>
		///Asymmetric deontic
		/// </summary>
		RingAsymmetricDeontic,
		/// <summary>
		///Asymmetric and Intransitive deontic
		/// </summary>
		RingAsymmetricIntransitiveDeontic,
		/// <summary>
		///Asymmetric StronglyIntransitive deontic
		/// </summary>
		RingAsymmetricStronglyIntransitiveDeontic,
		/// <summary>
		///Intransitive deontic
		/// </summary>
		RingIntransitiveDeontic,
		/// <summary>
		///Irreflexive deontic
		/// </summary>
		RingIrreflexiveDeontic,
		/// <summary>
		///Purely Reflexive deontic
		/// </summary>
		RingPurelyReflexiveDeontic,
		/// <summary>
		///Reflexive deontic
		/// </summary>
		RingReflexiveDeontic,
		/// <summary>
		///Reflexive Antisymmetric deontic
		/// </summary>
		RingReflexiveAntisymmetricDeontic,
		/// <summary>
		///Reflexive Symmetric deontic
		/// </summary>
		RingReflexiveSymmetricDeontic,
		/// <summary>
		///Reflexive Transitive deontic
		/// </summary>
		RingReflexiveTransitiveDeontic,
		/// <summary>
		///Reflexive Transitive Antisymmetric deontic
		/// </summary>
		RingReflexiveTransitiveAntisymmetricDeontic,
		/// <summary>
		///Strongly Intransitive deontic
		/// </summary>
		RingStronglyIntransitiveDeontic,
		/// <summary>
		///Symmetric deontic
		/// </summary>
		RingSymmetricDeontic,
		/// <summary>
		///Symmetric and Intransitive deontic
		/// </summary>
		RingSymmetricIntransitiveDeontic,
		/// <summary>
		///Symmetric and Irreflexive deontic
		/// </summary>
		RingSymmetricIrreflexiveDeontic,
		/// <summary>
		///Symmetric StronglyIntransitive deontic
		/// </summary>
		RingSymmetricStronglyIntransitiveDeontic,
		/// <summary>
		///Symmetric Transitive deontic
		/// </summary>
		RingSymmetricTransitiveDeontic,
		/// <summary>
		///Transitive deontic
		/// </summary>
		RingTransitiveDeontic,
		/// <summary>
		///Transitive Antisymmetric deontic
		/// </summary>
		RingTransitiveAntisymmetricDeontic,
		/// <summary>
		///Transitive Asymmetric deontic
		/// </summary>
		RingTransitiveAsymmetricDeontic,
		/// <summary>
		///Transitive Irreflexive deontic
		/// </summary>
		RingTransitiveIrreflexiveDeontic,
		/// <summary>
		/// ValueComparison Undefined Deontic
		/// </summary>
		ValueComparisonUndefinedDeontic,
		/// <summary>
		/// ValueComparison Equal Deontic
		/// </summary>
		ValueComparisonEqualDeontic,
		/// <summary>
		/// ValueComparison Not Equal Deontic
		/// </summary>
		ValueComparisonNotEqualDeontic,
		/// <summary>
		/// ValueComparison Less Than Deontic
		/// </summary>
		ValueComparisonLessThanDeontic,
		/// <summary>
		/// ValueComparison Greater Than Deontic
		/// </summary>
		ValueComparisonGreaterThanDeontic,
		/// <summary>
		/// ValueComparison Less Than Or Equal Deontic
		/// </summary>
		ValueComparisonLessThanOrEqualDeontic,
		/// <summary>
		/// ValueComparison Greater Than Or Equal Deontic
		/// </summary>
		ValueComparisonGreaterThanOrEqualDeontic,
		/// <summary>
		/// Primary Subtype Relationship
		/// </summary>
		PrimarySubtypeRelationship,
		/// <summary>
		/// Secondary SubType Relationship
		/// </summary>
		SecondarySubtypeRelationship,
		/// <summary>
		/// Grouping element
		/// </summary>
		Grouping,
		/// <summary>
		/// A note element
		/// </summary>
		Note,
		/// <summary>
		/// A role value constraint element
		/// </summary>
		RoleValueConstraint,
		/// <summary>
		/// A ValueType ValueConstraint element
		/// </summary>
		ValueTypeValueConstraint,
		/// <summary>
		/// The last glyph
		/// </summary>
		Last = ValueTypeValueConstraint,
	}
	#endregion // Survey Glyph questions
	#region NameGeneratorRefinement Question
	/// <summary>
	/// Determine if a <see cref="NameConsumer"/> refinement is
	/// a usage or type refinement.
	/// </summary>
	public enum SurveyNameGeneratorRefinementType
	{
		/// <summary>
		/// The refinement represents a different usage of the same type.
		/// These are marked with the <see cref="NameUsageAttribute"/> and
		/// represent different usages of the same refinement level. For example,
		/// relational usages are Table and Column.
		/// </summary>
		UsageRefinement,
		/// <summary>
		/// The refinement represents a more specific refinement of the same type.
		/// These correspond to more-derived generation elements of the context name generator.
		/// For example, 'SQL Server' would be a type refinement of 'Relational'.
		/// </summary>
		TypeRefinement,
	}
	#endregion // NameGeneratorRefinement Question
	#region SurveyGroupingChildType question
	/// <summary>
	/// The type of child elements for a survey grouping
	/// </summary>
	public enum SurveyGroupingChildType
	{
		/// <summary>
		/// The node corresponds to a <see cref="GroupingType"/>
		/// </summary>
		GroupingType,
		/// <summary>
		/// The node corresponds to a nested group
		/// </summary>
		NestedGrouping,
		/// <summary>
		/// The node corresponds to an directly reference element in the group
		/// </summary>
		ReferencedElement,
	}
	#endregion // SurveyGroupingChildType question
	#region SurveyGroupingReferenceType question
	/// <summary>
	/// Specify if a grouped element or nested group is included,
	/// excluded, or in a contradictory state within the group.
	/// </summary>
	public enum SurveyGroupingReferenceType
	{
		/// <summary>
		/// The element is included
		/// </summary>
		Inclusion,
		/// <summary>
		/// The element is excluded
		/// </summary>
		Exclusion,
		/// <summary>
		/// The element is in a contradictory state, meaning that
		/// it is both automatically included and blocked by different
		/// <see cref="ElementGroupingType"/> instances associated with
		/// the containing <see cref="ElementGrouping"/>
		/// </summary>
		Contradiction,
	}
	#endregion // SurveyGroupingReferenceType question
	#region SurveyDerivationType question
	/// <summary>
	/// Specify if an <see cref="ObjectType"/> or a <see cref="FactType"/>
	/// is derived.
	/// </summary>
	public enum SurveyDerivationType
	{
		/// <summary>
		/// The element has an associated derivation rule
		/// </summary>
		Derived,
		/// <summary>
		/// The element is a derived query
		/// </summary>
		Query,
	}
	#endregion // SurveyDerivationType question
	#region SurveyQueryParameterType question
	/// <summary>
	/// Identify a <see cref="QueryParameter"/> for overlay display
	/// </summary>
	public enum SurveyQueryParameterType
	{
		/// <summary>
		/// The element is an input parameter (only current kind)
		/// </summary>
		Input,
	}
	#endregion // SurveyQueryParameterType question
	#region Constraint Detail Question
	/// <summary>
	/// Constraint detail survey answers
	/// </summary>
	public enum SurveyConstraintDetailType
	{
		/// <summary>
		/// A reference to a <see cref="FactType"/> restricted
		/// by this constraint.
		/// </summary>
		ConsistuentFactType,
	}
	#endregion // Constraint Detail Question
}
