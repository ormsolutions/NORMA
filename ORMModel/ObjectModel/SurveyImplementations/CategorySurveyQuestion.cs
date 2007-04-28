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
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Neumont.Tools.Modeling.Design;
using System.ComponentModel;

namespace Neumont.Tools.ORM.ObjectModel
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
	/// error state enum question answers
	/// </summary>
	public enum SurveyErrorState
	{
		/// <summary>
		/// ORM element contains error
		/// </summary>
		HasError,
		/// <summary>
		/// ORM element does not contain an error
		/// </summary>
		NoError,
	}
	#endregion // Error State question
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
		/// Primary Subtype Relationship
		/// </summary>
		PrimarySubtypeRelationship,
		/// <summary>
		/// Secondary SubType Relationship
		/// </summary>
		SecondarySubtypeRelationship,
	}
	#endregion // Survey Glyph questions
}
