using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;

// Common Public License Copyright Notice
// /**************************************************************************\
// * Natural Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © Neumont University. All rights reserved.                     *
// * Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
	#region ReportVerbalizationSnippetType enum
	/// <summary>An enum with one value for each recognized snippet</summary>
	public enum ReportVerbalizationSnippetType
	{
		/// <summary>The 'ContextModelDescriptionClose' simple snippet value.</summary>
		ContextModelDescriptionClose,
		/// <summary>The 'ContextModelDescriptionOpen' simple snippet value.</summary>
		ContextModelDescriptionOpen,
		/// <summary>The 'EmptyContentListItemSnippet' simple snippet value.</summary>
		EmptyContentListItemSnippet,
		/// <summary>The 'FactTypeConstraintValidationHeader' simple snippet value.</summary>
		FactTypeConstraintValidationHeader,
		/// <summary>The 'FactTypeConstraintValidationListItemClose' simple snippet value.</summary>
		FactTypeConstraintValidationListItemClose,
		/// <summary>The 'FactTypeConstraintValidationListItemOpen' format string snippet. Contains 2 replacement fields.</summary>
		FactTypeConstraintValidationListItemOpen,
		/// <summary>The 'FactTypeConstraintValidationListSpacer' simple snippet value.</summary>
		FactTypeConstraintValidationListSpacer,
		/// <summary>The 'FactTypeConstraintValidationSignature' simple snippet value.</summary>
		FactTypeConstraintValidationSignature,
		/// <summary>The 'FactTypeListFooter' simple snippet value.</summary>
		FactTypeListFooter,
		/// <summary>The 'FactTypeListHeader' simple snippet value.</summary>
		FactTypeListHeader,
		/// <summary>The 'FactTypePageHeader' format string snippet. Contains 1 replacement field.</summary>
		FactTypePageHeader,
		/// <summary>The 'FactTypePageObjectTypeListClose' simple snippet value.</summary>
		FactTypePageObjectTypeListClose,
		/// <summary>The 'FactTypePageObjectTypeListOpen' simple snippet value.</summary>
		FactTypePageObjectTypeListOpen,
		/// <summary>The 'FactTypeRelationshipLinkClose' simple snippet value.</summary>
		FactTypeRelationshipLinkClose,
		/// <summary>The 'FactTypeRelationshipLinkOpen' format string snippet. Contains 1 replacement field.</summary>
		FactTypeRelationshipLinkOpen,
		/// <summary>The 'GenericConstraintHeader' simple snippet value.</summary>
		GenericConstraintHeader,
		/// <summary>The 'GenericConstraintListClose' simple snippet value.</summary>
		GenericConstraintListClose,
		/// <summary>The 'GenericConstraintListItemClose' simple snippet value.</summary>
		GenericConstraintListItemClose,
		/// <summary>The 'GenericConstraintListItemOpen' format string snippet. Contains 2 replacement fields.</summary>
		GenericConstraintListItemOpen,
		/// <summary>The 'GenericConstraintListOpen' simple snippet value.</summary>
		GenericConstraintListOpen,
		/// <summary>The 'GenericListItemClose' simple snippet value.</summary>
		GenericListItemClose,
		/// <summary>The 'GenericListItemOpen' simple snippet value.</summary>
		GenericListItemOpen,
		/// <summary>The 'GenericRelationshipsListClose' simple snippet value.</summary>
		GenericRelationshipsListClose,
		/// <summary>The 'GenericRelationshipsListOpen' simple snippet value.</summary>
		GenericRelationshipsListOpen,
		/// <summary>The 'GenericSubTypeListClose' simple snippet value.</summary>
		GenericSubTypeListClose,
		/// <summary>The 'GenericSubTypeListOpen' simple snippet value.</summary>
		GenericSubTypeListOpen,
		/// <summary>The 'GenericSummaryClose' simple snippet value.</summary>
		GenericSummaryClose,
		/// <summary>The 'GenericSummaryOpen' simple snippet value.</summary>
		GenericSummaryOpen,
		/// <summary>The 'GenericSuperTypeListClose' simple snippet value.</summary>
		GenericSuperTypeListClose,
		/// <summary>The 'GenericSuperTypeListOpen' simple snippet value.</summary>
		GenericSuperTypeListOpen,
		/// <summary>The 'ObjectTypeListFooter' simple snippet value.</summary>
		ObjectTypeListFooter,
		/// <summary>The 'ObjectTypeListHeader' simple snippet value.</summary>
		ObjectTypeListHeader,
		/// <summary>The 'ObjectTypeListObjectTypeValueLink' format string snippet. Contains 2 replacement fields.</summary>
		ObjectTypeListObjectTypeValueLink,
		/// <summary>The 'ObjectTypePageFactTypeListClose' simple snippet value.</summary>
		ObjectTypePageFactTypeListClose,
		/// <summary>The 'ObjectTypePageFactTypeListOpen' simple snippet value.</summary>
		ObjectTypePageFactTypeListOpen,
		/// <summary>The 'ObjectTypePageHeader' format string snippet. Contains 1 replacement field.</summary>
		ObjectTypePageHeader,
		/// <summary>The 'ObjectTypeRelationshipValueLink' format string snippet. Contains 2 replacement fields.</summary>
		ObjectTypeRelationshipValueLink,
		/// <summary>The 'ObjectTypeValueLink' format string snippet. Contains 2 replacement fields.</summary>
		ObjectTypeValueLink,
		/// <summary>The 'ReportDocumentContents' format string snippet. Contains 1 replacement field.</summary>
		ReportDocumentContents,
		/// <summary>The 'ReportDocumentContentsReplacementAll' simple snippet value.</summary>
		ReportDocumentContentsReplacementAll,
		/// <summary>The 'ReportDocumentContentsReplacementObject' simple snippet value.</summary>
		ReportDocumentContentsReplacementObject,
		/// <summary>The 'ReportDocumentContentsReplacementValidation' simple snippet value.</summary>
		ReportDocumentContentsReplacementValidation,
		/// <summary>The 'ReportDocumentFooter' simple snippet value.</summary>
		ReportDocumentFooter,
		/// <summary>The 'ReportDocumentHeader' format string snippet. Contains 1 replacement field.</summary>
		ReportDocumentHeader,
		/// <summary>The 'VerbalizerNewLine' simple snippet value.</summary>
		VerbalizerNewLine,
	}
	#endregion // ReportVerbalizationSnippetType enum
	#region ReportVerbalizationSets class
	/// <summary>A class deriving from VerbalizationSets.</summary>
	public class ReportVerbalizationSets : VerbalizationSets<ReportVerbalizationSnippetType>
	{
		/// <summary>The default verbalization snippet set. Contains english HTML snippets.</summary>
		public static readonly ReportVerbalizationSets Default = (ReportVerbalizationSets)VerbalizationSets<ReportVerbalizationSnippetType>.Create<ReportVerbalizationSets>(null);
		/// <summary>Populates the snippet sets of the ReportVerbalizationSets object.</summary>
		/// <param name="sets">The sets to be populated.</param>
		/// <param name="userData">User-defined data passed to the Create method</param>
		protected override void PopulateVerbalizationSets(VerbalizationSet[] sets, object userData)
		{
			sets[0] = new ArrayVerbalizationSet(new string[]{
				@"
				
				</div>
			",
				@"
				
				<div>
				
			",
				@"<li class=""ListLink"">There are no items for this section.</li>",
				@"
				<div class=""topHeader"">Constraint Validation Report</div>
			",
				@"
				
					</span>
				</div>
				
			",
				@"
				
				<div class=""constraintName""><input type=""checkbox"" name=""chkConstraint"" value=""1""/>&#xA0;<span class=""constraintName"">{0}</span></div>
				<div class=""constraintHeader"">Type:</div>
				<div class=""constraintType"">{1}</div>
				<div class=""constraintHeaderNoWrap"">Verbalization:</div>
				<div class=""constraintVerbalization"">
					<span class=""verbalization"">
					
			",
				@"
				
				<div class=""constraintListSpacer""></div>
				
			",
				@"
				
				<div class=""signatureLine""></div>
				<div class=""signatureLabel"">Signature</div>
				
			",
				@"
				
					</ul>
				</div>
				
			",
				@"
				
				<div class=""topHeader"">
					Fact Types
				</div>
				<div class=""body"">
					<ul>
				
			",
				@"
				
				<div class=""topHeader"">
					<div class=""objectTypeName"">&quot;{0}&quot;</div>
					<div class=""typeName"">Fact Type<a href=""../ObjectTypeList.html"">&#xA0;&#x2191;</a></div>
				</div>
				
			",
				@"
				
					</ul>
				</div>
				
			",
				@"
				
				<div class=""list"">
				<div class=""header"">Role Players</div>
					<ul>
				
			",
				"</a>",
				@"<a href=""../FactTypes/{0}.html"">",
				@"
				Constraints
			",
				@"
				
				</div>
				
			",
				@"
				
				</span>
				</div>
				
			",
				@"
				
				<div class=""constraintName""><span class=""constraintName"">{0}</span></div>
				<div class=""constraintHeader"">Type:</div>
				<div class=""constraintType"">{1}</div>
				<div class=""constraintHeaderNoWrap"">Verbalization:</div>
				<div class=""constraintVerbalization"">
				<span class=""verbalization"">
				
			",
				@"
				
				<div class=""header"">Constraints</div>
				<div class=""body"">
				
			",
				"</li>",
				@"<li class=""ListLink"">",
				@"
				
					</ul>
				</div>
				
			",
				@"
				
				<div class=""list"">
				<div class=""header"">Related Types</div>
					<ul>
				
			",
				@"
				
					</ul>
				</div>
				
			",
				@"
				
				<div class=""list"">
				<div class=""header"">Sub Types</div>
					<ul>
				
			",
				"</span></div>",
				@"
				
					<div class=""header"">Summary</div>
					<div class=""body"">
					<span class=""verbalization"">
				
			",
				@"
				
					</ul>
				</div>
				
			",
				@"
				
				<div class=""list"">
				<div class=""header"">Super Types</div>
					<ul>
				
			",
				@"
				
					</ul>
				</div>
				
			",
				@"
				
				<div class=""topHeader"">
					Object Types
				</div>
				<div class=""body"">
					<ul>
				
			",
				@"<span class=""objectType""><a href=""ObjectTypes/{1}.html"">{0}</a></span>",
				@"
				
					</ul>
				</div>
				
			",
				@"
				
				<div class=""list"">
				<div class=""header"">Fact Types</div>
					<ul class=""verbalization"">
				
			",
				@"
				
				<div class=""topHeader"">
					<div class=""objectTypeName"">&quot;{0}&quot;</div>
					<div class=""typeName"">Object Type<a href=""../ObjectTypeList.html"">&#xA0;&#x2191;</a></div>
				</div>
				
			",
				@"<span class=""objectType""><a href=""../ObjectTypes/{1}.html"">{0}</a></span>",
				@"<span class=""objectType""><a href=""{1}.html"">{0}</a></span>",
				@"
				
				<div id=""navBar"">
					<div class=""content"">
					<div style=""width:250px;"">
					 <b class=""roundTop1_b"">
					  <b class=""roundTop1_1"" style=""background:#9DCEFF; color: inherit;""></b>
					  <b class=""roundTop1_2"" style=""background:#DDEEFF; color: inherit; border-color: #9DCEFF;""></b>
					  <b class=""roundTop1_3"" style=""background:#DDEEFF; color: inherit; border-color: #9DCEFF;""></b>
					  <b class=""roundTop1_4"" style=""background:#DDEEFF; color: inherit; border-color: #9DCEFF;""></b>
					 </b>
					 <div class=""roundTop1_c"" style=""background:#DDEEFF; color: inherit; border-color: #9DCEFF;"">
					  <b class=""roundTop1_s""></b>
					  <h4>Verbalization Report Contents</h4>
						<ol>
							{0}
						</ol>				
				  <b class=""roundTop1_s""></b>
				 </div>
				 <b class=""roundTop1_b"">
				  <b class=""roundTop1_4"" style=""background:#DDEEFF;border-color: #9DCEFF; color: inherit;""></b>
				  <b class=""roundTop1_3"" style=""background:#DDEEFF;border-color: #9DCEFF; color: inherit;""></b>
				  <b class=""roundTop1_2"" style=""background:#DDEEFF;border-color: #9DCEFF; color: inherit;""></b>
				  <b class=""roundTop1_1"" style=""background:#9DCEFF; color: inherit;""></b>
				 </b>
				</div>
				<div class=""clearfix""></div>
				</div>
				</div>
			",
				@"
				
				<li><a href=""ObjectTypeList.html"">Object Types</a></li>
				<li><a href=""ConstraintValidationReport.html"">Constraint Validation</a></li>
			",
				@"
				
				<li><a href=""ObjectTypeList.html"">Object Types</a></li>
			",
				@"
				
				<li><a href=""ConstraintValidationReport.html"">Constraint Validation</a></li>
			",
				@"
				</body>
				</html>
			",
				@"
				<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
						<html xmlns=""http://www.w3.org/1999/xhtml"">
						<head>
							<title>NORMA Verbalization Report</title>
							<style type=""text/css"">
								/*******************************************************
								* Global Page Styles								   *
								******************************************************/
								.verbalization { color: DarkGreen; }
								.objectType { color: Purple; font-weight: normal; }
								.objectTypeMissing { color: Purple; font-weight: normal; }
								.referenceMode { color: Brown; font-weight: normal; }
								.predicateText { color: DarkGreen; font-weight: normal; }
								.quantifier { color: MediumBlue; font-weight: bold; }
								.primaryErrorReport { color: red; font-weight: bolder; }
								.secondaryErrorReport { color: red; }
								.indent { left: 20px; position: relative; }
								.smallIndent { left: 8px; position: relative;}
								.listSeparator { color: windowtext; font-weight: 200;}
								.logicalOperator { color: MediumBlue; font-weight: bold;}
								.note { color: Black; font-style: italic; font-weight: normal; }
								.definition { color: Black; font-style: italic; font-weight: normal; }
								.notAvailable { font-style: italic; }
								.instance { color: Brown; font-weight: normal; }

								table.hidden, tr.hidden, td.hidden {{ margin: 0em; padding: 0em; border-collapse: collapse;}}
								td.hidden {{ vertical-align: top; }}
								table.hidden {{ display:inline; }}

								div.disclaimer
								{
									font-weight:bold;
									color:#FF0000;
									padding-bottom:25px;
									padding-left:20px;
								}
								div.topHeader
								{
									position:relative;
									border-bottom: 2px solid #FFCE84;
									font-size:18px;
									margin-top: 8px;
								}
								div.container
								{
									position:relative;
									width:650px;
									padding: 0px;
									overflow:visible;
								}
								body,td,th {
									font-family: Geneva, Arial, Helvetica, sans-serif;
									font-size: 12px;
									color: #000000;
								}
								body {
									background-color: #FFFFFF;
								}

								/*******************************************************
								* Global Link Styles								   *
								******************************************************/
								:link { text-decoration:none; color:#0088B5; }
								a:active { text-decoration:none; color:#0088B5; }
								a:hover { text-decoration:none; color:#333333; }
								:visited { text-decoration:none; color:#0088B5; }
								
								div.objectTypeName
								{
									font-size:18px;
									float:left;
									margin-right:8px;
									padding-bottom:5px;
								}
								div.typeName
								{
									font-size:18px;
									padding-top:0px;
									padding-bottom:5px;
								}
								div.header
								{
									font-weight:bold;
									margin-top:10px;
								}
								div.body
								{
									margin-top:5px;
									margin-left:10px;
								}
								div.constraintName
								{
									padding-left:5px;
									padding-bottom:5px;
								}
								div.constraintName span.constraintName
								{
									padding-left:5px;
									text-decoration:underline;
								}
								div.constraintHeader
								{
									color: #333333;
									padding-left:10px;
									float:left;
									margin-right:5px;
								}
								div.constraintHeaderNoWrap
								{
									color: #333333;
									padding-left:10px;
								}
								div.constraintVerbalization
								{
									padding-top:0px;
									margin-left:20px;
									padding-bottom:10px;
								}
								div.constraintListSpacer
								{
									padding-top:5px;
									padding-bottom:5px;
									height:5px;
								}
								div.signatureLabel
								{
									text-align:left;
								}
								div.signatureLine
								{
									padding-top:50px;
									border-bottom: 1px solid #000000;
									width:300px;
								}
								div#navBar
								{
									float:right;
									padding-top:15px;
									padding-left:20px;
								}
								div#navBar h4
								{
									padding-left:10px;
								}
								li.ListLink
								{
								}
								.roundTop1_b, .roundTop1_s { font-size:1px; }
								.roundTop1_1, .roundTop1_2, .roundTop1_3, .roundTop1_4, .roundTop1_b, .roundTop1_s {display:block; overflow:hidden;}
								.roundTop1_1, .roundTop1_2, .roundTop1_3, .roundTop1_s {height:1px;}
								.roundTop1_2, .roundTop1_3, .roundTop1_4 {border-style: solid; border-width: 0 1px; }
								.roundTop1_1 {margin:0 5px; }
								.roundTop1_2 {margin:0 3px; border-width:0 2px;}
								.roundTop1_3 {margin:0 2px;}
								.roundTop1_4 {height:2px; margin:0 1px;}
								.roundTop1_c {display:block; border-style: solid ; border-width: 0 1px;}
							</style>
						</head>
						<body>
			",
				"<br/>"});
			sets[1] = sets[0];
			sets[2] = sets[0];
			sets[3] = sets[0];
		}
		/// <summary>Converts enum value of ReportVerbalizationSnippetType to an integer index value.</summary>
		protected override int ValueToIndex(ReportVerbalizationSnippetType enumValue)
		{
			return (int)enumValue;
		}
	}
	#endregion // ReportVerbalizationSets class
}
