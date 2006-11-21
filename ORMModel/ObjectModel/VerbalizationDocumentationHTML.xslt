<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" indent="yes"/>
	<xsl:template match="/snippets">
		<html xmlns="http://www.w3.org/1999/xhtml">
			<head>
				<title>NORMA Verbalization Documentation</title>
				<style type="text/css">
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
					.note { color: DarkGray; font-style: italic; font-weight: normal; }
					.notAvailable { font-style: italic; }
					.instance { color: Brown; font-weight: normal; }
					div#header
					{
					position:relative;
					}
					div#snippetContainer
					{
					padding-top:25px;
					}
					div#container
					{
					position:relative;
					width:650px;
					padding: 0px;
					overflow:visible;
					}
					div#abstract
					{
					padding-top:15px;
					padding-left:50px;
					padding-bottom:50px;
					}
					body,td,th {
					font-family: Geneva, Arial, Helvetica, sans-serif;
					font-size: 12px;
					color: #000000;
					}
					body {
					background-color: #FFFFFF;
					}
					b.rtop2, b.rbottom2{display:block;background: #FFF}
					b.rtop2 b, b.rbottom2 b{display:block;height: 1px;
					overflow: hidden; background: #757D8A}
					b.rtop2 b.r4, b.rbottom2 b.r4{margin: 0 1px;height: 2px}

					/*******************************************************
					* Global Link Styles								   *
					******************************************************/
					:link { text-decoration:none; color:#0088B5; }			/* for unvisited links */
					a:active { text-decoration:none; color:#0088B5; }		/* when link is clicked */
					a:hover { text-decoration:none; color:#333333; }		/* when mouse is over link */
					:visited { text-decoration:none; color:#0088B5; }		/* for visited links */


					span.snippetHeader
					{
					border-bottom: 2px solid #FFCE84;
					font-weight:bold;
					font-size:18px;
					}

					span.reportItem
					{
					font-weight:bold;
					border-bottom: 1px solid #666666;
					}

					/*******************************************************
					* Snippet Styles									   *
					******************************************************/
					div.snippet div
					{
					padding-left:25px;
					}
					div.snippet
					{
					padding-bottom:25px;
					}

					div.snippetStatement
					{
					padding-top:10px;
					padding-left: 15px;
					padding-bottom: 25px;
					color: #666666;
					}
					div.unformattedSnippet
					{
					padding-left:20px;
					padding-top:10px;
					padding-bottom:10px;
					}
					div#toc
					{
					position:absolute;
					left:650px;
					width:350px;

					}
					pre.unformattedSnippetDecorator
					{
					background-color:#F0F0F0;
					font-family: Geneva, Arial, Helvetica, sans-serif;
					font-size: 12px;

					}
					.pmb1_b, .pmb1_s { font-size:1px; }
					.pmb1_1, .pmb1_2, .pmb1_3, .pmb1_4, .pmb1_b, .pmb1_s {display:block; overflow:hidden;}
					.pmb1_1, .pmb1_2, .pmb1_3, .pmb1_s {height:1px;}
					.pmb1_2, .pmb1_3, .pmb1_4 {border-style: solid; border-width: 0 1px; }
					.pmb1_1 {margin:0 5px; }
					.pmb1_2 {margin:0 3px; border-width:0 2px;}
					.pmb1_3 {margin:0 2px;}
					.pmb1_4 {height:2px; margin:0 1px;}
					.pmb1_c {display:block; border-style: solid ; border-width: 0 1px;}
					h4#snippetLinkHeader
					{
					text-align:center;
					padding-top:0px;
					font-size:12px;
					}
					div#examples
					{
					border-top: 2px solid #FFCE84;
					border-bottom: 2px solid #FFCE84;
					}
					h2#examplesHeader
					{
					font-weight:bold;
					font-size:18px;
					}
					div.example
					{
					padding-top:15px;
					padding-left:50px;
					padding-bottom:50px;
					}
					div.exampleHeader
					{
					padding-bottom:10px;
					}
					div.exampleDescription li
					{
					padding-bottom:5px;
					}
				</style>
			</head>
			<body>
				<div id="container">
					<div id="header">
						<h1>NORMA Verbalization Documentation</h1>
					</div>
					<div id="abstract">
						<p>The Object Role Modeling verbalization implementation in NORMA allows for flexibility and customization. Snippets may be used in many different scenarios and it is possible that some snippets may be used inside of others. This document outlines the usage of each snippet to aide in the development of a full implementation for a new language.</p>
					</div>
					<div id="examples">
						<h2 id="examplesHeader">Examples</h2>
						<p>To aide in the development of a new language implementation, the following example has been provided:</p>
						<div class="example">
							<div class="exampleHeader">
								<strong>Snippet:</strong> <a href="#Conditional">Conditional</a>
							</div>
							<div class="exampleDescription">
								The Conditional snippet makes use of two snippets:
								<ol>
									<li>
										<a href="#ExistentialQuantifier">ExistentialQuantifier</a>: "<span class="verbalization">
											<span class="quantifier">some</span> {0}
										</span>"</li>
									<li>
										<a href="#DefiniteArticle">DefiniteArticle</a>: "<span class="verbalization">
										<span class="quantifier">that</span> {0}
									</span>"</li>
								</ol>
								For this example, we will replace these snippets with the following:
								<ol>
									<li>
										<a href="#ExistentialQuantifier">ExistentialQuantifier</a>: "<span class="verbalization">{0}</span>"
									</li>
									<li>
										<a href="#DefiniteArticle">DefiniteArticle</a>: "<span class="verbalization">
										<span class="quantifier">sono</span> {0}
									</span>"</li>
								</ol>
								We will then replace the Conditional Snippet's English format with the following Japanese format:
								<ol>
									<li>
										<strong>
											<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>English Format:</strong>
										<span class="verbalization">
											<span class="quantifier">if </span>{0}<span class="quantifier"> then </span>{1}
										</span>
									</li>
									<li>
										<strong>Japanese Format:</strong> <span class="verbalization">
											<span class="quantifier">moshi </span>{0}<span class="quantifier"> nara </span>{1}
										</span>
									</li>
								</ol>
							</div>
							<div class="exampleLanguage">
								<div class="exampleDescription">
									Consider the following unary facts:
									<ol>
										<li>
											<strong>
												<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>English:
											</strong>"<span class="verbalization">
											<span class="objectType">Person</span> smokes<span class="listSeparator">.</span>
										</span>"<br/>
											<strong>Japanese: </strong>"<span class="verbalization">
												<span class="objectType">Hito</span> ga tabako wo suu<span class="listSeparator">.</span>
											</span>"
										</li>
										<li>
											<strong>
												<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>English: </strong>"<span class="verbalization"><span class="objectType">Person</span> is cancer prone<span class="listSeparator">.</span>
											</span>"<br/>
											<strong>Japanese: </strong>"<span class="verbalization">
											<span class="objectType">Hito</span> wa gan ni naru keikou ga aru<span class="listSeparator">.</span>
										</span>"<br/>
										</li>
									</ol>
									If we place a subset constraint between these two facts, then they will verbalize as follows:
									<ol>
										<li>
											<strong>
												<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>English:
											</strong>"<span class="verbalization">
												<span class="quantifier">If </span><span class="quantifier">some</span> <span class="objectType">Person</span> smokes<span class="quantifier"> then </span><span class="quantifier">that</span> <span class="objectType">Person</span> is cancer prone<span class="listSeparator">.</span>
											</span>"<br/>
											<strong>Japanese: </strong>"<span class="verbalization">
												<span class="quantifier">Moshi </span> <span class="objectType">Hito</span> ga tabako wo suu<span class="quantifier"> nara </span><span class="quantifier">sono</span> <span class="objectType">Hito</span> wa gan ni naru keikou ga aru<span class="listSeparator">.</span>
											</span>"<br/>
										</li>
									</ol>
								</div>
							</div>
						</div>
					</div>
					<div id="toc">
						<b class="pmb1_b">
							<b class="pmb1_1" style="background:#9DCEFF; color: inherit;">
								<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
							</b>
							<b class="pmb1_2" style="background:#DDEEFF; color: inherit; border-color: #9DCEFF;">
								<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
							</b>
							<b class="pmb1_3" style="background:#DDEEFF; color: inherit; border-color: #9DCEFF;">
								<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
							</b>
							<b class="pmb1_4" style="background:#DDEEFF; color: inherit; border-color: #9DCEFF;">
								<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
							</b>
						</b>
						<div class="pmb1_c" style="background:#DDEEFF; color: inherit; border-color: #9DCEFF;">
							<b class="pmb1_s">
								<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
							</b>
							<div id="snippetLinks">
								<h4 id="snippetLinkHeader">Snippets</h4>
								<ol>
									<xsl:for-each select="snippet">
										<li>
											<a href="#{@name}">
												<xsl:value-of select="@name"/>
											</a>
										</li>
									</xsl:for-each>
								</ol>
							</div>
							<b class="pmb1_s">
								<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
							</b>
						</div>
						<b class="pmb1_b">
							<b class="pmb1_4" style="background:#DDEEFF;border-color: #9DCEFF; color: inherit;">
								<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
							</b>
							<b class="pmb1_3" style="background:#DDEEFF;border-color: #9DCEFF; color: inherit;">
								<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
							</b>
							<b class="pmb1_2" style="background:#DDEEFF;border-color: #9DCEFF; color: inherit;">
								<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
							</b>
							<b class="pmb1_1" style="background:#9DCEFF; color: inherit;">
								<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
							</b>
						</b>
					</div>
					<div id="snippetContainer">
						<xsl:for-each select="snippet">
							<div id="{@name}" class="snippet">
								<span class="snippetHeader">
									<a name="{@name}" id="{@name}">
										<xsl:value-of select="@name"/>
									</a>
								</span>
								<div class="snippetStatement">
									<strong>Description: </strong>
									<xsl:choose>
										<xsl:when test="contains(statement, 'Format')">
											<xsl:value-of select="substring-after(substring-before(statement, 'Format'), 'Description:')"/>	
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="substring-after(statement, 'Description:')"/>
										</xsl:otherwise>
									</xsl:choose>
									<br/>
									<xsl:if test="contains(statement, 'Format:')">
										<strong>Format: </strong>
										<xsl:value-of select="substring-after(statement, 'Format:')"/>
									</xsl:if>
									<xsl:if test="contains(statement, 'Format for alethic:')">
										<strong>Alethic Format: </strong>
										<xsl:value-of select="substring-before(substring-after(statement, 'Format for alethic:'), 'Format for deontic:')"/>
										<br/>
									</xsl:if>
									<xsl:if test="contains(statement, 'Format for deontic:')">
										<strong>Deontic Format: </strong>
										<xsl:value-of select="substring-after(statement, 'Format for deontic:')"/>
										<br/>
									</xsl:if>
									<xsl:if test="contains(statement, 'Format for alethic and positive:')">
										<strong>Alethic/Positive Format: </strong>
										<xsl:value-of select="substring-before(substring-after(statement, 'Format for alethic and positive:'), 'Format for deontic and positive:')"/>
										<br/>
									</xsl:if>
									<xsl:if test="contains(statement, 'Format for alethic and negative:')">
										<strong>Alethic/Negative Format: </strong>
										<xsl:value-of select="substring-before(substring-after(statement, 'Format for alethic and negative:'), 'Format for deontic and negative:')"/>
										<br/>
									</xsl:if>
									<xsl:if test="contains(statement, 'Format for deontic and positive:')">
										<strong>Deontic/Positive Format: </strong>
										<xsl:value-of select="substring-before(substring-after(statement, 'Format for deontic and positive:'), 'Format for alethic and negative:')"/>
										<br/>
									</xsl:if>
									<xsl:if test="contains(statement, 'Format for deontic and negative:')">
										<strong>Deontic/Negative Format: </strong>
										<xsl:value-of select="substring-after(statement, 'Format for deontic and negative:')"/>
										<br/>
									</xsl:if>
								</div>
								<div class="snippetReplacementFieldCount">
									<span class="reportItem">Number of replacement fields: </span>
									<xsl:value-of select="@replacementCount"/>
								</div>
								<div class="snippetUnformattedText">
									<span class="reportItem">Unformatted version: </span>
									<div class="unformattedSnippet">
										<xsl:choose>
											<xsl:when test="normalize-space(unformattedSnippet) = ''">
												<strong>Not Available</strong>
											</xsl:when>
											<xsl:otherwise>
												<pre class="unformattedSnippetDecorator">
													<xsl:value-of select="unformattedSnippet"/>
												</pre>
											</xsl:otherwise>
										</xsl:choose>
									</div>
								</div>
								<xsl:if test="count(contains/snippet) > 0">
									<div class="snippetContains">
										<p>
											<xsl:text>The </xsl:text>
											<strong>
												<xsl:value-of select="@name"/>
											</strong>
											<xsl:text> snippet contains the following snippets: </xsl:text>
											<ul class="contains">
												<xsl:for-each select="contains/snippet">
													<li>
														<a href="#{@name}">
															<xsl:value-of select="@name"/>
														</a>
													</li>
												</xsl:for-each>
											</ul>
										</p>
									</div>
								</xsl:if>
								<xsl:if test="count(containedIn/snippet) > 0">
									<div classs="snippetContainedIn">
										<p>
											<xsl:text>The </xsl:text>
											<strong>
												<xsl:value-of select="@name"/>
											</strong>
											<xsl:text> snippet is contained in the following snippets: </xsl:text>
											<ul class="containedIn">
												<xsl:for-each select="containedIn/snippet">
													<li>
														<a href="#{@name}">
															<xsl:value-of select="@name"/>
														</a>
													</li>
												</xsl:for-each>
											</ul>
										</p>
									</div>
								</xsl:if>
								<xsl:if test="count(usedBy/constraint) > 0">
									<div class="snippetUsedBy">
										<p>
											<xsl:text>The </xsl:text>
											<strong>
												<xsl:value-of select="@name"/>
											</strong>
											<xsl:text> snippet is used by the following Constraints: </xsl:text>
											<ul class="usedBy">
												<xsl:for-each select="usedBy/constraint">
													<li>
														<xsl:value-of select="@name"/>
													</li>
												</xsl:for-each>
											</ul>
										</p>
									</div>
								</xsl:if>
							</div>
						</xsl:for-each>
					</div>
				</div>
			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>
