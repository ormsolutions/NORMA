using System;
using System.IO;
using System.Text;
namespace Neumont.Tools.ORM.ObjectModel
{
	#region VerbalizationTextSnippetType enum
	/// <summary>
	/// An enum with one value for each recognized snippet
	/// </summary>
	[CLSCompliant(true)]
	public enum VerbalizationTextSnippetType
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
		/// The 'DefiniteArticle' format string snippet. Contains 1 replacement field.
		/// </summary>
		DefiniteArticle,
		/// <summary>
		/// The 'EachInstanceQuantifier' format string snippet. Contains 1 replacement field.
		/// </summary>
		EachInstanceQuantifier,
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
		/// The 'NegativeReadingForUnaryOnlyDisjunctiveMandatory' format string snippet. Contains 2 replacement fields.
		/// </summary>
		NegativeReadingForUnaryOnlyDisjunctiveMandatory,
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
		/// The 'PersonalPronoun' format string snippet. Contains 1 replacement field.
		/// </summary>
		PersonalPronoun,
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
		/// The 'VerbalizerDocumentHeader' format string snippet. Contains 12 replacement fields.
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
	#endregion // VerbalizationTextSnippetType enum
	#region VerbalizationSet structure
	/// <summary>
	/// A structure holding an array of strings. Strings are retrieved with values from VerbalizationTextSnippetType.
	/// </summary>
	public struct VerbalizationSet
	{
		private string[] mySnippets;
		/// <summary>
		/// VerbalizationSet constructor.
		/// </summary>
		/// <param name="snippets">
		/// An array of strings with one string for each value in the VerbalizationTextSnippetType enum.
		/// </param>
		public VerbalizationSet(string[] snippets)
		{
			this.mySnippets = snippets;
		}
		/// <summary>
		/// Retrieve a snippet value
		/// </summary>
		/// <param name="snippetType">
		/// A value from the VerbalizationTextSnippetType enum representing the snippet string to retrieve.
		/// </param>
		/// <returns>
		/// Snippet string
		/// </returns>
		public string GetSnippet(VerbalizationTextSnippetType snippetType)
		{
			return this.mySnippets[(int)snippetType];
		}
	}
	#endregion // VerbalizationSet structure
	#region VerbalizationSets class
	/// <summary>
	/// A class containing one VerbalizationSet structure for each combination of {alethic,deontic} and {positive,negative} snippets.
	/// </summary>
	[CLSCompliant(true)]
	public class VerbalizationSets
	{
		/// <summary>
		/// The default verbalization snippet set. Contains english HTML snippets.
		/// </summary>
		public static readonly VerbalizationSets Default = VerbalizationSets.CreateDefaultVerbalizationSets();
		private VerbalizationSet[] mySets;
		private VerbalizationSets()
		{
		}
		/// <summary>
		/// Retrieve a snippet for the specified type with default criteria.
		/// </summary>
		/// <param name="snippetType">
		/// A value from the VerbalizationTextSnippetType enum representing the snippet string to retrieve.
		/// </param>
		/// <returns>
		/// Snippet string
		/// </returns>
		public string GetSnippet(VerbalizationTextSnippetType snippetType)
		{
			return this.mySets[0].GetSnippet(snippetType);
		}
		/// <summary>
		/// Retrieve a snippet for the specified type and criteria.
		/// </summary>
		/// <param name="snippetType">
		/// A value from the VerbalizationTextSnippetType enum representing the snippet string to retrieve.
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
		public string GetSnippet(VerbalizationTextSnippetType snippetType, bool isDeontic, bool isNegative)
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
			return this.mySets[setIndex].GetSnippet(snippetType);
		}
		private static VerbalizationSets CreateDefaultVerbalizationSets()
		{
			VerbalizationSets retVal = new VerbalizationSets();
			retVal.mySets = new VerbalizationSet[]{
				new VerbalizationSet(new string[]{
					@"<span class=""quantifier"">at most one</span> {0}",
					@"<span class=""listSeparator"">.</span>",
					@"{0} <span class=""quantifier"">combination</span>",
					"",
					@"<span class=""listSeparator"">, </span>",
					"",
					@"<span class=""listSeparator"">, </span>",
					@"<span class=""listSeparator"">, </span>",
					"</span>",
					@"<span class=""listSeparator"">; </span>",
					@"<br/><span class=""smallIndent"">",
					@"<span class=""listSeparator"">; </span>",
					@"<span class=""listSeparator"">; </span>",
					@"<span class=""quantifier"">that</span> {0}",
					@"<span class=""quantifier"">each instance of</span> {0} <span class=""quantifier"">occurs only once</span>",
					@"<span class=""quantifier"">some</span> {0}",
					@"<span class=""quantifier"">for each</span> {0}, {1}",
					@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
					@"<span class=""quantifier"">the same</span> {0}",
					@"<span class=""quantifier"">that</span> {0}",
					"{0}",
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
					@"<span class=""quantifier"">it is necessary that</span> {0}",
					@"<span class=""quantifier"">it is possible that</span> {0}",
					@"<span class=""quantifier"">more than one</span> {0}",
					@"<span class=""quantifier"">some</span> {0} <span class=""quantifier"">participates in none of the following:</span>{1}",
					@"<span class=""objectType"">{0}</span>",
					@"<span class=""objectTypeMissing"">Missing<sub>{0}</sub></span>",
					@"<span class=""objectType"">{0}<sub>{1}</sub></span>",
					@"{0} <span class=""quantifier"">occurs at most once in the population of</span> {1}",
					@"<span class=""quantifier"">at most one</span> {0}",
					@"<span class=""quantifier"">who</span> {0}",
					"",
					@"<span class=""listSeparator"">, and </span>",
					"",
					@"<span class=""listSeparator""> and </span>",
					@"<span class=""listSeparator"">, </span>",
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
			.referenceMode {{ color: #840084; font-weight: bold;}}
			.predicateText {{ color: #0000ff; }}
			.quantifier {{ color: {6}; {7} }}
			.error {{ color: red; }}
			.verbalization {{ }}
			.indent {{ left: 20px; position: relative; }}
			.smallIndent {{ left: 8px; position: relative;}}
			.listSeparator {{ color: windowtext; font-weight: 200;}}
			.logicalOperator {{ color: {6}; {7}}}
		</style>
	</head>
	<body>",
					"font-weight: bold;",
					"font-weight: normal;",
					@"<span class=""indent"">",
					"<br/>\n",
					@"<span class=""error"">",
					@"<p class=""verbalization"">"}),
				new VerbalizationSet(new string[]{
					@"<span class=""quantifier"">at most one</span> {0}",
					@"<span class=""listSeparator"">.</span>",
					@"{0} <span class=""quantifier"">combination</span>",
					"",
					@"<span class=""listSeparator"">, </span>",
					"",
					@"<span class=""listSeparator"">, </span>",
					@"<span class=""listSeparator"">, </span>",
					"</span>",
					@"<span class=""listSeparator"">; </span>",
					@"<br/><span class=""smallIndent"">",
					@"<span class=""listSeparator"">; </span>",
					@"<span class=""listSeparator"">; </span>",
					@"<span class=""quantifier"">that</span> {0}",
					@"<span class=""quantifier"">each instance of</span> {0} <span class=""quantifier"">occurs only once</span>",
					@"<span class=""quantifier"">some</span> {0}",
					@"<span class=""quantifier"">for each</span> {0}, {1}",
					@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
					@"<span class=""quantifier"">the same</span> {0}",
					@"<span class=""quantifier"">that</span> {0}",
					@"<span class=""quantifier"">it is obligatory that</span> {0}",
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
					@"<span class=""quantifier"">it is obligatory that</span> {0}",
					@"<span class=""quantifier"">it is permitted that</span> {0}",
					@"<span class=""quantifier"">more than one</span> {0}",
					@"<span class=""quantifier"">some</span> {0} <span class=""quantifier"">participates in none of the following:</span>{1}",
					@"<span class=""objectType"">{0}</span>",
					@"<span class=""objectTypeMissing"">Missing<sub>{0}</sub></span>",
					@"<span class=""objectType"">{0}<sub>{1}</sub></span>",
					@"{0} <span class=""quantifier"">occurs at most once in the population of</span> {1}",
					@"<span class=""quantifier"">at most one</span> {0}",
					@"<span class=""quantifier"">who</span> {0}",
					"",
					@"<span class=""listSeparator"">, and </span>",
					"",
					@"<span class=""listSeparator""> and </span>",
					@"<span class=""listSeparator"">, </span>",
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
			.referenceMode {{ color: #840084; font-weight: bold;}}
			.predicateText {{ color: #0000ff; }}
			.quantifier {{ color: {6}; {7} }}
			.error {{ color: red; }}
			.verbalization {{ }}
			.indent {{ left: 20px; position: relative; }}
			.smallIndent {{ left: 8px; position: relative;}}
			.listSeparator {{ color: windowtext; font-weight: 200;}}
			.logicalOperator {{ color: {6}; {7}}}
		</style>
	</head>
	<body>",
					"font-weight: bold;",
					"font-weight: normal;",
					@"<span class=""indent"">",
					"<br/>\n",
					@"<span class=""error"">",
					@"<p class=""verbalization"">"}),
				new VerbalizationSet(new string[]{
					@"<span class=""quantifier"">at most one</span> {0}",
					@"<span class=""listSeparator"">.</span>",
					@"{0} <span class=""quantifier"">combination</span>",
					"",
					@"<span class=""listSeparator"">, </span>",
					"",
					@"<span class=""listSeparator"">, </span>",
					@"<span class=""listSeparator"">, </span>",
					"</span>",
					@"<span class=""listSeparator"">; </span>",
					@"<br/><span class=""smallIndent"">",
					@"<span class=""listSeparator"">; </span>",
					@"<span class=""listSeparator"">; </span>",
					@"<span class=""quantifier"">that</span> {0}",
					@"<span class=""quantifier"">each instance of</span> {0} <span class=""quantifier"">occurs only once</span>",
					@"<span class=""quantifier"">some</span> {0}",
					@"<span class=""quantifier"">for each</span> {0}, {1}",
					@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
					@"<span class=""quantifier"">the same</span> {0}",
					@"<span class=""quantifier"">that</span> {0}",
					"{0}",
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
					@"<span class=""quantifier"">it is necessary that</span> {0}",
					@"<span class=""quantifier"">it is impossible that</span> {0}",
					@"<span class=""quantifier"">more than one</span> {0}",
					@"<span class=""quantifier"">some</span> {0} <span class=""quantifier"">participates in none of the following:</span>{1}",
					@"<span class=""objectType"">{0}</span>",
					@"<span class=""objectTypeMissing"">Missing<sub>{0}</sub></span>",
					@"<span class=""objectType"">{0}<sub>{1}</sub></span>",
					@"{0} <span class=""quantifier"">occurs more than once in the population of</span> {1}",
					@"<span class=""quantifier"">more than one</span> {0}",
					@"<span class=""quantifier"">who</span> {0}",
					"",
					@"<span class=""listSeparator"">, and </span>",
					"",
					@"<span class=""listSeparator""> and </span>",
					@"<span class=""listSeparator"">, </span>",
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
			.referenceMode {{ color: #840084; font-weight: bold;}}
			.predicateText {{ color: #0000ff; }}
			.quantifier {{ color: {6}; {7} }}
			.error {{ color: red; }}
			.verbalization {{ }}
			.indent {{ left: 20px; position: relative; }}
			.smallIndent {{ left: 8px; position: relative;}}
			.listSeparator {{ color: windowtext; font-weight: 200;}}
			.logicalOperator {{ color: {6}; {7}}}
		</style>
	</head>
	<body>",
					"font-weight: bold;",
					"font-weight: normal;",
					@"<span class=""indent"">",
					"<br/>\n",
					@"<span class=""error"">",
					@"<p class=""verbalization"">"}),
				new VerbalizationSet(new string[]{
					@"<span class=""quantifier"">at most one</span> {0}",
					@"<span class=""listSeparator"">.</span>",
					@"{0} <span class=""quantifier"">combination</span>",
					"",
					@"<span class=""listSeparator"">, </span>",
					"",
					@"<span class=""listSeparator"">, </span>",
					@"<span class=""listSeparator"">, </span>",
					"</span>",
					@"<span class=""listSeparator"">; </span>",
					@"<br/><span class=""smallIndent"">",
					@"<span class=""listSeparator"">; </span>",
					@"<span class=""listSeparator"">; </span>",
					@"<span class=""quantifier"">that</span> {0}",
					@"<span class=""quantifier"">each instance of</span> {0} <span class=""quantifier"">occurs only once</span>",
					@"<span class=""quantifier"">some</span> {0}",
					@"<span class=""quantifier"">for each</span> {0}, {1}",
					@"<span class=""quantifier"">for each</span> {0},<br/><span class=""smallIndent"">{1}</span>",
					@"<span class=""quantifier"">the same</span> {0}",
					@"<span class=""quantifier"">that</span> {0}",
					@"<span class=""quantifier"">it is obligatory that</span> {0}",
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
					@"<span class=""quantifier"">it is obligatory that</span> {0}",
					@"<span class=""quantifier"">it is forbidden that</span> {0}",
					@"<span class=""quantifier"">more than one</span> {0}",
					@"<span class=""quantifier"">some</span> {0} <span class=""quantifier"">participates in none of the following:</span>{1}",
					@"<span class=""objectType"">{0}</span>",
					@"<span class=""objectTypeMissing"">Missing<sub>{0}</sub></span>",
					@"<span class=""objectType"">{0}<sub>{1}</sub></span>",
					@"{0} <span class=""quantifier"">occurs more than once in the population of</span> {1}",
					@"<span class=""quantifier"">more than one</span> {0}",
					@"<span class=""quantifier"">who</span> {0}",
					"",
					@"<span class=""listSeparator"">, and </span>",
					"",
					@"<span class=""listSeparator""> and </span>",
					@"<span class=""listSeparator"">, </span>",
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
			.referenceMode {{ color: #840084; font-weight: bold;}}
			.predicateText {{ color: #0000ff; }}
			.quantifier {{ color: {6}; {7} }}
			.error {{ color: red; }}
			.verbalization {{ }}
			.indent {{ left: 20px; position: relative; }}
			.smallIndent {{ left: 8px; position: relative;}}
			.listSeparator {{ color: windowtext; font-weight: 200;}}
			.logicalOperator {{ color: {6}; {7}}}
		</style>
	</head>
	<body>",
					"font-weight: bold;",
					"font-weight: normal;",
					@"<span class=""indent"">",
					"<br/>\n",
					@"<span class=""error"">",
					@"<p class=""verbalization"">"})};
			return retVal;
		}
	}
	#endregion // VerbalizationSets class
	#region FactType verbalization
	public partial class FactType : IVerbalize
	{
		/// <summary>
		/// IVerbalize.GetVerbalization implementation
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, VerbalizationSets snippets, NotifyBeginVerbalization beginVerbalization, bool isNegative)
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
			RoleMoveableCollection factRoles = this.RoleCollection;
			int factArity = factRoles.Count;
			ReadingOrderMoveableCollection allReadingOrders = this.ReadingOrderCollection;
			const bool isDeontic = false;
			ReadingOrder readingOrder;
			string[] basicRoleReplacements = new string[factArity];
			int i = 0;
			for (; i < factArity; ++i)
			{
				ObjectType rolePlayer = factRoles[i].RolePlayer;
				string basicReplacement;
				if (rolePlayer != null)
				{
					basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name);
				}
				else
				{
					basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
				}
				basicRoleReplacements[i] = basicReplacement;
			}
			beginVerbalization(VerbalizationContent.Normal);
			readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
			FactType.WriteVerbalizerSentence(writer, FactType.PopulatePredicateText(readingOrder, factRoles, basicRoleReplacements), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, VerbalizationSets snippets, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return this.GetVerbalization(writer, snippets, beginVerbalization, isNegative);
		}
	}
	#endregion // FactType verbalization
	#region InternalUniquenessConstraint verbalization
	public partial class InternalUniquenessConstraint : IVerbalize
	{
		/// <summary>
		/// IVerbalize.GetVerbalization implementation
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, VerbalizationSets snippets, NotifyBeginVerbalization beginVerbalization, bool isNegative)
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
			int i = 0;
			for (; i < factArity; ++i)
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
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), rolePlayer.Name, subscript + 1);
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name);
					}
				}
				else
				{
					basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
				}
				basicRoleReplacements[i] = basicReplacement;
			}
			string[] roleReplacements = new string[factArity];
			ReadingOrder readingOrder;
			if ((factArity == 1) && !(isNegative))
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.OccursInPopulation, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.UniversalQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1Replace1 = null;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				int snippet1Replace1Replace1ReplaceRoleIter1 = 0;
				for (; snippet1Replace1Replace1ReplaceRoleIter1 < includedArity; ++snippet1Replace1Replace1ReplaceRoleIter1)
				{
					VerbalizationTextSnippetType listSnippet;
					if (snippet1Replace1Replace1ReplaceRoleIter1 == 0)
					{
						listSnippet = VerbalizationTextSnippetType.SimpleListOpen;
					}
					else if (snippet1Replace1Replace1ReplaceRoleIter1 == (includedArity - 1))
					{
						if (snippet1Replace1Replace1ReplaceRoleIter1 == 1)
						{
							listSnippet = VerbalizationTextSnippetType.SimpleListPairSeparator;
						}
						else
						{
							listSnippet = VerbalizationTextSnippetType.SimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = VerbalizationTextSnippetType.SimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[snippet1Replace1Replace1ReplaceRoleIter1])]);
					if (snippet1Replace1Replace1ReplaceRoleIter1 == (includedArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1Replace1Replace1 = sbTemp.ToString();
				snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1);
				string snippet1Replace1Replace2 = null;
				readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
				snippet1Replace1Replace2 = FactType.PopulatePredicateText(readingOrder, factRoles, basicRoleReplacements);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (factArity == 1)
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.OccursInPopulation, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1Replace1 = null;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				int snippet1Replace1Replace1ReplaceRoleIter1 = 0;
				for (; snippet1Replace1Replace1ReplaceRoleIter1 < includedArity; ++snippet1Replace1Replace1ReplaceRoleIter1)
				{
					VerbalizationTextSnippetType listSnippet;
					if (snippet1Replace1Replace1ReplaceRoleIter1 == 0)
					{
						listSnippet = VerbalizationTextSnippetType.SimpleListOpen;
					}
					else if (snippet1Replace1Replace1ReplaceRoleIter1 == (includedArity - 1))
					{
						if (snippet1Replace1Replace1ReplaceRoleIter1 == 1)
						{
							listSnippet = VerbalizationTextSnippetType.SimpleListPairSeparator;
						}
						else
						{
							listSnippet = VerbalizationTextSnippetType.SimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = VerbalizationTextSnippetType.SimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[snippet1Replace1Replace1ReplaceRoleIter1])]);
					if (snippet1Replace1Replace1ReplaceRoleIter1 == (includedArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1Replace1Replace1 = sbTemp.ToString();
				snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1);
				string snippet1Replace1Replace2 = null;
				readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
				snippet1Replace1Replace2 = FactType.PopulatePredicateText(readingOrder, factRoles, basicRoleReplacements);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if ((factArity == includedArity) && !(isNegative))
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				int snippet1ReplaceRoleIter1 = 0;
				for (; snippet1ReplaceRoleIter1 < factArity; ++snippet1ReplaceRoleIter1)
				{
					Role primaryRole = factRoles[snippet1ReplaceRoleIter1];
					VerbalizationTextSnippetType listSnippet;
					if (snippet1ReplaceRoleIter1 == 0)
					{
						listSnippet = VerbalizationTextSnippetType.TopLevelIndentedLogicalAndListOpen;
					}
					else if (snippet1ReplaceRoleIter1 == (factArity - 1))
					{
						if (snippet1ReplaceRoleIter1 == 1)
						{
							listSnippet = VerbalizationTextSnippetType.TopLevelIndentedLogicalAndListPairSeparator;
						}
						else
						{
							listSnippet = VerbalizationTextSnippetType.TopLevelIndentedLogicalAndListFinalSeparator;
						}
					}
					else
					{
						listSnippet = VerbalizationTextSnippetType.TopLevelIndentedLogicalAndListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					snippet1Replace1 = null;
					readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, primaryRole, null, false, false, factRoles, true);
					int snippet1ReplaceFactRoleIter1 = 0;
					for (; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
					{
						Role currentRole = factRoles[snippet1ReplaceFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = basicRoleReplacements[snippet1ReplaceFactRoleIter1];
						if (primaryRole == currentRole)
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.MoreThanOneQuantifier, isDeontic, isNegative), basicReplacement);
						}
						else
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative), basicReplacement);
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
					}
					snippet1Replace1 = FactType.PopulatePredicateText(readingOrder, factRoles, roleReplacements);
					sbTemp.Append(snippet1Replace1);
					if (snippet1ReplaceRoleIter1 == (factArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.TopLevelIndentedLogicalAndListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1 = sbTemp.ToString();
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				writer.WriteLine();
				string snippetFormat2 = snippets.GetSnippet(VerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet2Replace1 = null;
				string snippet2ReplaceFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.OccursInPopulation, isDeontic, isNegative);
				string snippet2Replace1Replace1 = null;
				string snippet2Replace1ReplaceFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.CombinationIdentifier, isDeontic, isNegative);
				string snippet2Replace1Replace1Replace1 = null;
				string snippet2Replace1Replace1ReplaceFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.UniversalQuantifier, isDeontic, isNegative);
				string snippet2Replace1Replace1Replace1Replace1 = null;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				int snippet2Replace1Replace1Replace1ReplaceRoleIter1 = 0;
				for (; snippet2Replace1Replace1Replace1ReplaceRoleIter1 < includedArity; ++snippet2Replace1Replace1Replace1ReplaceRoleIter1)
				{
					VerbalizationTextSnippetType listSnippet;
					if (snippet2Replace1Replace1Replace1ReplaceRoleIter1 == 0)
					{
						listSnippet = VerbalizationTextSnippetType.SimpleListOpen;
					}
					else if (snippet2Replace1Replace1Replace1ReplaceRoleIter1 == (includedArity - 1))
					{
						if (snippet2Replace1Replace1Replace1ReplaceRoleIter1 == 1)
						{
							listSnippet = VerbalizationTextSnippetType.SimpleListPairSeparator;
						}
						else
						{
							listSnippet = VerbalizationTextSnippetType.SimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = VerbalizationTextSnippetType.SimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[snippet2Replace1Replace1Replace1ReplaceRoleIter1])]);
					if (snippet2Replace1Replace1Replace1ReplaceRoleIter1 == (includedArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
					}
				}
				snippet2Replace1Replace1Replace1Replace1 = sbTemp.ToString();
				snippet2Replace1Replace1Replace1 = string.Format(writer.FormatProvider, snippet2Replace1Replace1ReplaceFormat1, snippet2Replace1Replace1Replace1Replace1);
				snippet2Replace1Replace1 = string.Format(writer.FormatProvider, snippet2Replace1ReplaceFormat1, snippet2Replace1Replace1Replace1);
				string snippet2Replace1Replace2 = null;
				readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
				snippet2Replace1Replace2 = FactType.PopulatePredicateText(readingOrder, factRoles, basicRoleReplacements);
				snippet2Replace1 = string.Format(writer.FormatProvider, snippet2ReplaceFormat1, snippet2Replace1Replace1, snippet2Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat2, snippet2Replace1), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (factArity == includedArity)
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.OccursInPopulation, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				string snippet1Replace1ReplaceFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.CombinationIdentifier, isDeontic, isNegative);
				string snippet1Replace1Replace1Replace1 = null;
				string snippet1Replace1Replace1ReplaceFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1Replace1Replace1 = null;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				int snippet1Replace1Replace1Replace1ReplaceRoleIter1 = 0;
				for (; snippet1Replace1Replace1Replace1ReplaceRoleIter1 < includedArity; ++snippet1Replace1Replace1Replace1ReplaceRoleIter1)
				{
					VerbalizationTextSnippetType listSnippet;
					if (snippet1Replace1Replace1Replace1ReplaceRoleIter1 == 0)
					{
						listSnippet = VerbalizationTextSnippetType.CompactSimpleListOpen;
					}
					else if (snippet1Replace1Replace1Replace1ReplaceRoleIter1 == (includedArity - 1))
					{
						if (snippet1Replace1Replace1Replace1ReplaceRoleIter1 == 1)
						{
							listSnippet = VerbalizationTextSnippetType.CompactSimpleListPairSeparator;
						}
						else
						{
							listSnippet = VerbalizationTextSnippetType.CompactSimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = VerbalizationTextSnippetType.CompactSimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[snippet1Replace1Replace1Replace1ReplaceRoleIter1])]);
					if (snippet1Replace1Replace1Replace1ReplaceRoleIter1 == (includedArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.CompactSimpleListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1Replace1Replace1Replace1 = sbTemp.ToString();
				snippet1Replace1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1Replace1);
				snippet1Replace1Replace1 = string.Format(writer.FormatProvider, snippet1Replace1ReplaceFormat1, snippet1Replace1Replace1Replace1);
				string snippet1Replace1Replace2 = null;
				readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
				snippet1Replace1Replace2 = FactType.PopulatePredicateText(readingOrder, factRoles, basicRoleReplacements);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if ((factArity == 2) && !(isNegative))
			{
				readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, null, includedRoles, false, false, factRoles, false);
				if (readingOrder != null)
				{
					beginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					int snippet1ReplaceFactRoleIter1 = 0;
					for (; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
					{
						Role currentRole = factRoles[snippet1ReplaceFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = basicRoleReplacements[snippet1ReplaceFactRoleIter1];
						if (includedRoles.Contains(currentRole))
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
						}
						else
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.OneQuantifier, isDeontic, isNegative), basicReplacement);
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
					}
					snippet1Replace1 = FactType.PopulatePredicateText(readingOrder, factRoles, roleReplacements);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
					if (readingOrder != null)
					{
						beginVerbalization(VerbalizationContent.Normal);
						string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace1 = null;
						string snippet1ReplaceFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
						string snippet1Replace1Replace1 = null;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						int snippet1Replace1ReplaceRoleIter1 = 0;
						for (; snippet1Replace1ReplaceRoleIter1 < includedArity; ++snippet1Replace1ReplaceRoleIter1)
						{
							VerbalizationTextSnippetType listSnippet;
							if (snippet1Replace1ReplaceRoleIter1 == 0)
							{
								listSnippet = VerbalizationTextSnippetType.SimpleListOpen;
							}
							else if (snippet1Replace1ReplaceRoleIter1 == (includedArity - 1))
							{
								if (snippet1Replace1ReplaceRoleIter1 == 1)
								{
									listSnippet = VerbalizationTextSnippetType.SimpleListPairSeparator;
								}
								else
								{
									listSnippet = VerbalizationTextSnippetType.SimpleListFinalSeparator;
								}
							}
							else
							{
								listSnippet = VerbalizationTextSnippetType.SimpleListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[snippet1Replace1ReplaceRoleIter1])]);
							if (snippet1Replace1ReplaceRoleIter1 == (includedArity - 1))
							{
								sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
							}
						}
						snippet1Replace1Replace1 = sbTemp.ToString();
						string snippet1Replace1Replace2 = null;
						readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
						int snippet1Replace1ReplaceFactRoleIter2 = 0;
						for (; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
						{
							Role currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
							string roleReplacement = null;
							string basicReplacement = basicRoleReplacements[snippet1Replace1ReplaceFactRoleIter2];
							if (includedRoles.Contains(currentRole))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.OneQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
						}
						snippet1Replace1Replace2 = FactType.PopulatePredicateText(readingOrder, factRoles, roleReplacements);
						snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
				}
			}
			else if (factArity == 2)
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, null, includedRoles, false, false, factRoles, true);
				int snippet1ReplaceFactRoleIter1 = 0;
				for (; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
				{
					Role currentRole = factRoles[snippet1ReplaceFactRoleIter1];
					string roleReplacement = null;
					string basicReplacement = basicRoleReplacements[snippet1ReplaceFactRoleIter1];
					if (includedRoles.Contains(currentRole))
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative), basicReplacement);
					}
					else if (!(includedRoles.Contains(currentRole)))
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.OneQuantifier, isDeontic, isNegative), basicReplacement);
					}
					if (roleReplacement == null)
					{
						roleReplacement = basicReplacement;
					}
					roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
				}
				snippet1Replace1 = FactType.PopulatePredicateText(readingOrder, factRoles, roleReplacements);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (isNegative)
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				int snippet1Replace1ReplaceRoleIter1 = 0;
				for (; snippet1Replace1ReplaceRoleIter1 < includedArity; ++snippet1Replace1ReplaceRoleIter1)
				{
					VerbalizationTextSnippetType listSnippet;
					if (snippet1Replace1ReplaceRoleIter1 == 0)
					{
						listSnippet = VerbalizationTextSnippetType.SimpleListOpen;
					}
					else if (snippet1Replace1ReplaceRoleIter1 == (includedArity - 1))
					{
						if (snippet1Replace1ReplaceRoleIter1 == 1)
						{
							listSnippet = VerbalizationTextSnippetType.SimpleListPairSeparator;
						}
						else
						{
							listSnippet = VerbalizationTextSnippetType.SimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = VerbalizationTextSnippetType.SimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[snippet1Replace1ReplaceRoleIter1])]);
					if (snippet1Replace1ReplaceRoleIter1 == (includedArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1Replace1 = sbTemp.ToString();
				string snippet1Replace1Replace2 = null;
				readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, null, includedRoles, false, false, factRoles, true);
				int snippet1Replace1ReplaceFactRoleIter2 = 0;
				for (; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
				{
					Role currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
					string roleReplacement = null;
					string basicReplacement = basicRoleReplacements[snippet1Replace1ReplaceFactRoleIter2];
					if (includedRoles.Contains(currentRole))
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
					}
					else if (!(includedRoles.Contains(currentRole)))
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.OneQuantifier, isDeontic, isNegative), basicReplacement);
					}
					if (roleReplacement == null)
					{
						roleReplacement = basicReplacement;
					}
					roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
				}
				snippet1Replace1Replace2 = FactType.PopulatePredicateText(readingOrder, factRoles, roleReplacements);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ForEachQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				int snippet1Replace1ReplaceRoleIter1 = 0;
				for (; snippet1Replace1ReplaceRoleIter1 < includedArity; ++snippet1Replace1ReplaceRoleIter1)
				{
					VerbalizationTextSnippetType listSnippet;
					if (snippet1Replace1ReplaceRoleIter1 == 0)
					{
						listSnippet = VerbalizationTextSnippetType.SimpleListOpen;
					}
					else if (snippet1Replace1ReplaceRoleIter1 == (includedArity - 1))
					{
						if (snippet1Replace1ReplaceRoleIter1 == 1)
						{
							listSnippet = VerbalizationTextSnippetType.SimpleListPairSeparator;
						}
						else
						{
							listSnippet = VerbalizationTextSnippetType.SimpleListFinalSeparator;
						}
					}
					else
					{
						listSnippet = VerbalizationTextSnippetType.SimpleListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[snippet1Replace1ReplaceRoleIter1])]);
					if (snippet1Replace1ReplaceRoleIter1 == (includedArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1Replace1 = sbTemp.ToString();
				string snippet1Replace1Replace2 = null;
				readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
				int snippet1Replace1ReplaceFactRoleIter2 = 0;
				for (; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
				{
					Role currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
					string roleReplacement = null;
					string basicReplacement = basicRoleReplacements[snippet1Replace1ReplaceFactRoleIter2];
					if (includedRoles.Contains(currentRole))
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
					}
					else if (!(includedRoles.Contains(currentRole)))
					{
						roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.OneQuantifier, isDeontic, isNegative), basicReplacement);
					}
					if (roleReplacement == null)
					{
						roleReplacement = basicReplacement;
					}
					roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
				}
				snippet1Replace1Replace2 = FactType.PopulatePredicateText(readingOrder, factRoles, roleReplacements);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, VerbalizationSets snippets, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return this.GetVerbalization(writer, snippets, beginVerbalization, isNegative);
		}
	}
	#endregion // InternalUniquenessConstraint verbalization
	#region SimpleMandatoryConstraint verbalization
	public partial class SimpleMandatoryConstraint : IVerbalize
	{
		/// <summary>
		/// IVerbalize.GetVerbalization implementation
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, VerbalizationSets snippets, NotifyBeginVerbalization beginVerbalization, bool isNegative)
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
			int i = 0;
			for (; i < factArity; ++i)
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
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), rolePlayer.Name, subscript + 1);
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name);
					}
				}
				else
				{
					basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
				}
				basicRoleReplacements[i] = basicReplacement;
			}
			string[] roleReplacements = new string[factArity];
			ReadingOrder readingOrder;
			if (factArity == 1)
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.UniversalQuantifier, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
				snippet1Replace1Replace1 = FactType.PopulatePredicateText(readingOrder, factRoles, basicRoleReplacements);
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
			}
			else if (factArity == 2)
			{
				readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, null, includedRoles, false, false, factRoles, false);
				if (readingOrder != null)
				{
					beginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					int snippet1ReplaceFactRoleIter1 = 0;
					for (; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
					{
						Role currentRole = factRoles[snippet1ReplaceFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = basicRoleReplacements[snippet1ReplaceFactRoleIter1];
						if (includedRoles.Contains(currentRole))
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
						}
						else
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
					}
					snippet1Replace1 = FactType.PopulatePredicateText(readingOrder, factRoles, roleReplacements);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
					if (readingOrder != null)
					{
						beginVerbalization(VerbalizationContent.Normal);
						string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace1 = null;
						string snippet1ReplaceFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
						string snippet1Replace1Replace1 = null;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						int snippet1Replace1ReplaceRoleIter1 = 0;
						for (; snippet1Replace1ReplaceRoleIter1 < includedArity; ++snippet1Replace1ReplaceRoleIter1)
						{
							VerbalizationTextSnippetType listSnippet;
							if (snippet1Replace1ReplaceRoleIter1 == 0)
							{
								listSnippet = VerbalizationTextSnippetType.SimpleListOpen;
							}
							else if (snippet1Replace1ReplaceRoleIter1 == (includedArity - 1))
							{
								if (snippet1Replace1ReplaceRoleIter1 == 1)
								{
									listSnippet = VerbalizationTextSnippetType.SimpleListPairSeparator;
								}
								else
								{
									listSnippet = VerbalizationTextSnippetType.SimpleListFinalSeparator;
								}
							}
							else
							{
								listSnippet = VerbalizationTextSnippetType.SimpleListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[snippet1Replace1ReplaceRoleIter1])]);
							if (snippet1Replace1ReplaceRoleIter1 == (includedArity - 1))
							{
								sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
							}
						}
						snippet1Replace1Replace1 = sbTemp.ToString();
						string snippet1Replace1Replace2 = null;
						readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
						int snippet1Replace1ReplaceFactRoleIter2 = 0;
						for (; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
						{
							Role currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
							string roleReplacement = null;
							string basicReplacement = basicRoleReplacements[snippet1Replace1ReplaceFactRoleIter2];
							if (includedRoles.Contains(currentRole))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
						}
						snippet1Replace1Replace2 = FactType.PopulatePredicateText(readingOrder, factRoles, roleReplacements);
						snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
				}
			}
			else
			{
				readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, null, includedRoles, false, false, factRoles, false);
				if (readingOrder != null)
				{
					beginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					int snippet1ReplaceFactRoleIter1 = 0;
					for (; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
					{
						Role currentRole = factRoles[snippet1ReplaceFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = basicRoleReplacements[snippet1ReplaceFactRoleIter1];
						if (includedRoles.Contains(currentRole))
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
						}
						else
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
						}
						if (roleReplacement == null)
						{
							roleReplacement = basicReplacement;
						}
						roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
					}
					snippet1Replace1 = FactType.PopulatePredicateText(readingOrder, factRoles, roleReplacements);
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
					if (readingOrder != null)
					{
						beginVerbalization(VerbalizationContent.Normal);
						string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ImpliedModalNecessityOperator, isDeontic, isNegative);
						string snippet1Replace1 = null;
						string snippet1ReplaceFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ForEachQuantifier, isDeontic, isNegative);
						string snippet1Replace1Replace1 = null;
						if (sbTemp == null)
						{
							sbTemp = new StringBuilder();
						}
						else
						{
							sbTemp.Length = 0;
						}
						int snippet1Replace1ReplaceRoleIter1 = 0;
						for (; snippet1Replace1ReplaceRoleIter1 < includedArity; ++snippet1Replace1ReplaceRoleIter1)
						{
							VerbalizationTextSnippetType listSnippet;
							if (snippet1Replace1ReplaceRoleIter1 == 0)
							{
								listSnippet = VerbalizationTextSnippetType.SimpleListOpen;
							}
							else if (snippet1Replace1ReplaceRoleIter1 == (includedArity - 1))
							{
								if (snippet1Replace1ReplaceRoleIter1 == 1)
								{
									listSnippet = VerbalizationTextSnippetType.SimpleListPairSeparator;
								}
								else
								{
									listSnippet = VerbalizationTextSnippetType.SimpleListFinalSeparator;
								}
							}
							else
							{
								listSnippet = VerbalizationTextSnippetType.SimpleListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(includedRoles[snippet1Replace1ReplaceRoleIter1])]);
							if (snippet1Replace1ReplaceRoleIter1 == (includedArity - 1))
							{
								sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
							}
						}
						snippet1Replace1Replace1 = sbTemp.ToString();
						string snippet1Replace1Replace2 = null;
						readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
						int snippet1Replace1ReplaceFactRoleIter2 = 0;
						for (; snippet1Replace1ReplaceFactRoleIter2 < factArity; ++snippet1Replace1ReplaceFactRoleIter2)
						{
							Role currentRole = factRoles[snippet1Replace1ReplaceFactRoleIter2];
							string roleReplacement = null;
							string basicReplacement = basicRoleReplacements[snippet1Replace1ReplaceFactRoleIter2];
							if (includedRoles.Contains(currentRole))
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1Replace1ReplaceFactRoleIter2] = roleReplacement;
						}
						snippet1Replace1Replace2 = FactType.PopulatePredicateText(readingOrder, factRoles, roleReplacements);
						snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
						FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
					}
				}
			}
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, VerbalizationSets snippets, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return this.GetVerbalization(writer, snippets, beginVerbalization, isNegative);
		}
	}
	#endregion // SimpleMandatoryConstraint verbalization
	#region DisjunctiveMandatoryConstraint verbalization
	public partial class DisjunctiveMandatoryConstraint : IVerbalize
	{
		/// <summary>
		/// IVerbalize.GetVerbalization implementation
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, VerbalizationSets snippets, NotifyBeginVerbalization beginVerbalization, bool isNegative)
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
			bool isDeontic = (this as IConstraint).Modality == ConstraintModality.Deontic;
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
				int i = 0;
				for (; i < factArity; ++i)
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
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), rolePlayer.Name, subscript + 1);
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name);
						}
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				allBasicRoleReplacements[iFact] = basicRoleReplacements;
			}
			int constraintRoleArity = allConstraintRoles.Count;
			ReadingOrder[] allConstraintRoleReadingOrders = new ReadingOrder[constraintRoleArity];
			string[] roleReplacements = new string[maxFactArity];
			ReadingOrder readingOrder;
			if (isNegative && (maxFactArity <= 1))
			{
				beginVerbalization(VerbalizationContent.Normal);
				string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
				string snippet1Replace1 = null;
				string snippet1ReplaceFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.NegativeReadingForUnaryOnlyDisjunctiveMandatory, isDeontic, isNegative);
				string snippet1Replace1Replace1 = null;
				if (sbTemp == null)
				{
					sbTemp = new StringBuilder();
				}
				else
				{
					sbTemp.Length = 0;
				}
				int snippet1Replace1ReplaceRoleIter1 = 0;
				for (; snippet1Replace1ReplaceRoleIter1 < 1; ++snippet1Replace1ReplaceRoleIter1)
				{
					Role primaryRole = allConstraintRoles[snippet1Replace1ReplaceRoleIter1];
					parentFact = primaryRole.FactType;
					factRoles = parentFact.RoleCollection;
					factArity = factRoles.Count;
					allReadingOrders = parentFact.ReadingOrderCollection;
					int currentFactIndex = allFacts.IndexOf(parentFact);
					string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
					VerbalizationTextSnippetType listSnippet;
					listSnippet = VerbalizationTextSnippetType.SimpleListOpen;
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(allConstraintRoles[snippet1Replace1ReplaceRoleIter1])]);
					sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
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
				int snippet1Replace1ReplaceRoleIter2 = 0;
				for (; snippet1Replace1ReplaceRoleIter2 < constraintRoleArity; ++snippet1Replace1ReplaceRoleIter2)
				{
					Role primaryRole = allConstraintRoles[snippet1Replace1ReplaceRoleIter2];
					parentFact = primaryRole.FactType;
					factRoles = parentFact.RoleCollection;
					factArity = factRoles.Count;
					allReadingOrders = parentFact.ReadingOrderCollection;
					int currentFactIndex = allFacts.IndexOf(parentFact);
					string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
					VerbalizationTextSnippetType listSnippet;
					if (snippet1Replace1ReplaceRoleIter2 == 0)
					{
						listSnippet = VerbalizationTextSnippetType.CompoundListOpen;
					}
					else if (snippet1Replace1ReplaceRoleIter2 == (constraintRoleArity - 1))
					{
						if (snippet1Replace1ReplaceRoleIter2 == 1)
						{
							listSnippet = VerbalizationTextSnippetType.CompoundListPairSeparator;
						}
						else
						{
							listSnippet = VerbalizationTextSnippetType.CompoundListFinalSeparator;
						}
					}
					else
					{
						listSnippet = VerbalizationTextSnippetType.CompoundListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					snippet1Replace1Replace2 = null;
					readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
					snippet1Replace1Replace2 = FactType.PopulatePredicateText(readingOrder, factRoles, basicRoleReplacements);
					sbTemp.Append(snippet1Replace1Replace2);
					if (snippet1Replace1ReplaceRoleIter2 == (constraintRoleArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.CompoundListClose, isDeontic, isNegative));
					}
				}
				snippet1Replace1Replace2 = sbTemp.ToString();
				snippet1Replace1 = string.Format(writer.FormatProvider, snippet1ReplaceFormat1, snippet1Replace1Replace1, snippet1Replace1Replace2);
				FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
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
				int factTextRoleIter1 = 0;
				bool factTextIsFirstPass1 = true;
				for (; factTextRoleIter1 < constraintRoleArity; ++factTextRoleIter1)
				{
					Role primaryRole = allConstraintRoles[factTextRoleIter1];
					parentFact = primaryRole.FactType;
					factRoles = parentFact.RoleCollection;
					factArity = factRoles.Count;
					allReadingOrders = parentFact.ReadingOrderCollection;
					int currentFactIndex = allFacts.IndexOf(parentFact);
					string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
					VerbalizationTextSnippetType listSnippet;
					if (factTextRoleIter1 == 0)
					{
						listSnippet = VerbalizationTextSnippetType.TopLevelIndentedLogicalOrListOpen;
					}
					else if (factTextRoleIter1 == (constraintRoleArity - 1))
					{
						if (factTextRoleIter1 == 1)
						{
							listSnippet = VerbalizationTextSnippetType.TopLevelIndentedLogicalOrListPairSeparator;
						}
						else
						{
							listSnippet = VerbalizationTextSnippetType.TopLevelIndentedLogicalOrListFinalSeparator;
						}
					}
					else
					{
						listSnippet = VerbalizationTextSnippetType.TopLevelIndentedLogicalOrListSeparator;
					}
					sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
					string factText1 = null;
					readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
					int factTextFactRoleIter1 = 0;
					for (; factTextFactRoleIter1 < factArity; ++factTextFactRoleIter1)
					{
						Role currentRole = factRoles[factTextFactRoleIter1];
						string roleReplacement = null;
						string basicReplacement = basicRoleReplacements[factTextFactRoleIter1];
						if ((primaryRole == currentRole) && factTextIsFirstPass1)
						{
							roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
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
					factText1 = FactType.PopulatePredicateText(readingOrder, factRoles, roleReplacements);
					sbTemp.Append(factText1);
					if (factTextRoleIter1 == (constraintRoleArity - 1))
					{
						sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.TopLevelIndentedLogicalOrListClose, isDeontic, isNegative));
					}
					factTextIsFirstPass1 = false;
				}
				FactType.WriteVerbalizerSentence(writer, sbTemp.ToString(), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
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
					readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, primaryRole, null, false, false, factRoles, false);
					if (readingOrder == null)
					{
						missingReading1 = true;
					}
					else
					{
						allConstraintRoleReadingOrders[readingMatchIndex1] = readingOrder;
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
					int list1ItemRoleIter1 = 0;
					bool list1ItemIsFirstPass1 = true;
					for (; list1ItemRoleIter1 < constraintRoleArity; ++list1ItemRoleIter1)
					{
						Role primaryRole = allConstraintRoles[list1ItemRoleIter1];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						int currentFactIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
						if (factArity >= 2)
						{
							VerbalizationTextSnippetType listSnippet;
							if (listCompositeIterator1 == 0)
							{
								listSnippet = VerbalizationTextSnippetType.TopLevelIndentedLogicalOrListOpen;
							}
							else if (listCompositeIterator1 == (listCompositeCount1 - 1))
							{
								if (listCompositeIterator1 == 1)
								{
									listSnippet = VerbalizationTextSnippetType.TopLevelIndentedLogicalOrListPairSeparator;
								}
								else
								{
									listSnippet = VerbalizationTextSnippetType.TopLevelIndentedLogicalOrListFinalSeparator;
								}
							}
							else
							{
								listSnippet = VerbalizationTextSnippetType.TopLevelIndentedLogicalOrListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							list1Item1 = null;
							readingOrder = allConstraintRoleReadingOrders[list1ItemRoleIter1];
							int list1ItemFactRoleIter1 = 0;
							for (; list1ItemFactRoleIter1 < factArity; ++list1ItemFactRoleIter1)
							{
								Role currentRole = factRoles[list1ItemFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = basicRoleReplacements[list1ItemFactRoleIter1];
								if ((primaryRole == currentRole) && list1ItemIsFirstPass1)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.UniversalQuantifier, isDeontic, isNegative), basicReplacement);
								}
								else if (primaryRole == currentRole)
								{
									roleReplacement = "";
								}
								else
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								}
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[list1ItemFactRoleIter1] = roleReplacement;
							}
							list1Item1 = FactType.PopulatePredicateText(readingOrder, factRoles, roleReplacements);
							sbTemp.Append(list1Item1);
							if (list1ItemRoleIter1 == (listCompositeCount1 - 1))
							{
								sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.TopLevelIndentedLogicalOrListClose, isDeontic, isNegative));
							}
							listCompositeIterator1 = listCompositeIterator1 + 1;
							list1ItemIsFirstPass1 = false;
						}
					}
					string list1Item2 = null;
					int list1ItemRoleIter2 = 0;
					for (; list1ItemRoleIter2 < constraintRoleArity; ++list1ItemRoleIter2)
					{
						Role primaryRole = allConstraintRoles[list1ItemRoleIter2];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						int currentFactIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
						if (factArity == 1)
						{
							VerbalizationTextSnippetType listSnippet;
							if (listCompositeIterator1 == 0)
							{
								listSnippet = VerbalizationTextSnippetType.TopLevelIndentedLogicalOrListOpen;
							}
							else if (listCompositeIterator1 == (listCompositeCount1 - 1))
							{
								if (listCompositeIterator1 == 1)
								{
									listSnippet = VerbalizationTextSnippetType.TopLevelIndentedLogicalOrListPairSeparator;
								}
								else
								{
									listSnippet = VerbalizationTextSnippetType.TopLevelIndentedLogicalOrListFinalSeparator;
								}
							}
							else
							{
								listSnippet = VerbalizationTextSnippetType.TopLevelIndentedLogicalOrListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							list1Item2 = null;
							readingOrder = allConstraintRoleReadingOrders[list1ItemRoleIter2];
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
							list1Item2 = FactType.PopulatePredicateText(readingOrder, factRoles, roleReplacements);
							sbTemp.Append(list1Item2);
							if (list1ItemRoleIter2 == (listCompositeCount1 - 1))
							{
								sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.TopLevelIndentedLogicalOrListClose, isDeontic, isNegative));
							}
							listCompositeIterator1 = listCompositeIterator1 + 1;
						}
					}
					FactType.WriteVerbalizerSentence(writer, sbTemp.ToString(), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
				else
				{
					beginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ForEachQuantifier, isDeontic, isNegative);
					string snippet1Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					int snippet1ReplaceRoleIter1 = 0;
					for (; snippet1ReplaceRoleIter1 < 1; ++snippet1ReplaceRoleIter1)
					{
						Role primaryRole = allConstraintRoles[snippet1ReplaceRoleIter1];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						int currentFactIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
						VerbalizationTextSnippetType listSnippet;
						listSnippet = VerbalizationTextSnippetType.SimpleListOpen;
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(allConstraintRoles[snippet1ReplaceRoleIter1])]);
						sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
					}
					snippet1Replace1 = sbTemp.ToString();
					string snippet1Replace2 = null;
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
					int snippet1Replace2ItemRoleIter1 = 0;
					for (; snippet1Replace2ItemRoleIter1 < constraintRoleArity; ++snippet1Replace2ItemRoleIter1)
					{
						Role primaryRole = allConstraintRoles[snippet1Replace2ItemRoleIter1];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						int currentFactIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
						if (factArity >= 2)
						{
							VerbalizationTextSnippetType listSnippet;
							if (snippet1ReplaceCompositeIterator2 == 0)
							{
								listSnippet = VerbalizationTextSnippetType.IndentedLogicalOrListOpen;
							}
							else if (snippet1ReplaceCompositeIterator2 == (snippet1ReplaceCompositeCount2 - 1))
							{
								if (snippet1ReplaceCompositeIterator2 == 1)
								{
									listSnippet = VerbalizationTextSnippetType.IndentedLogicalOrListPairSeparator;
								}
								else
								{
									listSnippet = VerbalizationTextSnippetType.IndentedLogicalOrListFinalSeparator;
								}
							}
							else
							{
								listSnippet = VerbalizationTextSnippetType.IndentedLogicalOrListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							snippet1Replace2Item1 = null;
							readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, primaryRole, null, false, true, factRoles, true);
							int snippet1Replace2ItemFactRoleIter1 = 0;
							for (; snippet1Replace2ItemFactRoleIter1 < factArity; ++snippet1Replace2ItemFactRoleIter1)
							{
								Role currentRole = factRoles[snippet1Replace2ItemFactRoleIter1];
								string roleReplacement = null;
								string basicReplacement = basicRoleReplacements[snippet1Replace2ItemFactRoleIter1];
								if (currentRole == primaryRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
								}
								else if (currentRole != primaryRole)
								{
									roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ExistentialQuantifier, isDeontic, isNegative), basicReplacement);
								}
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace2ItemFactRoleIter1] = roleReplacement;
							}
							snippet1Replace2Item1 = FactType.PopulatePredicateText(readingOrder, factRoles, roleReplacements);
							sbTemp.Append(snippet1Replace2Item1);
							if (snippet1Replace2ItemRoleIter1 == (snippet1ReplaceCompositeCount2 - 1))
							{
								sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.IndentedLogicalOrListClose, isDeontic, isNegative));
							}
							snippet1ReplaceCompositeIterator2 = snippet1ReplaceCompositeIterator2 + 1;
						}
					}
					string snippet1Replace2Item2 = null;
					int snippet1Replace2ItemRoleIter2 = 0;
					for (; snippet1Replace2ItemRoleIter2 < constraintRoleArity; ++snippet1Replace2ItemRoleIter2)
					{
						Role primaryRole = allConstraintRoles[snippet1Replace2ItemRoleIter2];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						int currentFactIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
						if (factArity == 1)
						{
							VerbalizationTextSnippetType listSnippet;
							if (snippet1ReplaceCompositeIterator2 == 0)
							{
								listSnippet = VerbalizationTextSnippetType.IndentedLogicalOrListOpen;
							}
							else if (snippet1ReplaceCompositeIterator2 == (snippet1ReplaceCompositeCount2 - 1))
							{
								if (snippet1ReplaceCompositeIterator2 == 1)
								{
									listSnippet = VerbalizationTextSnippetType.IndentedLogicalOrListPairSeparator;
								}
								else
								{
									listSnippet = VerbalizationTextSnippetType.IndentedLogicalOrListFinalSeparator;
								}
							}
							else
							{
								listSnippet = VerbalizationTextSnippetType.IndentedLogicalOrListSeparator;
							}
							sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
							snippet1Replace2Item2 = null;
							readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, factRoles[0], null, false, false, factRoles, true);
							int snippet1Replace2ItemFactRoleIter2 = 0;
							for (; snippet1Replace2ItemFactRoleIter2 < factArity; ++snippet1Replace2ItemFactRoleIter2)
							{
								Role currentRole = factRoles[snippet1Replace2ItemFactRoleIter2];
								string roleReplacement = null;
								string basicReplacement = basicRoleReplacements[snippet1Replace2ItemFactRoleIter2];
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
								if (roleReplacement == null)
								{
									roleReplacement = basicReplacement;
								}
								roleReplacements[snippet1Replace2ItemFactRoleIter2] = roleReplacement;
							}
							snippet1Replace2Item2 = FactType.PopulatePredicateText(readingOrder, factRoles, roleReplacements);
							sbTemp.Append(snippet1Replace2Item2);
							if (snippet1Replace2ItemRoleIter2 == (snippet1ReplaceCompositeCount2 - 1))
							{
								sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.IndentedLogicalOrListClose, isDeontic, isNegative));
							}
							snippet1ReplaceCompositeIterator2 = snippet1ReplaceCompositeIterator2 + 1;
						}
					}
					snippet1Replace2 = sbTemp.ToString();
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
			}
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, VerbalizationSets snippets, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return this.GetVerbalization(writer, snippets, beginVerbalization, isNegative);
		}
	}
	#endregion // DisjunctiveMandatoryConstraint verbalization
	#region ExternalUniquenessConstraint verbalization
	public partial class ExternalUniquenessConstraint : IVerbalize
	{
		/// <summary>
		/// IVerbalize.GetVerbalization implementation
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, VerbalizationSets snippets, NotifyBeginVerbalization beginVerbalization, bool isNegative)
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
			bool isDeontic = (this as IConstraint).Modality == ConstraintModality.Deontic;
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
				int i = 0;
				for (; i < factArity; ++i)
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
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ObjectTypeWithSubscript, isDeontic, isNegative), rolePlayer.Name, subscript + 1);
						}
						else
						{
							basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ObjectType, isDeontic, isNegative), rolePlayer.Name);
						}
					}
					else
					{
						basicReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.ObjectTypeMissing, isDeontic, isNegative), i + 1);
					}
					basicRoleReplacements[i] = basicReplacement;
				}
				allBasicRoleReplacements[iFact] = basicRoleReplacements;
			}
			int constraintRoleArity = allConstraintRoles.Count;
			ReadingOrder[] allConstraintRoleReadingOrders = new ReadingOrder[constraintRoleArity];
			string[] roleReplacements = new string[maxFactArity];
			ReadingOrder readingOrder;
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
					readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, primaryRole, null, true, true, factRoles, false);
					if (readingOrder == null)
					{
						missingReading1 = true;
					}
					else
					{
						allConstraintRoleReadingOrders[readingMatchIndex1] = readingOrder;
					}
				}
				if (!(missingReading1))
				{
					beginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ForEachCompactQuantifier, isDeontic, isNegative);
					string snippet1Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					int snippet1ReplaceRoleIter1 = 0;
					for (; snippet1ReplaceRoleIter1 < constraintRoleArity; ++snippet1ReplaceRoleIter1)
					{
						Role primaryRole = allConstraintRoles[snippet1ReplaceRoleIter1];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						int currentFactIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
						VerbalizationTextSnippetType listSnippet;
						if (snippet1ReplaceRoleIter1 == 0)
						{
							listSnippet = VerbalizationTextSnippetType.SimpleListOpen;
						}
						else if (snippet1ReplaceRoleIter1 == (constraintRoleArity - 1))
						{
							if (snippet1ReplaceRoleIter1 == 1)
							{
								listSnippet = VerbalizationTextSnippetType.SimpleListPairSeparator;
							}
							else
							{
								listSnippet = VerbalizationTextSnippetType.SimpleListFinalSeparator;
							}
						}
						else
						{
							listSnippet = VerbalizationTextSnippetType.SimpleListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						sbTemp.Append(basicRoleReplacements[factRoles.IndexOf(allConstraintRoles[snippet1ReplaceRoleIter1])]);
						if (snippet1ReplaceRoleIter1 == (constraintRoleArity - 1))
						{
							sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
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
					int snippet1ReplaceRoleIter2 = 0;
					bool snippet1ReplaceIsFirstPass2 = true;
					for (; snippet1ReplaceRoleIter2 < constraintRoleArity; ++snippet1ReplaceRoleIter2)
					{
						Role primaryRole = allConstraintRoles[snippet1ReplaceRoleIter2];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						int currentFactIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
						VerbalizationTextSnippetType listSnippet;
						if (snippet1ReplaceRoleIter2 == 0)
						{
							listSnippet = VerbalizationTextSnippetType.IndentedListOpen;
						}
						else if (snippet1ReplaceRoleIter2 == (constraintRoleArity - 1))
						{
							if (snippet1ReplaceRoleIter2 == 1)
							{
								listSnippet = VerbalizationTextSnippetType.IndentedListPairSeparator;
							}
							else
							{
								listSnippet = VerbalizationTextSnippetType.IndentedListFinalSeparator;
							}
						}
						else
						{
							listSnippet = VerbalizationTextSnippetType.IndentedListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						snippet1Replace2 = null;
						readingOrder = allConstraintRoleReadingOrders[snippet1ReplaceRoleIter2];
						int snippet1ReplaceFactRoleIter2 = 0;
						for (; snippet1ReplaceFactRoleIter2 < factArity; ++snippet1ReplaceFactRoleIter2)
						{
							Role currentRole = factRoles[snippet1ReplaceFactRoleIter2];
							string roleReplacement = null;
							string basicReplacement = basicRoleReplacements[snippet1ReplaceFactRoleIter2];
							if ((currentRole != primaryRole) && snippet1ReplaceIsFirstPass2)
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.AtMostOneQuantifier, isDeontic, isNegative), basicReplacement);
							}
							else if (currentRole != primaryRole)
							{
								roleReplacement = "";
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.DefiniteArticle, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1ReplaceFactRoleIter2] = roleReplacement;
						}
						snippet1Replace2 = FactType.PopulatePredicateText(readingOrder, factRoles, roleReplacements);
						sbTemp.Append(snippet1Replace2);
						if (snippet1ReplaceRoleIter2 == (constraintRoleArity - 1))
						{
							sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.IndentedListClose, isDeontic, isNegative));
						}
						snippet1ReplaceIsFirstPass2 = false;
					}
					snippet1Replace2 = sbTemp.ToString();
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1, snippet1Replace2), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
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
					readingOrder = FactType.GetMatchingReadingOrder(allReadingOrders, null, primaryRole, null, true, true, factRoles, false);
					if (readingOrder == null)
					{
						missingReading1 = true;
					}
					else
					{
						allConstraintRoleReadingOrders[readingMatchIndex1] = readingOrder;
					}
				}
				if (!(missingReading1))
				{
					beginVerbalization(VerbalizationContent.Normal);
					string snippetFormat1 = snippets.GetSnippet(VerbalizationTextSnippetType.ModalPossibilityOperator, isDeontic, isNegative);
					string snippet1Replace1 = null;
					if (sbTemp == null)
					{
						sbTemp = new StringBuilder();
					}
					else
					{
						sbTemp.Length = 0;
					}
					int snippet1ReplaceRoleIter1 = 0;
					bool snippet1ReplaceIsFirstPass1 = true;
					for (; snippet1ReplaceRoleIter1 < constraintRoleArity; ++snippet1ReplaceRoleIter1)
					{
						Role primaryRole = allConstraintRoles[snippet1ReplaceRoleIter1];
						parentFact = primaryRole.FactType;
						factRoles = parentFact.RoleCollection;
						factArity = factRoles.Count;
						allReadingOrders = parentFact.ReadingOrderCollection;
						int currentFactIndex = allFacts.IndexOf(parentFact);
						string[] basicRoleReplacements = allBasicRoleReplacements[currentFactIndex];
						VerbalizationTextSnippetType listSnippet;
						if (snippet1ReplaceRoleIter1 == 0)
						{
							listSnippet = VerbalizationTextSnippetType.SimpleListOpen;
						}
						else if (snippet1ReplaceRoleIter1 == (constraintRoleArity - 1))
						{
							if (snippet1ReplaceRoleIter1 == 1)
							{
								listSnippet = VerbalizationTextSnippetType.SimpleListPairSeparator;
							}
							else
							{
								listSnippet = VerbalizationTextSnippetType.SimpleListFinalSeparator;
							}
						}
						else
						{
							listSnippet = VerbalizationTextSnippetType.SimpleListSeparator;
						}
						sbTemp.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
						snippet1Replace1 = null;
						readingOrder = allConstraintRoleReadingOrders[snippet1ReplaceRoleIter1];
						int snippet1ReplaceFactRoleIter1 = 0;
						for (; snippet1ReplaceFactRoleIter1 < factArity; ++snippet1ReplaceFactRoleIter1)
						{
							Role currentRole = factRoles[snippet1ReplaceFactRoleIter1];
							string roleReplacement = null;
							string basicReplacement = basicRoleReplacements[snippet1ReplaceFactRoleIter1];
							if ((currentRole != primaryRole) && snippet1ReplaceIsFirstPass1)
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.MoreThanOneQuantifier, isDeontic, isNegative), basicReplacement);
							}
							else if (currentRole != primaryRole)
							{
								roleReplacement = "";
							}
							else
							{
								roleReplacement = string.Format(writer.FormatProvider, snippets.GetSnippet(VerbalizationTextSnippetType.IdentityReferenceQuantifier, isDeontic, isNegative), basicReplacement);
							}
							if (roleReplacement == null)
							{
								roleReplacement = basicReplacement;
							}
							roleReplacements[snippet1ReplaceFactRoleIter1] = roleReplacement;
						}
						snippet1Replace1 = FactType.PopulatePredicateText(readingOrder, factRoles, roleReplacements);
						sbTemp.Append(snippet1Replace1);
						if (snippet1ReplaceRoleIter1 == (constraintRoleArity - 1))
						{
							sbTemp.Append(snippets.GetSnippet(VerbalizationTextSnippetType.SimpleListClose, isDeontic, isNegative));
						}
						snippet1ReplaceIsFirstPass1 = false;
					}
					snippet1Replace1 = sbTemp.ToString();
					FactType.WriteVerbalizerSentence(writer, string.Format(writer.FormatProvider, snippetFormat1, snippet1Replace1), snippets.GetSnippet(VerbalizationTextSnippetType.CloseVerbalizationSentence, isDeontic, isNegative));
				}
			}
			return true;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, VerbalizationSets snippets, NotifyBeginVerbalization beginVerbalization, bool isNegative)
		{
			return this.GetVerbalization(writer, snippets, beginVerbalization, isNegative);
		}
	}
	#endregion // ExternalUniquenessConstraint verbalization
}
